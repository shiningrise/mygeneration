using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration.Forms
{
    public partial class ConsoleForm : DockContent, IMyGenConsole
    {
        private IMyGenerationMDI mdi;
        private ZeusScintillaControl zcs;

        public ConsoleForm(IMyGenerationMDI mdi)
        {
            this.mdi = mdi;

            zcs = new ZeusScintillaControl();
            zcs.Dock = DockStyle.Fill;
            zcs.IsReadOnly = true;
            zcs.BringToFront();
            zcs.Name = "ScintillaConsole";
            this.Controls.Add(zcs);

            InitializeComponent();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            StringBuilder fname = new StringBuilder();
            fname.Append("ConsoleData_").Append(DateTime.Now.Year)
                .Append(DateTime.Now.Month.ToString().PadRight(2, '0'))
                .Append(DateTime.Now.Day.ToString().PadRight(2, '0'))
                .Append(DateTime.Now.Hour.ToString().PadRight(2, '0'))
                .Append(DateTime.Now.Minute.ToString().PadRight(2, '0'))
                .Append(DateTime.Now.Second.ToString().PadRight(2, '0'))
                .Append(DateTime.Now.Millisecond.ToString().PadRight(2, '0'))
                .Append(".txt");

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = fname.ToString();
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                System.IO.File.WriteAllText(sfd.FileName, this.zcs.Text);
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            zcs.IsReadOnly = false;
            this.zcs.ClearAll();
            zcs.IsReadOnly = true;
        }

        private void ConsoleForm_VisibleChanged(object sender, EventArgs e)
        {
            zcs.EnsureVisibleEnforcePolicy(zcs.LineCount);
            zcs.SelectionStart = zcs.CurrentPos;
            zcs.SelectionEnd = zcs.CurrentPos;
            zcs.ScrollCaret();
            
            zcs.GrabFocus();
        }

        #region IMyGenConsole Members

        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        public void Write(string text)
        {
            zcs.IsReadOnly = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now);
            sb.Append(" - ");
            sb.Append(text);
            if (!text.EndsWith(Environment.NewLine)) sb.Append(Environment.NewLine);

            zcs.AppendText(sb.ToString());
            zcs.GrabFocus();
            zcs.GotoLine(zcs.LineCount);
            zcs.EnsureVisibleEnforcePolicy(zcs.LineCount);
            zcs.SelectionStart = zcs.CurrentPos;
            zcs.SelectionEnd = zcs.CurrentPos;
            zcs.ScrollCaret();
            zcs.IsReadOnly = true;
        }

        public void Write(string text, params object[] args)
        {
            if (args.Length == 0)
                Write(text);
            else
                Write(string.Format(text, args));
        }

        public void Write(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ex.GetType().Name)
            .Append(": ")
            .Append(ex.Message)
            .Append(" - ")
            .Append(ex.StackTrace);

            Write(sb.ToString());
        }

        #endregion

    }
}