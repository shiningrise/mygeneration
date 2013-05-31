'==============================================================================
' MyGeneration.dOOdads
'
' DynamicQuery.vb
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

Imports System.Configuration
Imports System.Data
Imports System.Data.Common

Namespace MyGeneration.dOOdads

    Public MustInherit Class DynamicQuery

        Protected MustOverride Function _Load(Optional ByVal conjuction As String = "AND") As IDbCommand

        Public Sub New(ByVal entity As BusinessEntity)
            Me._entity = entity
        End Sub

        Public Function Load(Optional ByVal conjuction As String = "AND") As Boolean

            Dim loaded As Boolean = False
            Dim dt As DataTable = Nothing

			Try

				If ((_aggregateParameters Is Nothing OrElse _aggregateParameters.Count <= 0) AndAlso _
				 _resultColumns.Length <= 0 AndAlso _countAll = False) Then
					Me._entity._canSave = True
				End If

				Me._entity.OnQueryLoad(conjuction)

				Dim cmd As IDbCommand = _Load(conjuction)
				_lastQuery = cmd.CommandText

				Dim da As IDbDataAdapter = Me._entity.CreateIDbDataAdapter()
				da.SelectCommand = cmd

				Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

				dt = New DataTable(_entity.MappingName)

				txMgr.Enlist(cmd, _entity)
				Dim adapter As DbDataAdapter = Me._entity.ConvertIDbDataAdapter(da)
				adapter.Fill(dt)
				txMgr.DeEnlist(cmd, _entity)

			Catch ex As Exception

				Throw ex

			Finally

				Me._entity.DataTable = dt
				loaded = (dt.Rows.Count > 0)

			End Try

			Return loaded

        End Function

        Public Function GenerateSQL(Optional ByVal conjuction As String = "AND") As String
            ' This is for debugging purposes
            Dim sql As String = ""

            Try
                Dim cmd As IDbCommand = _Load(conjuction)
                sql = cmd.CommandText
            Catch ex As Exception
                Throw ex
            Finally
            End Try

            Return sql

        End Function

        Public ReadOnly Property LastQuery() As String
            Get
                Return _lastQuery
            End Get
        End Property

        Public Function ReturnReader(Optional ByVal conjuction As String = "AND") As IDataReader

            Dim loaded As Boolean = False

            Try

                Dim cmd As IDbCommand = _Load(conjuction)

                cmd.Connection = Me._entity.CreateIDbConnection()
                cmd.Connection.ConnectionString = _entity.RawConnectionString
                cmd.Connection.Open()
                Return cmd.ExecuteReader(CommandBehavior.CloseConnection)

            Catch ex As Exception

                Throw ex

            End Try

        End Function

        Public Property Top() As Integer
            Get
                Return _top
            End Get
            Set(ByVal Value As Integer)
                _top = Value
            End Set
        End Property

        Public Property Distinct() As Boolean
            Get
                Return _distinct
            End Get
            Set(ByVal Value As Boolean)
                _distinct = Value
            End Set
        End Property

		Public Property CountAll() As Boolean
			Get
				Return _countAll
			End Get
			Set(ByVal Value As Boolean)
				_countAll = Value
				If Value Then
					Me._entity._canSave = False
				End If
			End Set
		End Property

		Public Property CountAllAlias() As String
			Get
				Return _countAllAlias
			End Get
			Set(ByVal Value As String)
				_countAllAlias = Value
			End Set
		End Property

		Public Property WithRollup() As Boolean
			Get
				Return _withRollup
			End Get
			Set(ByVal Value As Boolean)
				_withRollup = Value
			End Set
		End Property

		Public Sub AddWhereParemeter(ByRef wItem As WhereParameter)

			If _whereParameters Is Nothing Then
				_whereParameters = New ArrayList
			End If

			_whereParameters.Add(wItem)

		End Sub

		Public ReadOnly Property ParameterCount() As Integer
			Get
				Dim count As Integer = 0
				If Not _whereParameters Is Nothing Then
					count = _whereParameters.Count
				End If
				Return count
			End Get
		End Property

		Public Sub AddAggregateParameter(ByVal wItem As AggregateParameter)

			If _aggregateParameters Is Nothing Then
				_aggregateParameters = New ArrayList
			End If

			_aggregateParameters.Add(wItem)

			' We don't allow Save() to succeed once they reduce the columns
			Me._entity._canSave = False
		End Sub

		Public ReadOnly Property AggregateCount() As Integer
			Get
				Dim count As Integer = 0

				If Not _aggregateParameters Is Nothing Then
					count = _aggregateParameters.Count
				End If

				Return count
			End Get
		End Property

		Public Overridable Sub AddOrderBy(ByVal column As String, ByVal direction As WhereParameter.Dir)

			If _orderBy.Length > 0 Then
				_orderBy += ", "
			End If

			_orderBy += column

			If direction = WhereParameter.Dir.ASC Then
				_orderBy += " ASC"
			Else
				_orderBy += " DESC"
			End If

		End Sub

		Public MustOverride Sub AddOrderBy(ByVal countAll As DynamicQuery, ByVal direction As WhereParameter.Dir)
		Public MustOverride Sub AddOrderBy(ByVal aggregate As AggregateParameter, ByVal direction As WhereParameter.Dir)

		Public Overridable Sub AddGroupBy(ByVal column As String)

			If _groupBy.Length > 0 Then
				_groupBy += ", "
			End If

			_groupBy += column
		End Sub

		Public MustOverride Sub AddGroupBy(ByVal aggregate As AggregateParameter)

		Public Sub FlushWhereParameters()

			If Not _whereParameters Is Nothing Then
				_whereParameters.Clear()
			End If

			_orderBy = String.Empty

		End Sub

		Public Sub FlushAggregateParameters()

			If Not _aggregateParameters Is Nothing Then
				_aggregateParameters.Clear()
			End If

			_countAll = False
			_countAllAlias = String.Empty
			_groupBy = String.Empty

		End Sub

		Public Overridable Sub AddResultColumn(ByVal columnName As String)

			If _resultColumns.Length > 0 Then
				_resultColumns += ","
			End If

			_resultColumns += columnName

			Me._entity._canSave = False

		End Sub

		Public Sub ResultColumnsClear()

			_resultColumns = String.Empty

		End Sub

		Public Sub AddConjunction(ByVal conjuction As WhereParameter.Conj)

			If _whereParameters Is Nothing Then
				_whereParameters = New ArrayList
			End If

			If Not conjuction = WhereParameter.Conj.UseDefault Then
				If conjuction = WhereParameter.Conj.AND_ Then
					_whereParameters.Add(" AND ")
				Else
					_whereParameters.Add(" OR ")
				End If
			End If

		End Sub

		Public Sub OpenParenthesis()

			If _whereParameters Is Nothing Then
				_whereParameters = New ArrayList
			End If

			_whereParameters.Add("(")

		End Sub

		Public Sub CloseParenthesis()

			If _whereParameters Is Nothing Then
				_whereParameters = New ArrayList
			End If

			_whereParameters.Add(")")

		End Sub

		Protected _whereParameters As ArrayList = Nothing
		Protected _aggregateParameters As ArrayList = Nothing
		Protected _resultColumns As String = String.Empty
		Protected _orderBy As String = String.Empty
		Protected _groupBy As String = String.Empty

		Protected _top As Integer = -1
		Protected _distinct As Boolean = False
		Protected _countAll As Boolean = False
		Protected _countAllAlias As String = String.Empty
		Protected _withRollup As Boolean = False

		Protected _entity As BusinessEntity
		Protected inc As Integer = 0

		Private _lastQuery As String = ""

	End Class

End Namespace
