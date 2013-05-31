using System;
using System.Collections;

namespace MyGeneration.CodeSmithConversion.Template
{
	/// <summary>
	/// Summary description for CstToken. - C#, VB.NET, VB, and Jscript 
	/// </summary>
	public class CstTemplate
	{
		public const string LANGUAGE_VBNET = "VB";
		public const string LANGUAGE_CSHARP = "C#";
		public const string LANGUAGE_JSCRIPT = "Jscript";

		private string language;
		private string inherits;
		private string targetLanguage;
		private string description;
		private string src;
		private bool debug = false;
		private string rawCode;
		private string filename;

		private ArrayList properties;
		private ArrayList tokens;
		private ArrayList assemblies;
		private ArrayList namespaces;

		public CstTemplate()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public ArrayList Properties 
		{
			get 
			{
				if (properties == null) 
				{
					properties = new ArrayList();
				}
				return properties; 
			}
		}

		public ArrayList Tokens
		{
			get 
			{
				if (tokens == null) 
				{
					tokens = new ArrayList();
				}
				return tokens; 
			}
		}
 
		public ArrayList Assemblies
		{
			get 
			{
				if (assemblies == null) 
				{
					assemblies = new ArrayList();
				}
				return assemblies; 
			}
		}
 
		public ArrayList NameSpaces
		{
			get 
			{
				if (namespaces == null) 
				{
					namespaces = new ArrayList();
				}
				return namespaces; 
			}
		}
 
		public string Filename 
		{
			get { return filename; }
			set { filename = value; }
		}
 
		public string Language 
		{
			get { return language; }
			set { language = value; }
		}
 
		public string TargetLanguage 
		{
			get { return targetLanguage; }
			set { targetLanguage = value; }
		}
 
		public string Description 
		{
			get { return description; }
			set { description = value; }
		}
 
		public string Src 
		{
			get { return src; }
			set { src = value; }
		}
 
		public string Inherits 
		{
			get { return inherits; }
			set { inherits = value; }
		}
 
		public bool Debug 
		{
			get { return debug; }
			set { debug = value; }
		}
 
		public string RawCode 
		{
			get { return rawCode; }
			set { rawCode = value; }
		}
	}
}
