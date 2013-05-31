using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration.UI.Plugins.SqlTool
{
    public partial class SqlToolForm : DockContent, IMyGenDocument
    {
        private IMyGenerationMDI _mdi;
        private string _filename = string.Empty;

        public SqlToolForm(IMyGenerationMDI mdi)
        {
            this._mdi = mdi;
            this.DockAreas = DockAreas.Document;

            InitializeComponent();

            sqlToolUserControl1.Initialize(mdi, this.MainMenuStrip.Items);
            this.TabText = "New SQL File";
            this.Text = "New SQL File";
            BindForm();
        }

        public void Open(string fileName)
        {
            this._filename = fileName;
            this.sqlToolUserControl1.Open(fileName);
            if (!string.IsNullOrEmpty(sqlToolUserControl1.FileName))
            {
                this.TabText = sqlToolUserControl1.FileName;
                this.Text = sqlToolUserControl1.FileName;
            }
        }

        public void Save()
        {
            this.sqlToolUserControl1.Save();
            if (!string.IsNullOrEmpty(sqlToolUserControl1.FileName))
            {
                this.TabText = sqlToolUserControl1.FileName;
                this.Text = sqlToolUserControl1.FileName;
            }
        }

        public void SaveAs()
        {
            this.sqlToolUserControl1.SaveAs();
            if (!string.IsNullOrEmpty(sqlToolUserControl1.FileName))
            {
                this.TabText = sqlToolUserControl1.FileName;
                this.Text = sqlToolUserControl1.FileName;
            }
        }

        public void Execute()
        {
            if (toolStripComboBoxDatabase.SelectedIndex > -1)
                this.sqlToolUserControl1.SelectedDatabase = toolStripComboBoxDatabase.SelectedItem.ToString();

            this.sqlToolUserControl1.Execute();
        }

        public bool IsEmpty
        {
            get { return this.sqlToolUserControl1.IsEmpty; }
        }

        public bool IsNew
        {
            get { return this.sqlToolUserControl1.IsNew; }
        }

        protected override string GetPersistString()
        {
            if (!string.IsNullOrEmpty(_filename))
            {
                return "file," + this.sqlToolUserControl1.FileName;
            }
            else
            {
                return "type," + SqlToolEditorManager.SQL_FILE;
            }
        }

        private bool PromptForSave(bool allowPrevent)
        {
            bool canClose = true;

            if (sqlToolUserControl1.IsDirty)
            {
                DialogResult result;

                if (allowPrevent)
                {
                    result = MessageBox.Show("This file has been modified, Do you wish to save before closing?",
                        sqlToolUserControl1.FileName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                }
                else
                {
                    result = MessageBox.Show("This file has been modified, Do you wish to save before closing?",
                        sqlToolUserControl1.FileName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                switch (result)
                {
                    case DialogResult.Yes:
                        this.Save();
                        break;
                    case DialogResult.Cancel:
                        canClose = false;
                        break;
                }
            }

            return canClose;
        }

        private void SqlToolForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void toolStripComboBoxDatabase_Click(object sender, EventArgs e)
        {
            if (toolStripComboBoxDatabase.SelectedIndex > -1) 
                this.sqlToolUserControl1.SelectedDatabase = toolStripComboBoxDatabase.SelectedItem.ToString();
        }

        private void toolStripTextBoxSeperator_KeyUp(object sender, KeyEventArgs e)
        {
            this.sqlToolUserControl1.CommandSeperator = toolStripTextBoxSeperator.Text;
        }

        public void BindForm()
        {
            this.toolStripComboBoxDatabase.Items.Clear();
            this.toolStripTextBoxSeperator.Text = sqlToolUserControl1.CommandSeperator;

            bool showDbThingy = (this.sqlToolUserControl1.DatabaseNames.Count > 0);
            this.toolStripComboBoxDatabase.Visible = showDbThingy;
            this.toolStripLabelDatabase.Visible = showDbThingy;
            this.toolStripSeparatorDatabase.Visible = showDbThingy;

            if (showDbThingy)
            {

                foreach (string dbname in sqlToolUserControl1.DatabaseNames)
                {
                    int i = this.toolStripComboBoxDatabase.Items.Add(dbname);
                    if (dbname == this.sqlToolUserControl1.SelectedDatabase)
                    {
                        this.toolStripComboBoxDatabase.SelectedIndex = i;
                    }
                }

                if (this.sqlToolUserControl1.DatabaseNames.Count == 1)
                {
                    this.toolStripComboBoxDatabase.Enabled = false;
                }
            }
        }

        #region IMyGenDocument Members

        public ToolStrip ToolStrip
        {
            get { return this.toolStripSqlTool; }
        }

        public string DocumentIndentity
        {
            get { return sqlToolUserControl1.DocumentIndentity; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            {
                sqlToolUserControl1.RefreshConnectionInfo();
                BindForm();
            }
        }

        public bool CanClose(bool allowPrevent)
        {
            return PromptForSave(allowPrevent);
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        public string TextContent
        {
            get { return this.sqlToolUserControl1.TextContent; }
            set { this.sqlToolUserControl1.TextContent = value; }
        }

        #endregion

        #region Toolstrip Events
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            this.Execute();
        }
        #endregion

        #region Menu Events
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Execute();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sqlToolUserControl1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sqlToolUserControl1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sqlToolUserControl1.Paste();
        }
        #endregion
    }
}