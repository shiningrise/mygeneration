Imports MyGeneration.dOOdads

Public Class Form1
    Inherits System.Windows.Forms.Form

    Dim emps As New Employees
    Dim prds As New Products

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents panel1 As System.Windows.Forms.Panel
	Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents tbcFirstName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents tcbLastName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents tcbTitle As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents tcbHomePhone As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtLastName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents DataGrid2 As System.Windows.Forms.DataGrid
    Friend WithEvents DataGridTableStyle2 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents tbcProductName As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents tcbUnitPrice As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents tcbUnitsInStock As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents tcbDiscontinued As System.Windows.Forms.DataGridBoolColumn
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtUnitPrice As System.Windows.Forms.TextBox
    Friend WithEvents grpDiscontinued As System.Windows.Forms.GroupBox
    Friend WithEvents cmbOperator As System.Windows.Forms.ComboBox
    Friend WithEvents btnEmployeeSearch As System.Windows.Forms.Button
    Friend WithEvents btnProductSearch As System.Windows.Forms.Button
    Friend WithEvents btnEmployeesEditRow As System.Windows.Forms.Button
    Friend WithEvents btnEmployeesSave As System.Windows.Forms.Button
    Friend WithEvents btnProductsSave As System.Windows.Forms.Button
    Friend WithEvents btnSaveTransaction As System.Windows.Forms.Button
    Friend WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnAbout As System.Windows.Forms.Button
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents tcbEmpID As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents btnComboBoxFill As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
		Me.panel1 = New System.Windows.Forms.Panel
		Me.btnAbout = New System.Windows.Forms.Button
		Me.DataGrid1 = New System.Windows.Forms.DataGrid
		Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
		Me.tcbEmpID = New System.Windows.Forms.DataGridTextBoxColumn
		Me.tbcFirstName = New System.Windows.Forms.DataGridTextBoxColumn
		Me.tcbLastName = New System.Windows.Forms.DataGridTextBoxColumn
		Me.tcbTitle = New System.Windows.Forms.DataGridTextBoxColumn
		Me.tcbHomePhone = New System.Windows.Forms.DataGridTextBoxColumn
		Me.Label1 = New System.Windows.Forms.Label
		Me.txtLastName = New System.Windows.Forms.TextBox
		Me.Label2 = New System.Windows.Forms.Label
		Me.txtTitle = New System.Windows.Forms.TextBox
		Me.GroupBox1 = New System.Windows.Forms.GroupBox
		Me.Label4 = New System.Windows.Forms.Label
		Me.btnEmployeeSearch = New System.Windows.Forms.Button
		Me.DataGrid2 = New System.Windows.Forms.DataGrid
		Me.DataGridTableStyle2 = New System.Windows.Forms.DataGridTableStyle
		Me.tbcProductName = New System.Windows.Forms.DataGridTextBoxColumn
		Me.tcbUnitPrice = New System.Windows.Forms.DataGridTextBoxColumn
		Me.tcbUnitsInStock = New System.Windows.Forms.DataGridTextBoxColumn
		Me.tcbDiscontinued = New System.Windows.Forms.DataGridBoolColumn
		Me.grpDiscontinued = New System.Windows.Forms.GroupBox
		Me.cmbOperator = New System.Windows.Forms.ComboBox
		Me.chkDiscontinued = New System.Windows.Forms.CheckBox
		Me.txtUnitPrice = New System.Windows.Forms.TextBox
		Me.Label3 = New System.Windows.Forms.Label
		Me.btnProductSearch = New System.Windows.Forms.Button
		Me.btnEmployeesEditRow = New System.Windows.Forms.Button
		Me.btnEmployeesSave = New System.Windows.Forms.Button
		Me.btnProductsSave = New System.Windows.Forms.Button
		Me.btnSaveTransaction = New System.Windows.Forms.Button
		Me.btnComboBoxFill = New System.Windows.Forms.Button
		Me.panel1.SuspendLayout()
		CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox1.SuspendLayout()
		CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpDiscontinued.SuspendLayout()
		Me.SuspendLayout()
		'
		'panel1
		'
		Me.panel1.BackgroundImage = CType(resources.GetObject("panel1.BackgroundImage"), System.Drawing.Image)
		Me.panel1.Controls.Add(Me.btnAbout)
		Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
		Me.panel1.Location = New System.Drawing.Point(0, 0)
		Me.panel1.Name = "panel1"
		Me.panel1.Size = New System.Drawing.Size(944, 64)
		Me.panel1.TabIndex = 5
		'
		'btnAbout
		'
		Me.btnAbout.BackColor = System.Drawing.Color.DodgerBlue
		Me.btnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.btnAbout.ForeColor = System.Drawing.Color.White
		Me.btnAbout.Location = New System.Drawing.Point(48, 24)
		Me.btnAbout.Name = "btnAbout"
		Me.btnAbout.Size = New System.Drawing.Size(136, 23)
		Me.btnAbout.TabIndex = 1
		Me.btnAbout.Text = "About dOOdads"
		'
		'DataGrid1
		'
		Me.DataGrid1.AlternatingBackColor = System.Drawing.Color.LightSteelBlue
		Me.DataGrid1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
					Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.DataGrid1.BackColor = System.Drawing.Color.SteelBlue
		Me.DataGrid1.CaptionText = "Northwind.Employees"
		Me.DataGrid1.DataMember = ""
		Me.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.DataGrid1.Location = New System.Drawing.Point(8, 208)
		Me.DataGrid1.Name = "DataGrid1"
		Me.DataGrid1.Size = New System.Drawing.Size(472, 296)
		Me.DataGrid1.TabIndex = 6
		Me.DataGrid1.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
		'
		'DataGridTableStyle1
		'
		Me.DataGridTableStyle1.AlternatingBackColor = System.Drawing.Color.SkyBlue
		Me.DataGridTableStyle1.BackColor = System.Drawing.Color.LightSteelBlue
		Me.DataGridTableStyle1.DataGrid = Me.DataGrid1
		Me.DataGridTableStyle1.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.tcbEmpID, Me.tbcFirstName, Me.tcbLastName, Me.tcbTitle, Me.tcbHomePhone})
		Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.DataGridTableStyle1.MappingName = "Employees"
		'
		'tcbEmpID
		'
		Me.tcbEmpID.Format = ""
		Me.tcbEmpID.FormatInfo = Nothing
		Me.tcbEmpID.HeaderText = "ID"
		Me.tcbEmpID.MappingName = "EmployeeID"
		Me.tcbEmpID.Width = 50
		'
		'tbcFirstName
		'
		Me.tbcFirstName.Format = ""
		Me.tbcFirstName.FormatInfo = Nothing
		Me.tbcFirstName.HeaderText = "FirstName"
		Me.tbcFirstName.MappingName = "FirstName"
		Me.tbcFirstName.Width = 75
		'
		'tcbLastName
		'
		Me.tcbLastName.Format = ""
		Me.tcbLastName.FormatInfo = Nothing
		Me.tcbLastName.HeaderText = "LastName"
		Me.tcbLastName.MappingName = "LastName"
		Me.tcbLastName.Width = 80
		'
		'tcbTitle
		'
		Me.tcbTitle.Format = ""
		Me.tcbTitle.FormatInfo = Nothing
		Me.tcbTitle.HeaderText = "Title"
		Me.tcbTitle.MappingName = "Title"
		Me.tcbTitle.Width = 140
		'
		'tcbHomePhone
		'
		Me.tcbHomePhone.Format = ""
		Me.tcbHomePhone.FormatInfo = Nothing
		Me.tcbHomePhone.HeaderText = "HomePhone"
		Me.tcbHomePhone.MappingName = "HomePhone"
		Me.tcbHomePhone.Width = 75
		'
		'Label1
		'
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(8, 30)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(72, 24)
		Me.Label1.TabIndex = 7
		Me.Label1.Text = "LastName:"
		'
		'txtLastName
		'
		Me.txtLastName.Location = New System.Drawing.Point(80, 32)
		Me.txtLastName.Name = "txtLastName"
		Me.txtLastName.Size = New System.Drawing.Size(168, 20)
		Me.txtLastName.TabIndex = 8
		Me.txtLastName.Text = ""
		'
		'Label2
		'
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(8, 71)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(56, 23)
		Me.Label2.TabIndex = 9
		Me.Label2.Text = "Title:"
		'
		'txtTitle
		'
		Me.txtTitle.Location = New System.Drawing.Point(80, 72)
		Me.txtTitle.Name = "txtTitle"
		Me.txtTitle.Size = New System.Drawing.Size(168, 20)
		Me.txtTitle.TabIndex = 10
		Me.txtTitle.Text = ""
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.Label4)
		Me.GroupBox1.Controls.Add(Me.btnEmployeeSearch)
		Me.GroupBox1.Controls.Add(Me.Label1)
		Me.GroupBox1.Controls.Add(Me.txtLastName)
		Me.GroupBox1.Controls.Add(Me.Label2)
		Me.GroupBox1.Controls.Add(Me.txtTitle)
		Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.GroupBox1.Location = New System.Drawing.Point(0, 80)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(480, 112)
		Me.GroupBox1.TabIndex = 11
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Employees Query "
		'
		'Label4
		'
		Me.Label4.Location = New System.Drawing.Point(296, 24)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(144, 40)
		Me.Label4.TabIndex = 12
		Me.Label4.Text = "Use Wild Card % as This Query Uses LIKE"
		'
		'btnEmployeeSearch
		'
		Me.btnEmployeeSearch.Location = New System.Drawing.Point(384, 80)
		Me.btnEmployeeSearch.Name = "btnEmployeeSearch"
		Me.btnEmployeeSearch.TabIndex = 11
		Me.btnEmployeeSearch.Text = "Search"
		'
		'DataGrid2
		'
		Me.DataGrid2.CaptionText = "Northwind.Products"
		Me.DataGrid2.DataMember = ""
		Me.DataGrid2.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.DataGrid2.Location = New System.Drawing.Point(488, 208)
		Me.DataGrid2.Name = "DataGrid2"
		Me.DataGrid2.Size = New System.Drawing.Size(448, 296)
		Me.DataGrid2.TabIndex = 12
		Me.DataGrid2.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle2})
		'
		'DataGridTableStyle2
		'
		Me.DataGridTableStyle2.AlternatingBackColor = System.Drawing.Color.Khaki
		Me.DataGridTableStyle2.BackColor = System.Drawing.Color.Wheat
		Me.DataGridTableStyle2.DataGrid = Me.DataGrid2
		Me.DataGridTableStyle2.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.tbcProductName, Me.tcbUnitPrice, Me.tcbUnitsInStock, Me.tcbDiscontinued})
		Me.DataGridTableStyle2.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.DataGridTableStyle2.MappingName = "Products"
		'
		'tbcProductName
		'
		Me.tbcProductName.Format = ""
		Me.tbcProductName.FormatInfo = Nothing
		Me.tbcProductName.HeaderText = "ProductName"
		Me.tbcProductName.MappingName = "ProductName"
		Me.tbcProductName.Width = 170
		'
		'tcbUnitPrice
		'
		Me.tcbUnitPrice.Format = ""
		Me.tcbUnitPrice.FormatInfo = Nothing
		Me.tcbUnitPrice.HeaderText = "UnitPrice"
		Me.tcbUnitPrice.MappingName = "UnitPrice"
		Me.tcbUnitPrice.Width = 75
		'
		'tcbUnitsInStock
		'
		Me.tcbUnitsInStock.Format = ""
		Me.tcbUnitsInStock.FormatInfo = Nothing
		Me.tcbUnitsInStock.HeaderText = "UnitsInStock"
		Me.tcbUnitsInStock.MappingName = "UnitsInStock"
		Me.tcbUnitsInStock.Width = 75
		'
		'tcbDiscontinued
		'
		Me.tcbDiscontinued.FalseValue = False
		Me.tcbDiscontinued.HeaderText = "Discontinued"
		Me.tcbDiscontinued.MappingName = "Discontinued"
		Me.tcbDiscontinued.NullValue = CType(resources.GetObject("tcbDiscontinued.NullValue"), Object)
		Me.tcbDiscontinued.TrueValue = True
		Me.tcbDiscontinued.Width = 75
		'
		'grpDiscontinued
		'
		Me.grpDiscontinued.Controls.Add(Me.cmbOperator)
		Me.grpDiscontinued.Controls.Add(Me.chkDiscontinued)
		Me.grpDiscontinued.Controls.Add(Me.txtUnitPrice)
		Me.grpDiscontinued.Controls.Add(Me.Label3)
		Me.grpDiscontinued.Controls.Add(Me.btnProductSearch)
		Me.grpDiscontinued.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.grpDiscontinued.Location = New System.Drawing.Point(488, 80)
		Me.grpDiscontinued.Name = "grpDiscontinued"
		Me.grpDiscontinued.Size = New System.Drawing.Size(448, 112)
		Me.grpDiscontinued.TabIndex = 13
		Me.grpDiscontinued.TabStop = False
		Me.grpDiscontinued.Text = "Products Query "
		'
		'cmbOperator
		'
		Me.cmbOperator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbOperator.Items.AddRange(New Object() {"Equal", "Not Equal", "Greater Than", "Greater Than Or Equal", "Less Than", "Less Than Or Equal"})
		Me.cmbOperator.Location = New System.Drawing.Point(256, 24)
		Me.cmbOperator.Name = "cmbOperator"
		Me.cmbOperator.Size = New System.Drawing.Size(168, 21)
		Me.cmbOperator.TabIndex = 12
		'
		'chkDiscontinued
		'
		Me.chkDiscontinued.Location = New System.Drawing.Point(16, 64)
		Me.chkDiscontinued.Name = "chkDiscontinued"
		Me.chkDiscontinued.Size = New System.Drawing.Size(136, 24)
		Me.chkDiscontinued.TabIndex = 3
		Me.chkDiscontinued.Text = "Discontinued Only"
		'
		'txtUnitPrice
		'
		Me.txtUnitPrice.Location = New System.Drawing.Point(128, 24)
		Me.txtUnitPrice.Name = "txtUnitPrice"
		Me.txtUnitPrice.Size = New System.Drawing.Size(104, 20)
		Me.txtUnitPrice.TabIndex = 1
		Me.txtUnitPrice.Text = ""
		'
		'Label3
		'
		Me.Label3.Location = New System.Drawing.Point(16, 24)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(72, 23)
		Me.Label3.TabIndex = 0
		Me.Label3.Text = "UnitPrice:"
		'
		'btnProductSearch
		'
		Me.btnProductSearch.Location = New System.Drawing.Point(352, 80)
		Me.btnProductSearch.Name = "btnProductSearch"
		Me.btnProductSearch.TabIndex = 11
		Me.btnProductSearch.Text = "Search"
		'
		'btnEmployeesEditRow
		'
		Me.btnEmployeesEditRow.Location = New System.Drawing.Point(144, 512)
		Me.btnEmployeesEditRow.Name = "btnEmployeesEditRow"
		Me.btnEmployeesEditRow.Size = New System.Drawing.Size(152, 23)
		Me.btnEmployeesEditRow.TabIndex = 14
		Me.btnEmployeesEditRow.Text = "Edit Selected Row"
		'
		'btnEmployeesSave
		'
		Me.btnEmployeesSave.Location = New System.Drawing.Point(8, 512)
		Me.btnEmployeesSave.Name = "btnEmployeesSave"
		Me.btnEmployeesSave.Size = New System.Drawing.Size(120, 23)
		Me.btnEmployeesSave.TabIndex = 15
		Me.btnEmployeesSave.Text = "Save Employees"
		'
		'btnProductsSave
		'
		Me.btnProductsSave.Location = New System.Drawing.Point(488, 512)
		Me.btnProductsSave.Name = "btnProductsSave"
		Me.btnProductsSave.Size = New System.Drawing.Size(120, 23)
		Me.btnProductsSave.TabIndex = 15
		Me.btnProductsSave.Text = "Save Products"
		'
		'btnSaveTransaction
		'
		Me.btnSaveTransaction.Location = New System.Drawing.Point(8, 544)
		Me.btnSaveTransaction.Name = "btnSaveTransaction"
		Me.btnSaveTransaction.Size = New System.Drawing.Size(928, 23)
		Me.btnSaveTransaction.TabIndex = 16
		Me.btnSaveTransaction.Text = "Save All Changes in a Transaction"
		'
		'btnComboBoxFill
		'
		Me.btnComboBoxFill.Location = New System.Drawing.Point(624, 512)
		Me.btnComboBoxFill.Name = "btnComboBoxFill"
		Me.btnComboBoxFill.Size = New System.Drawing.Size(144, 23)
		Me.btnComboBoxFill.TabIndex = 17
		Me.btnComboBoxFill.Text = "Fill a ComboBox"
		'
		'Form1
		'
