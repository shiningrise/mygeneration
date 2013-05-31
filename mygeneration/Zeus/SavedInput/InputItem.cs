using System;
using System.Xml;

namespace Zeus
{
	/// <summary>
	/// Summary description for InputItem.
	/// </summary>
	public class InputItem
	{
		private string _varName;
		private string _typeName;
		private string _data;
		private Type _type = null;
		private Object _dataObject = null;

		public InputItem() {}

		public InputItem(string varName, object item) 
		{
			this._varName = varName;
			this.DataObject = item;
		}

		public string VariableName 
		{
			get { return this._varName; }
			set { this._varName = value; }
		}

		public string DataTypeName 
		{
			get { return this._typeName; }
			set 
			{
				this._typeName = value;
				this._type = null;
			}
		}

		public string Data 
		{
			get { return this._data; }
			set 
			{
				this._data = value;
				this._dataObject = null;
			}
		}

		public Type DataType
		{
			get 
			{ 
				if ((this._type == null) && (this._typeName != null))
				{
					this._type = MasterSerializer.GetTypeFromName(this._typeName);
				}
				return this._type; 
			}
			set 
			{
				this._type = value;
				this._typeName = value.FullName;
			}
		}

		public Object DataObject 
		{
			get 
			{
				if ((this._dataObject == null) && (this._data != null) && (this.DataType != null)) 
				{
					this._dataObject = MasterSerializer.Reconstitute(this.DataType, this._data);
				}
				return this._dataObject; 
			}
			set 
			{
				this._dataObject = value;
				this._data = MasterSerializer.Dissolve(value);
				
				this.DataType = value.GetType();
			}
		}

		public void BuildXML(XmlTextWriter xml) 
		{
			xml.WriteStartElement("item");
			xml.WriteAttributeString("name", this.VariableName);
			xml.WriteAttributeString("type", this.DataTypeName);
			xml.WriteAttributeString("val", this.Data);
			xml.WriteEndElement();
		}

		public void ReadXML(XmlTextReader xr) 
		{
			this.VariableName = xr.GetAttribute("name");
			this.DataTypeName = xr.GetAttribute("type");
			this.Data = xr.GetAttribute("val");
		}

	}
}
