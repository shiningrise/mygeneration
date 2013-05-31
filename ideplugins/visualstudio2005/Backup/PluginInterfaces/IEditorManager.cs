using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace MyGeneration
{
    public interface IEditorManager
    {
        string Name { get; }
        string Description { get; }
        Uri AuthorUri { get; }
        SortedList<string, string> FileExtensions { get; }
        List<string> FileTypes { get; }
        bool CanOpenFile(FileInfo file);
        Image GetMenuImage(string fileType);
        IMyGenDocument Open(IMyGenerationMDI mdi, FileInfo file, params string[] args);
        IMyGenDocument Create(IMyGenerationMDI mdi, params string[] args);
    }
}
