using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

using Zeus.ErrorHandling;

namespace Zeus
{
	public enum DotNetLanguage 
	{
		CSharp = 0,
		VBNet
	}

	/// <summary>
	/// Summary description for DynamicAssemblyTools.
	/// </summary>
	public class DynamicAssemblyTools
	{
		public static Assembly LoadDynamicAssembly(string filepath)
		{
			string tmpDirectory = System.Environment.CurrentDirectory;
            System.Environment.CurrentDirectory = FileTools.AssemblyPath;

			Assembly assembly = null;
			
			try 
			{
				assembly = Assembly.LoadFile(filepath);
			}
			catch  
			{
				//TODO Should catch this error and handle it at some point
			}

			System.Environment.CurrentDirectory = tmpDirectory;

			return assembly;
		}

		public static object[] InstantiateClassesByType(Assembly assembly, System.Type matchingType) 
		{
            try
            {
                if (matchingType.IsInterface)
                {
                    return InstantiateClassesByInterfaceType(assembly, matchingType);
                }
                else
                {
                    return InstantiateClassesByClassType(assembly, matchingType);
                }
            }
            catch (Exception ex)
            {
                string assInfo = "<NULL>";
                string typInfo = "<NULL>";
                if (assembly != null) assInfo = assembly.FullName;
                if (matchingType != null) typInfo = matchingType.FullName;
                throw new Exception("Assembly=" + assInfo + " Type=" + typInfo, ex);
            }
		}
				
		protected static object[] InstantiateClassesByInterfaceType(Assembly assembly, System.Type matchingType)
		{
			ArrayList objects = new ArrayList();

			//cant call the entry method if the assembly is null
			if (assembly != null)
			{
				bool classFound = false;

				//Use reflection to call the static Run function
				Module[] mods = assembly.GetModules(false);
				Type[] types = mods[0].GetTypes();

				//loop through each class that was defined and look for the first occurrance of the entry point method
				foreach (Type type in types)
				{
					Type[] interfaces = type.GetInterfaces();
					foreach (Type iface in interfaces) 
					{
						if (iface == matchingType) 
						{
							ConstructorInfo[] constructors = type.GetConstructors();
							ConstructorInfo constructor = constructors[0];

							objects.Add( constructor.Invoke(BindingFlags.CreateInstance, null, new object[] {}, null) );
							classFound = true;
							break;
						}
					}
				}

				//if it got here, then there was no entry point method defined.  Tell user about it
				if (!classFound) 
				{
					//TODO: handle error with correct Exception Type or just log the error
				}
			}

			return objects.ToArray();
		}

		protected static object[] InstantiateClassesByClassType(Assembly assembly, System.Type matchingType)
		{
			ArrayList objects = new ArrayList();

			//cant call the entry method if the assembly is null
			if (assembly != null)
			{
				bool classFound = false;

				//Use reflection to call the static Run function
				Module[] mods = assembly.GetModules(false);
				Type[] types = mods[0].GetTypes();

				//loop through each class that was defined and look for the first occurrance of the entry point method
				foreach (Type type in types)
				{
					if ((type == matchingType) && !type.IsAbstract) 
					{
						ConstructorInfo[] constructors = type.GetConstructors();
						ConstructorInfo constructor = constructors[0];

						objects.Add( constructor.Invoke(BindingFlags.CreateInstance, null, new object[] {}, null) );
						classFound = true;
					}
				}

				//if it got here, then there was no entry point method defined.  Tell user about it
				if (!classFound) 
				{
					//TODO: handle error with correct Exception Type or just log the error
				}
			}

			return objects.ToArray();
		}

		public static object InstantiateClassByType(System.Type type)
		{
			object obj = null;
			//cant call the entry method if the assembly is null
			ConstructorInfo[] constructors = type.GetConstructors();

			foreach (ConstructorInfo constructor in constructors) 
			{
				if (constructor.GetParameters().Length == 0) 
				{
					obj =  constructor.Invoke(BindingFlags.CreateInstance, null, new object[] {}, null);
					break;
				}
			}

			return obj;
		}

		public static object RunMethod(Object obj, string methodName, object[] parameters) 
		{
			Type type = obj.GetType();
			MethodInfo methodInfo = type.GetMethod(methodName);

			if (methodInfo != null) 
			{
				return methodInfo.Invoke(obj, parameters);
			}
			else 
			{
				//TODO: handle error with correct Exception Type or just log the error
				return null;
			}
		}

		public static void ExecuteStaticMethod(Assembly assembly, string methodName, object[] parameters)
		{
			//cant call the entry method if the assembly is null
			if (assembly != null)
			{
				bool classFound = false;

				//Use reflection to call the static Run function
				Module[] mods = assembly.GetModules(false);
				Type[] types = mods[0].GetTypes();

				//loop through each class that was defined and look for the first occurrance of the entry point method
				foreach (Type type in types)
				{
					MethodInfo mi = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
					if (mi != null)
					{
						//if the entry point method doesnt return an Int32, then return the error constant
						mi.Invoke(null, new object[] {parameters});
						classFound = true;
					}
				}

				//if it got here, then there was no entry point method defined.  Tell user about it
				if (!classFound) 
				{
					//TODO: handle error with correct Exception Type or just log the error
				}
			}
		}
	}
}
