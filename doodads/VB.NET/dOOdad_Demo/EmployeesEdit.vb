Public Class EmployeesEdit
    Inherits System.Windows.Forms.Form

    Dim emps As New Employees

    Sub New(ByVal EmployeeID As Int32)

        Me.New()

        ' Normally, I would use Employees.LoadByPrimaryKey but for this demo, if the user hasn't
        ' generated the stored procs this method will work.
        emps.Where.EmployeeID.Value = EmployeeID
        emps.Query.Load()

        'emps.LoadByPrimaryKey(EmployeeID)

        ' NOTE: We could do this but we'll data bind instead
        'Me.txtFirstName.Text = emps.FirstName
        'Me.txtLastName.Text = emps.LastName

		Me.txtFirstName.DataBindings.Add(New Binding("Text", emps, Employees.StringPropertyNames.FirstName))
		Me.txtLastName.DataBindings.Add(New Binding("Text", emps, Employees.StringPropertyNames.LastName))

        Me.Text = "Northwind.Employee : ID = [" + emps.EmployeeID.ToString() + "]"

    End Sub

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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFirstName As System.Windows.Forms.TextBox
    Friend WithEvents txtLastName As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtFirstName = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtLastName = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtFirstName
        '
        Me.txtFirstName.Location = New System.Drawing.Point(112, 16)
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.Size = New System.Drawing.Size(152, 20)
        Me.txtFirstName.TabIndex = 3
        Me.txtFirstName.Text = ""
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(32, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "First Name:"
        '
        'txtLastName
        '
        Me.txtLastName.Location = New System.Drawing.Point(112, 64)
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.Size = New System.Drawing.Size(152, 20)
        Me.txtLastName.TabIndex = 5
        Me.txtLastName.Text = ""
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(32, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 23)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Last Name:"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(104, 112)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(192, 112)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "Save"
        '
        'EmployeesEdit
        '
        Me.AcceptButton = Me.btnOK
#If (VS2005) Then
		Me.AutoScaleDimensions = New System.Drawing.Size(5, 13)
#Else
		Me.AutoScaleBaseSize = new System.Drawing.Size(5, 13)
#End If
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(312, 151)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtLastName)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtFirstName)
        Me.Controls.Add(Me.Label2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EmployeesEdit"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ' NOTE: We used databinding so this isn't necessary
        'emps.FirstName = Me.txtFirstName.Text
        'emps.LastName = Me.txtLastName.Text

        Try
            emps.Save()
        Catch ex As Exception
            MessageBox.Show("You need to generate the stored procedures for 'Employees' and 'Products' to be able to save. Use 'SQL_StoredProcs.vbgen' to generate them.")
        End Try

        Me.DialogResult = DialogResult.OK

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.DialogResult = DialogResult.Cancel

    End Sub

End Class
