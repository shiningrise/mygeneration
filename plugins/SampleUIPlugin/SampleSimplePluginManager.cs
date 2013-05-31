using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyGeneration;

namespace SampleUIPlugin
{
    public class SampleSimplePluginManager : ISimplePluginManager
    {
        public string Name
        {
            get { return "Sample Simple Plugin"; }
        }

        public string Description
        {
            get { return "This is a sample simple plugin. It basically shows how to add a simple plugin into MyGeneration. - komma8.komma1"; }
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
            get
            {
                return true;
            }
        }

        public Image MenuImage
        {
            get { return Properties.Resources.puter; }
        }

        public void Execute(IMyGenerationMDI mdi, params string[] args)
        {
            SampleDialogContent sdc = new SampleDialogContent(mdi);
            DialogResult result = sdc.ShowDialog(mdi.DockPanel.Parent.FindForm());
        }
    }
}
