using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace MyGeneration.CodeSmithConversion.Plugins
{
	/// <summary>
	/// Summary description for PluginController.
	/// </summary>
	public class PluginController
	{
		private static ICstProcessor[] plugins;

		public static ICstProcessor[] Plugins
		{
			get 
			{
				if (plugins == null)
				{
					ArrayList list = new ArrayList();
					FileInfo[] csfiles = GetFiles("*.plugin.cs");
					FileInfo[] vbfiles = GetFiles("*.plugin.vb");

					Assembly a;
					CodeDomProvider p;
					ICodeCompiler c;
					CompilerParameters ps = new CompilerParameters();
					ps.GenerateInMemory = true;
					ps.TreatWarningsAsErrors = false;
					ps.ReferencedAssemblies.Add("System.dll");
					ps.ReferencedAssemblies.Add("System.Xml.dll");
					ps.ReferencedAssemblies.Add("System.Data.dll");
                    ps.ReferencedAssemblies.Add("System.Drawing.dll");
                    ps.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                    ps.ReferencedAssemblies.Add("MyGeneration.UI.Plugins.CodeSmith2MyGen.dll");

					if (vbfiles.Length > 0) 
					{
						string[] vbfilenames = new string[vbfiles.Length];
						for (int i = 0; i < vbfiles.Length; i++) 
						{
							vbfilenames[i] = vbfiles[i].FullName;
						}

						p = new VBCodeProvider();
						c = p.CreateCompiler();
						CompilerResults vbr = c.CompileAssemblyFromFileBatch(ps, vbfilenames);
						
						if (!vbr.Errors.HasErrors) 
						{
							a = vbr.CompiledAssembly;
							GetObjectsOfType(list, a, typeof(ICstProcessor));
						}
					}

					if (csfiles.Length > 0) 
					{
						string[] csfilenames = new string[csfiles.Length];
						for (int i = 0; i < csfiles.Length; i++) 
						{
							csfilenames[i] = csfiles[i].FullName;
						}

						p = new CSharpCodeProvider();
						c = p.CreateCompiler();
						CompilerResults csr = c.CompileAssemblyFromFileBatch(ps, csfilenames);

						if (!csr.Errors.HasErrors) 
						{
							a = csr.CompiledAssembly;
							GetObjectsOfType(list, a, typeof(ICstProcessor));
						}
					}
					plugins = list.ToArray(typeof(ICstProcessor)) as ICstProcessor[];
				}
				return plugins;
			}
		}

		private static void GetObjectsOfType(ArrayList list, Assembly assembly, Type interfacetype) 
		{
			foreach (System.Type type in assembly.GetTypes()) 
			{
				ArrayList interfaces = new ArrayList(type.GetInterfaces());
				if ( interfaces.Contains(interfacetype) && !type.IsAbstract ) 
				{
					ConstructorInfo[] cinfs = type.GetConstructors();
					foreach (ConstructorInfo cinf in cinfs) 
					{
						if (cinf.GetParameters().Length == 0) 
						{
							list.Add( cinf.Invoke( new object[0] {} ) );
							break;
						}
					}
				}
			}
		}

		private static FileInfo[] GetFiles(string ext)
		{
			string path = Assembly.GetEntryAssembly().Location;
			FileInfo inf = new FileInfo(path);
			DirectoryInfo dirInfo = new DirectoryInfo(inf.DirectoryName + "/Plugins");
			if (!dirInfo.Exists) 
			{
				dirInfo.Create();
			}

			return dirInfo.GetFiles(ext);
		}
	}
}
