using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

namespace Zeus.Configuration
{
	/// <summary>
	/// Summary description for ZeusConfig.
	/// </summary>
	public class ZeusConfig
	{
		private const string SETTINGS_FOLDER = @"\Settings";

		private const string CONFIG_LOCATION = SETTINGS_FOLDER + @"\ZeusConfig.xml";
		private const string CONFIG_RESOURCE_PATH = "Zeus.Configuration.DefaultZeusConfig.xml";

        private string _compilerVersion = null;
		private string _filename = null;
		private string _webupdateurl = "http://www.mygenerationsoftware.com/webupdate/";
        private List<string> _serializers = null;
        private List<string> _preprocessors = null;
        private List<string> _projectRoots = null;
        private List<string> _templateRoots = null;
        private List<string> _scriptingEngines = null;
        private List<string> _templateNamespaces = null;
        private List<string> _templateReferences = null;
        private List<ZeusIntrinsicObject> _intrinsicObjects = null;

		#region Static Accessor for config File!
		private static ZeusConfig _zeusConfig;
		public static ZeusConfig Current 
		{
			get 
			{
				if (_zeusConfig == null) 
				{
					_zeusConfig = new ZeusConfig();
				}
				return _zeusConfig;
			}
		}

		public static void Refresh()
		{
			_zeusConfig = null;
		}
		#endregion

		private ZeusConfig()
		{
            bool dirExists = Directory.Exists(FileTools.ApplicationPath + SETTINGS_FOLDER);

			// Create the settings directory if it doesn't exist already
            if (!dirExists) Directory.CreateDirectory(FileTools.ApplicationPath + SETTINGS_FOLDER);

			XmlDocument xmldoc;

            _filename = FileTools.ApplicationPath + CONFIG_LOCATION;
			xmldoc = LoadFromFile(_filename);

			if (xmldoc == null) 
			{
                _filename = FileTools.ApplicationPath + CONFIG_LOCATION;
				xmldoc = LoadFromResource();
			}

			try 
			{
				PopulateObject(xmldoc);
				xmldoc = null;
			}
			catch 
			{
				xmldoc = LoadFromResource();
				PopulateObject(xmldoc);
			}

			if (!File.Exists(_filename)) 
			{
				CopyResourceToFile( CONFIG_RESOURCE_PATH, _filename );
			}
		}

		public void Save() 
		{
			if (File.Exists(this._filename)) 
			{
				FileAttributes fa = File.GetAttributes(this._filename);

				if ((FileAttributes.ReadOnly & fa) == FileAttributes.ReadOnly) 
					throw new Exception(this._filename + " is read only!");
			}
			StreamWriter sw = new StreamWriter(this._filename, false);
			XmlTextWriter xml = new XmlTextWriter(sw.BaseStream as Stream, Encoding.Unicode);
			xml.Formatting = Formatting.Indented;
			xml.WriteStartDocument();

			this.BuildXML(xml);

			xml.WriteEndDocument();

			xml.Flush();
			xml.Close();
		}

		public string WebUpdateUrl 
		{
			get 
			{
				return _webupdateurl;
			}
		}

		public string CompilerVersion 
		{
			get 
			{
                if (string.IsNullOrEmpty(_compilerVersion))
                    return "v4.0";
                else 
				    return _compilerVersion;
			}
            set 
            {
                if ((value == "v2.0") ||
                    (value == "v3.0") ||
                    (value == "v3.5") ||
                    (value == "v4.0")) 
                {
                    _compilerVersion = value;
                }
            }
		}

        public List<string> Preprocessors 
		{
			get 
			{
				if (_preprocessors == null) 
				{
                    _preprocessors = new List<string>();
				}
				return _preprocessors;
			}
		}

        public List<string> ProjectRoots 
		{
			get 
			{
				if (_projectRoots == null) 
				{
                    _projectRoots = new List<string>();
				}
				return _projectRoots;
			}
		}

