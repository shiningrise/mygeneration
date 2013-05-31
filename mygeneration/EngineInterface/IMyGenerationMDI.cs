using System;
using System.Collections.Generic;
using System.Text;
using Scintilla.Forms;
using Scintilla;
using Zeus;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public interface IMyGenerationMDI
    {
        void OpenDocuments(params string[] filenames);
        void CreateDocument(params string[] args);
        bool IsDocumentOpen(string text, params IMyGenDocument[] docsToExclude);
        IMyGenDocument FindDocument(string text, params IMyGenDocument[] docsToExclude);
        FindForm FindDialog { get; }
        ReplaceForm ReplaceDialog { get; }
        ScintillaConfigureDelegate ConfigureDelegate { get; }
        IZeusController ZeusController { get; }
        DockPanel DockPanel { get; }
        void SendAlert(IMyGenContent sender, string command, params object[] args);
        object PerformMdiFuntion(IMyGenContent sender, string function, params object[] args);

        IMyGenConsole Console { get; }
        IMyGenErrorList ErrorList { get; }
        void WriteConsole(string text, params object[] args);
        void ErrorsOccurred(params Exception[] ex);
    }
}
