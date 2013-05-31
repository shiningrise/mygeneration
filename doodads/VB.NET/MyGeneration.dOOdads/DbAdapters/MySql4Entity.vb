'==============================================================================
' MyGeneration.dOOdads
'
' MySql4Entity.vb
' Version 5.0
' Updated - 09/15/2005
'------------------------------------------------------------------------------
' Copyright 2004, 2005 by MyGeneration Software.
' All Rights Reserved.
'
' Permission to use, copy, modify, and distribute this software and its 
' documentation for any purpose and without fee is hereby granted, 
' provided that the above copyright notice appear in all copies and that 
' both that copyright notice and this permission notice appear in 
' supporting documentation. 
'
' MYGENERATION SOFTWARE DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS 
' SOFTWARE, INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY 
' AND FITNESS, IN NO EVENT SHALL MYGENERATION SOFTWARE BE LIABLE FOR ANY 
' SPECIAL, INDIRECT OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES 
' WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, 
' WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER 
' TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE 
' OR PERFORMANCE OF THIS SOFTWARE. 
'==============================================================================

Imports System.Data
Imports System.Data.Common
Imports MySql.Data.MySqlClient

Namespace MyGeneration.dOOdads

    Public Class MySql4Entity
        Inherits BusinessEntity

        Friend Overrides Function ConvertIDbDataAdapter(ByVal dataAdapter As IDbDataAdapter) As Common.DbDataAdapter
            Return CType(dataAdapter, DbDataAdapter)
        End Function

        Friend Overrides Function CreateDynamicQuery(ByVal entity As BusinessEntity) As DynamicQuery
            Return New MySql4DynamicQuery(entity)
        End Function

        Friend Overloads Overrides Function CreateIDataParameter() As IDataParameter
            Return New MySqlParameter
        End Function

        Friend Overloads Overrides Function CreateIDataParameter(ByVal name As String, ByVal value As Object) As IDataParameter
            Dim p As New MySqlParameter
            p.ParameterName = name
            p.Value = value
            Return p
        End Function

        Friend Overrides Function CreateIDbCommand() As IDbCommand
            Return New MySqlCommand
        End Function

        Friend Overrides Function CreateIDbConnection() As IDbConnection
            Return New MySqlConnection
        End Function

        Friend Overrides Function CreateIDbDataAdapter() As IDbDataAdapter
            Return New MySqlDataAdapter
        End Function

        Public Overrides Sub AddNew()

            If MyBase.DataTable Is Nothing Then
                Me.LoadFromSql("SELECT * FROM `" + QuerySource() + "` WHERE 1=0", Nothing, CommandType.Text)
            End If

            Dim newRow As DataRow = MyBase.DataTable.NewRow()
            MyBase.DataTable.Rows.Add(newRow)
            MyBase.DataRow = newRow

        End Sub

        Protected Overrides Function _LoadFromRawSql(ByVal rawSql As String, ByVal ParamArray parameters() As Object) As IDbCommand

            Dim i As Integer = 0
            Dim token As String = ""
            Dim sIndex As String = ""
            Dim param As String = ""

            Dim cmd As MySqlCommand = New MySqlCommand

            Dim o As Object
            For Each o In parameters

                sIndex = i.ToString()
                token = "{" + sIndex + "}"
                param = "?p" + sIndex

                rawSql = rawSql.Replace(token, param)

                Dim p As MySqlParameter = New MySqlParameter(param, o)
                cmd.Parameters.Add(p)
                i = i + 1
            Next

            cmd.CommandText = rawSql
            Return cmd

        End Function

#Region "@@IDENTITY Logic"

        ' Overloaded in the generated class
        Public Overridable Function GetAutoKeyColumns() As String
            Return ""
        End Function

        ' Called just before the Save() is truly executed
        Protected Overrides Sub HookupRowUpdateEvents(ByVal adapter As DbDataAdapter)

            ' We only bother hooking up the event if we have an AutoKey
            If Me.GetAutoKeyColumns().Length > 0 Then
                Dim da As MySqlDataAdapter = CType(adapter, MySqlDataAdapter)
                AddHandler da.RowUpdated, AddressOf OnRowUpdated
            End If
        End Sub

        ' If it's an Insert we fetch the @@Identity value and stuff it in the proper column
        Protected Sub OnRowUpdated(ByVal sender As Object, ByVal e As MySqlRowUpdatedEventArgs)

            Try
                If e.Status = UpdateStatus.[Continue] AndAlso e.StatementType = StatementType.Insert Then

                    Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()
                    Dim s As String

                    Dim identityCol As String = Me.GetAutoKeyColumns()

                    Dim cmd As MySqlCommand = New MySqlCommand
                    cmd.CommandText = "SELECT LAST_INSERT_ID();"

                    ' We make sure we enlist in the ongoing transaction, otherwise, we 
                    ' would most likely deadlock
                    txMgr.Enlist(cmd, Me)
                    Dim o As Object = cmd.ExecuteScalar() ' Get the Identity Value
                    txMgr.DeEnlist(cmd, Me)

                    If Not o Is Nothing Then
                        e.Row(identityCol) = o
                    End If

                    e.Row.AcceptChanges()
                End If

            Catch ex As Exception

            End Try

        End Sub

#End Region

    End Class

End Namespace