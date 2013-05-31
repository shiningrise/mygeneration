using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using MyGeneration;

namespace SampleUIPlugin
{
    public class SampleEditorManager : IEditorManager
    {
        public const string SAMPLE_IMAGE = "Plugin Sample Image";
        
        private SortedList<string, string> fileExtensions;
        private List<string> fileTypes;

        public string Name
        {
            get
            {
                return "Plugin Sample Image Editor";
            }
        }

        public string Description
        {
            get { return "This is a sample document plugin. It basically shows how to add document windows into MyGeneration. - komma8.komma1"; }
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
            return Properties.Resources.NewImage;
        }

        public SortedList<string, string> FileExtensions
        {
            get
            {
                if (fileExtensions == null)
                {
                    fileExtensions = new SortedList<string, string>();
                    fileExtensions.Add("jpg", "Joint Photographic Experts Group File");
                    fileExtensions.Add("png", "Portable Network Graphics File");
                    fileExtensions.Add("bmp", "Bitmap File");
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
                    fileTypes.Add(SampleEditorManager.SAMPLE_IMAGE);
                }
                return fileTypes;
            }
        }

        public IMyGenDocument Open(IMyGenerationMDI mdi, FileInfo file, params string[] args)
        {
            SampleEditor edit = null;

            if (file.Exists)
            {
                bool isopen = mdi.IsDocumentOpen(file.FullName);

                if (!isopen)
                {
                    edit = new SampleEditor(mdi);
                    edit.LoadImage(file.FullName);
                }
                else
                {
                    edit = mdi.FindDocument(file.FullName) as SampleEditor;
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
            SampleEditor edit = new SampleEditor(mdi);

            switch (args[0])
            {
                case SampleEditorManager.SAMPLE_IMAGE:
                default:
                    edit.CreateNewImage();
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
