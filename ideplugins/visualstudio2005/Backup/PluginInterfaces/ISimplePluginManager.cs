using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace MyGeneration
{
    public interface ISimplePluginManager
    {
        string Name { get; }
        string Description { get; }
        Uri AuthorUri { get; }
        Image MenuImage { get; }
        bool AddToolbarIcon { get; }
        void Execute(IMyGenerationMDI mdi, params string[] args);
    }
}
