using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace Zeus.DotNetScript
{
	/// <summary>
	/// Summary description for DotNetScriptExecutioner.
	/// </summary>
	public class DotNetScriptExecutioner : IZeusExecutionHelper
	{
		protected object _currentObject;
		protected _DotNetScriptGui _interfaceObject;
		protected _DotNetScriptTemplate _primaryObject;
		protected DotNetScriptEngine _engine;
		protected IZeusCodeSegment _codeSegment;
		protected List<DotNetScriptError> _errors;
		protected int _timeout = -1;
        protected bool _compiledInMemory = true;
        protected string _compilerVersion = null;
        protected Stack<Assembly> _assemblyStack = new Stack<Assembly>();

		protected event ShowGUIEventHandler ShowGUI;

		public DotNetScriptExecutioner(DotNetScriptEngine engine) 
		{
			this._engine = engine;
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

		public void ExecuteFunction(string functionName, params object[] parameters)
		{
			if (this._currentObject != null) 
			{
				RunMethod(this._currentObject, functionName, parameters);
			}
		}

		public void EngineExecuteGuiCode(IZeusCodeSegment segment, IZeusContext context)
		{
			bool assemblyLoaded = false;
			try 
			{
				this.Cleanup();
				this._codeSegment = segment;
				
				assemblyLoaded = this.LoadAssembly(context);
				if (assemblyLoaded && !HasErrors) 
				{
					this._currentObject = InstantiateClass( CurrentAssembly, typeof(_DotNetScriptGui), context );

					if (this._currentObject is _DotNetScriptGui)
					{
						this._interfaceObject = this._currentObject as _DotNetScriptGui;
						this._interfaceObject.Setup();
					}
				
					if (context.Gui.ShowGui || context.Gui.ForceDisplay)
					{
						OnShowGUI(context.Gui);
					}
					this._assemblyStack.Pop();
					assemblyLoaded = false;
				}
			}
			catch (Exception ex)
			{
				this.Cleanup( assemblyLoaded );

				throw ex;
			}
		}

		public void EngineExecuteCode(IZeusCodeSegment segment, IZeusContext context)
		{
			bool assemblyLoaded = false;
			try 
			{
				this.Cleanup();
				this._codeSegment = segment;
			
				assemblyLoaded = this.LoadAssembly(context);
				if (assemblyLoaded && !HasErrors) 
				{
					this._currentObject = InstantiateClass( CurrentAssembly, typeof(_DotNetScriptTemplate), context );

					if (this._currentObject is _DotNetScriptTemplate)
					{
						this._primaryObject = this._currentObject as _DotNetScriptTemplate;
						this._primaryObject.Render();
					}
					this._assemblyStack.Pop();
					assemblyLoaded = false;
				}
			}
			catch (Exception ex)
			{
				this.Cleanup( assemblyLoaded );
				throw ex;
			}
        }

        protected string CompilerVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_compilerVersion))
                    return "v3.5";
                else
                    return _compilerVersion;
            }
            set
            {
                if ((value == "v2.0") ||
                    (value == "v3.0") ||
                    (value == "v3.5"))
                {
                    _compilerVersion = value;
                }
            }
        }

		public Assembly CurrentAssembly 
		{
			get 
			{
				return this._assemblyStack.Peek() as Assembly;
			}
		}

		protected void Cleanup(bool newAssemblyCreated) 
		{
			_primaryObject = null;
			_interfaceObject = null;
			_currentObject = null;
			
			if (newAssemblyCreated)
			{
				this._assemblyStack.Pop();
			}
		}

		public void Cleanup() 
		{
			Cleanup(false);
		}
		
		public void ClearErrors() 
		{
			this._errors = null;
		}

		public bool HasErrors
		{
			get { return (_errors != null); }
		}

		public bool CompiledInMemory
		{
			get { return _compiledInMemory; }
			set { _compiledInMemory = value; }
		}

		public IZeusExecutionError[] Errors
		{
			get 
			{ 
				if (_errors != null)
					return (IZeusExecutionError[])_errors.ToArray(); 
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

		protected bool LoadAssembly(IZeusContext context)
		{
			bool assemblyLoaded = false;
			bool cacheAssembly = (_codeSegment.ITemplate.SourceType == ZeusConstants.SourceTypes.COMPILED);
			Assembly newAssembly = null;
            List<DotNetScriptError> errors = null;

			if ((cacheAssembly) && (_codeSegment.CachedAssembly != null))
			{
				newAssembly = this._codeSegment.CachedAssembly;
			}
			else 
			{
				this._compiledInMemory = true;

                List<string> references = new List<string>();
                List<string> namespaces = new List<string>();

                CompilerVersion = Zeus.Configuration.ZeusConfig.Current.CompilerVersion;

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
                        if ((array.Length == 2) && (array[0] == DotNetScriptEngine.DEBUG))
                        {
                            this._compiledInMemory = false;
                        }
                        if ((array.Length == 2) && (array[0] == DotNetScriptEngine.VERSION))
                        {
                            this.CompilerVersion = array[1];
                        }
					}
				}
				foreach (string reference in _engine.BuildDLLNames(context)) 
				{
					if (!references.Contains(reference)) references.Add(reference);
				}

				if (cacheAssembly) 
				{
					_compiledInMemory = false;
				}

				DotNetLanguage lang = (_codeSegment.Language == ZeusConstants.Languages.CSHARP) ? DotNetLanguage.CSharp : DotNetLanguage.VBNet;
                newAssembly = CreateAssembly(_codeSegment.Code, CompilerVersion, lang, _compiledInMemory, references, out errors, context);

				if (cacheAssembly && (newAssembly != null))
				{
					_codeSegment.CachedAssembly = newAssembly;
				}
			}

			if (errors != null) 
			{
				foreach (DotNetScriptError error in errors) this.AddError(error);
			}
			else 
			{
				this._assemblyStack.Push(newAssembly);
				assemblyLoaded = true;
			}

			return assemblyLoaded;
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
					throw new Exception("Entry creating \"" + matchingType.Name + "\" was either not found on any classes or had an invalid signature.");
				}
			}

			return obj;
		}

		protected void AddError(DotNetScriptError error) 
		{
            if (_errors == null) _errors = new List<DotNetScriptError>();
 
			this._errors.Add(error);
		}

		#region Methods For Dynamic Loading
        public static Assembly CreateAssembly(string code, string compilerVersion, DotNetLanguage language, bool compileInMemory, List<string> references, out List<DotNetScriptError> errors, IZeusContext context)
		{
			string tmpDirectory = System.Environment.CurrentDirectory;
			System.Environment.CurrentDirectory = RootFolder;

			Assembly generatedAssembly = null;
			string ext = ".cs", lang = "C#";
			errors = null;

			//Create an instance whichever code provider that is needed
			CodeDomProvider codeProvider = null;

            // string extraParams;
            if (language == DotNetLanguage.VBNet)
            {
                lang = "VB";
                ext = ".vb";
                codeProvider = new VBCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", compilerVersion } });
            }
            else
            {
                codeProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", compilerVersion } });
            }

            //add compiler parameters
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.CompilerOptions = "/target:library /optimize"; // + extraParams;
            CompilerResults results = null; 

			// Add References
			if (!references.Contains("mscorlib.dll")) references.Add("mscorlib.dll");
			if (!references.Contains("System.dll")) references.Add("System.dll");
			if (!references.Contains("System.EnterpriseServices.dll")) references.Add("System.EnterpriseServices.dll");
			if (!references.Contains("Zeus.dll")) references.Add("Zeus.dll");
            if (!references.Contains("PluginInterfaces.dll")) references.Add("PluginInterfaces.dll");

            foreach (string reference in Zeus.Configuration.ZeusConfig.Current.TemplateReferences)
            {
                if (!references.Contains(reference)) references.Add(reference);
            }

			foreach (string reference in references) 
			{
				compilerParams.ReferencedAssemblies.Add(reference);
			}

			if (compileInMemory) 
			{
				compilerParams.GenerateExecutable = false;
				compilerParams.IncludeDebugInformation = false;
				compilerParams.GenerateInMemory = true;

                results = codeProvider.CompileAssemblyFromSource(compilerParams, code);
			}
			else 
			{
				string guid = Guid.NewGuid().ToString();
				string assemblyname = DotNetScriptEngine.DEBUG_FILE_PREFIX + guid + ".dll";;
				string codefilename = DotNetScriptEngine.DEBUG_FILE_PREFIX + guid + ext;
				
				StreamWriter writer = File.CreateText(codefilename);
				writer.Write(code);
				writer.Flush();
				writer.Close();

				compilerParams.GenerateExecutable = false;
				compilerParams.IncludeDebugInformation = true;
				compilerParams.GenerateInMemory = false;
				compilerParams.OutputAssembly = assemblyname;

                results = codeProvider.CompileAssemblyFromFile(compilerParams, codefilename);
			}

			//Do we have any compiler errors
			if (results.Errors.HasErrors)
			{
                errors = new List<DotNetScriptError>();
				foreach (CompilerError compileError in results.Errors) 
				{
					errors.Add(new DotNetScriptError(compileError, context));
				}
			}
			else 
			{
				//get a hold of the actual assembly that was generated
				generatedAssembly = results.CompiledAssembly;
			}

			System.Environment.CurrentDirectory = tmpDirectory;

			//return the assembly
			return generatedAssembly;
		}

		public static object RunMethod(Object obj, string methodName, object[] parameters) 
		{
			Type type = obj.GetType();
			MethodInfo methodInfo = type.GetMethod(methodName);

			if (methodInfo != null) 
			{
				return methodInfo.Invoke(obj, parameters);
			}
			else 
			{
				//TODO: handle error with correct Exception Type or just log the error
				return null;
			}
		}

		private static string _rootFolder = null;
		public static string RootFolder 
		{
			get 
			{
				if (_rootFolder == null) 
				{
					_rootFolder = Assembly.GetEntryAssembly().Location;
					_rootFolder = _rootFolder.Substring(0, _rootFolder.LastIndexOf(@"\"));
				}

				return _rootFolder;
			}
		}
		#endregion
	}
}
