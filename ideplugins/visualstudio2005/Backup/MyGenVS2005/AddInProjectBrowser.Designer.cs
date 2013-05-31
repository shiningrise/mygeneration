namespace MyGenVS2005
{
    partial class AddInProjectBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddInProjectBrowser));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.projectBrowserControl1 = new MyGeneration.ProjectBrowserControl();
            this.checkBoxOpenFile = new System.Windows.Forms.CheckBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.projectBrowserControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxOpenFile);
            this.splitContainer1.Size = new System.Drawing.Size(288, 464);
            this.splitContainer1.SplitterDistance = 429;
            this.splitContainer1.TabIndex = 1;
            // 
            // projectBrowserControl1
            // 
            this.projectBrowserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectBrowserControl1.Location = new System.Drawing.Point(0, 0);
            this.projectBrowserControl1.Name = "projectBrowserControl1";
            this.projectBrowserControl1.Size = new System.Drawing.Size(288, 429);
            this.projectBrowserControl1.TabIndex = 0;
            this.projectBrowserControl1.ExecutionStatusUpdate += new MyGeneration.ProjectExecutionStatusHandler(this.projectBrowserControl1_ExecutionStatusUpdate);
            this.projectBrowserControl1.ErrorsOccurred += new System.EventHandler(this.projectBrowserControl1_ErrorsOccurred);
            // 
            // checkBoxOpenFile
            // 
            this.checkBoxOpenFile.AutoSize = true;
            this.checkBoxOpenFile.Checked = true;
            this.checkBoxOpenFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOpenFile.Location = new System.Drawing.Point(12, 3);
            this.checkBoxOpenFile.Name = "checkBoxOpenFile";
            this.checkBoxOpenFile.Size = new System.Drawing.Size(199, 17);
            this.checkBoxOpenFile.TabIndex = 0;
            this.checkBoxOpenFile.Text = "Open generated files in Visual Studio";
            this.checkBoxOpenFile.UseVisualStyleBackColor = true;
            // 
            // AddInProjectBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 464);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddInProjectBrowser";
            this.Text = "MyGen Project Browser";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox checkBoxOpenFile;
        private MyGeneration.ProjectBrowserControl projectBrowserControl1;
    }
}