#If (VS2005) Then
		Me.AutoScaleDimensions = New System.Drawing.Size(5, 13)
#Else
		Me.AutoScaleBaseSize = new System.Drawing.Size(5, 13)
#End If
		Me.ClientSize = New System.Drawing.Size(944, 581)
		Me.Controls.Add(Me.btnComboBoxFill)
		Me.Controls.Add(Me.btnSaveTransaction)
		Me.Controls.Add(Me.btnEmployeesSave)
		Me.Controls.Add(Me.btnEmployeesEditRow)
		Me.Controls.Add(Me.grpDiscontinued)
		Me.Controls.Add(Me.DataGrid2)
		Me.Controls.Add(Me.DataGrid1)
		Me.Controls.Add(Me.panel1)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.btnProductsSave)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "Form1"
		Me.Text = "MyGeneration Software's VB.NET dOOdad Demo"
		Me.panel1.ResumeLayout(False)
		CType(Me.DataGrid1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox1.ResumeLayout(False)
		CType(Me.DataGrid2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpDiscontinued.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub

#End Region

    Sub MasterSampleTest()

		Dim ms As New TheMasterSample
		ms.AggregateTest()
        ms.SimpleLoad()
        ms.MoreComplexLoad()
        ms.TheDeluxeQuery()
        ms.GenerateSql()
        ms.DataReader()
        ms.Iteration()
        ms.FilterAndSort()
        ms.DemonstrateBulkUpdates()
        ms.Transactions()
        ms.FillComboBox()
        ms.AddColumn()

    End Sub

	Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		' This is the Invoice View, Its in the project for unit test, you can generate
		' business entities from Views using "VbNet_SQL_dOOdads_View.vbgen", there are read only
		'Dim inv As New Invoices
		'inv.LoadAll()

		' ** WARNING: ** THIS METHOD MODIFIES DATA IN NORTHWIND
		' MasterSampleTest()

		BindEmployeesGrid()
		BindProductsGrid()

		Me.cmbOperator.SelectedItem = "Equal"


	End Sub

	Private Sub BindEmployeesGrid()

		emps = New Employees

		Try
			emps.Query.Load()

			Dim lastName As String
			Do
				lastName = emps.LastName
			Loop Until Not emps.MoveNext

		Catch ex As Exception
			MessageBox.Show("Edit your 'dOOdad_Demo.exe.config' file to correct the connection string for your SQL Northwind database")
		End Try

		Me.DataGrid1.DataSource = emps.DefaultView

	End Sub

	Private Sub BindProductsGrid()

		prds = New Products

		Try
			prds.Query.Load()
		Catch ex As Exception
			MessageBox.Show("Edit your 'dOOdad_Demo.exe.config' file to correct the connection string for your SQL Northwind database")
		End Try

		Me.DataGrid2.DataSource = prds.DefaultView

	End Sub

	Private Sub btnEmployeeSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmployeeSearch.Click

		emps = New Employees

		Try
			If Not Me.txtLastName.Text.Trim() = String.Empty Then
				emps.Where.LastName.[Operator] = WhereParameter.Operand.Like_
				emps.Where.LastName.Value = Me.txtLastName.Text.Trim()
			End If

			If Not Me.txtTitle.Text.Trim() = String.Empty Then
				emps.Where.Title.[Operator] = WhereParameter.Operand.Like_
				emps.Where.Title.Value = Me.txtTitle.Text.Trim()
			End If

			' Could it be any easier?
			emps.Query.Load()

		Catch ex As Exception
		End Try

		Me.DataGrid1.DataSource = emps.DefaultView

	End Sub

	Private Sub btnEmployeesEditRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmployeesEditRow.Click

		Dim row As DataRow = SelectedRow(Me.DataGrid1)

		If Not row Is Nothing Then

			Dim employeeID As String = row("EmployeeID", DataRowVersion.Current).ToString()

			If Not employeeID = String.Empty Then

				Dim empEdit As New EmployeesEdit(Convert.ToInt32(employeeID))
				Dim result As DialogResult = empEdit.ShowDialog()
				If result = DialogResult.OK Then
					' Employees Grid
					emps = New Employees
					emps.Query.Load()
					Me.DataGrid1.DataSource = emps.DefaultView
				End If
			End If
		End If

	End Sub

	Private Function SelectedRow(ByVal grid As DataGrid) As DataRow

		If Not grid.DataSource Is Nothing Then
			Dim bm As BindingManagerBase = grid.BindingContext(grid.DataSource, grid.DataMember)

			If Not bm.Current Is Nothing Then
				Return CType(bm.Current, DataRowView).Row
			End If
		End If

		Return Nothing

	End Function

	Private Sub btnEmployeesSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmployeesSave.Click
		Try
			Me.emps.Save()
			BindEmployeesGrid()
		Catch ex As Exception
			MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.")
		End Try
	End Sub

	Private Sub btnProductSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProductSearch.Click

		prds = New Products

		Try
			If Not Me.txtUnitPrice.Text.Trim() = String.Empty Then
				prds.Where.UnitPrice.Value = Me.txtUnitPrice.Text.Trim()

				Select Case CType(Me.cmbOperator.SelectedItem, String)
					Case "Equal"
						prds.Where.UnitPrice.[Operator] = WhereParameter.Operand.Equal
					Case "Not Equal"
						prds.Where.UnitPrice.[Operator] = WhereParameter.Operand.NotEqual
					Case "Greater Than"
						prds.Where.UnitPrice.[Operator] = WhereParameter.Operand.GreaterThan
					Case "Greater Than Or Equal"
						prds.Where.UnitPrice.[Operator] = WhereParameter.Operand.GreaterThanOrEqual
					Case "Less Than"
						prds.Where.UnitPrice.[Operator] = WhereParameter.Operand.LessThan
					Case "Less Than Or Equal"
						prds.Where.UnitPrice.[Operator] = WhereParameter.Operand.LessThanOrEqual
				End Select
			End If

			If Me.chkDiscontinued.Checked Then
				prds.Where.Discontinued.Value = Me.chkDiscontinued.Checked
			End If

			' Could it be any easier?
			prds.Query.Load()

		Catch ex As Exception
		End Try

		Me.DataGrid2.DataSource = prds.DefaultView
	End Sub

	Private Sub btnProductsEditRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

	End Sub

	Private Sub btnProductsSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProductsSave.Click
		Try
			Me.prds.Save()
			BindProductsGrid()
		Catch ex As Exception
			MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.")
		End Try
	End Sub

	Private Sub btnSaveTransaction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveTransaction.Click

		Dim tx As TransactionMgr
		tx = TransactionMgr.ThreadTransactionMgr()

		' Business Entities no nothing about the transaction, you can next BeginTransaction/CommitTransaction as well, as
		' long as you sandwich the calls, once you call RollbackTransaction, it's game over 
		Try
			tx.BeginTransaction()

			Me.emps.Save()
			Me.prds.Save()

			tx.CommitTransaction()

			BindEmployeesGrid()
			BindProductsGrid()

		Catch ex As Exception

			tx.RollbackTransaction()
			TransactionMgr.ThreadTransactionMgrReset()
			MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.")

		End Try

	End Sub

	Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
		Dim about As New About
		about.ShowDialog()
	End Sub

	Private Sub btnComboBoxFill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnComboBoxFill.Click

		Dim comboFill As New FillComboBox
		comboFill.ShowDialog()

	End Sub

End Class
