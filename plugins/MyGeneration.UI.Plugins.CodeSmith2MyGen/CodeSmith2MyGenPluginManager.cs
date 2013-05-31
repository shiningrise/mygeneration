using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyGeneration;
using MyGeneration.CodeSmithConversion;

namespace MyGeneration.UI.Plugins.CodeSmith2MyGen
{
    public class CodeSmith2MyGenPluginManager : ISimplePluginManager
    {
        public string Name
        {
            get { return "CodeSmith2MyGen Plugin"; }
        }

        public string Description
        {
            get { return "This is a port of the CodeSmith to MyGeneration software as a MyGeneration plugin. - komma8.komma1"; }
        }

        public Uri AuthorUri
        {
            get
            {
                return new Uri("http://sourceforge.net/projects/mygeneration/");
            }
        }

        public bool AddToolbarIcon
        {
            get { return true; }
        }

        public Image MenuImage
        {
            get { return Properties.Resources.Hammer; }
        }

        public void Execute(IMyGenerationMDI mdi, params string[] args)
        {
            FormConvertCodeSmith form = new FormConvertCodeSmith(mdi);
            form.ShowDialog(mdi.DockPanel.Parent.FindForm());
        }
    }
}
