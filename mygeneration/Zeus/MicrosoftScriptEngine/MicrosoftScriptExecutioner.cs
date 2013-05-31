using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

using Zeus;

using MSScriptControl;

namespace Zeus.MicrosoftScript
{
	/// <summary>
	/// Summary description for DotNetScriptExecutioner.
	/// </summary>
	public class MicrosoftScriptExecutioner : IZeusExecutionHelper
	{
		protected const string GUI_ENTRY_NAME = "setup";
		protected const string BODY_ENTRY_NAME = "zQh__rw_ZEUS";
		protected const string CALL_ENTRY_VBSCRIPT = "Sub " + BODY_ENTRY_NAME + "()\r\nEnd Sub";
		protected const string CALL_ENTRY_JSCRIPT = "function " + BODY_ENTRY_NAME + "() {}";

		protected MicrosoftScriptEngine _engine;
		protected ArrayList _errors;
		protected int _timeout = -1;
		protected Stack _scriptControlStack = new Stack();

		protected event ShowGUIEventHandler ShowGUI;

		public MicrosoftScriptExecutioner(MicrosoftScriptEngine engine) 
		{
			_engine = engine;
		}

		public void SetShowGuiHandler(ShowGUIEventHandler guiEventHandler)
		{
			this.ShowGUI = null;
			this.ShowGUI += guiEventHandler;
		}

		protected virtual void OnShowGUI(IZeusGuiControl gui) 
		{
			if (ShowGUI != null) 
			{
				ShowGUI(gui, this);
			}
		}

		public void Cleanup() 
		{
			MicrosoftScriptControl.Language = "JScript";
			MicrosoftScriptControl.Reset();
		}
		
		public void ExecuteFunction(string functionName, params object[] parameters)
		{
			IZeusContext context = null;
			try 
			{
				context = MicrosoftScriptControl.Eval("context") as IZeusContext;	
				object tmpReturn = MicrosoftScriptControl.Run(functionName, ref parameters);
			}
			catch (System.Runtime.InteropServices.COMException)
			{
                MicrosoftScriptError error = new MicrosoftScriptError(MicrosoftScriptControl.Error, context);
				this.AddError(error);
			}
		}

		public void EngineExecuteGuiCode(IZeusCodeSegment segment, IZeusContext context)
		{
			// If the template has an interface block, execute it
			if (!segment.IsEmpty) 
			{
				if (!HasErrors) 
				{
                    int step = 0;
                    try
                    {
                        MicrosoftScriptControl.Timeout = (_timeout == -1 ? MSScriptControl.ScriptControlConstants.NoTimeout : (_timeout * 1000));
                        MicrosoftScriptControl.Language = segment.Language;
                        MicrosoftScriptControl.AllowUI = true;
                        MicrosoftScriptControl.UseSafeSubset = false;
                        MicrosoftScriptControl.Reset();
                        MicrosoftScriptControl.AddObject("input", context.Input, true);
                        MicrosoftScriptControl.AddObject("context", context, true);

                        foreach (string key in context.Objects.Keys)
                        {
                            if (key != "input")
                                MicrosoftScriptControl.AddObject(key, context.Objects[key], true);
                        }

                        step = 1;

                        MicrosoftScriptControl.AddCode(segment.Code);
                        
                        step = 2;

                        object[] paramList = new object[0];

                        object tmpReturn = MicrosoftScriptControl.Run(GUI_ENTRY_NAME, ref paramList);

                        if (context.Gui.ShowGui || context.Gui.ForceDisplay)
                        {
                            OnShowGUI(context.Gui);
                        }
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        MicrosoftScriptError error = new MicrosoftScriptError(MicrosoftScriptControl.Error, context);
                        if (step < 2) error.IsRuntime = false;
                        this.AddError(error);
                    }
                    catch (Exception ex)
                    {
                        if ((MicrosoftScriptControl.Error == null) || (MicrosoftScriptControl.Error.Description == null))
                        {
                            throw ex;
                        }
                        else
                        {
                            MicrosoftScriptError error = new MicrosoftScriptError(MicrosoftScriptControl.Error, context);
                            if (step < 2) error.IsRuntime = false;
                            this.AddError(error);
                        }
                    }

					MicrosoftScriptControl.Reset();
				}
			}
		}

