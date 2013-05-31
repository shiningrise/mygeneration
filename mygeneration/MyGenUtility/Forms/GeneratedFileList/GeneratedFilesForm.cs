using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration.Forms
{
    public partial class GeneratedFilesForm : DockContent, IMyGenContent
    {
        private IMyGenerationMDI _mdi;
        private List<FileInfo> _generatedFiles = new List<FileInfo>();

        public GeneratedFilesForm(IMyGenerationMDI mdi)
        {
            this._mdi = mdi;
            InitializeComponent();
        }

        private void AddFile(FileInfo finf)
        {
            if (_generatedFiles.Count == 0) _generatedFiles.Add(finf);
            else _generatedFiles.Insert(0, finf);
            BindFiles();
        }

        private void BindFiles()
        {
            this.listView1.Items.Clear();
            for (int i = _generatedFiles.Count - 1; i >= 0; i--) 
            {
                FileInfo finf = _generatedFiles[i];
                ListViewItem item = new ListViewItem(finf.LastWriteTime.ToString());
                item.SubItems.Add(finf.Name);
                item.SubItems.Add(finf.FullName);
                item.Tag = finf;
                this.listView1.Items.Add(item);
            }
            Application.DoEvents();
            if (this.IsHidden)
            {
                this.Show(this._mdi.DockPanel);
                this.VisibleState = DockState.DockBottomAutoHide;
            }

            this.Activate();
            this.Refresh();
        }

        public FileInfo SelectedFile
        {
            get
            {
                if (this.listView1.SelectedItems.Count == 0) return null;
                return this.listView1.SelectedItems[0].Tag as FileInfo;
            }
        }

        public List<FileInfo> SelectedFiles
        {
            get
            {
                List<FileInfo> files = new List<FileInfo>();
                foreach (ListViewItem i in this.listView1.SelectedItems)
                {
                    files.Add(i.Tag as FileInfo);
                }
                return files;
            }
        }

        private void toolStripButtonViewDetail_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                this._mdi.PerformMdiFuntion(this, "OpenFile", this.SelectedFiles);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 1)
            {
                this._mdi.PerformMdiFuntion(this,
                    "OpenFile", this.SelectedFile);
            }
            else if (this.listView1.SelectedItems.Count > 1)
            {
                this._mdi.PerformMdiFuntion(this, "OpenFile", this.SelectedFiles);
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            this._generatedFiles.Clear();
            this.BindFiles();
        }

        private void ErrorsForm_SizeChanged(object sender, EventArgs e)
        {
            ResizeGridColumns();
        }

        private void ResizeGridColumns() 
        {
            int columnCount = this.listView1.Columns.Count;
            int targetWidth = this.listView1.Width;

            if (targetWidth == 0) return;

            int currentWidth = 0;
            List<int> individualWidths = new List<int>();
            bool ohCrapFlag = false;
            foreach (ColumnHeader c in this.listView1.Columns) 
            {
                if (c.Width == 0) ohCrapFlag = true;
                individualWidths.Add(c.Width);
                currentWidth += c.Width;
            }
            if (currentWidth == 0) return;
            else
            {
                if (ohCrapFlag)
                {
                    for (int i = 0; i < this.listView1.Columns.Count; i++)
                    {
                        individualWidths[i] = (targetWidth / columnCount);
                    }
                    currentWidth += individualWidths[0] * columnCount;
                }
                if (currentWidth != targetWidth && (currentWidth > 0))
                {
                    for (int i = 0; i < this.listView1.Columns.Count; i++)
                    {
                        ColumnHeader c = this.listView1.Columns[i];
                        c.Width = Convert.ToInt32((int)targetWidth * ((float)individualWidths[i] / (float)currentWidth));
                    }
                }
            }
        }

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command.Equals("FileGenerated", StringComparison.CurrentCultureIgnoreCase))
            {
                List<FileInfo> files = new List<FileInfo>();
                foreach (object arg in args)
                {
                    if (arg is FileInfo)
                    {
                        this.AddFile(arg as FileInfo);
                    }
                    else if (arg is String)
                    {
                        this.AddFile(new FileInfo(arg.ToString()));
                    }
                    else if (arg is List<FileInfo>)
                    {
                        foreach (FileInfo finf in (arg as List<FileInfo>))
                            this.AddFile(finf);
                    }
                    else if (arg is List<String>)
                    {
                        List<String> strs = arg as List<String>;
                        foreach (string s in strs)
                            this.AddFile(new FileInfo(s));
                    }
                }
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