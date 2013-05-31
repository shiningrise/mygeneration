using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace dOOdad_Demo
{
	/// <summary>
	/// Summary description for EmployeesEdit.
	/// </summary>
	public class EmployeesEdit : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.TextBox txtLastName;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.TextBox txtFirstName;
		internal System.Windows.Forms.Label Label2;

		Employees emps = new Employees();

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EmployeesEdit(int EmployeeID)
		{
			InitializeComponent();

			// Normally, I would use Employees.LoadByPrimaryKey but for this demo, if the user hasn't
			// generated the stored procs this method will work.
			emps.Where.EmployeeID.Value = EmployeeID;
			emps.Query.Load();

			//emps.LoadByPrimaryKey(EmployeeID);

			// NOTE: We could do this but we'll data bind instead
			//Me.txtFirstName.Text = emps.FirstName
			//Me.txtLastName.Text = emps.LastName

			// Bind to the string properties
			this.txtFirstName.DataBindings.Add(new Binding("Text", emps, Employees.StringPropertyNames.FirstName));
			this.txtLastName.DataBindings.Add(new Binding("Text", emps, Employees.StringPropertyNames.LastName));

			this.Text = "Northwind.Employee : ID = [" + emps.EmployeeID.ToString() + "]";
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(189, 112);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 13;
			this.btnOK.Text = "Save";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(101, 112);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtLastName
			// 
			this.txtLastName.Location = new System.Drawing.Point(109, 64);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(152, 20);
			this.txtLastName.TabIndex = 11;
			this.txtLastName.Text = "";
			// 
			// Label3
			// 
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label3.Location = new System.Drawing.Point(29, 64);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(72, 23);
			this.Label3.TabIndex = 10;
			this.Label3.Text = "Last Name:";
			// 
			// txtFirstName
			// 
			this.txtFirstName.Location = new System.Drawing.Point(109, 16);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(152, 20);
			this.txtFirstName.TabIndex = 9;
			this.txtFirstName.Text = "";
			// 
			// Label2
			// 
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label2.Location = new System.Drawing.Point(29, 16);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(72, 23);
			this.Label2.TabIndex = 8;
			this.Label2.Text = "First Name:";
			// 
			// EmployeesEdit
			// 
#if(VS2005)
			this.AutoScaleDimensions = new System.Drawing.Size(5, 13);
#else
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
#endif
			this.ClientSize = new System.Drawing.Size(292, 149);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.txtLastName);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.txtFirstName);
			this.Controls.Add(this.Label2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EmployeesEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EmployeesEdit";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				emps.Save();
			}
			catch
			{
				MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.");
			}

			this.DialogResult = DialogResult.OK;
		}
	}
}