		public void EngineExecuteCode(IZeusCodeSegment segment, IZeusContext context)
		{
			// If the segment isn't empty, execute it
			if (!segment.IsEmpty) 
			{
				if (!HasErrors)
                {
                    int step = 0;
					try 
					{
						if (context.ExecutionDepth > 1) 
						{
								ScriptStack.Push( new MSScriptControl.ScriptControl() );
						}

						MicrosoftScriptControl.Timeout = (_timeout == -1 ? MSScriptControl.ScriptControlConstants.NoTimeout : (_timeout * 1000) );
						MicrosoftScriptControl.Language = segment.Language;
						MicrosoftScriptControl.AllowUI = true;
						MicrosoftScriptControl.UseSafeSubset = false;

						MicrosoftScriptControl.AddObject("output", context.Output, true);
						MicrosoftScriptControl.AddObject("input", context.Input, true);
						MicrosoftScriptControl.AddObject("context", context, true);
			
						foreach (string key in context.Objects.Keys) 
						{
							if ((key != "output") && (key != "input"))
								MicrosoftScriptControl.AddObject(key, context.Objects[key], true);
						}

						string entryCode = this.ScriptingEntryCall(segment.Language) as String;
						string entryCall = BODY_ENTRY_NAME;

                        step = 1;
						MicrosoftScriptControl.AddCode(segment.Code);
						MicrosoftScriptControl.AddCode(entryCode);
                        step = 2;

						object[] paramList = new object[0];
			
						object tmpReturn = MicrosoftScriptControl.Run(entryCall, ref paramList);
					}
					catch (System.Runtime.InteropServices.COMException)
					{
						MicrosoftScriptError error = new MicrosoftScriptError(MicrosoftScriptControl.Error, context);
                        if (step < 2) error.IsRuntime = false;
						this.AddError(error);
					}
                    catch (Exception ex)
                    {
                        if ((MicrosoftScriptControl.Error == null) || (MicrosoftScriptControl.Error.Description == null))
                        {
                            throw ex;
                        }
                        else
                        {
                            MicrosoftScriptError error = new MicrosoftScriptError(MicrosoftScriptControl.Error, context);
                            if (step < 2) error.IsRuntime = false;
                            this.AddError(error);
                        }
                    }
					finally 
					{
						MicrosoftScriptControl.Reset();
						if (context.ExecutionDepth > 1) ScriptStack.Pop();
					}
				}
			}
		}

		public void ClearErrors() 
		{
			this._errors = null;
		}

		public bool HasErrors
		{
			get { return (_errors != null); }
		}

		public IZeusExecutionError[] Errors
		{
			get 
			{ 
				if (_errors != null)
					return (IZeusExecutionError[])_errors.ToArray(typeof(IZeusExecutionError)); 
				else
					return null;
			}
		}

		/// <summary>
		/// -1 means no timeout, otherwise we set the timeout = to this number.
		/// </summary>
		public int Timeout 
		{
			get 
			{
				return _timeout;
			}
			set 
			{
				_timeout = value;
			}
		}

        protected MSScriptControl.IScriptControl MicrosoftScriptControl 
		{
			get 
			{
				if (ScriptStack.Count == 0) 
				{
					ScriptControlClass scriptControl = new ScriptControlClass();
					//HACK_BEGIN
					ScriptStack.Clear();
					ScriptStack.Push(scriptControl);
					//HACK_END
				}
				//HACK_BEGIN
				return ScriptStack.Peek() as MSScriptControl.ScriptControl;
				//return _scriptControl;
				//HACK_END
			}
		}

		//HACK_BEGIN
		protected Stack ScriptStack
		{
			get 
			{
				return _scriptControlStack;
			}
		}
		//HACK_END

		protected string ScriptingEntryCall(string language)
		{
			string tmp = System.Environment.NewLine;

			if (language.ToLower() == ZeusConstants.Languages.JSCRIPT.ToLower()) 
			{
				tmp += CALL_ENTRY_JSCRIPT;
			}
			else
			{
				tmp += CALL_ENTRY_VBSCRIPT;
			}

			return tmp;
		}

		protected void AddError(MicrosoftScriptError error) 
		{
			if (_errors == null) _errors = new ArrayList();
 
			this._errors.Add(error);
		}
	}
}
