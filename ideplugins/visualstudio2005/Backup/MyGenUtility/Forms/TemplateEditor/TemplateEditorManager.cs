using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Zeus;

namespace MyGeneration
{
    public class TemplateEditorManager : EditorManager
    {
        public const string CSHARP_TEMPLATE = "C# Zeus Template";
        public const string VBNET_TEMPLATE = "VB.Net Zeus Template";
        public const string VBSCRIPT_TEMPLATE = "VBScript Zeus Template";
        public const string JSCRIPT_TEMPLATE = "JScript Zeus Template";
                
        private SortedList<string, string> fileExtensions;
        private List<string> fileTypes;

        public override string Name
        {
            get
            {
                return "Zeus Template Editor";
            }
        }

        public override string Description
        {
            get
            {
                return "The MyGeneration Zeus Template editor.";
            }
        }

        public override Uri AuthorUri
        {
            get
            {
                return new Uri("http://sourceforge.net/projects/mygeneration/");
            }
        }

        public override SortedList<string, string> FileExtensions
        {
            get
            {
                if (fileExtensions == null)
                {
                    fileExtensions = new SortedList<string, string>();
                    fileExtensions.Add("jgen", "JScript Zeus Templates");
                    fileExtensions.Add("vbgen", "VBScript Zeus Templates");
                    fileExtensions.Add("csgen", "C# Zeus Templates");
                    fileExtensions.Add("zeus", "Zeus Templates");
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
                    fileTypes.Add(TemplateEditorManager.CSHARP_TEMPLATE);
                    fileTypes.Add(TemplateEditorManager.VBNET_TEMPLATE);
                    fileTypes.Add(TemplateEditorManager.VBSCRIPT_TEMPLATE);
                    fileTypes.Add(TemplateEditorManager.JSCRIPT_TEMPLATE);
                }
                return fileTypes;
            }
        }

        public override IMyGenDocument Open(IMyGenerationMDI mdi, FileInfo file, params string[] args)
        {
            TemplateEditor edit = null;

            if (file.Exists)
            {
                bool isopen = mdi.IsDocumentOpen(file.FullName);

                if (!isopen)
                {
                    edit = new TemplateEditor(mdi);
                    edit.FileOpen(file.FullName);
                }
                else
                {
                    edit = mdi.FindDocument(file.FullName) as TemplateEditor;
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
            TemplateEditor edit = new TemplateEditor(mdi);

            switch (args[0])
            {
                case TemplateEditorManager.CSHARP_TEMPLATE:
                    edit.FileNew("ENGINE", ZeusConstants.Engines.DOT_NET_SCRIPT, "LANGUAGE", ZeusConstants.Languages.CSHARP);
                    break;
                case TemplateEditorManager.VBNET_TEMPLATE:
                    edit.FileNew("ENGINE", ZeusConstants.Engines.DOT_NET_SCRIPT, "LANGUAGE", ZeusConstants.Languages.VBNET);
                    break;
                case TemplateEditorManager.VBSCRIPT_TEMPLATE:
                    edit.FileNew("ENGINE", ZeusConstants.Engines.MICROSOFT_SCRIPT, "LANGUAGE", ZeusConstants.Languages.VBSCRIPT);
                    break;
                case TemplateEditorManager.JSCRIPT_TEMPLATE:
                default:
                    edit.FileNew("ENGINE", ZeusConstants.Engines.MICROSOFT_SCRIPT, "LANGUAGE", ZeusConstants.Languages.JSCRIPT);
                    break;
            }

            return edit;
        }

        public override Image GetMenuImage(string fileType)
        {
            Image image = null;

            switch (fileType)
            {
                case TemplateEditorManager.CSHARP_TEMPLATE:
                    image = Properties.Resources.newcsharp;
                    break;
                case TemplateEditorManager.VBNET_TEMPLATE:
                    image = Properties.Resources.newvbnet;
                    break;
                case TemplateEditorManager.VBSCRIPT_TEMPLATE:
                    image = Properties.Resources.newvbscript;
                    break;
                case TemplateEditorManager.JSCRIPT_TEMPLATE:
                default:
                    image = Properties.Resources.newjscript;
                    break;
            }

            return image;
        }
    }
}