        public List<string> TemplateRoots 
		{
			get 
			{
				if (_templateRoots == null) 
				{
                    _templateRoots = new List<string>();
				}
				return _templateRoots;
			}
		}

        public List<string> TemplateReferences
		{
			get 
			{
                if (_templateReferences == null) 
				{
                    _templateReferences = new List<string>();
				}
                return _templateReferences;
			}
		}
        
        public List<string> TemplateNamespaces
		{
			get 
			{
				if (_templateNamespaces == null) 
				{
                    _templateNamespaces = new List<string>();
				}
				return _templateNamespaces;
			}
		}

        public List<string> ScriptingEngines 
		{
			get 
			{
				if (_scriptingEngines == null) 
				{
                    _scriptingEngines = new List<string>();
				}
				return _scriptingEngines;
			}
		}

        public List<string> Serializers 
		{
			get 
			{
				if (_serializers == null) 
				{
                    _serializers = new List<string>();
				}
				return _serializers;
			}
		}

        public List<ZeusIntrinsicObject> IntrinsicObjects 
		{
			get 
			{
				if (_intrinsicObjects == null) 
				{
                    _intrinsicObjects = new List<ZeusIntrinsicObject>();
				}
				return _intrinsicObjects;
			}
		}

		#region Parse Xml File
		protected void PopulateObject(XmlDocument xmldoc) 
		{
			CultureInfo info = CultureInfo.CreateSpecificCulture("en-US");
			XmlAttribute attr;
			string parentname, configname, path, classpath, varname;

			foreach (XmlNode parentnode in xmldoc.ChildNodes) 
			{
				parentname = parentnode.Name.ToLower(info);
				if (parentname == "configuration") 
				{
					foreach (XmlNode confignode in parentnode.ChildNodes) 
					{
						configname = confignode.Name.ToLower(info);
						if (configname == "serializer") 
						{
							attr = confignode.Attributes["assembly"];
							if (attr != null) 
							{
								path = attr.Value;
								this.Serializers.Add(path);
							}
                        }
                        else if (configname == "templatereference")
                        {
                            attr = confignode.Attributes["add"];
                            if (attr != null)
                            {
                                path = attr.Value;
                                this.TemplateReferences.Add(path);
                            }
                        }
                        else if (configname == "templatenamespace")
                        {
                            attr = confignode.Attributes["add"];
                            if (attr != null)
                            {
                                path = attr.Value;
                                this.TemplateNamespaces.Add(path);
                            }
                        }
                        else if (configname == "preprocessor")
                        {
                            attr = confignode.Attributes["assembly"];
                            if (attr != null)
                            {
                                path = attr.Value;
                                this.Preprocessors.Add(path);
                            }
                        }
						else if (configname == "templateroot") 
						{
							attr = confignode.Attributes["path"];
							if (attr != null) 
							{
								path = attr.Value;
								this.TemplateRoots.Add(path);
							}
						}
						else if (configname == "projectroot") 
						{
							attr = confignode.Attributes["path"];
							if (attr != null) 
							{
								path = attr.Value;
								this.ProjectRoots.Add(path);
							}
						}
						else if (configname == "scriptingengine") 
						{
							attr = confignode.Attributes["assembly"];
							if (attr != null) 
							{
								path = attr.Value;
								this.ScriptingEngines.Add(path);
							}
						}
						else if (configname == "intrinsicobject") 
						{
							path = classpath = varname = null;

							attr = confignode.Attributes["assembly"];
							if (attr != null) 
							{
								path = attr.Value;
							}
							attr = confignode.Attributes["classpath"];
							if (attr != null) 
							{
								classpath = attr.Value;
							}
							attr = confignode.Attributes["varname"];
							if (attr != null) 
							{
								varname = attr.Value;
							}

							if (classpath != null && varname != null) 
							{
								this.IntrinsicObjects.Add(new ZeusIntrinsicObject(path, classpath, varname));
							}
                        }
                        else if (configname == "webupdate")
                        {
                            attr = confignode.Attributes["url"];
                            if (attr != null)
                            {
                                this._webupdateurl = attr.Value;
                            }
                        }
                        else if (configname == "compilerparams")
                        {
                            attr = confignode.Attributes["version"];
                            if (attr != null)
                            {
                                this.CompilerVersion = attr.Value;
                            }
                        }
					}
				}
			}
		}
		#endregion

