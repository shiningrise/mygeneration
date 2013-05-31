using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace MyGeneration
{
    public static class PluginManager
    {
        private static Dictionary<FileInfo, Exception> pluginLoadErrors = new Dictionary<FileInfo, Exception>();
        private static Dictionary<string, IContentManager> contentManagers = null;
        private static Dictionary<string, IEditorManager> editorManagers = null;
        private static Dictionary<string, ISimplePluginManager> simplePluginManagers = null;
        private static string openFileDialogString = null;

        public static void LoadPlugins()
        {
            TemplateEditorManager tem = new TemplateEditorManager();
            ProjectEditorManager pem = new ProjectEditorManager();

            editorManagers = new Dictionary<string, IEditorManager>();
            editorManagers.Add(tem.Name, tem);
            editorManagers.Add(pem.Name, pem);

            contentManagers = new Dictionary<string, IContentManager>();

            simplePluginManagers = new Dictionary<string, ISimplePluginManager>();

            FileInfo info = new FileInfo(Assembly.GetExecutingAssembly().Location);
            if (info.Exists)
            {
                foreach (FileInfo dllFile in info.Directory.GetFiles("MyGeneration.UI.Plugins.*.dll"))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(dllFile.FullName);
                        foreach (Type type in assembly.GetTypes())
                        {
                            Type[] interfaces = type.GetInterfaces();
                            foreach (Type iface in interfaces)
                            {
                                if (iface == typeof(IEditorManager) || 
                                    iface == typeof(IContentManager) || 
                                    iface == typeof(ISimplePluginManager))
                                {
                                    try
                                    {
                                        ConstructorInfo[] constructors = type.GetConstructors();
                                        ConstructorInfo constructor = constructors[0];

                                        object plugin = constructor.Invoke(BindingFlags.CreateInstance, null, new object[] { }, null);
                                        if (plugin is IEditorManager)
                                        {
                                            IEditorManager em = plugin as IEditorManager;
                                            if (!editorManagers.ContainsKey(em.Name))
                                            {
                                                editorManagers[em.Name] = em;
                                            }
                                        }
                                        else if (plugin is IContentManager)
                                        {
                                            IContentManager cm = plugin as IContentManager;
                                            if (!contentManagers.ContainsKey(cm.Name))
                                            {
                                                contentManagers[cm.Name] = cm;
                                            }
                                        }
                                        else if (plugin is ISimplePluginManager)
                                        {
                                            ISimplePluginManager spm = plugin as ISimplePluginManager;
                                            if (!simplePluginManagers.ContainsKey(spm.Name))
                                            {
                                                simplePluginManagers[spm.Name] = spm;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        pluginLoadErrors.Add(dllFile, ex);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        pluginLoadErrors.Add(dllFile, ex);
                    }
                }
            }
        }
        public static Dictionary<string, IEditorManager> EditorManagers
        {
            get
            {
                if (editorManagers == null)
                {
                    LoadPlugins();
                }
                return editorManagers;
            }
        }
        public static Dictionary<string, IContentManager> ContentManagers
        {
            get
            {
                if (contentManagers == null)
                {
                    LoadPlugins();
                }
                return contentManagers;
            }
        }

        public static Dictionary<string, ISimplePluginManager> SimplePluginManagers
        {
            get
            {
                if (simplePluginManagers == null)
                {
                    LoadPlugins();
                }
                return simplePluginManagers;
            }
        }

        public static void Refresh()
        {
            contentManagers = null;
            editorManagers = null;
            simplePluginManagers = null;
            pluginLoadErrors.Clear();
        }

        public static bool DoPluginErrorsExist
        {
            get
            {
                return (pluginLoadErrors.Count > 0);
            }
        }

        public static Dictionary<FileInfo, Exception> PluginLoadErrors
        {
            get
            {
                return pluginLoadErrors;
            }
        }

        public static void AddHelpMenuItems(EventHandler onClickEvent, ToolStripMenuItem pluginsMenuItem, int mergeIndex)
        {
            SortedList<string, string> chms = new SortedList<string, string>();
            chms.Add("MyMeta.chm", "MyMeta API Reference");
            chms.Add("Zeus.chm", "Zeus API Reference");
            chms.Add("Dnp.Utils.chm", "Dnp.Utils API Reference");
            chms.Add("dOOdads.chm", "dOOdads API Reference");
            chms.Add("MyGenXsd3b.chm", "MyGenXsd3b Help");
            chms.Add("xsd3b.chm", "xsd3b Help");

            foreach (string key in chms.Keys) 
            {
                ToolStripMenuItem i = new ToolStripMenuItem(chms[key]);
                i.Tag = @"\" + key;
                i.Click += new EventHandler(onClickEvent);
                pluginsMenuItem.DropDownItems.Insert(mergeIndex++, i);
            }


            // Could add others by adding to plugin API
        }

    }
}
