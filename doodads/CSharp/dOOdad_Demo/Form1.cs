using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Globalization;

using MyGeneration.dOOdads;

namespace dOOdad_Demo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Button btnComboBoxFill;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		internal System.Windows.Forms.DataGrid DataGrid1;
		internal System.Windows.Forms.DataGridTextBoxColumn tbcFirstName;
		internal System.Windows.Forms.DataGridTextBoxColumn tcbLastName;
		internal System.Windows.Forms.DataGridTextBoxColumn tcbTitle;
		internal System.Windows.Forms.DataGridTextBoxColumn tcbHomePhone;
		internal System.Windows.Forms.Button btnSaveTransaction;
		internal System.Windows.Forms.Button btnEmployeesSave;
		internal System.Windows.Forms.Button btnEmployeesEditRow;
		private System.Windows.Forms.Panel panel1;
		internal System.Windows.Forms.Button btnAbout;
		internal System.Windows.Forms.DataGrid DataGrid2;
		internal System.Windows.Forms.DataGridTableStyle DataGridTableStyle2;
		internal System.Windows.Forms.DataGridTextBoxColumn tbcProductName;
		internal System.Windows.Forms.DataGridTextBoxColumn tcbUnitPrice;
		internal System.Windows.Forms.DataGridTextBoxColumn tcbUnitsInStock;
		internal System.Windows.Forms.DataGridBoolColumn tcbDiscontinued;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.Button btnEmployeeSearch;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.TextBox txtLastName;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox txtTitle;
		internal System.Windows.Forms.GroupBox grpDiscontinued;
		internal System.Windows.Forms.ComboBox cmbOperator;
		internal System.Windows.Forms.CheckBox chkDiscontinued;
		internal System.Windows.Forms.TextBox txtUnitPrice;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Button btnProductSearch;
		internal System.Windows.Forms.Button btnProductsSave;

		Employees emps = new Employees();
		Products prds  = new Products();
		private System.Windows.Forms.DataGridTextBoxColumn tcbEmpID;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
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
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.btnComboBoxFill = new System.Windows.Forms.Button();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.DataGrid1 = new System.Windows.Forms.DataGrid();
			this.tcbEmpID = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tbcFirstName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tcbLastName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tcbTitle = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tcbHomePhone = new System.Windows.Forms.DataGridTextBoxColumn();
			this.btnSaveTransaction = new System.Windows.Forms.Button();
			this.btnEmployeesSave = new System.Windows.Forms.Button();
			this.btnEmployeesEditRow = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnAbout = new System.Windows.Forms.Button();
			this.DataGrid2 = new System.Windows.Forms.DataGrid();
			this.DataGridTableStyle2 = new System.Windows.Forms.DataGridTableStyle();
			this.tbcProductName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tcbUnitPrice = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tcbUnitsInStock = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tcbDiscontinued = new System.Windows.Forms.DataGridBoolColumn();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.btnEmployeeSearch = new System.Windows.Forms.Button();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.grpDiscontinued = new System.Windows.Forms.GroupBox();
			this.cmbOperator = new System.Windows.Forms.ComboBox();
			this.chkDiscontinued = new System.Windows.Forms.CheckBox();
			this.txtUnitPrice = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.btnProductSearch = new System.Windows.Forms.Button();
			this.btnProductsSave = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.DataGrid1)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DataGrid2)).BeginInit();
			this.GroupBox1.SuspendLayout();
			this.grpDiscontinued.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnComboBoxFill
			// 
			this.btnComboBoxFill.Location = new System.Drawing.Point(624, 512);
			this.btnComboBoxFill.Name = "btnComboBoxFill";
			this.btnComboBoxFill.Size = new System.Drawing.Size(144, 23);
			this.btnComboBoxFill.TabIndex = 27;
			this.btnComboBoxFill.Text = "Fill a ComboBox";
			this.btnComboBoxFill.Click += new System.EventHandler(this.btnComboBoxFill_Click);
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.AlternatingBackColor = System.Drawing.Color.SkyBlue;
			this.dataGridTableStyle1.BackColor = System.Drawing.Color.LightSteelBlue;
			this.dataGridTableStyle1.DataGrid = this.DataGrid1;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.tcbEmpID,
																												  this.tbcFirstName,
																												  this.tcbLastName,
																												  this.tcbTitle,
																												  this.tcbHomePhone});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "Employees";
			// 
			// DataGrid1
			// 
			this.DataGrid1.AlternatingBackColor = System.Drawing.Color.LightSteelBlue;
			this.DataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.DataGrid1.BackColor = System.Drawing.Color.SteelBlue;
			this.DataGrid1.CaptionText = "Northwind.Employees";
			this.DataGrid1.DataMember = "";
			this.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.DataGrid1.Location = new System.Drawing.Point(8, 208);
			this.DataGrid1.Name = "DataGrid1";
			this.DataGrid1.Size = new System.Drawing.Size(472, 299);
			this.DataGrid1.TabIndex = 19;
			this.DataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								  this.dataGridTableStyle1});
			// 
			// tcbEmpID
			// 
			this.tcbEmpID.Format = "";
			this.tcbEmpID.FormatInfo = null;
			this.tcbEmpID.HeaderText = "ID";
			this.tcbEmpID.MappingName = "EmployeeID";
			this.tcbEmpID.Width = 50;
			// 
			// tbcFirstName
			// 
			this.tbcFirstName.Format = "";
			this.tbcFirstName.FormatInfo = null;
			this.tbcFirstName.HeaderText = "FirstName";
			this.tbcFirstName.MappingName = "FirstName";
			this.tbcFirstName.Width = 75;
			// 
			// tcbLastName
			// 
			this.tcbLastName.Format = "";
			this.tcbLastName.FormatInfo = null;
			this.tcbLastName.HeaderText = "LastName";
			this.tcbLastName.MappingName = "LastName";
			this.tcbLastName.Width = 80;
			// 
			// tcbTitle
			// 
			this.tcbTitle.Format = "";
			this.tcbTitle.FormatInfo = null;
			this.tcbTitle.HeaderText = "Title";
			this.tcbTitle.MappingName = "Title";
			this.tcbTitle.Width = 140;
			// 
			// tcbHomePhone
			// 
			this.tcbHomePhone.Format = "";
			this.tcbHomePhone.FormatInfo = null;
			this.tcbHomePhone.HeaderText = "HomePhone";
			this.tcbHomePhone.MappingName = "HomePhone";
			this.tcbHomePhone.Width = 75;
			// 
			// btnSaveTransaction
			// 
			this.btnSaveTransaction.Location = new System.Drawing.Point(8, 544);
			this.btnSaveTransaction.Name = "btnSaveTransaction";
			this.btnSaveTransaction.Size = new System.Drawing.Size(928, 23);
			this.btnSaveTransaction.TabIndex = 26;
			this.btnSaveTransaction.Text = "Save All Changes in a Transaction";
			this.btnSaveTransaction.Click += new System.EventHandler(this.btnSaveTransaction_Click);
			// 
			// btnEmployeesSave
			// 
			this.btnEmployeesSave.Location = new System.Drawing.Point(8, 512);
			this.btnEmployeesSave.Name = "btnEmployeesSave";
			this.btnEmployeesSave.Size = new System.Drawing.Size(120, 23);
			this.btnEmployeesSave.TabIndex = 25;
			this.btnEmployeesSave.Text = "Save Employees";
			this.btnEmployeesSave.Click += new System.EventHandler(this.btnEmployeesSave_Click);
			// 
			// btnEmployeesEditRow
			// 
			this.btnEmployeesEditRow.Location = new System.Drawing.Point(144, 512);
			this.btnEmployeesEditRow.Name = "btnEmployeesEditRow";
			this.btnEmployeesEditRow.Size = new System.Drawing.Size(152, 23);
			this.btnEmployeesEditRow.TabIndex = 23;
			this.btnEmployeesEditRow.Text = "Edit Selected Row";
			this.btnEmployeesEditRow.Click += new System.EventHandler(this.btnEmployeesEditRow_Click);
			// 
			// panel1
			// 
			this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
			this.panel1.Controls.Add(this.btnAbout);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(944, 64);
			this.panel1.TabIndex = 18;
			// 
			// btnAbout
			// 
			this.btnAbout.BackColor = System.Drawing.Color.DodgerBlue;
			this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnAbout.ForeColor = System.Drawing.Color.White;
			this.btnAbout.Location = new System.Drawing.Point(32, 24);
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size(136, 23);
			this.btnAbout.TabIndex = 1;
			this.btnAbout.Text = "About dOOdads";
			this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
			// 
			// DataGrid2
			// 
			this.DataGrid2.CaptionText = "Northwind.Products";
			this.DataGrid2.DataMember = "";
			this.DataGrid2.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.DataGrid2.Location = new System.Drawing.Point(488, 208);
			this.DataGrid2.Name = "DataGrid2";
			this.DataGrid2.Size = new System.Drawing.Size(448, 296);
			this.DataGrid2.TabIndex = 21;
			this.DataGrid2.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								  this.DataGridTableStyle2});
			// 
			// DataGridTableStyle2
			// 
			this.DataGridTableStyle2.AlternatingBackColor = System.Drawing.Color.Khaki;
			this.DataGridTableStyle2.BackColor = System.Drawing.Color.Wheat;
			this.DataGridTableStyle2.DataGrid = this.DataGrid2;
			this.DataGridTableStyle2.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.tbcProductName,
																												  this.tcbUnitPrice,
																												  this.tcbUnitsInStock,
																												  this.tcbDiscontinued});
			this.DataGridTableStyle2.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.DataGridTableStyle2.MappingName = "Products";
			// 
			// tbcProductName
			// 
			this.tbcProductName.Format = "";
			this.tbcProductName.FormatInfo = null;
			this.tbcProductName.HeaderText = "ProductName";
			this.tbcProductName.MappingName = "ProductName";
			this.tbcProductName.Width = 170;
			// 
			// tcbUnitPrice
			// 
			this.tcbUnitPrice.Format = "";
			this.tcbUnitPrice.FormatInfo = null;
			this.tcbUnitPrice.HeaderText = "UnitPrice";
			this.tcbUnitPrice.MappingName = "UnitPrice";
			this.tcbUnitPrice.Width = 75;
			// 
			// tcbUnitsInStock
			// 
			this.tcbUnitsInStock.Format = "";
			this.tcbUnitsInStock.FormatInfo = null;
			this.tcbUnitsInStock.HeaderText = "UnitsInStock";
			this.tcbUnitsInStock.MappingName = "UnitsInStock";
			this.tcbUnitsInStock.Width = 75;
			// 
			// tcbDiscontinued
			// 
			this.tcbDiscontinued.FalseValue = false;
			this.tcbDiscontinued.HeaderText = "Discontinued";
			this.tcbDiscontinued.MappingName = "Discontinued";
			this.tcbDiscontinued.NullValue = ((object)(resources.GetObject("tcbDiscontinued.NullValue")));
			this.tcbDiscontinued.TrueValue = true;
			this.tcbDiscontinued.Width = 75;
			// 
			// GroupBox1
			// 
			this.GroupBox1.Controls.Add(this.Label4);
			this.GroupBox1.Controls.Add(this.btnEmployeeSearch);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.txtLastName);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.txtTitle);
			this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.GroupBox1.Location = new System.Drawing.Point(0, 80);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(480, 112);
			this.GroupBox1.TabIndex = 20;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Employees Query ";
			// 
			// Label4
			// 
			this.Label4.Location = new System.Drawing.Point(296, 24);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(144, 40);
			this.Label4.TabIndex = 12;
			this.Label4.Text = "Use Wild Card % as This Query Uses LIKE";
			// 
			// btnEmployeeSearch
			// 
			this.btnEmployeeSearch.Location = new System.Drawing.Point(384, 80);
			this.btnEmployeeSearch.Name = "btnEmployeeSearch";
			this.btnEmployeeSearch.TabIndex = 11;
			this.btnEmployeeSearch.Text = "Search";
			this.btnEmployeeSearch.Click += new System.EventHandler(this.btnEmployeeSearch_Click);
			// 
			// Label1
			// 
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label1.Location = new System.Drawing.Point(8, 30);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(72, 24);
			this.Label1.TabIndex = 7;
			this.Label1.Text = "LastName:";
			// 
			// txtLastName
			// 
			this.txtLastName.Location = new System.Drawing.Point(80, 32);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(168, 20);
			this.txtLastName.TabIndex = 8;
			this.txtLastName.Text = "";
			// 
			// Label2
			// 
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label2.Location = new System.Drawing.Point(8, 71);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(56, 23);
			this.Label2.TabIndex = 9;
			this.Label2.Text = "Title:";
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(80, 72);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(168, 20);
			this.txtTitle.TabIndex = 10;
			this.txtTitle.Text = "";
			// 
			// grpDiscontinued
			// 
			this.grpDiscontinued.Controls.Add(this.cmbOperator);
			this.grpDiscontinued.Controls.Add(this.chkDiscontinued);
			this.grpDiscontinued.Controls.Add(this.txtUnitPrice);
			this.grpDiscontinued.Controls.Add(this.Label3);
			this.grpDiscontinued.Controls.Add(this.btnProductSearch);
			this.grpDiscontinued.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpDiscontinued.Location = new System.Drawing.Point(488, 80);
			this.grpDiscontinued.Name = "grpDiscontinued";
			this.grpDiscontinued.Size = new System.Drawing.Size(448, 112);
			this.grpDiscontinued.TabIndex = 22;
			this.grpDiscontinued.TabStop = false;
			this.grpDiscontinued.Text = "Products Query ";
			// 
			// cmbOperator
			// 
			this.cmbOperator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbOperator.Items.AddRange(new object[] {
															 "Equal",
															 "Not Equal",
															 "Greater Than",
															 "Greater Than Or Equal",
															 "Less Than",
															 "Less Than Or Equal"});
			this.cmbOperator.Location = new System.Drawing.Point(256, 24);
			this.cmbOperator.Name = "cmbOperator";
			this.cmbOperator.Size = new System.Drawing.Size(168, 21);
			this.cmbOperator.TabIndex = 12;
			// 
			// chkDiscontinued
			// 
			this.chkDiscontinued.Location = new System.Drawing.Point(16, 64);
			this.chkDiscontinued.Name = "chkDiscontinued";
			this.chkDiscontinued.Size = new System.Drawing.Size(136, 24);
			this.chkDiscontinued.TabIndex = 3;
			this.chkDiscontinued.Text = "Discontinued Only";
			// 
			// txtUnitPrice
			// 
			this.txtUnitPrice.Location = new System.Drawing.Point(128, 24);
			this.txtUnitPrice.Name = "txtUnitPrice";
			this.txtUnitPrice.Size = new System.Drawing.Size(104, 20);
			this.txtUnitPrice.TabIndex = 1;
			this.txtUnitPrice.Text = "";
			// 
			// Label3
			// 
			this.Label3.Location = new System.Drawing.Point(16, 24);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(72, 23);
			this.Label3.TabIndex = 0;
			this.Label3.Text = "UnitPrice:";
			// 
			// btnProductSearch
			// 
			this.btnProductSearch.Location = new System.Drawing.Point(352, 80);
			this.btnProductSearch.Name = "btnProductSearch";
			this.btnProductSearch.TabIndex = 11;
			this.btnProductSearch.Text = "Search";
			this.btnProductSearch.Click += new System.EventHandler(this.btnProductSearch_Click);
			// 
			// btnProductsSave
			// 
			this.btnProductsSave.Location = new System.Drawing.Point(488, 512);
			this.btnProductsSave.Name = "btnProductsSave";
			this.btnProductsSave.Size = new System.Drawing.Size(120, 23);
			this.btnProductsSave.TabIndex = 24;
			this.btnProductsSave.Text = "Save Products";
			this.btnProductsSave.Click += new System.EventHandler(this.btnProductsSave_Click);
			// 
			// Form1
			// 
