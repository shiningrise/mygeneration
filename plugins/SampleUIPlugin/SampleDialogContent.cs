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
    public partial class SampleDialogContent : Form
    {
        private IMyGenerationMDI mdi;
        public SampleDialogContent(IMyGenerationMDI mdi)
        {
            this.mdi = mdi;
            InitializeComponent();

            if (!(this.mdi.DockPanel.ActiveDocument is IScintillaEditControl))
            {
                this.button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.mdi.DockPanel.ActiveDocument is IScintillaEditControl)
            {
                IScintillaEditControl editor = this.mdi.DockPanel.ActiveDocument as IScintillaEditControl;
                if (editor.ScintillaEditor.CurrentPos >= 0)
                {
                    editor.ScintillaEditor.InsertText(editor.ScintillaEditor.CurrentPos, this.textBox1.Text);
                }
            }
        }
    }
}