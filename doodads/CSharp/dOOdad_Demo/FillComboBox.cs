using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace dOOdad_Demo
{
	/// <summary>
	/// Summary description for FillComboBox.
	/// </summary>
	public class FillComboBox : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.ComboBox cmbBox;
		internal System.Windows.Forms.Button bntClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FillComboBox()
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
			this.cmbBox = new System.Windows.Forms.ComboBox();
			this.bntClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmbBox
			// 
			this.cmbBox.Location = new System.Drawing.Point(24, 24);
			this.cmbBox.Name = "cmbBox";
			this.cmbBox.Size = new System.Drawing.Size(320, 21);
			this.cmbBox.TabIndex = 3;
			// 
			// bntClose
			// 
			this.bntClose.Location = new System.Drawing.Point(264, 96);
			this.bntClose.Name = "bntClose";
			this.bntClose.TabIndex = 2;
			this.bntClose.Text = "Close";
			this.bntClose.Click += new System.EventHandler(this.bntClose_Click);
			// 
			// FillComboBox
			// 
#if(VS2005)
			this.AutoScaleDimensions = new System.Drawing.Size(5, 13);
#else
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
#endif
			this.ClientSize = new System.Drawing.Size(360, 165);
			this.Controls.Add(this.cmbBox);
			this.Controls.Add(this.bntClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FillComboBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Fill a ComboBox using a dOOdad";
			this.Load += new System.EventHandler(this.FillComboBox_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void bntClose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;		
		}

		private void FillComboBox_Load(object sender, System.EventArgs e)
		{
			//-----------------------------------------------------
			// ** dOOdad Tip **
			//-----------------------------------------------------
			// You will find that a dOOdad can do almost anything, no need to write a million little 
			// specific stored procedures, this code limits the columns, sorts, and fills a combo-box
			// there's nothing to it

			// Let's query on the Product Name and sort it
			Products prds = new Products();

			// Note we only bring back these two columns for performance, why bring back more?
			prds.Query.AddResultColumn(Products.ColumnNames.ProductID);
			prds.Query.AddResultColumn(Products.ColumnNames.ProductName);

			// Sort
			prds.Query.AddOrderBy(Products.ColumnNames.ProductName, MyGeneration.dOOdads.WhereParameter.Dir.ASC);

			// Load it
			try
			{
				prds.Query.Load();
			}
			catch {}


			// Bind it
			this.cmbBox.DisplayMember = Products.ColumnNames.ProductName;
			this.cmbBox.DataSource = prds.DefaultView;
		}
	}
}
