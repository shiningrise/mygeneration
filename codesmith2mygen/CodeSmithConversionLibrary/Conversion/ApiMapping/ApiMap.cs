using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Reflection;

namespace MyGeneration.CodeSmithConversion.Conversion.ApiMapping
{
	public enum ApiNodeType 
	{
		Root = 0,
		Class,
		Method,
		Property,
		Event,
		StraightText
	}

	/// <summary>
	/// Summary description for ApiMap.
	/// </summary>
	public class ApiMap
	{
		private ApiMapCollection apiMapCollection;
		private ApiNodeType nodeType;
		private string sourceName;
		private string sourceType;
		private string targetName;
		private string targetType;

		protected ApiMap() {}

		public ApiNodeType NodeType 
		{
			get { return nodeType; }
			set { nodeType = value; }
		}

		public string SourceName 
		{
			get { return sourceName; }
			set { sourceName = value; }
		}

		public string SourceType
		{
			get { return sourceType; }
			set { sourceType = value; }
		}

		public string TargetName 
		{
			get { return targetName; }
			set { targetName = value; }
		}

		public string TargetType
		{
			get { return targetType; }
			set { targetType = value; }
		}

		public ApiMapCollection Children
		{
			get 
			{
				if (apiMapCollection == null)
				{
					apiMapCollection = new ApiMapCollection();
				}

				return apiMapCollection;
			}
		}

		#region Load ApiMaps from XML
		/*private static Hashtable apiMaps;
		/// <summary>
		/// Loads "*.apixml" files from the program directory and puts each one in a seperate mapping.
		/// </summary>
		public static Hashtable ApiMaps 
		{
			get
			{
				if (apiMaps == null) 
				{
					apiMaps = new Hashtable();

					string path = Assembly.GetEntryAssembly().Location;
					FileInfo inf = new FileInfo(path);
					DirectoryInfo dirInfo = inf.Directory;

					FileInfo[] files = dirInfo.GetFiles("*.apimap");
					foreach (FileInfo file in files) 
					{
						try 
						{
							ApiMap map = LoadFromXml(file.FullName);
							apiMaps.Add(file.Name, map);
						}
						catch// (Exception ex)
						{
						}
					}
					
				}
			}
		}

		private static ApiMap LoadFromXml(string filename) 
		{
			FileInfo fileInfo = new FileInfo(filename);
			if (fileInfo.Exists) 
			{
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.Load(filename);


			}
		}*/
		#endregion

	}
}
