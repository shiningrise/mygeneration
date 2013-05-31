using System;using System.IO;using System.Collections;using System.Collections.Specialized;namespace Zeus{	/// <summary>	/// The ZeusTemplateHeader stores meta information about a template.	/// </summary>	public struct ZeusTemplateHeader	{		public ZeusTemplateHeader(string ns, string comments, string title, string uniqueID, string fileName, string filePath, string sourceType) 
		{
			this.Namespace = ns;
			this.Comments = comments; 
			this.Title = title;
			this.UniqueID = uniqueID; 
			this.FileName = fileName;
			this.FilePath = filePath; 
			this.SourceType = sourceType; 
		}		public string Namespace;		public string Comments;		public string FileName;		public string FilePath;		public string UniqueID;		public string Title;		public string SourceType;	}}