namespace MyGeneration.Forms
{
    partial class ApplicationReleasesControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationReleasesControl));
            this.dataGridViewUpdates = new System.Windows.Forms.DataGridView();
            this.ColumnTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReleaseNotes = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnDownload = new System.Windows.Forms.DataGridViewImageColumn();
            this.labelApplication = new System.Windows.Forms.Label();
            this.timerImgAnimate = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.pictureBoxAnimation = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnimation)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewUpdates
            // 
            this.dataGridViewUpdates.AllowUserToAddRows = false;
            this.dataGridViewUpdates.AllowUserToDeleteRows = false;
            this.dataGridViewUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewUpdates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewUpdates.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewUpdates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUpdates.ColumnHeadersVisible = false;
            this.dataGridViewUpdates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTitle,
            this.ColumnReleaseNotes,
            this.ColumnDownload});
            this.dataGridViewUpdates.Location = new System.Drawing.Point(3, 28);
            this.dataGridViewUpdates.Name = "dataGridViewUpdates";
            this.dataGridViewUpdates.ReadOnly = true;
            this.dataGridViewUpdates.RowHeadersVisible = false;
            this.dataGridViewUpdates.RowHeadersWidth = 20;
            this.dataGridViewUpdates.Size = new System.Drawing.Size(393, 257);
            this.dataGridViewUpdates.TabIndex = 0;
            this.dataGridViewUpdates.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewUpdates_CellClick);
            // 
            // ColumnTitle
            // 
            this.ColumnTitle.FillWeight = 215.7641F;
            this.ColumnTitle.HeaderText = "Title";
            this.ColumnTitle.Name = "ColumnTitle";
            this.ColumnTitle.ReadOnly = true;
            // 
            // ColumnReleaseNotes
            // 
            this.ColumnReleaseNotes.FillWeight = 31.24466F;
            this.ColumnReleaseNotes.HeaderText = "";
            this.ColumnReleaseNotes.Image = global::MyGeneration.Properties.Resources.release_notes;
            this.ColumnReleaseNotes.MinimumWidth = 16;
            this.ColumnReleaseNotes.Name = "ColumnReleaseNotes";
            this.ColumnReleaseNotes.ReadOnly = true;
            // 
            // ColumnDownload
            // 
            this.ColumnDownload.FillWeight = 31.16381F;
            this.ColumnDownload.HeaderText = "";
            this.ColumnDownload.Image = ((System.Drawing.Image)(resources.GetObject("ColumnDownload.Image")));
            this.ColumnDownload.MinimumWidth = 16;
            this.ColumnDownload.Name = "ColumnDownload";
            this.ColumnDownload.ReadOnly = true;
            // 
            // labelApplication
            // 
            this.labelApplication.AutoSize = true;
            this.labelApplication.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplication.Location = new System.Drawing.Point(3, 7);
            this.labelApplication.Name = "labelApplication";
            this.labelApplication.Size = new System.Drawing.Size(229, 14);
            this.labelApplication.TabIndex = 1;
            this.labelApplication.Text = "MyGeneration Releases on SourceForge";
            // 
            // timerImgAnimate
            // 
            this.timerImgAnimate.Interval = 75;
            this.timerImgAnimate.Tick += new System.EventHandler(this.timerImgAnimate_Tick);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.FillWeight = 31.24466F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::MyGeneration.Properties.Resources.release_notes;
            this.dataGridViewImageColumn1.MinimumWidth = 16;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 28;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.FillWeight = 31.16381F;
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn2.Image")));
            this.dataGridViewImageColumn2.MinimumWidth = 16;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Width = 30;
            // 
            // pictureBoxAnimation
            // 
            this.pictureBoxAnimation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxAnimation.Image = global::MyGeneration.Properties.Resources.Refresh16x16_1;
            this.pictureBoxAnimation.Location = new System.Drawing.Point(378, 3);
            this.pictureBoxAnimation.Name = "pictureBoxAnimation";
            this.pictureBoxAnimation.Size = new System.Drawing.Size(18, 19);
            this.pictureBoxAnimation.TabIndex = 2;
            this.pictureBoxAnimation.TabStop = false;
            this.pictureBoxAnimation.Click += new System.EventHandler(this.pictureBoxAnimation_Click);
            // 
            // ApplicationReleasesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxAnimation);
            this.Controls.Add(this.labelApplication);
            this.Controls.Add(this.dataGridViewUpdates);
            this.Name = "ApplicationReleasesControl";
            this.Size = new System.Drawing.Size(399, 288);
            this.Load += new System.EventHandler(this.ApplicationReleasesControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnimation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewUpdates;
        private System.Windows.Forms.Label labelApplication;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTitle;
        private System.Windows.Forms.DataGridViewImageColumn ColumnReleaseNotes;
        private System.Windows.Forms.DataGridViewImageColumn ColumnDownload;
        private System.Windows.Forms.PictureBox pictureBoxAnimation;
        private System.Windows.Forms.Timer timerImgAnimate;
    }
}
