using System;
using System.IO;
using System.Reflection;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusScriptableObject.
	/// </summary>
	public class ZeusIntrinsicObject : IZeusIntrinsicObject
	{
        protected System .Reflection.Assembly _assembly;
        protected string _variableName;
        protected string _classPath;
		protected string _assemblyPath;
		protected string _namespace = null;
        protected string _dllref = null;
        protected bool? _disabled = null;

		/*internal ZeusIntrinsicObject(string line) 
		{
			string[] items = line.Split(',');
			if (items.Length == 2) 
			{
				this._assemblyPath = string.Empty;
				this._classPath = items[0];
				this._variableName = items[1];
			}
			else if (items.Length == 3) 
			{
				this._assemblyPath = items[0];
				this._classPath = items[1];
				this._variableName = items[2];
			}
			else 
			{
				throw new Exception("Invalid entry in ZeusScriptingObjects.zcfg");
			}
		}*/

		public ZeusIntrinsicObject(string assemblyPath, string classPath, string variableName)
		{
            // Test Assembly
			if (assemblyPath == null) assemblyPath = string.Empty;
			this._assemblyPath = assemblyPath;
			this._classPath = classPath;
			this._variableName = variableName;
        }

        public bool Disabled
        {
            get 
            {
                if (!_disabled.HasValue) _disabled = !AssemblyExistsIfDefined;

                return _disabled.Value; 
            }
        }

        public System.Reflection.Assembly Assembly
        {
            get
            {
                if (!Disabled)
                {
                    return _assembly;
                }
                else
                {
                    return null;
                }
            }
        }

        public string DllReference
        {
            get
            {
                if (_dllref == null)
                {
                    _dllref = this.AssemblyPath;
                    int idx = _dllref.LastIndexOf("\\");
                    if (idx > 0) _dllref = _dllref.Substring(idx + 1);
                    if (_dllref == string.Empty) _dllref = null;
                }
                return _dllref;
            }
        }

		public string Namespace
		{
			get
			{
				if (_namespace == null)
				{
					_namespace = this.ClassPath;
					int idx = _namespace.LastIndexOf(".");
					if (idx > 0) _namespace = _namespace.Substring(0, idx);
					if (_namespace == string.Empty) _namespace = null;
				}
				return _namespace;
			}
		}

		public string VariableName
		{
			get { return _variableName; }
		}
		public string ClassPath
		{
			get { return _classPath; }
		}

		public string AssemblyPath
		{
			get { return _assemblyPath; }
		}

        private bool AssemblyExistsIfDefined
        {
            get
            {
                bool isdef = false;

                if (AssemblyPath == string.Empty) isdef = true;
                else
                {
                    string assemblyPath = FileTools.ResolvePath(this.AssemblyPath, true);
                    isdef = System.IO.File.Exists(assemblyPath);
                    if (isdef)
                    {
                        System.Collections.Generic.List<AssemblyName> refs = new System.Collections.Generic.List<AssemblyName>();
                        refs.Add(Assembly.GetExecutingAssembly().GetName());
                        refs.AddRange(Assembly.GetExecutingAssembly().GetReferencedAssemblies());
                        foreach (AssemblyName an in refs)
                        {
                            Assembly a = System.Reflection.Assembly.Load(an);
                            string l1 = a.Location.Substring(a.Location.LastIndexOf("\\"));
                            string l2 = assemblyPath.Substring(assemblyPath.LastIndexOf("\\"));
                            if (l1.Equals(l2, StringComparison.CurrentCultureIgnoreCase))
                            {
                                _assembly = a;
                                break;
                            }
                        }

                        if (_assembly == null)
                        {
                            _assembly = DynamicAssemblyTools.LoadDynamicAssembly(assemblyPath);
                        }

                        isdef = (_assembly != null);
                    }
                }

                return isdef;
            }
        }
	}
}
