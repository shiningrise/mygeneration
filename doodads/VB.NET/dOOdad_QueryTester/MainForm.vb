Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data
Imports MyGeneration.dOOdads

Namespace dOOdad_QueryTester
	
	Public Class MainForm
		Inherits System.Windows.Forms.Form
		Private buttonQuery1 As System.Windows.Forms.Button
		Private buttonAbout As System.Windows.Forms.Button
		Private buttonClose As System.Windows.Forms.Button
		Private buttonQuery2 As System.Windows.Forms.Button
		Private buttonLoadAll1 As System.Windows.Forms.Button
		Private panel2 As System.Windows.Forms.Panel
		Private panel1 As System.Windows.Forms.Panel
		Private dataGridResults As System.Windows.Forms.DataGrid
		Private textBoxQuery As System.Windows.Forms.TextBox
		Private buttonLoadAll2 As System.Windows.Forms.Button
		Private splitter1 As System.Windows.Forms.Splitter
		
		'*** Change these to test different Business Entities.
		'    You may need to change the connection string in App.config.
		Private tmpEnt1 As Products = New Products
		Private tmpEnt2 As Employees = New Employees

		Public Shared Sub Main
			Dim fMainForm As New MainForm
			fMainForm.ShowDialog()
		End Sub
		
		Public Sub New()
			MyBase.New
			'
			' The Me.InitializeComponent call is required for Windows Forms designer support.
			'
			Me.InitializeComponent
			BindResultsTable1
		End Sub
		
	    'Form overrides dispose to clean up the component list.
	    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
	        If disposing Then
	            If Not (components Is Nothing) Then
	                components.Dispose()
	            End If
	        End If
	        MyBase.Dispose(disposing)
	    End Sub
	
	    'Required by the Windows Form Designer
	    Private components As System.ComponentModel.IContainer
	
		#Region " Windows Forms Designer generated code "
		' This method is required for Windows Forms designer support.
		' Do not change the method contents inside the source code editor. The Forms designer might
		' not be able to load this method if it was changed manually.
		Private Sub InitializeComponent()
			Me.splitter1 = New System.Windows.Forms.Splitter
			Me.buttonLoadAll2 = New System.Windows.Forms.Button
			Me.textBoxQuery = New System.Windows.Forms.TextBox
			Me.dataGridResults = New System.Windows.Forms.DataGrid
			Me.panel1 = New System.Windows.Forms.Panel
			Me.panel2 = New System.Windows.Forms.Panel
			Me.buttonLoadAll1 = New System.Windows.Forms.Button
			Me.buttonQuery2 = New System.Windows.Forms.Button
			Me.buttonClose = New System.Windows.Forms.Button
			Me.buttonAbout = New System.Windows.Forms.Button
			Me.buttonQuery1 = New System.Windows.Forms.Button
			CType((Me.dataGridResults), System.ComponentModel.ISupportInitialize).BeginInit
			Me.panel1.SuspendLayout
			Me.panel2.SuspendLayout
			Me.SuspendLayout
			Me.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit
			Me.splitter1.Dock = System.Windows.Forms.DockStyle.Top
			Me.splitter1.Location = New System.Drawing.Point(0, 126)
			Me.splitter1.Name = "splitter1"
			Me.splitter1.Size = New System.Drawing.Size(792, 3)
			Me.splitter1.TabIndex = 3
			Me.splitter1.TabStop = False
			Me.buttonLoadAll2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
			Me.buttonLoadAll2.Location = New System.Drawing.Point(616, 6)
			Me.buttonLoadAll2.Name = "buttonLoadAll2"
			Me.buttonLoadAll2.TabIndex = 5
			Me.buttonLoadAll2.Text = "LoadAll 2"
			AddHandler Me.buttonLoadAll2.Click, AddressOf Me.ButtonLoadAll2Click
			Me.textBoxQuery.Dock = System.Windows.Forms.DockStyle.Top
			Me.textBoxQuery.Font = New System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType((0), System.Byte))
			Me.textBoxQuery.Location = New System.Drawing.Point(0, 0)
			Me.textBoxQuery.Multiline = True
			Me.textBoxQuery.Name = "textBoxQuery"
			Me.textBoxQuery.ReadOnly = True
			Me.textBoxQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both
			Me.textBoxQuery.Size = New System.Drawing.Size(792, 126)
			Me.textBoxQuery.TabIndex = 2
			Me.textBoxQuery.TabStop = False
			Me.textBoxQuery.Text = ""
			Me.dataGridResults.BackgroundColor = System.Drawing.SystemColors.Window
			Me.dataGridResults.CaptionBackColor = System.Drawing.SystemColors.AppWorkspace
			Me.dataGridResults.CaptionText = "Query Results"
			Me.dataGridResults.DataMember = ""
			Me.dataGridResults.Dock = System.Windows.Forms.DockStyle.Fill
			Me.dataGridResults.HeaderForeColor = System.Drawing.SystemColors.ControlText
			Me.dataGridResults.Location = New System.Drawing.Point(0, 0)
			Me.dataGridResults.Name = "dataGridResults"
			Me.dataGridResults.ReadOnly = True
			Me.dataGridResults.Size = New System.Drawing.Size(792, 366)
			Me.dataGridResults.TabIndex = 0
			Me.dataGridResults.TabStop = False
			Me.panel1.Controls.Add(Me.buttonLoadAll2)
			Me.panel1.Controls.Add(Me.buttonQuery2)
			Me.panel1.Controls.Add(Me.buttonAbout)
			Me.panel1.Controls.Add(Me.buttonQuery1)
			Me.panel1.Controls.Add(Me.buttonLoadAll1)
			Me.panel1.Controls.Add(Me.buttonClose)
			Me.panel1.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.panel1.Location = New System.Drawing.Point(0, 495)
			Me.panel1.Name = "panel1"
			Me.panel1.Size = New System.Drawing.Size(792, 37)
			Me.panel1.TabIndex = 4
			Me.panel2.Controls.Add(Me.dataGridResults)
			Me.panel2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.panel2.Location = New System.Drawing.Point(0, 129)
			Me.panel2.Name = "panel2"
			Me.panel2.Size = New System.Drawing.Size(792, 366)
			Me.panel2.TabIndex = 5
			Me.buttonLoadAll1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
			Me.buttonLoadAll1.Location = New System.Drawing.Point(528, 7)
			Me.buttonLoadAll1.Name = "buttonLoadAll1"
			Me.buttonLoadAll1.Size = New System.Drawing.Size(75, 22)
			Me.buttonLoadAll1.TabIndex = 4
			Me.buttonLoadAll1.Text = "LoadAll 1"
			AddHandler Me.buttonLoadAll1.Click, AddressOf Me.ButtonLoadAll1Click
			Me.buttonQuery2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
			Me.buttonQuery2.Location = New System.Drawing.Point(440, 6)
			Me.buttonQuery2.Name = "buttonQuery2"
			Me.buttonQuery2.TabIndex = 3
			Me.buttonQuery2.Text = "Query 2"
			AddHandler Me.buttonQuery2.Click, AddressOf Me.ButtonQuery2Click
			Me.buttonClose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
			Me.buttonClose.Location = New System.Drawing.Point(704, 7)
			Me.buttonClose.Name = "buttonClose"
			Me.buttonClose.Size = New System.Drawing.Size(75, 22)
			Me.buttonClose.TabIndex = 0
			Me.buttonClose.Text = "Close"
			AddHandler Me.buttonClose.Click, AddressOf Me.ButtonCloseClick
			Me.buttonAbout.Location = New System.Drawing.Point(16, 6)
			Me.buttonAbout.Name = "buttonAbout"
			Me.buttonAbout.TabIndex = 1
			Me.buttonAbout.Text = "About"
			AddHandler Me.buttonAbout.Click, AddressOf Me.ButtonAboutClick
			Me.buttonQuery1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
			Me.buttonQuery1.Location = New System.Drawing.Point(352, 7)
			Me.buttonQuery1.Name = "buttonQuery1"
			Me.buttonQuery1.Size = New System.Drawing.Size(75, 22)
			Me.buttonQuery1.TabIndex = 2
			Me.buttonQuery1.Text = "Query 1"
			AddHandler Me.buttonQuery1.Click, AddressOf Me.ButtonQuery1Click
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.ClientSize = New System.Drawing.Size(792, 532)
			Me.Controls.Add(Me.panel2)
			Me.Controls.Add(Me.panel1)
			Me.Controls.Add(Me.splitter1)
			Me.Controls.Add(Me.textBoxQuery)
			Me.Name = "MainForm"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "dOOdad Query Tester"
			CType((Me.dataGridResults), System.ComponentModel.ISupportInitialize).EndInit
			Me.panel1.ResumeLayout(False)
			Me.panel2.ResumeLayout(False)
			Me.ResumeLayout(False)
		End Sub
		#End Region
		
		Sub BindResultsTable1()
			Try
				' This clears out all DynamicQuery data.
				' Or, you could just re-initialize your
				' Business Entity.
				' tmpEnt1 As Products = New Products
				tmpEnt1.FlushData
				
				SetupQuery1
				tmpEnt1.Query.Load
				textBoxQuery.Text = "Query 1 Succeeded:" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & ""
				' Query.LastQuery contains the SQL of
				' your last Query.Load().
				textBoxQuery.Text += tmpEnt1.Query.LastQuery
			Catch ex As Exception
				' Query.GenerateSQL() returns the SQL of
				' a pending query. You cannot call
				' Query.Load() afterwards. It is for
				' debugging only.
				
				tmpEnt1.FlushData
				SetupQuery1
				textBoxQuery.Text = "Query 1 Failed:" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" + tmpEnt1.Query.GenerateSQL + "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" + ex.ToString
			End Try
			Me.dataGridResults.DataSource = tmpEnt1.DefaultView
		End Sub

		Sub BindResultsTable2()
			Try
				tmpEnt2.FlushData
				SetupQuery2
				tmpEnt2.Query.Load
				textBoxQuery.Text = "Query 2 Succeeded:" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & ""
				textBoxQuery.Text += tmpEnt2.Query.LastQuery
			Catch ex As Exception
				tmpEnt2.FlushData
				SetupQuery2
				textBoxQuery.Text = "Query 2 Failed:" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" + tmpEnt2.Query.GenerateSQL + "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" & Microsoft.VisualBasic.Chr(13) & "" & Microsoft.VisualBasic.Chr(10) & "" + ex.ToString
			End Try
			Me.dataGridResults.DataSource = tmpEnt2.DefaultView
		End Sub

		'*** This is where you consruct Query 1
		Sub SetupQuery1()
			' This demonstrates the use of aggregates
			' with dOOdads. Once you limit columns,
			' either with aggregates or AddResultColumn(),
			' you cannot call Save().
			
			' WithRollup adds additional rows with
			' heirarchal subtotals.
			' It is supported in SqlClient and MySQL.
			' In MySQL OrderBy and WithRollup are
			' mutually exclusive.
			tmpEnt1.Query.WithRollup = True
			
			' To include a COUNT(*) with NULLs included
			tmpEnt1.Query.CountAll = True
			tmpEnt1.Query.CountAllAlias = "Product Count"
			
			' To exclude NULLs in the COUNT for a column
			tmpEnt1.Aggregate.UnitsInStock.Function = AggregateParameter.Func.Count
			tmpEnt1.Aggregate.UnitsInStock.Alias = "With Stock"
			
			' To have two aggregates for the same column, use a tearoff
			Dim ap As AggregateParameter = tmpEnt1.Aggregate.TearOff.UnitsInStock
			ap.Function = AggregateParameter.Func.Sum
			ap.Alias = "Total Units"
			
			tmpEnt1.Aggregate.ReorderLevel.Function = AggregateParameter.Func.Avg
			tmpEnt1.Aggregate.ReorderLevel.Alias = "Avg Reorder"
			
			tmpEnt1.Aggregate.UnitPrice.Function = AggregateParameter.Func.Min
			tmpEnt1.Aggregate.UnitPrice.Alias = "Min Price"
			
			ap = tmpEnt1.Aggregate.TearOff.UnitPrice
			ap.Function = AggregateParameter.Func.Max
			ap.Alias = "Max Price"
			
			' If you have no aggregates or AddResultColumns,
			' then the query defaults to 'SELECT *'.
			' If you have an aggregate and no AddResultColumns,
			' Then only aggregates are reurned in the query.
			' The column order in a DataGrid is as follows:
			' AddResultColumns in the order added.
			' CountAll if true.
			' Aggregates in the order created.
			tmpEnt1.Query.AddResultColumn(Products.ColumnNames.CategoryID)
			tmpEnt1.Query.AddResultColumn(Products.ColumnNames.Discontinued)
			
			' Aggregates cannot be used inside the WHERE clause,
			' nor has a HAVING clause been implemented.
			' For more complex queries use LoadFromRawSql().
			tmpEnt1.Where.CategoryID.Operator = WhereParameter.Operand.LessThan
			tmpEnt1.Where.CategoryID.Value = "10"
			
			' If you have an Aggregate, then most DBMSs require an
			' AddGroupBy for each AddResultColumn.
			tmpEnt1.Query.AddGroupBy(Products.ColumnNames.CategoryID)
			tmpEnt1.Query.AddGroupBy(Products.ColumnNames.Discontinued)
			
			' You can use aggregates in AddGroupBy by
			' referencing either the entity AggregateParameter or a tearoff
			' You must create the aggregate before using it here.
			' SqlCient, OleDb, and MySQL4 do not support aggregates
			' in a GROUP BY clause.
