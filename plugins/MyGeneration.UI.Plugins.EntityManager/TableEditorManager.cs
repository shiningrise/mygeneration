using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGeneration.UI.Plugins.EntityManager
{
    public class TableEditorManager : IEditorManager
    {
        public const string XML_FILE = "XML File";

        private SortedList<string, string> fileExtensions;
        private List<string> fileTypes;

        public string Name
        {
            get
            {
                return "实体编辑器";
            }
        }

        public string Description
        {
            get { return "实体编辑器"; }
        }

        public Uri AuthorUri
        {
            get { return new Uri("http://www.cnblogs.com/shiningrise"); }
        }

        public SortedList<string, string> FileExtensions
        {
            get
            {
                if (fileExtensions == null)
                {
                    fileExtensions = new SortedList<string, string>();
                    fileExtensions.Add("xml", "Model Files");
                }
                return fileExtensions;
            }
        }

        public List<string> FileTypes
        {
            get
            {
                if (fileTypes == null)
                {
                    fileTypes = new List<string>();
                    fileTypes.Add(XML_FILE);
                }
                return fileTypes;
            }
        }

        public bool CanOpenFile(System.IO.FileInfo file)
        {
            return FileExtensions.ContainsKey(file.Extension.Trim('.'));
        }

        public System.Drawing.Image GetMenuImage(string fileType)
        {
            return Properties.Resources.Image1;
        }

        public IMyGenDocument Open(IMyGenerationMDI mdi, System.IO.FileInfo file, params string[] args)
        {
            TableBrowser edit = null;

            if (file.Exists)
            {
                bool isopen = mdi.IsDocumentOpen(file.FullName);

                if (!isopen)
                {
                    edit = new TableBrowser(mdi);
                    //edit.Open(file.FullName);
                }
                else
                {
                    edit = mdi.FindDocument(file.FullName) as TableBrowser;
                    if (edit != null)
                    {
                        edit.Activate();
                    }
                }
            }

            return edit;
        }

        public IMyGenDocument Create(IMyGenerationMDI mdi, params string[] args)
        {
            

            switch (args[0])
            {
                case XML_FILE:
                default:
                    //edit.CreateNewImage();
                    break;
            }
            //this.mdi.CreateDocument(fileType,databaseName,table.Name);
            var databaseName = args[1].ToString();
            var tableName = args[2].ToString();
            TableBrowser edit = new TableBrowser(mdi,databaseName,tableName);
            return edit;
        }
    }
}
