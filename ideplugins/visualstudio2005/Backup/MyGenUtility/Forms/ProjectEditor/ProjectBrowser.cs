using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.Projects;
using Zeus.Serializers;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public partial class ProjectBrowser : DockContent, IMyGenDocument
    {
        private bool _consoleWriteGeneratedDetails = false;
        private IMyGenerationMDI _mdi;

        public ProjectBrowser(IMyGenerationMDI mdi)
        {
            this._mdi = mdi;
            this._consoleWriteGeneratedDetails = DefaultSettings.Instance.ConsoleWriteGeneratedDetails;
            InitializeComponent();
            this.projectBrowserControl1.ExecutionStarted += new EventHandler(projectBrowserControl1_ExecutionStarted);
        }

        protected override string GetPersistString()
        {
            return this.projectBrowserControl1.GetPersistString();
        }

        public bool CanClose(bool allowPrevent)
        {
            return projectBrowserControl1.CanClose(allowPrevent);
        }


        #region Load Project Tree
        public void CreateNewProject()
        {
            this.projectBrowserControl1.CreateNewProject();
        }

        public void LoadProject(string filename)
        {
            this.projectBrowserControl1.LoadProject(filename);
        }
        #endregion

        #region Main Menu Event Handlers
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Execute();
        }
        #endregion

        #region ToolStrip Event Handlers
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Save();
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {

            this.projectBrowserControl1.SaveAs();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Edit();
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Execute();
        }
        #endregion

        #region IMyGenDocument Members

        public string DocumentIndentity
        {
            get
            {
                return this.projectBrowserControl1.DocumentIndentity;
            }
        }

        public ToolStrip ToolStrip
        {
            get { return this.toolStripOptions; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            {
                this._consoleWriteGeneratedDetails = DefaultSettings.Instance.ConsoleWriteGeneratedDetails;
            }
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        public string TextContent
        {
            get { return this.projectBrowserControl1.CompleteFilePath; }
        }

        #endregion

        #region ProjectBrowser Event Handlers
        private void ProjectBrowser_MouseLeave(object sender, System.EventArgs e)
        {
            //this.toolTipProjectBrowser.SetToolTip(treeViewProject, string.Empty);
            this.projectBrowserControl1.SetToolTip(string.Empty);
        }

        void ProjectBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) ||
                (e.CloseReason == CloseReason.FormOwnerClosing))
            {
                if (!this.CanClose(true))
                {
                    e.Cancel = true;
                }
            }
        }

        private void projectBrowserControl1_ExecutionStarted(object sender, EventArgs e)
        {
            this._mdi.PerformMdiFuntion(this, "ExecutionQueueStart");
        }

        private void projectBrowserControl1_ErrorsOccurred(object sender, EventArgs e)
        {
            this._mdi.ErrorsOccurred(sender as Exception);
        }

        private void projectBrowserControl1_ExecutionStatusUpdate(bool isRunning, string message)
        {
            if (this._mdi.Console.DockContent.IsHidden) this._mdi.Console.DockContent.Show(_mdi.DockPanel);
            if (!this._mdi.Console.DockContent.IsActivated) this._mdi.Console.DockContent.Activate();

            if (message.StartsWith(ZeusProcessManager.GENERATED_FILE_TAG))
            {
                string generatedFile = message.Substring(ZeusProcessManager.GENERATED_FILE_TAG.Length);
                this._mdi.WriteConsole("File Generated: " + generatedFile);
                this._mdi.SendAlert(this, "FileGenerated", generatedFile);

            } else if (message.StartsWith(ZeusProcessManager.ERROR_TAG)) {
                string error = message.Substring(ZeusProcessManager.ERROR_TAG.Length);
                this._mdi.ErrorList.AddErrors(new MyGenError() {
                    Class = MyGenErrorClass.Template,
                    Message = "Error processing template",
                    Detail = error,
                });

            } else {
                if (_consoleWriteGeneratedDetails) this._mdi.WriteConsole(message);
            }

            if (!isRunning)
            {
                this._mdi.PerformMdiFuntion(this, "ExecutionQueueUpdate");
            }

        }

        void projectBrowserControl1_TabTextChanged(string text, string tabText, string filename)
        {
            this.TabText = tabText;
            this._mdi.SendAlert(this, "UpdateProject", text, filename);
        }
        #endregion
    }
}