using System;
using System.Collections.Generic;
using System.Text;
using Scintilla;
using Scintilla.Forms;
using Scintilla.Configuration;
using Scintilla.Configuration.SciTE;

namespace MyGeneration
{
    public interface IScintillaEditControl : IMyGenDocument
    {
        IScintillaNet ScintillaEditor { get; }
    }
}
