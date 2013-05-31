using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using MyGeneration.dOOdads;

namespace dOOdad_QueryTester
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonQuery1;
		private System.Windows.Forms.Button buttonAbout;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonQuery2;
		private System.Windows.Forms.Button buttonLoadAll1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGrid dataGridResults;
		private System.Windows.Forms.TextBox textBoxQuery;
		private System.Windows.Forms.Button buttonLoadAll2;
		private System.Windows.Forms.Splitter splitter1;

		//*** Change these to test different Business Entities.
		//    You may need to change the connection string in App.config.
		Products tmpEnt1 = new Products();
		Employees tmpEnt2 = new Employees();
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			BindResultsTable1();
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
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

		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.buttonLoadAll2 = new System.Windows.Forms.Button();
			this.textBoxQuery = new System.Windows.Forms.TextBox();
			this.dataGridResults = new System.Windows.Forms.DataGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.buttonLoadAll1 = new System.Windows.Forms.Button();
			this.buttonQuery2 = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonAbout = new System.Windows.Forms.Button();
			this.buttonQuery1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridResults)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitter1
			// 
			this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 126);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(792, 3);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// buttonLoadAll2
			// 
			this.buttonLoadAll2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonLoadAll2.Location = new System.Drawing.Point(616, 6);
			this.buttonLoadAll2.Name = "buttonLoadAll2";
			this.buttonLoadAll2.TabIndex = 5;
			this.buttonLoadAll2.Text = "LoadAll 2";
			this.buttonLoadAll2.Click += new System.EventHandler(this.ButtonLoadAll2Click);
			// 
			// textBoxQuery
			// 
			this.textBoxQuery.Dock = System.Windows.Forms.DockStyle.Top;
			this.textBoxQuery.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxQuery.Location = new System.Drawing.Point(0, 0);
			this.textBoxQuery.Multiline = true;
			this.textBoxQuery.Name = "textBoxQuery";
			this.textBoxQuery.ReadOnly = true;
			this.textBoxQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxQuery.Size = new System.Drawing.Size(792, 126);
			this.textBoxQuery.TabIndex = 2;
			this.textBoxQuery.TabStop = false;
			this.textBoxQuery.Text = "";
			// 
			// dataGridResults
			// 
			this.dataGridResults.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dataGridResults.CaptionBackColor = System.Drawing.SystemColors.AppWorkspace;
			this.dataGridResults.CaptionText = "Query Results";
			this.dataGridResults.DataMember = "";
			this.dataGridResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridResults.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridResults.Location = new System.Drawing.Point(0, 0);
			this.dataGridResults.Name = "dataGridResults";
			this.dataGridResults.ReadOnly = true;
			this.dataGridResults.Size = new System.Drawing.Size(792, 366);
			this.dataGridResults.TabIndex = 0;
			this.dataGridResults.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonLoadAll2);
			this.panel1.Controls.Add(this.buttonQuery2);
			this.panel1.Controls.Add(this.buttonAbout);
			this.panel1.Controls.Add(this.buttonQuery1);
			this.panel1.Controls.Add(this.buttonLoadAll1);
			this.panel1.Controls.Add(this.buttonClose);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 495);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(792, 37);
			this.panel1.TabIndex = 4;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.dataGridResults);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 129);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(792, 366);
			this.panel2.TabIndex = 5;
			// 
			// buttonLoadAll1
			// 
			this.buttonLoadAll1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonLoadAll1.Location = new System.Drawing.Point(528, 7);
			this.buttonLoadAll1.Name = "buttonLoadAll1";
			this.buttonLoadAll1.Size = new System.Drawing.Size(75, 22);
			this.buttonLoadAll1.TabIndex = 4;
			this.buttonLoadAll1.Text = "LoadAll 1";
			this.buttonLoadAll1.Click += new System.EventHandler(this.ButtonLoadAll1Click);
			// 
			// buttonQuery2
			// 
			this.buttonQuery2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonQuery2.Location = new System.Drawing.Point(440, 6);
			this.buttonQuery2.Name = "buttonQuery2";
			this.buttonQuery2.TabIndex = 3;
			this.buttonQuery2.Text = "Query 2";
			this.buttonQuery2.Click += new System.EventHandler(this.ButtonQuery2Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.Location = new System.Drawing.Point(704, 7);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(75, 22);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "Close";
			this.buttonClose.Click += new System.EventHandler(this.ButtonCloseClick);
			// 
			// buttonAbout
			// 
			this.buttonAbout.Location = new System.Drawing.Point(16, 6);
			this.buttonAbout.Name = "buttonAbout";
			this.buttonAbout.TabIndex = 1;
			this.buttonAbout.Text = "About";
			this.buttonAbout.Click += new System.EventHandler(this.ButtonAboutClick);
			// 
			// buttonQuery1
			// 
			this.buttonQuery1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonQuery1.Location = new System.Drawing.Point(352, 7);
			this.buttonQuery1.Name = "buttonQuery1";
			this.buttonQuery1.Size = new System.Drawing.Size(75, 22);
			this.buttonQuery1.TabIndex = 2;
			this.buttonQuery1.Text = "Query 1";
			this.buttonQuery1.Click += new System.EventHandler(this.ButtonQuery1Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 532);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.textBoxQuery);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "dOOdad Query Tester";
			((System.ComponentModel.ISupportInitialize)(this.dataGridResults)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		void BindResultsTable1()
		{
			try
			{
				// This clears out all DynamicQuery data.
				// Or, you could just re-initialize your
				// Business Entity.
				// Products prds = new Products();
				tmpEnt1.FlushData();
				
				SetupQuery1();
				tmpEnt1.Query.Load();
				textBoxQuery.Text = "Query 1 Succeeded:\r\n\r\n";
				// Query.LastQuery contains the SQL of
				// your last Query.Load().
				textBoxQuery.Text += tmpEnt1.Query.LastQuery;
			}
			catch(Exception ex)
			{
				// Query.GenerateSQL() returns the SQL of
				// a pending query. You cannot call
				// Query.Load() afterwards. It is for
				// debugging only.
				
				tmpEnt1.FlushData();
				SetupQuery1();
				textBoxQuery.Text = "Query 1 Failed:\r\n\r\n"
					+ tmpEnt1.Query.GenerateSQL()
					+ "\r\n\r\n" + ex.ToString();
			}
			
			this.dataGridResults.DataSource = tmpEnt1.DefaultView;
		}

		void BindResultsTable2()
		{
			try
			{
				tmpEnt2.FlushData();
				SetupQuery2();
				tmpEnt2.Query.Load();
				textBoxQuery.Text = "Query 2 Succeeded:\r\n\r\n";
				textBoxQuery.Text += tmpEnt2.Query.LastQuery;
			}
			catch(Exception ex)
			{
				tmpEnt2.FlushData();
				SetupQuery2();
				textBoxQuery.Text = "Query 2 Failed:\r\n\r\n"
					+ tmpEnt2.Query.GenerateSQL()
					+ "\r\n\r\n" + ex.ToString();
			}
			
			this.dataGridResults.DataSource = tmpEnt2.DefaultView;
		}

		//*** This is where you consruct Query 1
		//    For C# remember to use the Concrete Class name
		//    when passing in columnName params:
		//      Products.ColumnNames.CategoryID
		//    Not:
		//      tmpEnt1.ColumnNames.CategoryID
		void SetupQuery1()
		{
			// This demonstrates the use of aggregates
			// with dOOdads. Once you limit columns,
			// either with aggregates or AddResultColumn(),
			// you cannot call Save().
			
			// WithRollup adds additional rows with
			// heirarchal subtotals.
			// It is supported in SqlClient and MySQL.
			// In MySQL OrderBy and WithRollup are
			// mutually exclusive.
			tmpEnt1.Query.WithRollup = true;
			
			// To include a COUNT(*) with NULLs included
			tmpEnt1.Query.CountAll = true;
			tmpEnt1.Query.CountAllAlias = "Product Count";

			// To exclude NULLs in the COUNT for a column
			tmpEnt1.Aggregate.UnitsInStock.Function = AggregateParameter.Func.Count;
			tmpEnt1.Aggregate.UnitsInStock.Alias = "With Stock";
			
			// To have two aggregates for the same column, use a tearoff
			AggregateParameter ap = tmpEnt1.Aggregate.TearOff.UnitsInStock;
			ap.Function = AggregateParameter.Func.Sum;
			ap.Alias = "Total Units";

			tmpEnt1.Aggregate.ReorderLevel.Function = AggregateParameter.Func.Avg;
			tmpEnt1.Aggregate.ReorderLevel.Alias = "Avg Reorder";

			tmpEnt1.Aggregate.UnitPrice.Function = AggregateParameter.Func.Min;
			tmpEnt1.Aggregate.UnitPrice.Alias = "Min Price";

			ap = tmpEnt1.Aggregate.TearOff.UnitPrice;
			ap.Function = AggregateParameter.Func.Max;
			ap.Alias = "Max Price";
			
			// If you have no aggregates or AddResultColumns,
			// then the query defaults to 'SELECT *'.
			// If you have an aggregate and no AddResultColumns,
			// Then only aggregates are reurned in the query.
			// The column order in a DataGrid is as follows:
			// AddResultColumns in the order added.
			// CountAll if true.
			// Aggregates in the order created.
			tmpEnt1.Query.AddResultColumn(Products.ColumnNames.CategoryID);
			tmpEnt1.Query.AddResultColumn(Products.ColumnNames.Discontinued);
			
			// Aggregates cannot be used inside the WHERE clause,
			// nor has a HAVING clause been implemented.
			// For more complex queries use LoadFromRawSql().
			tmpEnt1.Where.CategoryID.Operator = WhereParameter.Operand.LessThan;
			tmpEnt1.Where.CategoryID.Value = "10";
			
			// If you have an Aggregate, then most DBMSs require an
			// AddGroupBy for each AddResultColumn.
			tmpEnt1.Query.AddGroupBy(Products.ColumnNames.CategoryID);
			tmpEnt1.Query.AddGroupBy(Products.ColumnNames.Discontinued);
			
			// You can use aggregates in AddGroupBy by
			// referencing either the entity AggregateParameter or a tearoff
			// You must create the aggregate before using it here.
			// SqlCient, OleDb, and MySQL4 do not support aggregates
			// in a GROUP BY clause.
//			tmpEnt1.Query.AddGroupBy(tmpEnt1.Aggregate.UnitsInStock);
//			tmpEnt1.Query.AddGroupBy(ap);
			
			tmpEnt1.Query.AddOrderBy(Products.ColumnNames.Discontinued, WhereParameter.Dir.ASC);
			
			// You can use aggregates in AddOrderBy by
			// referencing either the entity AggregateParameter or a tearoff
			// You must create the aggregate before using it here.
			// For MySql4 you must set the Alias.
			tmpEnt1.Query.AddOrderBy(tmpEnt1.Aggregate.UnitsInStock, WhereParameter.Dir.ASC);
			tmpEnt1.Query.AddOrderBy(ap, WhereParameter.Dir.DESC);
			
			// To use COUNT(*) in an AddOrderBy,
			// pass in entity.Query in place of the columnName.
			// Query.CountAll must be set to true.
			// For MySQL4 you must set Query.CountAllAlias.
			tmpEnt1.Query.AddOrderBy(tmpEnt1.Query, WhereParameter.Dir.DESC);
		}

		//*** This is where you consruct Query 2
		void SetupQuery2()
		{
			// This is what we are going for:
			//   The top 5 distinct employees where
			//   (LastNames have "A" anywhere in them
			//   or begin with "P")
			//   and the HireDate is not NULL
			//   (and the BirthDate is between 1/1/1990 and today
			//   or BirthDate is NULL.)
			// In SQL syntax:
			//  SELECT  DISTINCT  TOP 5 [EmployeeID], [LastName], [FirstName], [BirthDate]
			//  FROM [Employees]
			//  WHERE ([LastName] LIKE @LastName1 OR [LastName] LIKE @LastName2 )
			//    OR [HireDate] IS NOT NULL
			//    AND ([BirthDate] BETWEEN @BirthDate4 AND @BirthDate5 OR [BirthDate] IS NULL )
			//  ORDER BY [LastName] ASC, [FirstName] ASC
			
			// 'LastNames that have "A" anywhere in them
			// or begin with "P".'
			// Use a TearOff to use LastName in 2 Where clauses.
			// Notice:
			//   The conjunction will precede the Where clause.
			//   The wp.Conjuction mis-spelling.
			tmpEnt2.Query.OpenParenthesis();
			tmpEnt2.Where.LastName.Value = "%A%";
			tmpEnt2.Where.LastName.Operator = WhereParameter.Operand.Like;
			WhereParameter wp = tmpEnt2.Where.TearOff.LastName;
			wp.Value = "P%";
			wp.Operator = WhereParameter.Operand.Like;
			wp.Conjuction = WhereParameter.Conj.Or;
			tmpEnt2.Query.CloseParenthesis();

			// 'And the HireDate is not NULL.'
			// The default conjunction is "AND".
			// To change the default conjuction use
			//    tmpEnt2.Query.Load("OR");
			tmpEnt2.Where.HireDate.Operator = WhereParameter.Operand.IsNotNull;

			// 'And the BirthDate is between 1/1/1990 and today
			// or BirthDate is NULL.'
			// We'll need another TearOff for the second use
			// of BirthDate.
			// We need this before OpenParenthesis().
			tmpEnt2.Query.AddConjunction(WhereParameter.Conj.And);

			tmpEnt2.Query.OpenParenthesis(); 
			tmpEnt2.Where.BirthDate.Operator = WhereParameter.Operand.Between; 
			tmpEnt2.Where.BirthDate.BetweenBeginValue = "1900/01/01 00:00:00";
			tmpEnt2.Where.BirthDate.BetweenEndValue = DateTime.Today.ToString();
			wp = tmpEnt2.Where.TearOff.BirthDate;
			wp.Operator = WhereParameter.Operand.IsNull; 
			wp.Conjuction = WhereParameter.Conj.Or; 
			tmpEnt2.Query.CloseParenthesis(); 
			
			// Only return the EmployeeID, LastName, FirstName, and BirthDate
			// If you have no AddResultColumns,
			// then the query defaults to 'SELECT *'.
			// Once we limit columns, we cannot call Save().
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.EmployeeID);
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.LastName);
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.FirstName);
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.BirthDate);

			// Order by LastName, FirstName 
			// You can add as many order by columns as you like
			// by repeatedly calling this.
			tmpEnt2.Query.AddOrderBy(Employees.ColumnNames.LastName, WhereParameter.Dir.ASC);
			tmpEnt2.Query.AddOrderBy(Employees.ColumnNames.FirstName, WhereParameter.Dir.ASC);

			// Bring back only distinct rows
			tmpEnt2.Query.Distinct = true;

			// Bring back the top 5 rows
			tmpEnt2.Query.Top = 5;
		}
		
		void ButtonCloseClick(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		void ButtonLoadAll1Click(object sender, System.EventArgs e)
		{
			tmpEnt1.FlushData();
			tmpEnt1.LoadAll();
			this.dataGridResults.DataSource = tmpEnt1.DefaultView;
		}
		
		void ButtonLoadAll2Click(object sender, System.EventArgs e)
		{
			tmpEnt2.FlushData();
			tmpEnt2.LoadAll();
			this.dataGridResults.DataSource = tmpEnt2.DefaultView;
		}
		
		void ButtonQuery1Click(object sender, System.EventArgs e)
		{
			BindResultsTable1();
		}
		
		void ButtonQuery2Click(object sender, System.EventArgs e)
		{
			BindResultsTable2();
		}
		
		void ButtonAboutClick(object sender, System.EventArgs e)
		{
			About about = new About();
			about.ShowDialog();
		}
		
	}
}
