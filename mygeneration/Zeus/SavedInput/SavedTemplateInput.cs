using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using Zeus;
using Zeus.UserInterface;

namespace Zeus
{
    public delegate void ApplyOverrideDataDelegate(IZeusInput input);
    
    /// <summary>
	/// Summary description for SavedTemplateInput.
	/// </summary>
	public class SavedTemplateInput : IZeusSavedTemplateInput
	{
		private string _savedObjectName;
		private string _templatePath;
		private string _templateUniqueID;
        private InputItemCollection _inputItems;
        private List<string> _filesChanged = null;
        private ApplyOverrideDataDelegate _applyOverrideDataDelegate;

		public SavedTemplateInput(string objectName, IZeusTemplate template)
		{
			_savedObjectName = objectName;
			_templateUniqueID = template.UniqueID;
			_templatePath = template.FilePath + template.FileName;
		}

		public SavedTemplateInput(string objectName, IZeusTemplate template, IZeusInput input) : this(objectName, template)
		{
			_inputItems = new InputItemCollection(input);
		}

		public SavedTemplateInput(string objectName, string uniqueid, string filepath)
		{
			_savedObjectName = objectName;
			_templateUniqueID = uniqueid;
			_templatePath = filepath;
		}

        public SavedTemplateInput() { }

        public ApplyOverrideDataDelegate ApplyOverrideDataDelegate
        {
            set
            {
                _applyOverrideDataDelegate = value;
            }
        }

        private void ApplyOverrideData(IZeusInput input)
        {
            if (_applyOverrideDataDelegate != null)
            {
                _applyOverrideDataDelegate(input);
            }
#if DEBUG
            else
            {
                throw new Exception("ERROR - ApplyOverrideData not set!!");
            }
#endif
        }
		
		public string TemplateUniqueID 
		{
			get { return _templateUniqueID; }
			set { _templateUniqueID = value; }
		}

		public string TemplatePath 
		{
			get { return _templatePath; }
			set { _templatePath = value; }
		}

		public string SavedObjectName 
		{
			get { return _savedObjectName; }
			set { _savedObjectName = value; }
		}

		public InputItemCollection InputItems 
		{
			get 
			{
				if (_inputItems == null) 
				{
					_inputItems = new InputItemCollection();
				}
				return _inputItems;
			}
			set 
			{
				_inputItems = value;
			}
		}

        public IDictionary InputItemsI
        {
            get
            {
                return this.InputItems;
            }
            set
            {
                this.InputItems = value as InputItemCollection;
            }
        }

        public List<string> SavedFiles
        {
            get
            {
                if (_filesChanged == null) _filesChanged = new List<string>();
                return this._filesChanged;
            }
        }

		#region Xml Related Methods
        public string XML
        {
            get
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                XmlTextWriter xml = new XmlTextWriter(ms, Encoding.UTF8);
                BuildXML(xml);
                xml.Flush();
                ms.Position = 0;
                System.IO.StreamReader reader = new System.IO.StreamReader(ms);
                return reader.ReadToEnd();
            }
            set
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.IO.StreamWriter sw = new  System.IO.StreamWriter(ms, Encoding.UTF8);
                sw.Write(value);
                sw.Flush();
                ms.Position = 0;
                XmlTextReader xr = new XmlTextReader(ms);
                xr.Read();
                ReadXML(xr);
            }
        }

		public void BuildXML(XmlTextWriter xml) 
		{
			xml.WriteStartElement("obj");
			xml.WriteAttributeString("name", this._savedObjectName);
			xml.WriteAttributeString("uid", this._templateUniqueID);
			xml.WriteAttributeString("path", this._templatePath);
			
			this.InputItems.BuildXML(xml);

			xml.WriteEndElement();
		}

		public string ReadXML(XmlTextReader xr) 
		{
			string tagName;
			bool inStartElement;

			this._savedObjectName = xr.GetAttribute("name");
			this._templateUniqueID = xr.GetAttribute("uid");
			this._templatePath = xr.GetAttribute("path");

			while (xr.Read()) 
			{
				inStartElement = xr.IsStartElement();
				tagName = xr.LocalName;

				if (inStartElement) 
				{
					// a module start
					if (tagName == "item") 
					{
						InputItem item = new InputItem();

						item.ReadXML(xr);
						
						this.InputItems.Add(item);
					}
				}
				else 
				{
					// if not in a sub object and this is an end object tag, break!
					if (tagName == "obj") 
					{
						break;
					}
				}				 
			}

			xr.Read();
			inStartElement = xr.IsStartElement();
			tagName = xr.LocalName;

			return tagName;
		}
		#endregion

		public void Execute(int timeout, ILog log)
		{
			log.Write("Executing Template Instance '{0}'", this.SavedObjectName);

			string path = FileTools.ResolvePath(this.TemplatePath);
			
			ZeusTemplate template = new ZeusTemplate(path);
			
			ZeusInput zin = new ZeusInput();
			zin.AddItems(this.InputItems);

            this.ApplyOverrideData(zin);

			ZeusContext context = new ZeusContext(zin, /*new GuiController(),*/ new Hashtable());
			context.Log = log;

			template.Execute(context, timeout, true);

            foreach (string file in context.Output.SavedFiles)
            {
                if (!SavedFiles.Contains(file)) SavedFiles.Add(file);
            }
        }

        public SavedTemplateInput Copy()
        {
            SavedTemplateInput copy = new SavedTemplateInput("Copy of " + this.SavedObjectName, this.TemplateUniqueID, this.TemplatePath);
            copy.InputItems = this.InputItems.Copy();
            return copy;
        }

        public IZeusSavedTemplateInput CopyI()
        {
            SavedTemplateInput copy = new SavedTemplateInput("Copy of " + this.SavedObjectName, this.TemplateUniqueID, this.TemplatePath);
            copy.InputItems = this.InputItems.Copy();
            return copy;
        }
	}
}
