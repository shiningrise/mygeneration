using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Reflection;

//using Zeus.DotNetScript;
using Zeus.ErrorHandling;
using Zeus.Configuration;
using TypeSerializer.MyMeta;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusFactory.
	/// </summary>
	public class ZeusFactory
	{
		private static Hashtable _engines;
		private static ArrayList _preprocessors;
		private static ArrayList _serializers;


		public static ArrayList Preprocessors
		{
			get 
			{
				if (_preprocessors == null) 
				{
					_preprocessors = new ArrayList();
                    _preprocessors.Add(new DefaultContextProcessor());

					ZeusConfig config = ZeusConfig.Current;
					Assembly assembly;
					object[] objects;

					foreach (string path in config.Preprocessors) 
					{
                        if (path.EndsWith("\\ContextProcessor.dll"))
                        {
                            continue;
                        }
                        else
                        {
                            assembly = Assembly.LoadFile(FileTools.ResolvePath(path, true));

                            if (assembly != null)
                            {
                                objects = DynamicAssemblyTools.InstantiateClassesByType(assembly, typeof(IZeusContextProcessor));
                                if (objects.Length > 0)
                                {
                                    _preprocessors.AddRange(objects);
                                }
                            }
                        }
					}	
				}
				return _preprocessors;
			}
		}

		public static ArrayList Serializers
		{
			get 
			{
				if (_serializers == null) 
				{
                    _serializers = new ArrayList();
                    _serializers.Add(new MyMetaSerializer());
                    _serializers.Add(new IDatabaseSerializer());
                    _serializers.Add(new ITableSerializer());
                    _serializers.Add(new IViewSerializer());
                    _serializers.Add(new IProcedureSerializer());
                    _serializers.Add(new IColumnSerializer());

					ZeusConfig config = ZeusConfig.Current;
					Assembly assembly;
					object[] objects;

					foreach (string path in config.Serializers) 
					{
                        if (path.EndsWith("\\TypeSerializer.dll"))
                        {
                            continue;
                        }
                        else
                        {
                            assembly = Assembly.LoadFile(FileTools.ResolvePath(path, true));

                            if (assembly != null)
                            {
                                objects = DynamicAssemblyTools.InstantiateClassesByType(assembly, typeof(ITypeSerializer));
                                if (objects.Length > 0)
                                {
                                    _serializers.AddRange(objects);
                                }
                            }
                        }
					}	
				}
				return _serializers;
			}
		}

		public static IZeusScriptingEngine GetEngine(string name)
		{
			if (_engines == null) LoadEngines();
			
			if (_engines.ContainsKey(name))
			{
				return _engines[name] as IZeusScriptingEngine;
			}
			else 
			{
				throw new ZeusDynamicException(ZeusDynamicExceptionType.ScriptingEnginePluginInvalid, "Scripting Engine \"" + name + "\" not loaded.");
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

		public static string[] TemplateModes 
		{
			get 
			{
				return new string[] {
										ZeusConstants.Modes.MARKUP,
										ZeusConstants.Modes.PURE
									};
			}
		}

		public static string[] TemplateTypes
		{
			get 
			{
				return new string[] {
										ZeusConstants.Types.GROUP,
										ZeusConstants.Types.TEMPLATE
									};
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

		public static IZeusIntrinsicObject[] IntrinsicObjectsArray
		{
			get 
			{
				return (IZeusIntrinsicObject[])ZeusConfig.Current.IntrinsicObjects.ToArray();
			}
		}

		private static void LoadEngines() 
		{
			ZeusConfig config = ZeusConfig.Current;
			IZeusScriptingEngine engine;
			_engines = new Hashtable();

            engine = new DotNetScript.DotNetScriptEngine();
            _engines.Add(engine.EngineName, engine);

            engine = new MicrosoftScript.MicrosoftScriptEngine();
            _engines.Add(engine.EngineName, engine);

			string path;
			Assembly assembly;
			foreach (string unparsedpath in config.ScriptingEngines) 
			{
                if (unparsedpath.EndsWith("MicrosoftScriptingEngine.dll", StringComparison.CurrentCultureIgnoreCase) ||
                    unparsedpath.EndsWith("DotNetScriptingEngine.dll", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                else
                {
                    path = FileTools.ResolvePath(unparsedpath, true);
                    assembly = DynamicAssemblyTools.LoadDynamicAssembly(path);

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
                    else
                    {
                        throw new ZeusDynamicException(ZeusDynamicExceptionType.ScriptingEnginePluginInvalid, path);
                    }

                    assembly = null;
                }
			}
		}
	}
}
