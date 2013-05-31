using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using Zeus;
using Zeus.UserInterface;

namespace Zeus
{
	/// <summary>
	/// Summary description for ZeusSavedInput.
	/// </summary>
    public class ZeusSavedInput : IZeusSavedInput
	{
		private string _path = "default.zinp";
		private SavedTemplateInput _input = new SavedTemplateInput();

		public ZeusSavedInput() {}

		public ZeusSavedInput(string path)
		{
			this._path = path;
		}

		public string FilePath
		{
			get { return _path; }
			set { _path = value; }
        }

        public SavedTemplateInput InputData
        {
            get { return _input; }
        }

        public IZeusSavedTemplateInput InputDataI
        {
            get { return InputData; }
        }

		public bool Load() 
		{
			if (!File.Exists(this._path)) 
			{
				throw new Exception("Saved Template Input file not found: " + this._path);
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

					if (inStartElement && ((tagName == "obj")) )
					{
						tagName = this.InputData.ReadXML(xr);
					}
				}
				xr.Close();

				return true;
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

			this.InputData.BuildXML(xml);

			xml.Flush();
			xml.Close();

			return true;
		}
	}
}
