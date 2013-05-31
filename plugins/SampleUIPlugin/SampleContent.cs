using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyGeneration;
using WeifenLuo.WinFormsUI.Docking;

namespace SampleUIPlugin
{
    public partial class SampleContent : DockContent, IMyGenContent
    {
        IMyGenerationMDI mdi;

        public SampleContent(IMyGenerationMDI mdi)
        {
            this.mdi = mdi;
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //
        }

        public bool AddToolbarIcon
        {
            get { return false; }
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public new DockContent DockContent
        {
            get { return this; }
        }

        #endregion
    }
}