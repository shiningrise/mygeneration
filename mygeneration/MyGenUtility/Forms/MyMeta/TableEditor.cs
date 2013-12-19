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

        public TableEditor(IMyGenerationMDI mdi)
        {
            InitializeComponent();
            this.mdi = mdi;
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
        //    this.ShowHint = DockState.DockRight;
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

        private void Clear()
        {
            this.tableEditorControl1.ClearDataSource();
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
    }
}
