namespace MyGeneration
{
    partial class TemplateBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateBrowser));
            this.templateBrowserControl = new MyGeneration.TemplateBrowserControl();
            this.SuspendLayout();
            // 
            // templateBrowserControl
            // 
            this.templateBrowserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateBrowserControl.Location = new System.Drawing.Point(0, 0);
            this.templateBrowserControl.Name = "templateBrowserControl";
            this.templateBrowserControl.Size = new System.Drawing.Size(386, 677);
            this.templateBrowserControl.TabIndex = 0;
            this.templateBrowserControl.TemplateUpdate += new System.EventHandler(this.templateBrowserControl_TemplateUpdate);
            this.templateBrowserControl.TemplateDelete += new System.EventHandler(this.templateBrowserControl_TemplateDelete);
            this.templateBrowserControl.ErrorsOccurred += new System.EventHandler(this.templateBrowserControl_ErrorsOccurred);
            this.templateBrowserControl.TemplateOpen += new System.EventHandler(this.templateBrowserControl_TemplateOpen);
            // 
            // TemplateBrowser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(386, 677);
            this.ControlBox = false;
            this.Controls.Add(this.templateBrowserControl);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemplateBrowser";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.TabText = "Template Browser";
            this.Text = "Template Browser";
            this.ResumeLayout(false);

        }

        #endregion

        private TemplateBrowserControl templateBrowserControl;
    }
}