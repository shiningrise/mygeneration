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
    public interface IMyGenErrorList : IMyGenContent
    {
        void AddErrors(params Exception[] exceptions);
        void AddErrors(params IMyGenError[] errors);
        List<IMyGenError> Errors { get; }      
    }
}
