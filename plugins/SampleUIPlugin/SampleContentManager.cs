using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using MyGeneration;

namespace SampleUIPlugin
{
    public class SampleContentManager : IContentManager
    {
        public string Name
        {
            get { return "Sample Content"; }
        }

        public string Description
        {
            get { return "This is a sample content plugin. It basically shows how to add dockable content windows into MyGeneration. - komma8.komma1"; }
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
                return false;
            }
        }

        public Image MenuImage
        {
            get { return Properties.Resources.bgb; }
        }

        public IMyGenContent Create(IMyGenerationMDI mdi, params string[] args)
        {
            return new SampleContent(mdi);
        }
    }
}
