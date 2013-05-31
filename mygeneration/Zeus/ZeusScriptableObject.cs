using System;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusScriptableObject.
	/// </summary>
	public class ZeusScriptableObject
	{
		public string VariableName;
		public string ClassPath;
		public string AssemblyPath;
		public bool InstantiateObject = true;
		protected string _namespace = null;
		protected string _dllref = null;

		internal ZeusScriptableObject(string line) 
		{
			string[] items = line.Split(',');
			if (items.Length == 2) 
			{
				this.AssemblyPath = string.Empty;
				this.ClassPath = items[0];
				this.VariableName = items[1];
				this.InstantiateObject = true;
			}
			else if (items.Length == 3) 
			{
				this.AssemblyPath = items[0];
				this.ClassPath = items[1];
				this.VariableName = items[2];
				this.InstantiateObject = true;
			}
			else 
			{
				throw new Exception("Invalid entry in ZeusScriptingObjects.zcfg");
			}
		}

		public ZeusScriptableObject(string assemblyPath, string classPath, string variableName, bool intantiateObject) 
		{
			this.AssemblyPath = assemblyPath;
			this.ClassPath = classPath;
			this.VariableName = variableName;
			this.InstantiateObject = intantiateObject;
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
	}
}
