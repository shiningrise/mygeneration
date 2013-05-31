using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Reflection;

namespace MyGeneration.CodeSmithConversion.Conversion.SimpleMapping
{
	/// <summary>
	/// Summary description for SimpleMap.
	/// </summary>
	public class SimpleMap
	{
		private SimpleMapCollection simpleMapCollection;
		private string source = string.Empty;
		private string target = string.Empty;
		private bool isRegExp = false;

		protected SimpleMap() {}

		public bool IsRegExp 
		{
			get { return isRegExp; }
			set { isRegExp = value; }
		}

		public string Source 
		{
			get { return source; }
			set { source = value; }
		}

		public string Target
		{
			get { return target; }
			set { target = value; }
		}

		public SimpleMapCollection Children
		{
			get 
			{
				if (simpleMapCollection == null)
				{
					simpleMapCollection = new SimpleMapCollection();
				}

				return simpleMapCollection;
			}
		}

		#region Load SimpleMaps from XML
		private static SimpleMapCollection vbMaps;
		private static SimpleMapCollection csMaps;
		
		/// <summary>
		/// Loads "*.simplexml" files from the program directory and puts each one in a seperate mapping.
		/// </summary>
		public static SimpleMapCollection VBNetMaps 
		{
			get
			{
				if (vbMaps == null) 
				{
					vbMaps = new SimpleMapCollection();
					GetMaps("*.vbmap", vbMaps);
				}
				return vbMaps;
			}
		}
		
		/// <summary>
		/// Loads "*.simplexml" files from the program directory and puts each one in a seperate mapping.
		/// </summary>
		public static SimpleMapCollection CSharpMaps 
		{
			get
			{
				if (csMaps == null) 
				{
					csMaps = new SimpleMapCollection();
					GetMaps("*.csmap", csMaps);
				}
				return csMaps;
			}
		}

		private static void GetMaps(string ext, SimpleMapCollection simpleMaps) 
		{
			string path = Assembly.GetEntryAssembly().Location;
			FileInfo inf = new FileInfo(path);
			DirectoryInfo dirInfo = inf.Directory;

			FileInfo[] files = dirInfo.GetFiles(ext);
			foreach (FileInfo file in files) 
			{
				LoadFromXml(simpleMaps, file.FullName);
			}
		}

		private static void LoadFromXml(SimpleMapCollection simpleMaps, string filename) 
		{
			try 
			{
				FileInfo fileInfo = new FileInfo(filename);
				SimpleMap map;
				XmlAttribute attr;
				if (fileInfo.Exists) 
				{
					XmlDocument xmldoc = new XmlDocument();
					xmldoc.Load(filename);
					foreach (XmlNode parentnode in xmldoc.ChildNodes) 
					{
						if (parentnode.HasChildNodes) 
						{
							foreach (XmlNode node in parentnode.ChildNodes) 
							{
								if (node.Name.ToLower() == "map") 
								{
									map = new SimpleMap();

									attr = node.Attributes["source"];
									if (attr != null) 
									{
										map.Source = attr.Value;
									}

									attr = node.Attributes["target"];
									if (attr != null) 
									{
										map.Target = attr.Value;
									}
									
									attr = node.Attributes["isregexp"];
									if (attr != null) 
									{
										map.isRegExp = Convert.ToBoolean(attr.Value);
									}

									simpleMaps.Add(map);
								}
							}
						}
					}
				}
			}
			catch// (Exception ex)
			{
			}
		}
		#endregion

	}
}
