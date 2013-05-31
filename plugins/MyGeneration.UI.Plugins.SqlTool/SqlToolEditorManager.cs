using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using MyGeneration;

namespace MyGeneration.UI.Plugins.SqlTool
{
    public class SqlToolEditorManager : IEditorManager
    {
        public const string SQL_FILE = "SQL File";
        
        private SortedList<string, string> fileExtensions;
        private List<string> fileTypes;

        public string Name
        {
            get
            {
                return "SQL Execution Tool";
            }
        }

        public string Description
        {
            get { return "This is a SQL Execution Tool document plugin. - komma8.komma1"; }
        }

        public Uri AuthorUri
        {
            get
            {
                return new Uri("http://sourceforge.net/projects/mygeneration/");
            }
        }

        public Image GetMenuImage(string fileType)
        {
            return Properties.Resources.riskdb;
        }

        public SortedList<string, string> FileExtensions
        {
            get
            {
                if (fileExtensions == null)
                {
                    fileExtensions = new SortedList<string, string>();
                    fileExtensions.Add("sql", "SQL Files");
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
                    fileTypes.Add(SqlToolEditorManager.SQL_FILE);
                }
                return fileTypes;
            }
        }

        public IMyGenDocument Open(IMyGenerationMDI mdi, FileInfo file, params string[] args)
        {
            SqlToolForm edit = null;

            if (file.Exists)
            {
                bool isopen = mdi.IsDocumentOpen(file.FullName);

                if (!isopen)
                {
                    edit = new SqlToolForm(mdi);
                    edit.Open(file.FullName);
                }
                else
                {
                    edit = mdi.FindDocument(file.FullName) as SqlToolForm;
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
            SqlToolForm edit = new SqlToolForm(mdi);

            switch (args[0])
            {
                case SqlToolEditorManager.SQL_FILE:
                default:
                    //edit.CreateNewImage();
                    break;
            }

            return edit;
        }

        public virtual bool CanOpenFile(FileInfo file)
        {
            return FileExtensions.ContainsKey(file.Extension.Trim('.'));
        }
    }
}
