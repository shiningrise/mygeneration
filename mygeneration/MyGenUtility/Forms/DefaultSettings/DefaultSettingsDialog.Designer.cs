namespace MyGeneration
{
    partial class DefaultSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefaultSettingsDialog));
            this.defaultSettingsControl = new MyGeneration.DefaultSettingsControl();
            this.SuspendLayout();
            // 
            // defaultSettingsControl
            // 
            this.defaultSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defaultSettingsControl.Location = new System.Drawing.Point(0, 0);
            this.defaultSettingsControl.MinimumSize = new System.Drawing.Size(601, 574);
            this.defaultSettingsControl.Name = "defaultSettingsControl";
            this.defaultSettingsControl.Size = new System.Drawing.Size(613, 576);
            this.defaultSettingsControl.TabIndex = 0;
            // 
            // DefaultSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 576);
            this.Controls.Add(this.defaultSettingsControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DefaultSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Default Settings";
            this.Load += new System.EventHandler(this.DefaultSettingsDialog_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefaultSettingsDialog_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private DefaultSettingsControl defaultSettingsControl;
    }
}