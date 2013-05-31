using System;
using System.Collections.Generic;
using System.Text;
using Scintilla;
using Scintilla.Forms;
using Scintilla.Configuration;
using Scintilla.Configuration.SciTE;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public interface IMyGenerationMDI
    {
        FindForm FindDialog { get; }
        ReplaceForm ReplaceDialog { get; }
        ScintillaConfigureDelegate ConfigureDelegate { get; }
        DockPanel DockPanel { get; }
    }
}