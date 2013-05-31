using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.CodeDom;
using System.Windows.Forms;

using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using Zeus.ErrorHandling;
using Zeus.Configuration;
using Zeus.Serializers;

namespace Zeus
{

	/// <summary>
	/// Summary description for Zeusecutioner.
	/// </summary>
	public class ZeusExecutioner
	{
		private ILog _log;

		public ZeusExecutioner(ILog log) 
		{
			this._log = log;
		}

		public IZeusContext Execute(ZeusTemplate template, IZeusContext context, int timeout, bool skipGui) 
		{
			return this.Execute(template, context, timeout, null, null, skipGui);
		}

		public IZeusContext ExecuteAndCollect(ZeusTemplate template, IZeusContext context, int timeout, InputItemCollection inputitems) 
		{
			return this.Execute(template, context, timeout, inputitems, null, false);
		}

		public bool Collect(ZeusTemplate template, IZeusContext context, int timeout, InputItemCollection inputitems) 
		{
			return this.Collect(template, context, timeout, inputitems, null);
		}

		public bool Collect(ZeusTemplate template, IZeusContext context, int timeout, InputItemCollection collectedinput, ShowGUIEventHandler eventhandler) 
		{
			IZeusExecutionHelper execHelper = null;
			bool exceptionOccurred = false;
			bool result = false;
			try 
			{
				//Initialize Context for collection 
				collectedinput.CopyTo(context.Gui.Defaults);
				context.Gui.ForceDisplay = true;
				collectedinput.Clear();

				execHelper = template.GuiSegment.ZeusScriptingEngine.ExecutionHelper;
				execHelper.Timeout = timeout;

				if (eventhandler == null) 
				{
					execHelper.SetShowGuiHandler(new ShowGUIEventHandler(DynamicGUI_Display));
				}
				else 
				{
					execHelper.SetShowGuiHandler(new ShowGUIEventHandler(eventhandler));
				}

				result = template.GuiSegment.Execute(context); 
				execHelper.Cleanup();

				if (collectedinput != null)
				{
					collectedinput.Add(context.Input);
				}
			}
			catch (Exception ex)
			{
				context.Log.Write(ex);
				exceptionOccurred = true;
			}

			if (!exceptionOccurred && result)
			{
				context.Log.Write("Successfully collected input for Template: " + template.Title);
			}
			else 
			{
				context.Log.Write("Canceled Template execution: " + template.Title);
			}

			return result;
		}

		public IZeusContext Execute(ZeusTemplate template, IZeusContext context, int timeout, InputItemCollection collectedinput, ShowGUIEventHandler eventhandler, bool skipGui) 
		{
			IZeusExecutionHelper execHelper = null;
			bool exceptionOccurred = false;
			bool result = false;
			try 
			{
				if (skipGui)
				{
					PopulateContextObjects(context);
					foreach (IZeusContextProcessor processor in ZeusFactory.Preprocessors) 
					{
						processor.Process(context);
					}
					result = true;
				}
				else 
				{
					execHelper = template.GuiSegment.ZeusScriptingEngine.ExecutionHelper;
					execHelper.Timeout = timeout;

					if (eventhandler == null) 
					{
						execHelper.SetShowGuiHandler(new ShowGUIEventHandler(DynamicGUI_Display));
					}
					else 
					{
						execHelper.SetShowGuiHandler(new ShowGUIEventHandler(eventhandler));
					}

					result = template.GuiSegment.Execute(context); 
					execHelper.Cleanup();

					if (collectedinput != null)
					{
						collectedinput.Add(context.Input);
					}
				}
				if (result) 
				{
					execHelper = template.BodySegment.ZeusScriptingEngine.ExecutionHelper;
					execHelper.Timeout = timeout;
					result = template.BodySegment.Execute(context);
				}
			}
			catch (Exception ex)
			{
				context.Log.Write(ex);
				exceptionOccurred = true;
			}

			if (!exceptionOccurred && result)
			{
				context.Log.Write("Successfully rendered Template: " + template.Title);
			}
			else 
			{
				context.Log.Write("Canceled Template execution: " + template.Title);
			}

			return context;
		}
	
