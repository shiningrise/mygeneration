using System;
using System.Xml;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Reflection;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for DefaultSettings.
	/// </summary>
	public class DefaultSettings
	{
		private const string missing = "*&?$%";

		public DefaultSettings()
		{
			xmlDoc = new XmlDocument();

			Assembly asmblyMyGen = System.Reflection.Assembly.GetAssembly(typeof(NewAbout));
			string version = asmblyMyGen.GetName().Version.ToString();

			try
			{
				xmlDoc.Load(Application.StartupPath + @"\Settings\DefaultSettings.xml");

				if(this.Version != version)
				{
					// Our Version has changed, write any new settings and their defaults
					this.FillInMissingSettings(version);
					xmlDoc.Save(Application.StartupPath + @"\Settings\DefaultSettings.xml");
				}
			}
			catch
			{
				// Our file doesn't exist, let's create it
				StringBuilder defaultXML = new StringBuilder();
				defaultXML.Append(@"<?xml version='1.0' encoding='utf-8'?>");
				defaultXML.Append(@"<DefaultSettings Version='" + version + "' FirstTime='true'>");
				defaultXML.Append(@"</DefaultSettings>");

				xmlDoc.LoadXml(defaultXML.ToString());
				this.FillInMissingSettings(version);

				xmlDoc.Save(Application.StartupPath + @"\Settings\DefaultSettings.xml");
			}
		}

		private void FillInMissingSettings(string version)
		{
			string settingsPath = Application.StartupPath + @"\Settings\";

			// Version
			this.Version = version;

			// Connection
			if(missing == this.DbDriver)				this.DbDriver = "SQL";
			if(missing == this.ConnectionString)		this.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Data Source=localhost";

			// MyMeta
			if(missing == this.DbTargetMappingFile)		this.DbTargetMappingFile  = settingsPath + "DbTargets.xml";
			if(missing == this.LanguageMappingFile)		this.LanguageMappingFile  = settingsPath + "Languages.xml";
			if(missing == this.UserMetaDataFileName)	this.UserMetaDataFileName = settingsPath + "UserMetaData.xml";			
			if(missing == this.Language)				this.Language = "C#";
			if(missing == this.DbTarget)				this.DbTarget = "SqlClient";

			// Scripting
			if(missing == this.GetSetting("EnableLineNumbering"))	this.EnableLineNumbering = false;
			if(missing == this.GetSetting("EnableClipboard"))		this.EnableClipboard = true;
			if(missing == this.GetSetting("Tabs"))					this.Tabs = 4;
			if(missing == this.GetSetting("ScriptTimeout"))			this.ScriptTimeout = -1;
			if(missing == this.GetSetting("CheckForNewBuild"))		this.CheckForNewBuild = true;

			if(missing == this.DefaultTemplateDirectory)
			{
				string defaultTemplatePath = Application.StartupPath + @"\Templates\";
				if (!Directory.Exists(defaultTemplatePath)) 
					defaultTemplatePath = Application.StartupPath;

				this.DefaultTemplateDirectory = defaultTemplatePath;
			}

			if(missing == this.DefaultOutputDirectory)
			{
				string defaultOutputPath = Application.StartupPath + @"\GeneratedCode\";
				if (!Directory.Exists(defaultOutputPath)) 
					defaultOutputPath = Application.StartupPath;
				
				this.DefaultOutputDirectory = defaultOutputPath;
			}
		}

		public void Save()
		{
			xmlDoc.Save(Application.StartupPath + @"\Settings\DefaultSettings.xml");
		}

		#region Attributes on <DefaultSettings>
		public string Version
		{
			get
			{
				string version = missing;

				string xPath = @"//DefaultSettings";
				XmlNode node = xmlDoc.SelectSingleNode(xPath, null);

				if(node != null)
				{
					XmlAttribute attr = node.Attributes["Version"];
					if(null != attr)
					{
						version = attr.Value;
					}
				}

				return version;
			}

			set
			{
				string xPath = @"//DefaultSettings";
				XmlNode node = xmlDoc.SelectSingleNode(xPath, null);

				if(node != null)
				{
					XmlAttribute attr = node.Attributes["Version"];
					if(null != attr)
					{
						attr.Value = value;
					}
					else
					{
						attr = xmlDoc.CreateAttribute("Version", null);
						attr.Value = value;
						node.Attributes.Append(attr);
					}
				}
			}
		}

		public bool FirstLoad
		{
			get
			{
				bool first = false;

				string xPath = @"//DefaultSettings";
				XmlNode node = xmlDoc.SelectSingleNode(xPath, null);

				if(node != null)
				{
					XmlAttribute attr = node.Attributes["FirstTime"];
					string firstTime = attr.Value as string;
					first = firstTime == "true" ? true : false;
				}

				return first;
			}

			set
			{
				string xPath = @"//DefaultSettings";
				XmlNode node = xmlDoc.SelectSingleNode(xPath, null);

				if(node != null)
				{
					XmlAttribute attr = node.Attributes["FirstTime"];
					if(null != attr)
					{
						attr.Value = (value == true) ? "true" : "false";
					}
				}
			}
		}
		#endregion

		public string DbDriver
		{
			get { return this.GetSetting("DbDriver").ToUpper(); }
			set	{ this.SetSetting("DbDriver", value); }
		}

		public string ConnectionString
		{
			get { return this.GetSetting("ConnectionString"); }
			set	{ this.SetSetting("ConnectionString", value); }
		}

		public string LanguageMappingFile
		{
			get { return this.GetSetting("LanguageMappingFile"); }
			set	{ this.SetSetting("LanguageMappingFile", value); }
		}

		public string Language
		{
			get { return this.GetSetting("Language"); }
			set	{ this.SetSetting("Language", value); }
		}

		public string DbTargetMappingFile
		{
			get { return this.GetSetting("DbTargetMappingFile"); }
			set	{ this.SetSetting("DbTargetMappingFile", value); }
		}

		public string DbTarget
		{
			get { return this.GetSetting("DbTarget"); }
			set	{ this.SetSetting("DbTarget", value); }
		}

		public string UserMetaDataFileName
		{
			get { return this.GetSetting("UserMetaDataFileName"); }
			set	{ this.SetSetting("UserMetaDataFileName", value); }
		}

		public bool EnableLineNumbering
		{
			get { return Convert.ToBoolean(this.GetSetting("EnableLineNumbering")); }
			set	{ this.SetSetting("EnableLineNumbering", value.ToString()); }
		}

		public bool EnableClipboard
		{
			get { return Convert.ToBoolean(this.GetSetting("EnableClipboard")); }
			set	{ this.SetSetting("EnableClipboard", value.ToString()); }
		}

		public int Tabs
		{
			get { return Convert.ToInt32(this.GetSetting("Tabs")); }
			set { this.SetSetting("Tabs", value.ToString()); }
		}

		public int ScriptTimeout
		{
			get { return Convert.ToInt32(this.GetSetting("ScriptTimeout")); }
			set { this.SetSetting("ScriptTimeout", value.ToString()); }
		}

		public string DefaultTemplateDirectory
		{
			get { return this.GetSetting("DefaultTemplateDirectory"); }
			set	{ this.SetSetting("DefaultTemplateDirectory", value); }
		}

		public string DefaultOutputDirectory
		{
			get { return this.GetSetting("DefaultOutputDirectory"); }
			set	{ this.SetSetting("DefaultOutputDirectory", value); }
		}

		public bool UseProxyServer
		{
			get 
			{ 
				string useproxy = this.GetSetting("UseProxyServer"); 
				if (useproxy == missing) return false;
				else return Convert.ToBoolean(useproxy);
			}
			set	
			{ 
				this.SetSetting("UseProxyServer", value.ToString()); 
			}
		}

		public string ProxyServerUri
		{
			get 
			{
				string uri = this.GetSetting("ProxyServerUri");
				if (uri == missing) 
				{
					return string.Empty;
				}
				else
				{
					return uri; 
				}
			}
			set	{ this.SetSetting("ProxyServerUri", value); }
		}

		public string ProxyAuthUsername
		{
			get 
			{
				string user = this.GetSetting("ProxyAuthUsername");
				if (user == missing) 
				{
					return string.Empty;
				}
				else
				{
					return user; 
				}
			}
			set	{ this.SetSetting("ProxyAuthUsername", value); }
		}

		public string ProxyAuthPassword
		{
			get 
			{
				string passwd = this.GetSetting("ProxyAuthPassword");
				if (passwd == missing) 
				{
					return string.Empty;
				}
				else
				{
					return passwd; 
				}
			}
			set	{ this.SetSetting("ProxyAuthPassword", value); }
		}

		public string ProxyAuthDomain
		{
			get 
			{
				string domain = this.GetSetting("ProxyAuthDomain");
				if (domain == missing) 
				{
					return string.Empty;
				}
				else
				{
					return domain; 
				}
			}
			set	{ this.SetSetting("ProxyAuthDomain", value); }
		}

		public string WindowState
		{
			get { return this.GetSetting("WindowState"); }
			set	{ this.SetSetting("WindowState", value); }
		}

		public string WindowPosTop
		{
			get { return this.GetSetting("WindowPosTop"); }
			set	{ this.SetSetting("WindowPosTop", value); }
		}

		public string WindowPosLeft
		{
			get { return this.GetSetting("WindowPosLeft"); }
			set	{ this.SetSetting("WindowPosLeft", value); }
		}

		public string WindowPosWidth
		{
			get { return this.GetSetting("WindowPosWidth"); }
			set	{ this.SetSetting("WindowPosWidth", value); }
		}

		public string WindowPosHeight
		{
			get { return this.GetSetting("WindowPosHeight"); }
			set	{ this.SetSetting("WindowPosHeight", value); }
		}

		public bool CheckForNewBuild
		{
			get 
			{ 
				string chk = this.GetSetting("CheckForNewBuild");

				if(chk == DefaultSettings.missing)
				{
					return false;
				}
				else
				{
					return Convert.ToBoolean(chk); 
				}
			}

			set	{ this.SetSetting("CheckForNewBuild", value.ToString()); }
		}

		public bool DomainOverride
		{
			get 
			{ 
				// This is true by default
				string domain = this.GetSetting("DomainOverride");
				if (domain == missing) return true;
				else return Convert.ToBoolean(domain);
			}
			set	
			{ 
				this.SetSetting("DomainOverride", value.ToString()); 
			}
		}

		#region Internal Stuff
		protected string GetSetting(string name)
		{
			string xPath = @"//DefaultSettings/Setting[@Name='" + name + "']";
			XmlNode node = xmlDoc.SelectSingleNode(xPath, null);

			if(node != null)
			{
				return node.Attributes["value"].Value;
			}
			else
			{
				return missing;
			}
		}

		protected void SetSetting(string name, string data)
		{
			string xPath = @"//DefaultSettings/Setting[@Name='" + name + "']";
			XmlNode node = xmlDoc.SelectSingleNode(xPath, null);

			if(node != null)
			{
				node.Attributes["value"].Value = data;
			}
			else
			{
				AddSetting(name, data);
			}
		}

		private void AddSetting(string name, string data)
		{
			string xPath = @"//DefaultSettings";
			XmlNode node = xmlDoc.SelectSingleNode(xPath, null);

			if(node != null)
			{
				XmlAttribute attr;
				XmlNode setting = xmlDoc.CreateNode(XmlNodeType.Element, "Setting", null);

				attr = xmlDoc.CreateAttribute("Name", null);
				attr.Value = name;
				setting.Attributes.Append(attr);

				attr = xmlDoc.CreateAttribute("value", null);
				attr.Value = data;
				setting.Attributes.Append(attr);

				node.AppendChild(setting);
			}
		}
		#endregion

		private XmlDocument xmlDoc;
	}
}
