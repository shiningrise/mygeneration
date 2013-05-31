using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace MyGeneration.CodeSmithConversion
{
	/// <summary>
	/// Summary description for Config.
	/// </summary>
	public class Config
	{
		private const string CONFIG_LOCATION = "Config.xml";
		private const string VBMAP_LOCATION = "Default.vbmap";
		private const string CSMAP_LOCATION = "Default.csmap";
		private const string PLUGIN_LOCATION = "SamplePlugin.plugin.cs";
		private const string PLUGIN_FOLDER = @"Plugins\";
		private const string BASE_RESOURCE_PATH = "MyGeneration.CodeSmithConversion.";
		private const string CONFIG_RESOURCE_PATH = BASE_RESOURCE_PATH + CONFIG_LOCATION;
		private const string VBMAP_RESOURCE_PATH = BASE_RESOURCE_PATH + VBMAP_LOCATION;
		private const string CSMAP_RESOURCE_PATH = BASE_RESOURCE_PATH + CSMAP_LOCATION;
		private const string PLUGIN_RESOURCE_PATH = BASE_RESOURCE_PATH + PLUGIN_LOCATION;

		private string filename = null;
		private string myGenExePath = null;
		private string myGenTemplatePath = null;
		private string codeSmithAppPath = null;
		private string codeSmithTemplatePath = null;
		private bool launch = true;

		#region Static Accessor for config File!
		private static Config _config;
		public static Config Current 
		{
			get 
			{
				if (_config == null) 
				{
					_config = new Config();
				}
				return _config;
			}
		}

		public static void Refresh()
		{
			_config = null;
		}
		#endregion

		private Config()
		{
			FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
			DirectoryInfo dirInfo = fileInfo.Directory;

			filename = Path.Combine(dirInfo.FullName, CONFIG_LOCATION);
			XmlDocument xmldoc = LoadFromFile(filename);

			if (xmldoc == null) 
			{
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

			if (!File.Exists(filename)) 
			{
				CopyResourceToFile(CONFIG_RESOURCE_PATH, filename);
			}

			string tmp = Path.Combine(dirInfo.FullName, VBMAP_LOCATION);
			if (!File.Exists(tmp)) 
			{
				CopyResourceToFile(VBMAP_RESOURCE_PATH, tmp);
			}
			tmp = Path.Combine(dirInfo.FullName, CSMAP_LOCATION);
			if (!File.Exists(tmp)) 
			{
				CopyResourceToFile(CSMAP_RESOURCE_PATH, tmp);
			}
			tmp = Path.Combine(dirInfo.FullName, PLUGIN_FOLDER + PLUGIN_LOCATION);
			if (!File.Exists(tmp)) 
			{
				CopyResourceToFile(PLUGIN_RESOURCE_PATH, tmp);
			}
		}

		public void Save() 
		{
			if (File.Exists(this.filename)) 
			{
				FileAttributes fa = File.GetAttributes(this.filename);

				if ((FileAttributes.ReadOnly & fa) == FileAttributes.ReadOnly) 
					throw new Exception(this.filename + " is read only!");
			}

			StreamWriter sw = new StreamWriter(this.filename, false);
			XmlTextWriter xml = new XmlTextWriter(sw.BaseStream as Stream, Encoding.Unicode);
			xml.Formatting = Formatting.Indented;
			xml.WriteStartDocument();

			this.BuildXML(xml);

			xml.WriteEndDocument();

			xml.Flush();
			xml.Close();

			xml = null;
			sw = null;
		}

		public string CodeSmithAppPath 
		{
			get { return this.codeSmithAppPath; }
			set { this.codeSmithAppPath = value; }
		}

		public string CodeSmithTemplatePath 
		{
			get { return this.codeSmithTemplatePath; }
			set { this.codeSmithTemplatePath = value; }
		}

		public string MyGenExePath 
		{
			get { return this.myGenExePath; }
			set { this.myGenExePath = value; }
		}

		public string MyGenTemplatePath 
		{
			get { return this.myGenTemplatePath; }
			set { this.myGenTemplatePath = value; }
		}

		public bool Launch 
		{
			get { return this.launch; }
			set { this.launch = value; }
		}

		#region Parse Xml File
		protected void PopulateObject(XmlDocument xmldoc) 
		{
			CultureInfo info = CultureInfo.CreateSpecificCulture("en-US");
			XmlAttribute attr;
			string parentname, configname;

			foreach (XmlNode parentnode in xmldoc.ChildNodes) 
			{
				parentname = parentnode.Name.ToLower(info);
				if (parentname == "configuration") 
				{
					foreach (XmlNode confignode in parentnode.ChildNodes) 
					{
						configname = confignode.Name.ToLower(info);
						if (configname == "mygeneration") 
						{
							attr = confignode.Attributes["exepath"];
							if (attr != null) 
							{
								this.myGenExePath = attr.Value;
							}

							attr = confignode.Attributes["templatepath"];
							if (attr != null) 
							{
								this.myGenTemplatePath = attr.Value;
							}

							attr = confignode.Attributes["launch"];
							if (attr != null) 
							{
								try { this.launch = Convert.ToBoolean(attr.Value); } 
								catch { this.launch = true; }
							}
						}
						else if (configname == "codesmith") 
						{
							attr = confignode.Attributes["templatepath"];
							if (attr != null) 
							{
								this.codeSmithTemplatePath = attr.Value;
							}

							attr = confignode.Attributes["applicationpath"];
							if (attr != null) 
							{
								this.codeSmithAppPath = attr.Value;
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
			xml.WriteStartElement("configuration");
			
			xml.WriteStartElement("mygeneration");
			if (myGenExePath == null) myGenExePath = string.Empty;
			xml.WriteAttributeString("exepath", myGenExePath);
			if (myGenTemplatePath == null) myGenTemplatePath = string.Empty;
			xml.WriteAttributeString("templatepath", myGenTemplatePath);
			xml.WriteAttributeString("launch", launch.ToString());
			xml.WriteEndElement();

			xml.WriteStartElement("codesmith");
			if (codeSmithAppPath == null) codeSmithAppPath = string.Empty;
			xml.WriteAttributeString("applicationpath", codeSmithAppPath);
			if (codeSmithTemplatePath == null) codeSmithTemplatePath = string.Empty;
			xml.WriteAttributeString("templatepath", codeSmithTemplatePath);
			xml.WriteEndElement();

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
