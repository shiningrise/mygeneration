using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public partial class TableEditor : DockContent, IMyGenContent
    {
        private IMyGenerationMDI mdi;
        private int _currentHashCode = Int32.MinValue;
        private MyGeneration.MetaDataBrowser _metaDataBrowser;

        public MetaDataBrowser MetaDataBrowser
        {
            set
            {
                _metaDataBrowser = value;
            }
        }

        public TableEditor(IMyGenerationMDI mdi)
        {
            InitializeComponent();
            this.mdi = mdi;
            //this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.ShowHint = DockState.DockRight;

        }

        #region IMyGenContent Members
        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            {
                this.Clear();
            }
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public void ResetMenu()
        {
            //
        }

        public DockContent DockContent
        {
            get { return this; }
        }
        #endregion

        private void TableEditor_Load(object sender, EventArgs e)
        {
            //this.dataGridView1.DataSource = null;
            //this.ClearOrRefresh();
        }

        private void Clear()
        {
            //this.dataGridView1.Columns.Clear();
            this.dataGridView1.DataSource = null;
        }

        internal void Edit(MyMeta.Table o)
        {
            var table = o;
            if (this._currentHashCode == table.GetHashCode()) return;

            if (table != null)
            {
                //ClearOrRefresh();
                Clear();
                this.Text = "实体编辑器-" + table.Name;
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = table.Columns;
            }
            else
                throw new Exception("TableEditorControl table is null");
            this._currentHashCode = table.GetHashCode();
        }

        public void ClearOrRefresh()
        {
            if (_currentHashCode != Int32.MinValue)
            {
                this.Refresh();
            }
            else
            {
                Clear();
            }
        }
    }
}
