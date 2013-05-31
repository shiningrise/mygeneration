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

using Zeus.ErrorHandling;
using Zeus.UserInterface;

namespace Zeus.DotNetScript
{
	/// <summary>
	/// Summary description for DotNetScriptExecutioner.
	/// </summary>
	public class DotNetScriptExecutioner : ZeusExecutioner
	{
		protected Assembly _currentAssembly;
		protected object _currentObject;
		protected _DotNetScriptGui _interfaceObject;
		protected _DotNetScriptTemplate _primaryObject;
		protected DotNetScriptEngine _engine;

		protected ZeusCodeSegment _codeSegment;

		public DotNetScriptExecutioner(DotNetScriptEngine engine) 
		{
			this._engine = engine;
		}

		override public void ExecuteFunction(string functionName, params object[] parameters)
		{
			if (this._currentObject != null) 
			{
				DynamicAssemblyTools.RunMethod(this._currentObject, functionName, parameters);
			}
		}

		override protected void EngineExecuteGuiCode(ZeusCodeSegment segment, ZeusGuiContext context)
		{
			try 
			{
				this.Cleanup();
				this._codeSegment = segment;

				this._currentObject = InstantiateClass( this.CurrentAssembly, typeof(_DotNetScriptGui), context );

				if (this._currentObject is _DotNetScriptGui)
				{
					this._interfaceObject = this._currentObject as _DotNetScriptGui;
					this._interfaceObject.Setup();
				}
				
				if (context.Gui.ShowGui)
				{
					OnShowGUI(context.Gui);
					if (!context.Gui.IsCanceled) 
					{
						context.Input.AddItems(context.Gui);
					}
				}

			}
			catch (ZeusCompilerException ex)
			{
				ex.IsTemplateScript = false;
				throw ex;
			}
		}

		override protected void EngineExecuteCode(ZeusCodeSegment segment, ZeusTemplateContext context)
		{
			try 
			{
				this.Cleanup();
				this._codeSegment = segment;
			
				this._currentObject = InstantiateClass( this.CurrentAssembly, typeof(_DotNetScriptTemplate), context );

				if (this._currentObject is _DotNetScriptTemplate)
				{
					this._primaryObject = this._currentObject as _DotNetScriptTemplate;
					this._primaryObject.Render();
				}
			}
			catch (ZeusCompilerException ex)
			{
				ex.IsTemplateScript = true;
				throw ex;
			}
		}

		override public void Cleanup() 
		{
			this._currentAssembly = null;
			this._interfaceObject = null;
			this._primaryObject = null;
			this._currentObject = null;
		}
		
		protected Assembly CurrentAssembly
		{
			get
			{
				if (this._currentAssembly == null) 
				{
					ArrayList references = new ArrayList();
					ArrayList namespaces = new ArrayList();
					string[] array;
					foreach (object obj in _codeSegment.ExtraData) 
					{
						if (obj is String[]) 
						{
							array = (string[])obj;
							if ((array.Length == 2) && (array[0] == DotNetScriptEngine.DLLREF))
							{
								if (!references.Contains(array[1]))
									references.Add(array[1]);
							}
						}
					}
					foreach (string reference in _engine.BuildDLLNames()) 
					{
						if (!references.Contains(reference)) references.Add(reference);
					}

					DotNetLanguage lang = (_codeSegment.Language == ZeusConstants.Languages.CSHARP) ? DotNetLanguage.CSharp : DotNetLanguage.VBNet;
					this._currentAssembly = DynamicAssemblyTools.CreateAssembly(_codeSegment.Code, lang, references);
				}

				return this._currentAssembly;
			}
		}

		private object InstantiateClass(Assembly assembly, System.Type matchingType, IZeusContext context)
		{
			Object obj = null;

			//cant call the entry method if the assembly is null
			if (assembly != null)
			{
				bool classFound = false;

				//Use reflection to call the static Run function
				Module[] mods = assembly.GetModules(false);
				Type[] types = mods[0].GetTypes();

				//loop through each class that was defined and look for the first occurrance of the entry point method
				foreach (Type type in types)
				{
					if (type.IsSubclassOf(matchingType) && !type.IsAbstract) 
					{
						ConstructorInfo[] constructors = type.GetConstructors();
						ConstructorInfo constructor = constructors[0];

						obj = constructor.Invoke(BindingFlags.CreateInstance | BindingFlags.OptionalParamBinding, null, new object[] {context}, null);
						classFound = true;
						break;
					}
				}

				//if it got here, then there was no entry point method defined.  Tell user about it
				if (!classFound) 
				{
					throw new ZeusException("Entry creating \"" + matchingType.Name + "\" was either not found on any classes or had an invalid signature.");
				}
			}

			return obj;
		}
	}
}