'			tmpEnt1.Query.AddGroupBy(tmpEnt1.Aggregate.UnitsInStock);
'			tmpEnt1.Query.AddGroupBy(ap);

			tmpEnt1.Query.AddOrderBy(Products.ColumnNames.Discontinued, WhereParameter.Dir.ASC)
			
			' You can use aggregates in AddOrderBy by
			' referencing either the entity AggregateParameter or a tearoff
			' You must create the aggregate before using it here.
			' For MySql4 you must set the Alias.
			tmpEnt1.Query.AddOrderBy(tmpEnt1.Aggregate.UnitsInStock, WhereParameter.Dir.ASC)
			tmpEnt1.Query.AddOrderBy(ap, WhereParameter.Dir.DESC)
			
			' To use COUNT(*) in an AddOrderBy,
			' pass in entity.Query in place of the columnName.
			' Query.CountAll must be set to true.
			' For MySQL4 you must set Query.CountAllAlias.
			tmpEnt1.Query.AddOrderBy(tmpEnt1.Query, WhereParameter.Dir.DESC)
		End Sub

		'*** This is where you consruct Query 2
		Sub SetupQuery2()
			' This is what we are going for:
			'   The top 5 distinct employees where
			'   (LastNames have "A" anywhere in them
			'   or begin with "P")
			'   and the HireDate is not NULL
			'   (and the BirthDate is between 1/1/1990 and today
			'   or BirthDate is NULL.)
			' In SQL syntax:
			'  SELECT  DISTINCT  TOP 5 [EmployeeID], [LastName], [FirstName], [BirthDate]
			'  FROM [Employees]
			'  WHERE ([LastName] LIKE @LastName1 OR [LastName] LIKE @LastName2 )
			'    OR [HireDate] IS NOT NULL
			'    AND ([BirthDate] BETWEEN @BirthDate4 AND @BirthDate5 OR [BirthDate] IS NULL )
			'  ORDER BY [LastName] ASC, [FirstName] ASC
			
			' 'LastNames that have "A" anywhere in them
			' or begin with "P".'
			' Use a TearOff to use LastName in 2 Where clauses.
			' Notice:
			'   The conjunction will precede the Where clause.
			'   The wp.Conjuction mis-spelling.
			tmpEnt2.Query.OpenParenthesis
			tmpEnt2.Where.LastName.Value = "%A%"
			tmpEnt2.Where.LastName.Operator = WhereParameter.Operand.Like_
			Dim wp As WhereParameter = tmpEnt2.Where.TearOff.LastName
			wp.Value = "P%"
			wp.Operator = WhereParameter.Operand.Like_
			wp.Conjuction = WhereParameter.Conj.Or_
			tmpEnt2.Query.CloseParenthesis
			
			' 'And the HireDate is not NULL.'
			' The default conjunction is "AND".
			' To change the default conjuction use
			'    tmpEnt2.Query.Load("OR");
			tmpEnt2.Where.HireDate.Operator = WhereParameter.Operand.IsNotNull
			
			' 'And the BirthDate is between 1/1/1990 and today
			' or BirthDate is NULL.'
			' We'll need another TearOff for the second use
			' of BirthDate.
			' We need this before OpenParenthesis().
			tmpEnt2.Query.AddConjunction(WhereParameter.Conj.And_)
			
			tmpEnt2.Query.OpenParenthesis
			tmpEnt2.Where.BirthDate.Operator = WhereParameter.Operand.Between
			tmpEnt2.Where.BirthDate.BetweenBeginValue = "1900/01/01 00:00:00"
			tmpEnt2.Where.BirthDate.BetweenEndValue = DateTime.Today.ToString
			wp = tmpEnt2.Where.TearOff.BirthDate
			wp.Operator = WhereParameter.Operand.IsNull
			wp.Conjuction = WhereParameter.Conj.Or_
			tmpEnt2.Query.CloseParenthesis
			
			' Only return the EmployeeID, LastName, FirstName, and BirthDate
			' If you have no AddResultColumns,
			' then the query defaults to 'SELECT *'.
			' Once we limit columns, we cannot call Save().
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.EmployeeID)
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.LastName)
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.FirstName)
			tmpEnt2.Query.AddResultColumn(Employees.ColumnNames.BirthDate)
			
			' Order by LastName, FirstName 
			' You can add as many order by columns as you like
			' by repeatedly calling this.
			tmpEnt2.Query.AddOrderBy(Employees.ColumnNames.LastName, WhereParameter.Dir.ASC)
			tmpEnt2.Query.AddOrderBy(Employees.ColumnNames.FirstName, WhereParameter.Dir.ASC)
			
			' Bring back only distinct rows
			tmpEnt2.Query.Distinct = True
			
			' Bring back the top 5 rows
			tmpEnt2.Query.Top = 5
		End Sub

		Sub ButtonCloseClick(ByVal sender As Object, ByVal e As System.EventArgs)
			Application.Exit
		End Sub

		Sub ButtonLoadAll1Click(ByVal sender As Object, ByVal e As System.EventArgs)
			tmpEnt1.FlushData
			tmpEnt1.LoadAll
			Me.dataGridResults.DataSource = tmpEnt1.DefaultView
		End Sub

		Sub ButtonLoadAll2Click(ByVal sender As Object, ByVal e As System.EventArgs)
			tmpEnt2.FlushData
			tmpEnt2.LoadAll
			Me.dataGridResults.DataSource = tmpEnt2.DefaultView
		End Sub

		Sub ButtonQuery1Click(ByVal sender As Object, ByVal e As System.EventArgs)
			BindResultsTable1
		End Sub

		Sub ButtonQuery2Click(ByVal sender As Object, ByVal e As System.EventArgs)
			BindResultsTable2
		End Sub

		Sub ButtonAboutClick(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim about As About = New About
			about.ShowDialog
		End Sub
	End Class
End Namespace
