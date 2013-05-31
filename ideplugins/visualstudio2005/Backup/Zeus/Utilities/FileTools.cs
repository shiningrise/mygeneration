using System;
using System.IO;
using System.Reflection;
using System.Collections;

namespace Zeus
{
	/// <summary>
	/// Summary description for FileTools.
	/// </summary>
	public class FileTools
	{
		private const string ZEUSHOME_VAR = "ZEUSHOME";
		private static string _rootFolder = null;
        private static string _assemblyPath = null;

		public static string MakeRelative(string pathToChange, string basePath) 
		{
			if (pathToChange.StartsWith(".") || (basePath == string.Empty)) return pathToChange;

			string newPath = pathToChange;

			DirectoryInfo dirInfoTemplate = new DirectoryInfo( basePath );
			DirectoryInfo dirInfoNew = Directory.GetParent( pathToChange );

			string p1 = dirInfoTemplate.FullName;
			string p2 = dirInfoNew.FullName;
			if (p1.EndsWith("\\")) p1 = p1.Remove(p1.Length-1, 1);
			if (p2.EndsWith("\\")) p2 = p2.Remove(p2.Length-1, 1);

			if (p1 == p2) 
			{
				newPath = "." + pathToChange.Substring( pathToChange.LastIndexOf("\\") );
			}
			else 
			{
				
				string[] dirsOriginal = p1.Split('\\');
				string[] dirsNew = p2.Split('\\');
				int matches = 0;

				for (int i = 0; i < dirsOriginal.Length; i++)
				{
					if (i < dirsNew.Length)
					{
						if (dirsNew[i] == dirsOriginal[i])
						{
							matches++;
						}
						else break;
					}
					else break;
				}

				if (matches == 0) 
				{
					newPath = pathToChange;
				}
				else 
				{
					newPath = string.Empty;
					int diff = dirsOriginal.Length - matches;
				
					if (diff == 0) 
					{
						newPath = "." + pathToChange.Substring( pathToChange.LastIndexOf("\\") );
					}
					for (int i = 0; i < diff; i++)
					{
						newPath += "..\\";
					}

					for (int i = (matches); i < dirsNew.Length; i++)
					{
						newPath += dirsNew[i] + "\\";
					}

					newPath += pathToChange.Substring( pathToChange.LastIndexOf("\\") + 1 );
				}
			}

			return newPath;
		}

		public static string MakeAbsolute(string pathToChange, string basePath) 
		{
			string newPath = pathToChange;

			DirectoryInfo dinfo = new DirectoryInfo(basePath);
			string p1 = dinfo.FullName;
			if (!p1.EndsWith("\\")) p1 += "\\";

			if (dinfo.Exists) 
			{
				if (pathToChange.StartsWith("\\"))
				{
					newPath = dinfo.Root + pathToChange;
				}
				else if ((pathToChange.StartsWith(".")) ||
					(pathToChange.IndexOf(":") == -1))
				{
					newPath = p1 + pathToChange;
				}

				FileInfo finfo = new FileInfo(newPath);
				if (finfo.Exists) 
				{
					newPath = finfo.FullName;
				}
				else
				{
					finfo = new FileInfo(pathToChange);
					if (finfo.Exists) 
					{
						newPath = finfo.FullName;
					}
				}

			}

			return newPath;
        }

        public static string AssemblyPath
        {
            get
            {
                if (_assemblyPath == null)
                {
                    _assemblyPath = Assembly.GetExecutingAssembly().Location;
                    _assemblyPath = _assemblyPath.Substring(0, _assemblyPath.LastIndexOf(@"\"));
                }

                return _assemblyPath;
            }
        }

		public static string ApplicationPath 
		{
			get 
			{
				if (_rootFolder == null) 
				{
					_rootFolder = Assembly.GetEntryAssembly().Location;
					_rootFolder = _rootFolder.Substring(0, _rootFolder.LastIndexOf(@"\"));
				}

				return _rootFolder;
			}
            set
            {
                _rootFolder = value;
            }
		}

		public static string ResolvePath(string path) 
		{
			return ResolvePath(path, false);
        }

        public static string ResolvePath(string path, bool useAssemblyPath)
        {
            string[] items = path.Split('%');
            string newpath = string.Empty;

            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];
                if ((i % 2) == 1)
                {
                    if (item == ZEUSHOME_VAR)
                    {
                        if (useAssemblyPath)
                        {
                            newpath += AssemblyPath;
                        }
                        else
                        {
                            newpath += ApplicationPath;
                        }
                    }
                    else
                    {
                        newpath += Environment.GetEnvironmentVariable(item);
                    }
                }
                else
                {
                    newpath += item;
                }
            }

            return newpath;
        }

		public static ArrayList GetFilenamesRecursive(ArrayList rootPaths, ArrayList extensions)
		{
			ArrayList filenames = new ArrayList();
			DirectoryInfo rootInfo;

			if (rootPaths.Count == 0) 
				rootPaths.Add(Directory.GetCurrentDirectory());

			foreach (string rootPath in rootPaths)
			{
				rootInfo = new DirectoryInfo(ResolvePath(rootPath));
				if (rootInfo.Exists) 
				{
					_BuildChildren(rootInfo, filenames, extensions);
				}
			}

			return filenames;
		}

		private static void _BuildChildren(DirectoryInfo rootInfo, ArrayList filenames, ArrayList extensions)
		{
			string filename;

			foreach (DirectoryInfo dirInfo in rootInfo.GetDirectories()) 
			{
				if (dirInfo.Attributes != (FileAttributes.System | dirInfo.Attributes)) 
				{
					_BuildChildren(dirInfo, filenames, extensions);
				}
			}

			foreach (FileInfo fileInfo in rootInfo.GetFiles()) 
			{
				if (fileInfo.Attributes != (FileAttributes.System | fileInfo.Attributes)) 
				{
					if ( extensions.Contains(fileInfo.Extension) ) 
					{
						filename = fileInfo.FullName;

						if (!filenames.Contains(filename))
						{ 
							filenames.Add(filename);
						}
					}
				}
			}
		}
	}
}
