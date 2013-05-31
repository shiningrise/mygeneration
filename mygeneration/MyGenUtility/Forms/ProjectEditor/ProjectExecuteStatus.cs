using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for ProjectExecuteStatus.
	/// </summary>
	public class ProjectExecuteStatus : System.Windows.Forms.Form, ILog
	{
		private System.Windows.Forms.TextBox textBoxStatus;
		private System.Windows.Forms.Button buttonClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProjectExecuteStatus()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.textBoxStatus.Clear();
			this.Text = "Execution Status: Running";
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProjectExecuteStatus));
			this.textBoxStatus = new System.Windows.Forms.TextBox();
			this.buttonClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxStatus
			// 
			this.textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxStatus.BackColor = System.Drawing.Color.Black;
			this.textBoxStatus.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxStatus.ForeColor = System.Drawing.Color.Lime;
			this.textBoxStatus.Location = new System.Drawing.Point(8, 8);
			this.textBoxStatus.Multiline = true;
			this.textBoxStatus.Name = "textBoxStatus";
			this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxStatus.Size = new System.Drawing.Size(736, 304);
			this.textBoxStatus.TabIndex = 0;
			this.textBoxStatus.Text = "Test";
			this.textBoxStatus.WordWrap = false;
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.Enabled = false;
			this.buttonClose.Location = new System.Drawing.Point(688, 320);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(56, 23);
			this.buttonClose.TabIndex = 1;
			this.buttonClose.Text = "Close";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// ProjectExecuteStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(752, 350);
			this.ControlBox = false;
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.textBoxStatus);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ProjectExecuteStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Project Execution Status";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public bool Finished 
		{
			set 
			{
				this.buttonClose.Enabled = value;
				this.Text = "Execution Status: Completed";
			}
		}

		#region ILog Members

		public void Write(Exception ex)
		{
			string item = "[" + ex.GetType().Name + "] " + ex.Message;
			Write("**ERROR*** [{0}] {1}", ex.GetType().Name, ex.Message);
		}

		public void Write(string format, params object[] args)
		{
			string item = string.Format(format, args);
			Write(item);
		}

		public void Write(string text)
		{
			string item = DateTime.Now.ToString() + " - " + text;
			this.textBoxStatus.Text = this.textBoxStatus.Text + item + "\r\n";
			this.textBoxStatus.SelectionStart = this.textBoxStatus.Text.Length;
			this.textBoxStatus.ScrollToCaret();
			this.textBoxStatus.Invalidate();
			this.textBoxStatus.Refresh();
		}

		#endregion
	}
}
