using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace MyGeneration
{
    public interface IContentManager
    {
        string Name { get; }
        string Description { get; }
        Uri AuthorUri { get; }
        Image MenuImage { get; }
        bool AddToolbarIcon { get; }
        IMyGenContent Create(IMyGenerationMDI mdi, params string[] args);
    }
}
