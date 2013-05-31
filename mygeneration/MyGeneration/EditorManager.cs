using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace MyGeneration
{
    public abstract class EditorManager : IEditorManager
    {
        #region Static factory type members 
        private static Dictionary<string, IEditorManager> editorManagers = null;
        private static string openFileDialogString = null;

       public static Dictionary<string, IEditorManager> EditorManagers
        {
            get
            {
                if (editorManagers == null)
                {
                    TemplateEditorManager tem = new TemplateEditorManager();
                    ProjectEditorManager pem = new ProjectEditorManager();
                    
                    editorManagers = new Dictionary<string, IEditorManager>();
                    editorManagers.Add(tem.Name, tem);
                    editorManagers.Add(pem.Name, pem);

                    // Add dynamic based on the config file.
                }
                return editorManagers;
            }
        }

        public static void Refresh()
        {
            editorManagers = null;
            openFileDialogString = null;
        }

        public static string OpenFileDialogString
        {
            get
            {
                if (openFileDialogString == null)
                {
                    StringBuilder dialogString = new StringBuilder();
                    StringBuilder exts = new StringBuilder();

                    dialogString.Append("All MyGeneration Files (");
                    foreach (IEditorManager editorManager in EditorManagers.Values)
                    {
                        if (editorManager.FileExtensions.Count > 0)
                        {
                            foreach (string ext in editorManager.FileExtensions.Keys)
                            {
                                if (exts.Length == 0)
                                    exts.AppendFormat("*.{0}", ext);
                                else
                                    exts.AppendFormat(";*.{0}", ext);
                            }
                        }
                    }
                    dialogString.Append(exts).Append(")|").Append(exts).Append("|");
                    foreach (IEditorManager editorManager in EditorManagers.Values)
                    {
                        if (editorManager.FileExtensions.Count > 0)
                        {
                            foreach (string ext in editorManager.FileExtensions.Keys)
                                dialogString.AppendFormat("{0} (*.{1})|*.{1}|", editorManager.FileExtensions[ext], ext);
                        }
                    }
                    dialogString.Append("All files (*.*)|*.*");
                    openFileDialogString = dialogString.ToString();
                }

                return openFileDialogString;
            }
        }

        public static void AddNewDocumentMenuItems(EventHandler onClickEvent, params ToolStripItemCollection[] tsics)
        {
            foreach (IEditorManager editorManager in EditorManagers.Values)
            {
                foreach (string ftype in editorManager.FileTypes)
                {
                    foreach (ToolStripItemCollection tsic in tsics)
                    {
                        ToolStripMenuItem i = new ToolStripMenuItem(ftype, editorManager.GetMenuImage(ftype), onClickEvent);
                        i.ImageTransparentColor = Color.Magenta;
                        tsic.Add(i);
                    }
                }
            }
        }

        public static IMyGenDocument CreateDocument(IMyGenerationMDI mdi, string fileType)
        {
            IMyGenDocument mygenDoc = null;
            foreach (IEditorManager manager in EditorManagers.Values)
            {
                if (manager.FileTypes.Contains(fileType))
                {
                    mygenDoc = manager.Create(mdi, fileType);
                    if (mygenDoc != null) break;
                }
            }
            return mygenDoc;
        }

        public static IMyGenDocument OpenDocument(IMyGenerationMDI mdi, string filename)
        {
            IMyGenDocument mygenDoc = null;
            FileInfo info = new FileInfo(filename);
            if (info.Exists)
            {
                foreach (IEditorManager manager in EditorManagers.Values)
                {
                    if (manager.CanOpenFile(info))
                    {
                        mygenDoc = manager.Open(mdi, info);
                        if (mygenDoc != null) break;
                    }
                }
            }
            return mygenDoc;
        }
#endregion

        public abstract string Name { get; }
        public abstract SortedList<string, string> FileExtensions { get; }
        public abstract List<string> FileTypes { get; }
        public abstract IMyGenDocument Open(IMyGenerationMDI mdi, System.IO.FileInfo file, params string[] args);
        public abstract IMyGenDocument Create(IMyGenerationMDI mdi, params string[] args);

        public virtual Image GetMenuImage(string fileType)
        {
            return null;
        }

        public virtual bool CanOpenFile(FileInfo file)
        {
            return FileExtensions.ContainsKey(file.Extension.Trim('.'));
        }
    }
}
