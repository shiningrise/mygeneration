Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Text

Namespace MyGeneration.dOOdads
    Public Class VisualFoxProEntity
        Inherits BusinessEntity
        Public Sub New()
        End Sub

        Friend Overloads Overrides Function CreateDynamicQuery(ByVal entity As BusinessEntity) As DynamicQuery
            Return New VisualFoxProDynamicQuery(entity)
        End Function

        Friend Overloads Overrides Function CreateIDataParameter(ByVal name As String, ByVal value As Object) As IDataParameter
            Dim p As New OleDbParameter()
            p.ParameterName = name
            p.Value = value
            Return p
        End Function

        Friend Overloads Overrides Function CreateIDataParameter() As IDataParameter
            Return New OleDbParameter()
        End Function

        Friend Overloads Overrides Function CreateIDbCommand() As IDbCommand
            Return New OleDbCommand()
        End Function

        Friend Overloads Overrides Function CreateIDbDataAdapter() As IDbDataAdapter
            Return New OleDbDataAdapter()
        End Function

        Friend Overloads Overrides Function CreateIDbConnection() As IDbConnection
            Return New OleDbConnection()
        End Function

        Friend Overloads Overrides Function ConvertIDbDataAdapter(ByVal dataAdapter As IDbDataAdapter) As DbDataAdapter
            Return TryCast(TryCast(dataAdapter, OleDbDataAdapter), DbDataAdapter)
        End Function

        ' Overloaded in the generated class 
        Public Overridable Function GetAutoKeyColumn() As String
            Return ""
        End Function

        ' Called just before the Save() is truly executed 
        Protected Overloads Overrides Sub HookupRowUpdateEvents(ByVal adapter As DbDataAdapter)
            ' We only bother hooking up the event if we have an AutoKey 
            If Me.GetAutoKeyColumn().Length > 0 Then
                Dim da As OleDbDataAdapter = TryCast(adapter, OleDbDataAdapter)
                AddHandler da.RowUpdated, AddressOf OnRowUpdated
            End If
        End Sub

        ' If it's an Insert we fetch the @@Identity value and stuff it in the proper column 
        Protected Sub OnRowUpdated(ByVal sender As Object, ByVal e As OleDbRowUpdatedEventArgs)
            Try
                If e.Status = UpdateStatus.[Continue] AndAlso e.StatementType = StatementType.Insert Then
                    Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

                    Dim cmd As New OleDbCommand("SELECT @@IDENTITY")

                    ' We make sure we enlist in the ongoing transaction, otherwise, we 
                    ' would most likely deadlock 
                    txMgr.Enlist(cmd, Me)
                    Dim o As Object = cmd.ExecuteScalar()
                    ' Get the Identity Value 
                    txMgr.DeEnlist(cmd, Me)

                    If o IsNot Nothing Then
                        e.Row(Me.GetAutoKeyColumn()) = o
                        e.Row.AcceptChanges()
                    End If
                End If
            Catch
            End Try
        End Sub

        Protected Overloads Overrides Function _LoadFromRawSql(ByVal rawSql As String, ByVal ParamArray parameters As Object()) As IDbCommand
            Dim i As Integer = 0
            Dim token As String = ""
            Dim sIndex As String = ""
            Dim param As String = ""

            Dim cmd As New OleDbCommand()

            For Each o As Object In parameters
                sIndex = i.ToString()
                token = "{"c + sIndex + "}"c
                param = "@p" & sIndex

                rawSql = rawSql.Replace(token, param)

                Dim p As New OleDbParameter(param, o)
                cmd.Parameters.Add(p)
                i += 1
            Next

            cmd.CommandText = rawSql
            Return cmd
        End Function
    End Class
End Namespace