#if(VS2005)
			this.AutoScaleDimensions = new System.Drawing.Size(5, 13);
#else
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
#endif
			this.ClientSize = new System.Drawing.Size(944, 584);
			this.Controls.Add(this.btnComboBoxFill);
			this.Controls.Add(this.DataGrid1);
			this.Controls.Add(this.btnSaveTransaction);
			this.Controls.Add(this.btnEmployeesSave);
			this.Controls.Add(this.btnEmployeesEditRow);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.DataGrid2);
			this.Controls.Add(this.GroupBox1);
			this.Controls.Add(this.grpDiscontinued);
			this.Controls.Add(this.btnProductsSave);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "MyGeneration Software\'s C# dOOdad Demo";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.DataGrid1)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DataGrid2)).EndInit();
			this.GroupBox1.ResumeLayout(false);
			this.grpDiscontinued.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void MasterSampleTest()
		{
			TheMasterSample ms = new TheMasterSample();
			ms.SimpleLoad();
			ms.MoreComplexLoad();
			ms.TheDeluxeQuery();
			ms.GenerateSql();
			ms.DataReader();
			ms.Iteration();
			ms.FilterAndSort();
			ms.DemonstrateBulkUpdates();
			ms.Transactions();
			ms.FillComboBox();
			ms.AddColumn();
			ms.Serialize();
			ms.DataSetSerialize();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			// This is the Invoice View, Its in the project for unit test, you can generate
			// business entities from Views using "VbNet_SQL_dOOdads_View.vbgen", there are read only
			//Invoices inv = new Invoices();
			//inv.LoadAll();

			// ** WARNING: ** THIS METHOD MODIFIES DATA IN NORTHWIND
//			 MasterSampleTest();

			BindEmployeesGrid();
			BindProductsGrid();

			this.cmbOperator.SelectedItem = "Equal";
		}

		private void BindEmployeesGrid()
		{
			emps = new Employees();

			try
			{
				emps.Query.Load();
			}
			catch(Exception)
			{
				MessageBox.Show("Edit your 'app.config' file to correct the connection string for your SQL Northwind database");
			}

			this.DataGrid1.DataSource = emps.DefaultView;
		}

		private void BindProductsGrid()
		{
			prds = new Products();

			try
			{
				prds.Query.Load();
			}
			catch(Exception)
			{
				MessageBox.Show("Edit your 'app.config' file to correct the connection string for your SQL Northwind database");
			}

			this.DataGrid2.DataSource = prds.DefaultView;
		}

		private void btnEmployeeSearch_Click(object sender, System.EventArgs e)
		{
			emps = new Employees();

			try
			{
				if(this.txtLastName.Text.Trim() != string.Empty)
				{
					emps.Where.LastName.Operator = WhereParameter.Operand.Like;
					emps.Where.LastName.Value = this.txtLastName.Text.Trim();
				}

				if(this.txtTitle.Text.Trim() != string.Empty)
				{
					emps.Where.Title.Operator = WhereParameter.Operand.Like;
					emps.Where.Title.Value = this.txtTitle.Text.Trim();
				}

				// Could it be any easier?
				emps.Query.Load();
			}
			catch {}

			this.DataGrid1.DataSource = emps.DefaultView;	
		}

		private void btnEmployeesSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.emps.Save();
				BindEmployeesGrid();
			}
			catch
			{
			   MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.");
			}
		}

		private void btnEmployeesEditRow_Click(object sender, System.EventArgs e)
		{
			DataRow row = SelectedRow(DataGrid1);
			if ( row != null)
			{
				string sEmployeeID = row["EmployeeID", DataRowVersion.Current].ToString();

				if(sEmployeeID != string.Empty)
				{
					EmployeesEdit empEdit = new EmployeesEdit(Convert.ToInt32(sEmployeeID));
					DialogResult result = empEdit.ShowDialog();
					if(result == DialogResult.OK)
					{
						// Employees Grid
						emps = new Employees();
						emps.Query.Load();
						this.DataGrid1.DataSource = emps.DefaultView;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		DataRow SelectedRow(DataGrid grid)
		{
			if (grid.DataSource != null)
			{
				BindingManagerBase bm = grid.BindingContext[grid.DataSource, grid.DataMember]; 

				if(bm.Current != null)
				{
					return ((DataRowView)bm.Current).Row;
				}
			} 
			return null;
		}

		private void btnProductSearch_Click(object sender, System.EventArgs e)
		{
			prds = new Products();

			try
			{
				if(this.txtUnitPrice.Text.Trim() != String.Empty)
				{
					prds.Where.UnitPrice.Value = this.txtUnitPrice.Text.Trim();

					switch(this.cmbOperator.SelectedItem.ToString())
					{
						case "Equal":
							prds.Where.UnitPrice.Operator = WhereParameter.Operand.Equal;
							break;
						case "Not Equal":
							prds.Where.UnitPrice.Operator = WhereParameter.Operand.NotEqual;
							break;
						case "Greater Than":
							prds.Where.UnitPrice.Operator = WhereParameter.Operand.GreaterThan;
							break;
						case "Greater Than Or Equal":
							prds.Where.UnitPrice.Operator = WhereParameter.Operand.GreaterThanOrEqual;
							break;
						case "Less Than":
							prds.Where.UnitPrice.Operator = WhereParameter.Operand.LessThan;
							break;
						case "Less Than Or Equal":
							prds.Where.UnitPrice.Operator = WhereParameter.Operand.LessThanOrEqual;
							break;
				   }
				}

				if(this.chkDiscontinued.Checked)
				{
					prds.Where.Discontinued.Value = this.chkDiscontinued.Checked;
				}

				// Could it be any easier?
				prds.Query.Load();
			}
			catch(Exception) {}

			this.DataGrid2.DataSource = prds.DefaultView;	
		}

		private void btnProductsSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.prds.Save();
				BindProductsGrid();
			}
			catch
			{
				MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.");
			}		
		}

		private void btnComboBoxFill_Click(object sender, System.EventArgs e)
		{
			FillComboBox comboFill = new FillComboBox();
			comboFill.ShowDialog();
		}

		private void btnSaveTransaction_Click(object sender, System.EventArgs e)
		{
		   TransactionMgr tx = TransactionMgr.ThreadTransactionMgr();

			try
			{
				tx.BeginTransaction();

				this.emps.Save();
				this.prds.Save();

				tx.CommitTransaction();

				BindEmployeesGrid();
				BindProductsGrid();
			}
			catch(Exception)
			{
				tx.RollbackTransaction();
				TransactionMgr.ThreadTransactionMgrReset();
				MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.");
			}
		}

		private void btnAbout_Click(object sender, System.EventArgs e)
		{
			About about = new About();
			about.ShowDialog();
		}
	}
}
