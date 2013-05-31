using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace MyGeneration 
{
    public class ProjectEditorManager : EditorManager
    {
        public const string MYGEN_PROJECT = "MyGeneration Project";
        
        private SortedList<string, string> fileExtensions;
        private List<string> fileTypes;

        public override string Name
        {
            get
            {
                return "MyGeneration Project";
            }
        }

        public override string Description
        {
            get
            {
                return "The MyGeneration Project editor.";
            }
        }

        public override Uri AuthorUri
        {
            get
            {
                return new Uri("http://sourceforge.net/projects/mygeneration/");
            }
        }

        public override Image GetMenuImage(string fileType)
        {
            return Properties.Resources.newproject;
        }

        public override SortedList<string, string> FileExtensions
        {
            get
            {
                if (fileExtensions == null)
                {
                    fileExtensions = new SortedList<string, string>();
                    fileExtensions.Add("zprj", "MyGeneration Project Files");
                }
                return fileExtensions;
            }
        }

        public override List<string> FileTypes
        {
            get
            {
                if (fileTypes == null)
                {
                    fileTypes = new List<string>();
                    fileTypes.Add(ProjectEditorManager.MYGEN_PROJECT); ;
                }
                return fileTypes;
            }
        }

        public override IMyGenDocument Open(IMyGenerationMDI mdi, FileInfo file, params string[] args)
        {
            ProjectBrowser edit = null;

            if (file.Exists)
            {
                bool isopen = mdi.IsDocumentOpen(file.FullName);

                if (!isopen)
                {
                    edit = new ProjectBrowser(mdi);
                    edit.LoadProject(file.FullName);
                }
                else
                {
                    edit = mdi.FindDocument(file.FullName) as ProjectBrowser;
                    if (edit != null)
                    {
                        edit.Activate();
                    }
                }
            }

            return edit;
        }

        public override IMyGenDocument Create(IMyGenerationMDI mdi, params string[] args)
        {
            ProjectBrowser edit = new ProjectBrowser(mdi);

            switch (args[0])
            {
                case ProjectEditorManager.MYGEN_PROJECT:
                default:
                    edit.CreateNewProject();
                    break;
            }

            return edit;
        }
    }
}
