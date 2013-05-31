using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

using Zeus;
using Zeus.Configuration;
using MyGeneration;

namespace MyGenVS2005
{
    public partial class AddInTemplateBrowser : Form
    {
        private ZeusProcessStatusDelegate processCallback;
        private DTE2 _application;

        public AddInTemplateBrowser(DTE2 application)
        {
            _application = application;

            InitializeComponent();

            processCallback = new ZeusProcessStatusDelegate(ProcessOperation);
            this.templateBrowserControl1.Initialize();
        }

        private void templateBrowserControl1_ErrorsOccurred(object sender, EventArgs e)
        {
            if (sender is Exception)
            {
                AddInErrorForm errorForm = new AddInErrorForm(sender as Exception);
                errorForm.ShowDialog(this.ParentForm);
            }
        }

        private void templateBrowserControl1_TemplateOpen(object sender, EventArgs e)
        {
            if (this.checkBoxOpenTemplate.Checked)
            {
                String path = sender as String;
                if (!string.IsNullOrEmpty(path))
                {
                    _application.ItemOperations.OpenFile(path, EnvDTE.Constants.vsViewKindPrimary);
                }
            }
        }

        private void templateBrowserControl1_GeneratedFileSaved(object sender, EventArgs e)
        {
            if (this.checkBoxOpenFile.Checked)
            {
                String path = sender as String;
                if (!string.IsNullOrEmpty(path))
                {
                    _application.ItemOperations.OpenFile(path, EnvDTE.Constants.vsViewKindPrimary);
                }
            }
        }


        private bool templateBrowserControl1_ExecuteOverride(TemplateOperations operation, ZeusTemplate template, ZeusSavedInput input, ShowGUIEventHandler guiEventHandler)
        {
            switch (operation)
            {
                case TemplateOperations.Execute:
                    ZeusProcessManager.ExecuteTemplate(template.FullFileName, processCallback);
                    break;
                case TemplateOperations.ExecuteLoadedInput:
                    ZeusProcessManager.ExecuteSavedInput(input.FilePath, processCallback);
                    break;
                case TemplateOperations.SaveInput:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
                    saveFileDialog.FilterIndex = 0;
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ZeusProcessManager.RecordTemplateInput(template.FullFileName, saveFileDialog.FileName, processCallback);
                    }
                    break;
            }
            return true;
        }

        private void ProcessOperation(ZeusProcessStatusEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(processCallback, args);
            }
            else
            {
                if (args.Message != null)
                {
                    try
                    {
                        if (args.Message.StartsWith("[GENERATED_FILE]") && (this.checkBoxOpenFile.Checked))
                        {
                            string filename = args.Message.Substring(16);
                            _application.ItemOperations.OpenFile(filename, EnvDTE.Constants.vsViewKindPrimary);
                        }
                        else
                        {
                            Application.DoEvents();

                            OutputWindow outwin = _application.ToolWindows.OutputWindow;
                            OutputWindowPane pane = null;

                            if (outwin.OutputWindowPanes.Count > 0)
                            {
                                foreach (OutputWindowPane tmp in outwin.OutputWindowPanes)
                                {
                                    if (tmp.Name == "MyGeneration")
                                    {
                                        pane = tmp;
                                        break;
                                    }
                                }
                            }

                            if (pane == null)
                            {
                                pane = outwin.ActivePane.Collection.Add("MyGeneration");
                            }

                            if (pane != null)
                            {
                                pane.Activate();
                                pane.OutputString(args.Message);
                                pane.OutputString(Environment.NewLine);
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }
}