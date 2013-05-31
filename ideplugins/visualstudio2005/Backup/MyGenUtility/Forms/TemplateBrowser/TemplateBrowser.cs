using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Zeus;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration
{
    public partial class TemplateBrowser : DockContent, IMyGenContent
    {
        private bool _consoleWriteGeneratedDetails = false;
        private IMyGenerationMDI _mdi;
        private ZeusProcessStatusDelegate _executionCallback;

        public TemplateBrowser(IMyGenerationMDI mdi)
        {
            this._mdi = mdi;
            this._executionCallback = new ZeusProcessStatusDelegate(ExecutionCallback);
            this._consoleWriteGeneratedDetails = DefaultSettings.Instance.ConsoleWriteGeneratedDetails;
            this.DockPanel = mdi.DockPanel;

            InitializeComponent();

            this.templateBrowserControl.Initialize();
            if (DefaultSettings.Instance.ExecuteFromTemplateBrowserAsync)
            {
                this.templateBrowserControl.ExecuteTemplateOverride = new ExecuteTemplateDelegate(ExecuteTemplateOverride);
            }
        }

        private bool ExecuteTemplateOverride(TemplateOperations operation, ZeusTemplate template, ZeusSavedInput input, ShowGUIEventHandler guiEventHandler)
        {
            switch (operation)
            {
                case TemplateOperations.Execute:
                    this._mdi.PerformMdiFuntion(this, "ExecutionQueueStart");
                    ZeusProcessManager.ExecuteTemplate(template.FullFileName, _executionCallback);
                    break;
                case TemplateOperations.ExecuteLoadedInput:
                    this._mdi.PerformMdiFuntion(this, "ExecutionQueueStart");
                    ZeusProcessManager.ExecuteSavedInput(input.FilePath, _executionCallback);
                    break;
                case TemplateOperations.SaveInput:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
                    saveFileDialog.FilterIndex = 0;
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this._mdi.PerformMdiFuntion(this, "ExecutionQueueStart");
                        ZeusProcessManager.RecordTemplateInput(template.FullFileName, saveFileDialog.FileName, _executionCallback);
                    }
                    break;
            }
            return true;
        }

        private void ExecutionCallback(ZeusProcessStatusEventArgs args)
        {
            if (args.Message != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(_executionCallback, args);
                }
                else
                {
                    if (_consoleWriteGeneratedDetails)
                    {
                        if (this._mdi.Console.DockContent.IsHidden) this._mdi.Console.DockContent.Show(_mdi.DockPanel);
                        if (!this._mdi.Console.DockContent.IsActivated) this._mdi.Console.DockContent.Activate();
                    }

                    if (args.Message.StartsWith(ZeusProcessManager.GENERATED_FILE_TAG))
                    {
                        string generatedFile = args.Message.Substring(ZeusProcessManager.GENERATED_FILE_TAG.Length);
                        this._mdi.WriteConsole("File Generated: " + generatedFile);
                        this._mdi.SendAlert(this, "FileGenerated", generatedFile);
                    }
                    else
                    {
                        if (_consoleWriteGeneratedDetails) this._mdi.WriteConsole(args.Message);
                    }

                    if (!args.IsRunning)
                    {
                        this._mdi.PerformMdiFuntion(this, "ExecutionQueueUpdate");
                    }
                }
            }
        }

        private void templateBrowserControl_ExecutionStatusUpdate(bool isRunning, string message)
        {
            if (_consoleWriteGeneratedDetails)
            {
                if (this._mdi.Console.DockContent.IsHidden) this._mdi.Console.DockContent.Show(_mdi.DockPanel);
                if (!this._mdi.Console.DockContent.IsActivated) this._mdi.Console.DockContent.Activate();
            }

            if (message.StartsWith(ZeusProcessManager.GENERATED_FILE_TAG))
            {
                string generatedFile = message.Substring(ZeusProcessManager.GENERATED_FILE_TAG.Length);
                this._mdi.WriteConsole("File Generated: " + generatedFile);
                this._mdi.SendAlert(this, "FileGenerated", generatedFile);
            }
            else
            {
                if (_consoleWriteGeneratedDetails) this._mdi.WriteConsole(message);
            }
        }

        private void templateBrowserControl_ErrorsOccurred(object sender, EventArgs e)
        {
            if (sender is Exception)
            {
                this._mdi.ErrorsOccurred(sender as Exception);
            }
        }

        private void templateBrowserControl_TemplateOpen(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this._mdi.OpenDocuments(sender.ToString());
            }
        }

        private void templateBrowserControl_TemplateUpdate(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this._mdi.SendAlert(this, "UpdateTemplate", sender.ToString());
            }
        }

        private void templateBrowserControl_TemplateDelete(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this._mdi.SendAlert(this, "DeleteTemplate", sender.ToString());
            }
        }

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            DefaultSettings settings = DefaultSettings.Instance;
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            {
                this._consoleWriteGeneratedDetails = settings.ConsoleWriteGeneratedDetails;
                bool doRefresh = false;

                if (DefaultSettings.Instance.ExecuteFromTemplateBrowserAsync)
                    this.templateBrowserControl.ExecuteTemplateOverride = new ExecuteTemplateDelegate(ExecuteTemplateOverride);
                else
                    this.templateBrowserControl.ExecuteTemplateOverride = null;

                try
                {
                    if (this.templateBrowserControl.TreeBuilder.DefaultTemplatePath != settings.DefaultTemplateDirectory)
                        doRefresh = true;
                }
                catch
                {
                    doRefresh = true;
                }

                if (doRefresh)
                    templateBrowserControl.RefreshTree();
            }
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
    }
}