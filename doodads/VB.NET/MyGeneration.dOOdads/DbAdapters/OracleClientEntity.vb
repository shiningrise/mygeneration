'==============================================================================
' MyGeneration.dOOdads
'
' OracleClientEntity.vb
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
Imports System.Data.OracleClient

Namespace MyGeneration.dOOdads

	Public Class OracleClientEntity
		Inherits BusinessEntity

		Friend Overrides Function ConvertIDbDataAdapter(ByVal dataAdapter As IDbDataAdapter) As Common.DbDataAdapter
			Return CType(dataAdapter, DbDataAdapter)
		End Function

		Friend Overrides Function CreateDynamicQuery(ByVal entity As BusinessEntity) As DynamicQuery
			Return New OracleClientDynamicQuery(entity)
		End Function

		Friend Overloads Overrides Function CreateIDataParameter() As IDataParameter
			Return New OracleParameter
		End Function

		Friend Overloads Overrides Function CreateIDataParameter(ByVal name As String, ByVal value As Object) As IDataParameter
			Dim p As New OracleParameter
			p.ParameterName = name
			p.Value = value
			Return p
		End Function

		Friend Overrides Function CreateIDbCommand() As IDbCommand
			Return New OracleCommand
		End Function

		Friend Overrides Function CreateIDbConnection() As IDbConnection
			Return New OracleConnection
		End Function

		Friend Overrides Function CreateIDbDataAdapter() As IDbDataAdapter
			Return New OracleDataAdapter
        End Function

        ' Custom stuff for Oracle that is not MustOverride
        Public Overrides Sub AddNew()

            If MyBase.DataTable Is Nothing Then
                Me.LoadFromSql("SELECT * FROM " + QuerySource() + " WHERE rownum < 1", Nothing, CommandType.Text)
            End If

            Dim newRow As DataRow = MyBase.DataTable.NewRow()
            MyBase.DataTable.Rows.Add(newRow)
            MyBase.DataRow = newRow

        End Sub

        Friend Overrides Function RequiresSquareBrackets() As Boolean
            Return False
        End Function

        Protected Overrides Function _LoadFromRawSql(ByVal rawSql As String, ByVal ParamArray parameters() As Object) As IDbCommand

            Dim i As Integer = 0
            Dim token As String = ""
            Dim sIndex As String = ""
            Dim param As String = ""

            Dim cmd As OracleCommand = New OracleCommand

            Dim o As Object
            For Each o In parameters

                sIndex = i.ToString()
                token = "{" + sIndex + "}"
                param = ":p" + sIndex

                rawSql = rawSql.Replace(token, param)

                Dim p As OracleParameter = New OracleParameter(param, o)
                cmd.Parameters.Add(p)
                i = i + 1
            Next

            cmd.CommandText = rawSql
            Return cmd

        End Function

    End Class

End Namespace