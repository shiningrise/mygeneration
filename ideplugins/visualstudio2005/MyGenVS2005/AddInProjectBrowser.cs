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

namespace MyGenVS2005
{
    public partial class AddInProjectBrowser : Form
    {
        private DTE2 _application;

        public AddInProjectBrowser(DTE2 application)
        {
            _application = application;

            InitializeComponent();

            this.projectBrowserControl1.TabTextChanged += new MyGeneration.TextChangedEventHandler(projectBrowserControl1_TextChanged);

            string projectFileToOpen = null;
            UIHierarchy UIH = _application.ToolWindows.SolutionExplorer;
            Array sitems = UIH.SelectedItems as Array;
            foreach (UIHierarchyItem item in sitems)
            {
                string name = item.Name;
                if (name.EndsWith(".zprj", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProjectItem prj = item.Object as ProjectItem;
                    if (prj.FileCount > 0)
                    {
                        projectFileToOpen = prj.get_FileNames(0);
                    }
                    if (projectFileToOpen != null) break;
                }
            }

            if (projectFileToOpen == null)
            {
                projectFileToOpen = TraverseItems(UIH.UIHierarchyItems);
            }

            if (projectFileToOpen == null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = ".zprj";
                ofd.Filter = "Zeus Project Files (*.zprj)|*.zprj|All files (*.*)|*.*";
                if (ofd.ShowDialog(System.Windows.Forms.Form.ActiveForm) == DialogResult.OK)
                {
                    if (ofd.FileName.EndsWith(".zprj", StringComparison.CurrentCultureIgnoreCase))
                    {
                        projectFileToOpen = ofd.FileName;
                    }
                }
            }

            if (projectFileToOpen != null)
            {
                this.projectBrowserControl1.LoadProject(projectFileToOpen);
            }
        }

        private string TraverseItems(UIHierarchyItems items)
        {
            string returnVal = null;

            UIHierarchyItem item;
            for (int i = 0; i < items.Count; i++)
            {
                item = items.Item(i+1);
                string name = item.Name;
                if (name.EndsWith(".zprj", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProjectItem prj = item.Object as ProjectItem;
                    for (short j = 0; j < prj.FileCount; j++)
                    {
                        returnVal = prj.get_FileNames(j);
                        break;
                    }
                    if (returnVal != null) break;
                }

                if ((returnVal == null) && (item.UIHierarchyItems != null))
                {
                    returnVal = TraverseItems(item.UIHierarchyItems);
                }

                if (returnVal != null) break;
            }

            return returnVal;
        }

        private void projectBrowserControl1_TextChanged(string text, string tabText, string filename)
        {
            this.Text = tabText;
        }

        private void projectBrowserControl1_ErrorsOccurred(object sender, EventArgs e)
        {
            if (sender is Exception)
            {
                AddInErrorForm errorForm = new AddInErrorForm(sender as Exception);
                errorForm.ShowDialog(this.ParentForm);
            }
        }

        private void projectBrowserControl1_ExecutionStatusUpdate(bool isRunning, string message)
        {
            if (message != null)
            {
                try
                {
                    if (message.StartsWith("[GENERATED_FILE]"))
                    {
                        string filename = message.Substring(16);
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
                            pane.OutputString(message);
                            pane.OutputString(Environment.NewLine);
                        }
                    }
                }
                catch { }
            }
        }
    }
}