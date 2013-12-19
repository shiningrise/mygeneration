namespace MyGeneration
{
    partial class TableEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableEditorControl1 = new MyGeneration.UI.Plugins.EntityManager.TableEditorControl();
            this.SuspendLayout();
            // 
            // tableEditorControl1
            // 
            this.tableEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.tableEditorControl1.Name = "tableEditorControl1";
            this.tableEditorControl1.Size = new System.Drawing.Size(839, 642);
            this.tableEditorControl1.TabIndex = 0;
            // 
            // TableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 642);
            this.Controls.Add(this.tableEditorControl1);
            this.Name = "TableEditor";
            this.Text = "TableEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Plugins.EntityManager.TableEditorControl tableEditorControl1;
    }
}