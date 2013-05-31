using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Text;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for UpdatesForm.
	/// </summary>
	public class UpdatesForm : System.Windows.Forms.Form
    {
		private System.Windows.Forms.TextBox txtChanges;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblVersions;
		private System.Windows.Forms.Button btnUpgrade;
        private System.Windows.Forms.Button btnCancel;
        private AboutBoxLogo fun1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UpdatesForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.txtChanges = new System.Windows.Forms.TextBox();
            this.btnUpgrade = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersions = new System.Windows.Forms.Label();
            this.fun1 = new MyGeneration.AboutBoxLogo();
            this.SuspendLayout();
            // 
            // txtChanges
            // 
            this.txtChanges.AcceptsReturn = true;
            this.txtChanges.AcceptsTab = true;
            this.txtChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChanges.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChanges.Location = new System.Drawing.Point(8, 152);
            this.txtChanges.Multiline = true;
            this.txtChanges.Name = "txtChanges";
            this.txtChanges.ReadOnly = true;
            this.txtChanges.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChanges.Size = new System.Drawing.Size(560, 240);
            this.txtChanges.TabIndex = 6;
            // 
            // btnUpgrade
            // 
            this.btnUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpgrade.Location = new System.Drawing.Point(496, 400);
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.Size = new System.Drawing.Size(75, 23);
            this.btnUpgrade.TabIndex = 7;
            this.btnUpgrade.Text = "&Upgrade";
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(408, 400);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Changes in new build:";
            // 
            // lblVersions
            // 
            this.lblVersions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersions.Location = new System.Drawing.Point(16, 96);
            this.lblVersions.Name = "lblVersions";
            this.lblVersions.Size = new System.Drawing.Size(544, 23);
            this.lblVersions.TabIndex = 10;
            this.lblVersions.Tag = "";
            this.lblVersions.Text = "You are running version (";
            // 
            // fun1
            // 
            this.fun1.BackColor = System.Drawing.Color.White;
            this.fun1.Location = new System.Drawing.Point(1, 1);
            this.fun1.Name = "fun1";
            this.fun1.Size = new System.Drawing.Size(575, 82);
            this.fun1.TabIndex = 5;
            this.fun1.Text = "fun1";
            // 
            // UpdatesForm
            // 
            this.AcceptButton = this.btnUpgrade;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(576, 430);
            this.Controls.Add(this.fun1);
            this.Controls.Add(this.lblVersions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpgrade);
            this.Controls.Add(this.txtChanges);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdatesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "A New Version is Available";
            this.Load += new System.EventHandler(this.UpdatesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void UpdatesForm_Load(object sender, System.EventArgs e)
        {
            this.fun1.Start();
			this.lblVersions.Text += ThisVersion + ") and version (" + NewVersion + ") is available.";
			this.txtChanges.Text = UpgradeText.Replace("\n", "\r\n");
			this.txtChanges.Select(0,0);
		}

		public string NewVersion  = "";
		public string ThisVersion = "";
		public string UpgradeText = "";

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnUpgrade_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;		
		}
	}
}
