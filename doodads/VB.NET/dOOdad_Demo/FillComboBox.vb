Public Class FillComboBox
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents bntClose As System.Windows.Forms.Button
    Friend WithEvents cmbBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.bntClose = New System.Windows.Forms.Button
        Me.cmbBox = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'bntClose
        '
        Me.bntClose.Location = New System.Drawing.Point(264, 128)
        Me.bntClose.Name = "bntClose"
        Me.bntClose.TabIndex = 0
        Me.bntClose.Text = "Close"
        '
        'cmbBox
        '
        Me.cmbBox.Location = New System.Drawing.Point(16, 24)
        Me.cmbBox.Name = "cmbBox"
        Me.cmbBox.Size = New System.Drawing.Size(320, 21)
        Me.cmbBox.TabIndex = 1
        '
        'FillComboBox
        '
        Me.AcceptButton = Me.bntClose
#If (VS2005) Then
		Me.AutoScaleDimensions = New System.Drawing.Size(5, 13)
#Else
		Me.AutoScaleBaseSize = new System.Drawing.Size(5, 13)
#End If
        Me.ClientSize = New System.Drawing.Size(352, 165)
        Me.Controls.Add(Me.cmbBox)
        Me.Controls.Add(Me.bntClose)
        Me.Name = "FillComboBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fill a ComboBox using a dOOdad"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub bntClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntClose.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub FillComboBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-----------------------------------------------------
        ' ** dOOdad Tip **
        '-----------------------------------------------------
        ' You will find that a dOOdad can do almost anything, no need to write a million little 
        ' specific stored procedures, this code limits the columns, sorts, and fills a combo-box
        ' there's nothing to it

        ' Let's query on the Product Name and sort it
        Dim prds As New Products

        ' Note we only bring back these two columns for performance, why bring back more?
		prds.Query.AddResultColumn(Products.ColumnNames.ProductID)
		prds.Query.AddResultColumn(Products.ColumnNames.ProductName)

        ' Sort
		prds.Query.AddOrderBy(Products.ColumnNames.ProductName, MyGeneration.dOOdads.WhereParameter.Dir.ASC)

        ' Load it
        Try
            prds.Query.Load()
        Catch ex As Exception
        End Try


        ' Bind it
		Me.cmbBox.DisplayMember = Products.ColumnNames.ProductName
        Me.cmbBox.DataSource = prds.DefaultView

    End Sub
End Class
