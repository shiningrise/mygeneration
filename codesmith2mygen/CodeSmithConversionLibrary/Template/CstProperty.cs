using System;
using System.Collections;

namespace MyGeneration.CodeSmithConversion.Template
{
	/// <summary>
	/// Summary description for CstToken.
	/// </summary>
	public class CstProperty
	{
		private string name;
		private string type;
		private string defaultValue;
		private string category;
		private string description;
		private bool optional;

		public CstProperty()
		{
		}

		public string Name 
		{
			get { return name; }
			set { name = value; }
		}

		public string Type 
		{
			get { return type; }
			set { type = value; }
		}

		public string DefaultValue 
		{
			get { return defaultValue; }
			set { defaultValue = value; }
		}

		public string Category 
		{
			get { return category; }
			set { category = value; }
		}

		public string Description 
		{
			get { return description; }
			set { description = value; }
		}

		public bool Optional 
		{
			get { return optional; }
			set { optional = value; }
		}
		
		public System.Type SystemType 
		{
			get { return System.Type.GetType(this.Type, false); }
		}

		public bool IsCustomType
		{
			get { return (SystemType == null); }
		}
	}
}
