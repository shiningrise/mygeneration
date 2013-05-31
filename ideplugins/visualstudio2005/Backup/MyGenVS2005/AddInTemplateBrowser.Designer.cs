namespace MyGenVS2005
{
    partial class AddInTemplateBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddInTemplateBrowser));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.templateBrowserControl1 = new MyGeneration.TemplateBrowserControl();
            this.checkBoxOpenTemplate = new System.Windows.Forms.CheckBox();
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
            this.splitContainer1.Panel1.Controls.Add(this.templateBrowserControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxOpenTemplate);
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxOpenFile);
            this.splitContainer1.Size = new System.Drawing.Size(288, 464);
            this.splitContainer1.SplitterDistance = 411;
            this.splitContainer1.TabIndex = 1;
            // 
            // templateBrowserControl1
            // 
            this.templateBrowserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateBrowserControl1.Location = new System.Drawing.Point(0, 0);
            this.templateBrowserControl1.Name = "templateBrowserControl1";
            this.templateBrowserControl1.Size = new System.Drawing.Size(288, 411);
            this.templateBrowserControl1.TabIndex = 0;
            this.templateBrowserControl1.ErrorsOccurred += new System.EventHandler(this.templateBrowserControl1_ErrorsOccurred);
            this.templateBrowserControl1.TemplateOpen += new System.EventHandler(this.templateBrowserControl1_TemplateOpen);
            this.templateBrowserControl1.GeneratedFileSaved += new System.EventHandler(this.templateBrowserControl1_GeneratedFileSaved);
            this.templateBrowserControl1.ExecuteTemplateOverride = new MyGeneration.ExecuteTemplateDelegate(templateBrowserControl1_ExecuteOverride);
            // 
            // checkBoxOpenTemplate
            // 
            this.checkBoxOpenTemplate.AutoSize = true;
            this.checkBoxOpenTemplate.Location = new System.Drawing.Point(12, 26);
            this.checkBoxOpenTemplate.Name = "checkBoxOpenTemplate";
            this.checkBoxOpenTemplate.Size = new System.Drawing.Size(179, 17);
            this.checkBoxOpenTemplate.TabIndex = 1;
            this.checkBoxOpenTemplate.Text = "Open Templates in Visual Studio";
            this.checkBoxOpenTemplate.UseVisualStyleBackColor = true;
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
            // AddInTemplateBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 464);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddInTemplateBrowser";
            this.Text = "MyGen Template Browser";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyGeneration.TemplateBrowserControl templateBrowserControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox checkBoxOpenFile;
        private System.Windows.Forms.CheckBox checkBoxOpenTemplate;
    }
}