using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Text;

namespace MyGeneration.CrazyErrors
{
	/// <summary>
	/// Summary description for ExceptionDialog.
	/// </summary>
	public class ExceptionDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtError;


		private Exception ex;
		private System.Windows.Forms.Button btnReportError;
		private System.Windows.Forms.TextBox textBox1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ExceptionDialog(Exception ex)
		{
			Assembly asmblyMyGen = System.Reflection.Assembly.GetAssembly(typeof(NewAbout));

			this.ex = ex;

			InitializeComponent();

			StringBuilder sb = new StringBuilder();
			

			sb.Append("MyGeneration".PadRight(15) + asmblyMyGen.GetName().Version.ToString() + "\r\n");
			sb.Append(DateTime.Now.ToString() + "\r\n" + "\r\n");


			Exception tmp = ex;
			while (tmp != null) 
			{
				sb.Append("------------------------------------------\r\n");
				sb.Append(ex.Message  + "\r\n" + "\r\n");
				sb.Append(ex.TargetSite.ToString() + "\r\n" + "\r\n");
				sb.Append("Call Stack");
				sb.Append(ex.ToString() + "\r\n" + "\r\n");

				tmp = ex.InnerException;
			}

			this.txtError.Text = sb.ToString();
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtError = new System.Windows.Forms.TextBox();
			this.btnReportError = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(592, 448);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			// 
			// txtError
			// 
			this.txtError.BackColor = System.Drawing.Color.White;
			this.txtError.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtError.Location = new System.Drawing.Point(24, 56);
			this.txtError.Multiline = true;
			this.txtError.Name = "txtError";
			this.txtError.ReadOnly = true;
			this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtError.Size = new System.Drawing.Size(640, 376);
			this.txtError.TabIndex = 2;
			this.txtError.Text = "";
			// 
			// btnReportError
			// 
			this.btnReportError.Location = new System.Drawing.Point(24, 440);
			this.btnReportError.Name = "btnReportError";
			this.btnReportError.Size = new System.Drawing.Size(112, 23);
			this.btnReportError.TabIndex = 3;
			this.btnReportError.Text = "Report Error";
			this.btnReportError.Click += new System.EventHandler(this.btnReportError_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.White;
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(24, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(240, 20);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "Support@MyGenerationSoftware.com";
			// 
			// ExceptionDialog
			// 
			this.AcceptButton = this.btnReportError;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(680, 477);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.btnReportError);
			this.Controls.Add(this.txtError);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExceptionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MyGeneration Exception";
			this.Load += new System.EventHandler(this.ExceptionDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnReportError_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("mailto:Support@MyGenerationSoftware.com?subject=Exception: "
				+ this.ex.Message + "&body=" + this.txtError.Text);
		}

		private void ExceptionDialog_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