		#region Create Xml File
		protected void BuildXML(XmlWriter xml) 
		{
			xml.WriteStartElement("Configuration");
            
			if (_webupdateurl != null) 
			{
				xml.WriteStartElement("WebUpdate");
				xml.WriteAttributeString("url", _webupdateurl);
				xml.WriteEndElement();
			}

            if (_compilerVersion != null) 
			{
				xml.WriteStartElement("CompilerParams");
				xml.WriteAttributeString("version", _compilerVersion);
				xml.WriteEndElement();
			}

			foreach (string path in this.ProjectRoots) 
			{
				xml.WriteStartElement("ProjectRoot");
				xml.WriteAttributeString("path", path);
				xml.WriteEndElement();
			}
			foreach (string path in this.TemplateRoots) 
			{
				xml.WriteStartElement("TemplateRoot");
				xml.WriteAttributeString("path", path);
				xml.WriteEndElement();
			}
			foreach (string assembly in this.Preprocessors) 
			{
				xml.WriteStartElement("Preprocessor");
				xml.WriteAttributeString("assembly", assembly);
				xml.WriteEndElement();
            }
            foreach (string assembly in this.Serializers)
            {
                xml.WriteStartElement("Serializer");
                xml.WriteAttributeString("assembly", assembly);
                xml.WriteEndElement();
            }
            foreach (string reference in this.TemplateReferences)
            {
                xml.WriteStartElement("TemplateReference");
                xml.WriteAttributeString("add", reference);
                xml.WriteEndElement();
            }
            foreach (string ns in this.TemplateNamespaces)
            {
                xml.WriteStartElement("TemplateNamespace");
                xml.WriteAttributeString("add", ns);
                xml.WriteEndElement();
            }
			foreach (string assembly in this.ScriptingEngines) 
			{
				xml.WriteStartElement("ScriptingEngine");
				xml.WriteAttributeString("assembly", assembly);
				xml.WriteEndElement();
			}
			foreach (ZeusIntrinsicObject obj in this.IntrinsicObjects) 
			{
				xml.WriteStartElement("IntrinsicObject");
				if (obj.AssemblyPath != string.Empty)
					xml.WriteAttributeString("assembly", obj.AssemblyPath);
				xml.WriteAttributeString("classpath", obj.ClassPath);
				xml.WriteAttributeString("varname", obj.VariableName);
				xml.WriteEndElement();
			}
		
			xml.WriteEndElement();
		}
		#endregion

		#region Load From File or Embedded Resource
		protected XmlDocument LoadFromFile(string filename)
		{
			XmlDocument xmldoc = null;
			if (File.Exists(filename)) 
			{
				try 
				{
					xmldoc = new XmlDocument();
					xmldoc.Load(filename);
				}
				catch 
				{
					xmldoc = null;
				}
			}
			
			return xmldoc;
		}

		protected XmlDocument LoadFromResource()
		{
			XmlDocument xmldoc = new XmlDocument();
			
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(CONFIG_RESOURCE_PATH);
			xmldoc.Load(stream);

			return xmldoc;
		}

		protected void CopyResourceToFile(string resource, string file) 
		{
			FileStream outstream = File.OpenWrite(file);
			StreamWriter sw = new StreamWriter(outstream);
				
			Stream instream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
			StreamReader sr = new StreamReader(instream);

			sw.Write(sr.ReadToEnd());
			sw.Flush();
			sw.Close();

			sw = null;
			sr = null;
		}
		#endregion
	}
}