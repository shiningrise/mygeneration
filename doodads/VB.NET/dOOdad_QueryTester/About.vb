Imports System.Diagnostics

Public Class About
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
	Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.Label1 = New System.Windows.Forms.Label
		Me.btnClose = New System.Windows.Forms.Button
		Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
		Me.SuspendLayout()
		'
		'Label1
		'
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(40, 16)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(328, 24)
		Me.Label1.TabIndex = 7
		Me.Label1.Text = "This demo runs against the SQL Server Northwind Database"
		'
		'btnClose
		'
		Me.btnClose.Location = New System.Drawing.Point(152, 48)
		Me.btnClose.Name = "btnClose"
		Me.btnClose.TabIndex = 8
		Me.btnClose.Text = "Close"
		'
		'LinkLabel1
		'
		Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LinkLabel1.Location = New System.Drawing.Point(56, 88)
		Me.LinkLabel1.Name = "LinkLabel1"
		Me.LinkLabel1.Size = New System.Drawing.Size(320, 16)
		Me.LinkLabel1.TabIndex = 9
		Me.LinkLabel1.TabStop = True
		Me.LinkLabel1.Text = "Copyright (c) 2004-2005 MyGeneration Software"
		'
		'About
		'
		Me.AcceptButton = Me.btnClose
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(408, 136)
		Me.Controls.Add(Me.LinkLabel1)
		Me.Controls.Add(Me.btnClose)
		Me.Controls.Add(Me.Label1)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "About"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "About dOOdads"
		Me.ResumeLayout(False)

	End Sub

#End Region

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        Process.Start("http://www.mygenerationsoftware.com")
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.OK
    End Sub
End Class