		public void DynamicGUI_Display(IZeusGuiControl gui, IZeusFunctionExecutioner executioner) 
		{
			try 
			{
				DynamicForm df = new DynamicForm(gui as GuiController, executioner);
				DialogResult result = df.ShowDialog();
			
				if(result == DialogResult.Cancel) 
				{
					gui.IsCanceled = true;
				}
			}
			catch (Exception ex)
			{
				_log.Write(ex);
			}
		}

		#region Static Methods
        static internal bool ExecuteCodeSegment(IZeusCodeSegment segment, IZeusContext context)
        {
            bool returnValue = true;

            if (context == null) context = new ZeusContext();
            PopulateContextObjects(context as ZeusContext);

            //Push this template onto the template stack
            if (context is ZeusContext)
            {
                ((ZeusContext)context).TemplateStack.Push(segment.ITemplate);
            }

            if (segment.SegmentType == ZeusConstants.CodeSegmentTypes.GUI_SEGMENT)
            {
                foreach (IZeusContextProcessor processor in ZeusFactory.Preprocessors)
                {
                    processor.Process(context);
                }

                returnValue = ZeusExecutioner.ExecuteGuiCode(segment, context);
            }
            else
            {
                ZeusExecutioner.ExecuteCode(segment, context);
            }

            //Pop the template from the template stack
            if (context is ZeusContext)
            {
                ((ZeusContext)context).TemplateStack.Pop();
            }
            return returnValue;
        }

