using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MyMeta;

namespace MyGeneration
{
	/// <summary>
	/// Summary description for AddDbTargetMappingDialog.
	/// </summary>
	public class AddDbTargetMappingDialog : System.Windows.Forms.Form
	{
		public string BasedUpon = string.Empty;
		public string NewDbTarget = string.Empty;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cboxBasedUpon;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public AddDbTargetMappingDialog(string[] dbTargets, string dbDriver)
		{
			InitializeComponent();

			this.Text += " for " + dbDriver;

			if(dbTargets != null)
			{
				for(int i = 0; i < dbTargets.Length; i++)
				{
					this.cboxBasedUpon.Items.Add(dbTargets[i]);
				}
			}
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboxBasedUpon = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTo = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cboxBasedUpon);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtTo);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(392, 88);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// cboxBasedUpon
			// 
			this.cboxBasedUpon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboxBasedUpon.Location = new System.Drawing.Point(96, 48);
			this.cboxBasedUpon.Name = "cboxBasedUpon";
			this.cboxBasedUpon.Size = new System.Drawing.Size(288, 21);
			this.cboxBasedUpon.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 23);
			this.label3.TabIndex = 6;
			this.label3.Text = "Based Upon:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtTo
			// 
			this.txtTo.Location = new System.Drawing.Point(96, 16);
			this.txtTo.Name = "txtTo";
			this.txtTo.Size = new System.Drawing.Size(288, 20);
			this.txtTo.TabIndex = 5;
			this.txtTo.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "DbTarget:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(240, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(328, 104);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// AddDbTargetMappingDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(414, 133);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddDbTargetMappingDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add New DbTarget Mapping";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			NewDbTarget = this.txtTo.Text;

			if(this.cboxBasedUpon.SelectedIndex >= 0)
			{													
				BasedUpon = this.cboxBasedUpon.SelectedItem.ToString();
			}

			if(NewDbTarget == string.Empty)
			{
				MessageBox.Show("You must supply a DbTarget", "Error");
				return;
			}
			else
			{
				string dbTarget = "";

				for(int i = 0; i < this.cboxBasedUpon.Items.Count; i++)
				{
					dbTarget = this.cboxBasedUpon.Items[i] as string;

					if(dbTarget == NewDbTarget)
					{
						MessageBox.Show(NewDbTarget + " already exits", "Error");
						return;
					}
				}
			}

			this.DialogResult = DialogResult.OK;		
		}
	}
}
