namespace MyGeneration.Forms
{
    partial class ApplicationReleases
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationReleases));
            this.applicationReleasesControl1 = new MyGeneration.Forms.ApplicationReleasesControl();
            this.SuspendLayout();
            // 
            // applicationReleasesControl1
            // 
            this.applicationReleasesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applicationReleasesControl1.Location = new System.Drawing.Point(0, 0);
            this.applicationReleasesControl1.Name = "applicationReleasesControl1";
            this.applicationReleasesControl1.Size = new System.Drawing.Size(600, 350);
            this.applicationReleasesControl1.TabIndex = 0;
            // 
            // ApplicationReleases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Controls.Add(this.applicationReleasesControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ApplicationReleases";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MyGeneration Releases";
            this.ResumeLayout(false);

        }

        #endregion

        private ApplicationReleasesControl applicationReleasesControl1;

    }
}