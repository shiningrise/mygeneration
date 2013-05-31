using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Zeus.Projects
{
    public delegate Dictionary<string, string> GetDefaultSettingsDelegate();
	/// <summary>
	/// Summary description for ZeusModule.
	/// </summary>
	public class ZeusProject : ZeusModule
    {
        private const string USER_EXT = "usr";
        private string _path;
        private GetDefaultSettingsDelegate _getDefaultSettings;

		public ZeusProject() : base() {}

		public ZeusProject(string path) : base() 
		{
			//load from file!
			this._path = path;
		}

        public GetDefaultSettingsDelegate DefaultSettingsDelegate
        {
            set
            {
                _getDefaultSettings = value;
            }
        }

        public Dictionary<string, string> GetDefaultSettings()
        {
            if (_getDefaultSettings != null)
            {
                return _getDefaultSettings();
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

		public bool Save() 
		{
			if (File.Exists(this._path)) 
			{
				FileAttributes fa = File.GetAttributes(this._path);

				if ((FileAttributes.ReadOnly & fa) == FileAttributes.ReadOnly) 
					throw new Exception(this._path + " is read only!");
			}
			StreamWriter sw = new StreamWriter(this._path, false);
			XmlTextWriter xml = new XmlTextWriter(sw.BaseStream as Stream, Encoding.UTF8);
			xml.Formatting = Formatting.Indented;
			xml.WriteStartDocument();

			this.BuildXML(xml);

			xml.Flush();
			xml.Close();

            string userFile = UserFilePath;
            if (File.Exists(userFile))
            {
                FileAttributes fa = File.GetAttributes(userFile);

                if ((FileAttributes.ReadOnly & fa) == FileAttributes.ReadOnly)
                    throw new Exception(userFile + " is read only!");
            }
            sw = new StreamWriter(userFile, false);
            xml = new XmlTextWriter(sw.BaseStream as Stream, Encoding.UTF8);
            xml.Formatting = Formatting.Indented;
            xml.WriteStartDocument();

            this.BuildUserXML(xml);

            xml.Flush();
            xml.Close();

			return true;
		}

		override public ZeusModule ParentModule
		{
			get { return null; }
		}

		public string FilePath
		{
			get { return _path; }
			set { _path = value; }
        }

        public string UserFilePath
        {
            get 
            {
                if (string.IsNullOrEmpty(_path)) return _path;
                else return _path + USER_EXT; 
            }
        }


		public bool Load() 
		{
			if (!File.Exists(this._path)) 
			{
				throw new Exception("Project file not found: " + this._path);
			}		
			else
			{
				bool inStartElement = true;
				string tagName;
				string projectDirPath = this._path.Substring(0, (this._path.LastIndexOf('\\') + 1));

				XmlTextReader xr = new XmlTextReader(File.OpenText(this._path));

				while (xr.Read()) 
				{
					inStartElement = xr.IsStartElement();
					tagName = xr.LocalName;

					if (inStartElement && ((tagName == "project") || (tagName == "module")) )
					{
						tagName = this.ReadXML(xr);
					}
				}
				xr.Close();

                if (File.Exists(this.UserFilePath))
                {
                    inStartElement = true;

                    xr = new XmlTextReader(File.OpenText(this.UserFilePath));

                    while (xr.Read())
                    {
                        inStartElement = xr.IsStartElement();
                        tagName = xr.LocalName;

                        if (inStartElement && ((tagName == "project") || (tagName == "module")))
                        {
                            tagName = this.ReadUserXML(xr);
                        }
                    }
                    xr.Close();
                }

				return true;
			}
		}
	}
}
