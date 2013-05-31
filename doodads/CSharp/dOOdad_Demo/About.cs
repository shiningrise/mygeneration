using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace dOOdad_Demo
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.LinkLabel LinkLabel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public About()
		{
			InitializeComponent();
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
			this.btnClose = new System.Windows.Forms.Button();
			this.Label1 = new System.Windows.Forms.Label();
			this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(128, 56);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 12;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// Label1
			// 
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label1.Location = new System.Drawing.Point(16, 24);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(328, 24);
			this.Label1.TabIndex = 11;
			this.Label1.Text = "This demo runs against the SQL Server Northwind Database";
			// 
			// LinkLabel1
			// 
			this.LinkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LinkLabel1.Location = new System.Drawing.Point(32, 96);
			this.LinkLabel1.Name = "LinkLabel1";
			this.LinkLabel1.Size = new System.Drawing.Size(320, 16);
			this.LinkLabel1.TabIndex = 13;
			this.LinkLabel1.TabStop = true;
			this.LinkLabel1.Text = "Copyright (c) 2004-2005 MyGeneration Software";
			this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
			// 
			// About
			// 
#if(VS2005)
			this.AutoScaleDimensions = new System.Drawing.Size(5, 13);
#else
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
#endif
			this.ClientSize = new System.Drawing.Size(362, 136);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.LinkLabel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About dOOdads";
			this.ResumeLayout(false);

		}
		#endregion

		private void LinkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.mygenerationsoftware.com");
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;		
		}
	}
}
