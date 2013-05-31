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
    public enum MyGenErrorClass
    {
        Application = 0,
        Template
    }

    public interface IMyGenError
    {
        DateTime DateTimeOccurred { get; }
        string UniqueIdentifier { get; }
        MyGenErrorClass Class { get; }
        bool IsWarning { get; }
        bool IsRuntime { get; }
        bool IsTemplateCodeSegment { get; }
        Guid ErrorGuid { get; }
        string TemplateFileName { get; }
        string TemplateIdentifier { get; }
        string ErrorNumber { get; }
        string ErrorType { get; }
        string SourceFile { get; }
        string SourceLine { get; }
        int LineNumber { get; }
        int ColumnNumber { get; }
        string Message { get; }
        string Detail { get; }
    }
}