		static protected bool ExecuteGuiCode(IZeusCodeSegment segment, IZeusContext context) 
		{
			IZeusExecutionHelper helper = segment.ZeusScriptingEngine.ExecutionHelper;

			// If the template has an interface block, execute it
			if (!segment.IsEmpty)
			{
				ArrayList reqVars = segment.ITemplate.RequiredInputVariables;

				if (!context.Input.Contains(reqVars)) 
				{
                    try
                    {
                        helper.EngineExecuteGuiCode(segment, context);

                        if (helper.HasErrors)
                        {
                            IZeusExecutionError[] errors = helper.Errors;
                            helper.ClearErrors();
                            throw new ZeusExecutionException(segment.ITemplate, errors, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ZeusRuntimeException(segment.ITemplate, ex, false);
                    }
				}

				context.Input.AddItems(context.Gui);
			}

			return !context.Gui.IsCanceled;
		}

		static protected void ExecuteCode(IZeusCodeSegment segment, IZeusContext context) 
		{
			IZeusExecutionHelper helper = segment.ZeusScriptingEngine.ExecutionHelper;
			ExecuteCode(helper, segment.ITemplate, context, new ArrayList());
		}
		
		static protected void ExecuteCode(IZeusExecutionHelper helper, IZeusTemplate template, IZeusContext context, ArrayList templateGroupIds) 
		{
            if (!template.BodySegment.IsEmpty)
            {
                try
                {
                    // Execute Template Body
                    helper.EngineExecuteCode(template.BodySegment, context);

                    if (helper.HasErrors)
                    {
                        IZeusExecutionError[] errors = helper.Errors;
                        helper.ClearErrors();
                        throw new ZeusExecutionException(template, errors, true);
                    }
                }
                catch (Exception ex)
                {
                    throw new ZeusRuntimeException(template, ex, true);
                }
            }
			
			if (template.Type == ZeusConstants.Types.GROUP) 
			{
				if (template.IncludedTemplates.Count > 0) 
				{
					// Execute Template Body
					if (templateGroupIds.Contains(template.UniqueID)) return;

					templateGroupIds.Add(template.UniqueID);

					foreach (ZeusTemplate childTemplate in template.IncludedTemplates)
					{
						if (childTemplate.UniqueID != template.UniqueID) 
						{
							//clear the output buffer before executing the next template!
							context.Output.clear();

							//Push the current template onto the Execution stack
							if (context is ZeusContext)
							{
								((ZeusContext)context).TemplateStack.Push(childTemplate);
							}
							
							try 
							{
								IZeusScriptingEngine engine = ZeusFactory.GetEngine(childTemplate.BodySegment.Engine);
								ExecuteCode(engine.ExecutionHelper, childTemplate, context, templateGroupIds);
							}
							finally 
							{
								//Pop the current template off of the Execution stack
								if (context is ZeusContext)
								{
									((ZeusContext)context).TemplateStack.Pop();
								}
							}
						}
					}
				}
			}
		}
		#endregion

		#region Populate Context with Intrinsic Objects
        protected static void PopulateContextObjects(IZeusContext icontext)
        {
            ZeusContext context = icontext as ZeusContext;
            if (icontext != null)
            {
                ZeusConfig config = ZeusConfig.Current;
                context.SetIntrinsicObjects(ZeusFactory.IntrinsicObjectsArray);

                foreach (ZeusIntrinsicObject obj in config.IntrinsicObjects)
                {
                    Type intrinsicObjectType = null;
                    Assembly newassembly = null;
                    object[] objs = null;

                    if (!context.Objects.Contains(obj.VariableName) && !obj.Disabled)
                    {
                        newassembly = obj.Assembly;
                        // First thing, try to create the type without knowing the assembly.
                        /*try
                        {
                            intrinsicObjectType = Type.GetType(obj.ClassPath);
                        }
                        catch
                        {
                            intrinsicObjectType = null;
                        }*/

                        if (intrinsicObjectType == null)
                        {
                            try
                            {
                                if (obj.AssemblyPath != string.Empty)
                                {
                                    string assemblyPath = obj.AssemblyPath;

                                    if (newassembly == null)
                                    {
                                        assemblyPath = FileTools.ResolvePath(assemblyPath, true);
                                        FileInfo finf = new FileInfo(assemblyPath);
                                        FileInfo callingfinf = new FileInfo(Assembly.GetCallingAssembly().Location);
                                        if (callingfinf.FullName == finf.FullName)
                                        {
                                            newassembly = Assembly.GetCallingAssembly();
                                        }
                                    }

                                    if (newassembly == null)
                                    {
                                        throw new ZeusDynamicException(ZeusDynamicExceptionType.IntrinsicObjectPluginInvalid, "Invalid Assembly: " + assemblyPath);
                                    }
                                    else
                                    {
                                        intrinsicObjectType = newassembly.GetType(obj.ClassPath);
                                    }
                                }
                                else
                                {
                                    intrinsicObjectType = Type.GetType(obj.ClassPath);
                                }
                            }
                            catch (ZeusDynamicException zex)
                            {
                                context.Objects[obj.VariableName] = zex.Message;
                            }
                        }

                        if (intrinsicObjectType != null)
                        {
                            try
                            {
                                if (intrinsicObjectType != null)
                                {
                                    if (newassembly != null)
                                    {
                                        objs = DynamicAssemblyTools.InstantiateClassesByType(newassembly, newassembly.GetType(obj.ClassPath));
                                    }
                                    else
                                    {
                                        objs = new object[1];
                                        objs[0] = DynamicAssemblyTools.InstantiateClassByType(intrinsicObjectType);
                                    }
                                }
                                else
                                {
                                    throw new ZeusDynamicException(ZeusDynamicExceptionType.IntrinsicObjectPluginInvalid, "Invalid Type: " + obj.ClassPath);
                                }

                                if (objs != null)
                                {
                                    if (objs.Length > 0)
                                    {
                                        context.Objects[obj.VariableName] = objs[0];
                                    }
                                }
                            }
                            catch (ZeusDynamicException zex)
                            {
                                context.Objects[obj.VariableName] = zex.Message;
                            }
                        }
                    }
                }
            }
        }
		#endregion
	}
}
