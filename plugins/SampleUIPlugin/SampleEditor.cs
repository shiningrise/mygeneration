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
    public partial class SampleEditor : DockContent, IMyGenDocument
    {
        IMyGenerationMDI mdi;
        List<Point> pointsLeft = new List<Point>();
        List<Point> pointsRight = new List<Point>();
        Image bg = null;
        string filename = string.Empty;
        int isDrawing = 0;

        public SampleEditor(IMyGenerationMDI mdi)
        {
            this.mdi = mdi;
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public void CreateNewImage() 
        {
        }

        public void LoadImage(string file)
        {
            filename = file;
            bg = new Bitmap(file);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {

        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override string GetPersistString()
        {
            if (!string.IsNullOrEmpty(this.filename))
            {
                return GetType().ToString() + "," + filename;
            }
            else
            {
                return "type," + SampleEditorManager.SAMPLE_IMAGE;
            }
        }

        #region IMyGenDocument Members

        public string DocumentIndentity
        {
            get { return (this.filename == string.Empty) ? "New Image" : this.filename; }
        }

        public ToolStrip ToolStrip
        {
            get { return this.toolStrip1; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        public string TextContent
        {
            get { return this.GetPersistString(); }
        }
        #endregion

        private void SampleEditor_MouseDown(object sender, MouseEventArgs e)
        {
            this.isDrawing = 0;
            if (e.Button == MouseButtons.Left)
            {
                this.isDrawing = 1;
                if (!pointsLeft.Contains(e.Location)) pointsLeft.Add(e.Location);
                this.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.isDrawing = 2;
                if (!pointsRight.Contains(e.Location)) pointsRight.Add(e.Location);
                this.Invalidate();
            }
        }

        private void SampleEditor_MouseUp(object sender, MouseEventArgs e)
        {
            this.isDrawing = 0;
        }

        private void SampleEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDrawing == 1)
            {
                if (!pointsLeft.Contains(e.Location)) pointsLeft.Add(e.Location);
                this.Invalidate();
            }
            else if (this.isDrawing == 2)
            {
                if (!pointsRight.Contains(e.Location)) pointsRight.Add(e.Location);
                this.Invalidate();
            }
        }

        private void SampleEditor_Paint(object sender, PaintEventArgs e)
        {
            if (bg != null) e.Graphics.DrawImage(this.bg, 0, 0);

            foreach (Point p in pointsLeft)
            {
                e.Graphics.DrawEllipse(Pens.Red, p.X, p.Y, 6, 6);
            }
            foreach (Point p in pointsRight)
            {
                e.Graphics.DrawEllipse(Pens.Blue, p.X, p.Y, 4, 4);
            }
        }
    }
}