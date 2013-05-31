using System;
using System.Collections.Generic;
using System.Text;
using Scintilla.Forms;
using Scintilla.Configuration;
using Scintilla.Configuration.SciTE;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace MyGeneration
{
    public interface IMyGenDocument : IMyGenContent
    {
        string DocumentIndentity { get; }
        string TextContent { get; }
    }
}
