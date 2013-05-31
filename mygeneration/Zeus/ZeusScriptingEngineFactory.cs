using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Reflection;

using Zeus.DotNetScript;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusScriptingEngineFactory.
	/// </summary>
	public class ZeusScriptingEngineFactory
	{
		private const string ENGINE_CONFIG_FILE = @"\Settings\ZeusScriptingEngines.zcfg";
		private const string OBJECT_CONFIG_FILE = @"\Settings\ZeusScriptingObjects.zcfg";
		private static Hashtable _engines;
		private static ArrayList _objects;	

		public static IZeusScriptingEngine GetEngine(string name)
		{
			if (_engines == null) LoadEngines();
				
			if (_engines.ContainsKey(name))
			{
				return _engines[name] as IZeusScriptingEngine;
			}
			else 
			{
				return null;
			}
		}

		public static bool IsEngineSupported(string name)
		{
			if (_engines == null) LoadEngines();

			return _engines.ContainsKey(name);
		}

		public static ICollection EngineNames 
		{
			get 
			{
				if (_engines == null) LoadEngines();

				return _engines.Keys;
			}
		}

		public static string[] AvailableLanguages 
		{
			get 
			{
				return new string[] {
										ZeusConstants.Languages.CSHARP,
										ZeusConstants.Languages.HTML,
										ZeusConstants.Languages.JAVA,
										ZeusConstants.Languages.JETSQL,
										ZeusConstants.Languages.JSCRIPT,
										ZeusConstants.Languages.JSCRIPTNET,
										ZeusConstants.Languages.JSHARP,
										ZeusConstants.Languages.NONE,
										ZeusConstants.Languages.PERL,
										ZeusConstants.Languages.PHP,
										ZeusConstants.Languages.PLSQL,
										ZeusConstants.Languages.SQL,
										ZeusConstants.Languages.TSQL,
										ZeusConstants.Languages.VB,
										ZeusConstants.Languages.VBNET,
										ZeusConstants.Languages.VBSCRIPT,
										ZeusConstants.Languages.XML
									};
			}
		}

		public static IEnumerable ScriptingObjects
		{
			get 
			{
				if (_objects == null) LoadScriptingObjects();
				
				return _objects;
			}
		}

		public static void AddScriptingObject(ZeusScriptableObject obj)
		{
				if (_objects == null) LoadScriptingObjects();
				_objects.Add(obj);
		}

		private static void LoadEngines() 
		{
			ArrayList dllNames = new ArrayList();
			IZeusScriptingEngine engine = new DotNetScriptEngine();

			//TODO:FIX THIS!!!
			string prefix = @"E:\pub\Source Code\MyGeneration\SourceControl\zeus\source\ZeusSolution\ZeusExtremeTest\bin\debug\";

			_engines = new Hashtable();
			_engines.Add(engine.EngineName, engine);

			// Load text File with DLL file names
			if (File.Exists(prefix + ENGINE_CONFIG_FILE)) 
			{
				StreamReader reader = new StreamReader(prefix + ENGINE_CONFIG_FILE, Encoding.UTF8);
				
				string line = reader.ReadLine();
				while (line != null) 
				{
					dllNames.Add(line);
					line = reader.ReadLine();
				}
				reader.Close();
				
				Assembly assembly;
				foreach (string dllPath in dllNames) 
				{
					assembly = DynamicAssemblyTools.LoadDynamicAssembly(prefix +  dllPath);

					if (assembly != null) 
					{
						object[] tmp = DynamicAssemblyTools.InstantiateClassesByType(assembly, typeof(IZeusScriptingEngine));

						for (int i = 0; i < tmp.Length; i++)
						{
							engine = tmp[i] as IZeusScriptingEngine;
						
							if (engine != null)
							{
								_engines.Add(engine.EngineName, engine);
							}
						}
					}
				}
			}
		}

		private static void LoadScriptingObjects() 
		{
			//TODO:FIX THIS!!!
			string prefix = @"E:\pub\Source Code\MyGeneration\SourceControl\zeus\source\ZeusSolution\ZeusExtremeTest\bin\debug\";

			_objects = new ArrayList();

			// Load text File with Object information
			if (File.Exists(prefix + OBJECT_CONFIG_FILE)) 
			{
				StreamReader reader = new StreamReader(prefix + OBJECT_CONFIG_FILE, Encoding.UTF8);
				
				string line = reader.ReadLine();
				while (line != null) 
				{
					_objects.Add(new ZeusScriptableObject(line));
					line = reader.ReadLine();
				}
				reader.Close();
			}
		}

	}
}
