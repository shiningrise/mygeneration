'==============================================================================
' MyGeneration.dOOdads
'
' FirebirdSqlDynamicQuery.vb
' Version 5.0
' Updated - 10/12/2005
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
Imports FirebirdSql.Data.Firebird

Namespace MyGeneration.dOOdads

    Public Class FirebirdSqlDynamicQuery
        Inherits DynamicQuery

        Public Sub New(ByVal entity As BusinessEntity)
            MyBase.New(entity)
        End Sub

		Public Overloads Overrides Sub AddOrderBy(ByVal column As String, ByVal direction As WhereParameter.Dir)
			MyBase.AddOrderBy(D(column), direction)
		End Sub

		Public Overloads Overrides Sub AddOrderBy(ByVal countAll As DynamicQuery, ByVal direction As WhereParameter.Dir)
			If countAll.CountAll Then
				MyBase.AddOrderBy("COUNT(*)", direction)
			End If
		End Sub

		Public Overloads Overrides Sub AddOrderBy(ByVal aggregate As AggregateParameter, ByVal direction As WhereParameter.Dir)
			MyBase.AddOrderBy(GetAggregate(aggregate, False), direction)
		End Sub

		Public Overloads Overrides Sub AddGroupBy(ByVal column As String)
			MyBase.AddGroupBy(D(column))
		End Sub

		Public Overloads Overrides Sub AddGroupBy(ByVal aggregate As AggregateParameter)
			MyBase.AddGroupBy(GetAggregate(aggregate, False))
		End Sub

        Public Overrides Sub AddResultColumn(ByVal columnName As String)
            MyBase.AddResultColumn(D(columnName))
		End Sub

		Protected Function GetAggregate(ByVal wItem As AggregateParameter, ByVal withAlias As Boolean) As String

			Dim query As String = String.Empty

			Select Case (wItem.Function)

				Case AggregateParameter.Func.Avg
					query += "AVG("

				Case AggregateParameter.Func.Count
					query += "COUNT("

				Case AggregateParameter.Func.Max
					query += "MAX("

				Case AggregateParameter.Func.Min
					query += "MIN("

				Case AggregateParameter.Func.Sum
					query += "SUM("

				Case AggregateParameter.Func.StdDev
					query += "STDEV("

				Case AggregateParameter.Func.Var
					query += "VAR("

			End Select

			If wItem.Distinct Then
				query += "DISTINCT "
			End If

			query += D(wItem.Column) + ")"

			If withAlias AndAlso Not wItem.Alias = String.Empty Then
				query += " AS " + D(wItem.Alias)
			End If

			Return query

		End Function

		Protected Overrides Function _Load(Optional ByVal conjuction As String = "AND") As IDbCommand

			Dim hasColumn As Boolean = False
			Dim selectAll As Boolean = True
			Dim query As String
			Dim p As Integer = 1

			query = "SELECT "

			If Me._distinct Then query += " DISTINCT "
			If Me._top >= 0 Then query += " FIRST " + Me._top.ToString() + " "

			If Me._resultColumns.Length > 0 Then
				query += Me._resultColumns
				hasColumn = True
				selectAll = False
			End If

			If Me._countAll Then

				If hasColumn Then

					query += ", "
				End If

				query += "COUNT(*)"

				If Not Me._countAllAlias = String.Empty Then
					' Need DBMS string delimiter here
					query += " AS " + D(Me._countAllAlias)
				End If

				hasColumn = True
				selectAll = False
			End If

			If Not _aggregateParameters Is Nothing AndAlso _aggregateParameters.Count > 0 Then
				Dim isFirst As Boolean = True

				If hasColumn Then
					query += ", "
				End If

				Dim wItem As AggregateParameter
				Dim obj As Object

				For Each obj In _aggregateParameters

					wItem = CType(obj, AggregateParameter)

					If wItem.IsDirty Then

						If isFirst Then
							query += GetAggregate(wItem, True)
							isFirst = False
						Else
							query += ", " + GetAggregate(wItem, True)
						End If
					End If
				Next

				selectAll = False
			End If

			If selectAll Then
				query += "*"
			End If

			query += " FROM " + D(Me._entity.QuerySource())

			Dim cmd As New FbCommand

			If Not _whereParameters Is Nothing AndAlso _whereParameters.Count > 0 Then

				query += " WHERE "

				Dim first As Boolean = True

				Dim requiresParam As Boolean

				Dim obj As Object
				Dim text As String
				Dim wItem As WhereParameter
				Dim skipConjuction As Boolean = False

				Dim paramName As String
				Dim columnName As String

				Dim qCount As Integer = _whereParameters.Count - 1
				Dim index As Integer = 0

				For index = 0 To qCount

					' Maybe we injected text or a WhereParameter
					obj = _whereParameters(index)

					If TypeOf obj Is String Then

						text = CType(obj, String)
						query += text

						If text = "(" Then
							skipConjuction = True
						End If

					Else

						wItem = CType(obj, WhereParameter)

						If wItem.IsDirty Then

							If Not first AndAlso Not skipConjuction Then

								If Not wItem.Conjuction = WhereParameter.Conj.UseDefault Then
									If wItem.Conjuction = WhereParameter.Conj.AND_ Then
										query += " AND "
									Else
										query += " OR "
									End If
								Else
									query += " " + conjuction + " "
								End If

							End If

							requiresParam = True

							inc = inc + 1
							columnName = D(wItem.Column)
							paramName = "@" + wItem.Column + inc.ToString()
							wItem.Param.ParameterName = paramName

							Select Case wItem.[Operator]
								Case WhereParameter.Operand.Equal
									query += columnName + " = " + paramName + " "
								Case WhereParameter.Operand.NotEqual
									query += columnName + " <> " + paramName + " "
								Case WhereParameter.Operand.GreaterThan
									query += columnName + " > " + paramName + " "
								Case WhereParameter.Operand.LessThan
									query += columnName + " < " + paramName + " "
								Case WhereParameter.Operand.LessThanOrEqual
									query += columnName + " <= " + paramName + " "
								Case WhereParameter.Operand.GreaterThanOrEqual
									query += columnName + " >= " + paramName + " "
								Case WhereParameter.Operand.Like_
									query += columnName + " LIKE " + paramName + " "
								Case WhereParameter.Operand.NotLike
									query += columnName + " NOT LIKE " + paramName + " "
								Case WhereParameter.Operand.IsNull
									query += columnName + " IS NULL "
									requiresParam = False
								Case WhereParameter.Operand.IsNotNull
									query += columnName + " IS NOT NULL "
									requiresParam = False
								Case WhereParameter.Operand.In_
									query += columnName + " IN (" + wItem.Value.ToString() + ") "
									requiresParam = False
								Case WhereParameter.Operand.NotIn
									query += columnName + " NOT IN (" + wItem.Value.ToString() + ") "
									requiresParam = False
								Case WhereParameter.Operand.Between
									query += D(columnName) + " BETWEEN " + paramName
									cmd.Parameters.Add(paramName, wItem.BetweenBeginValue)

									inc = inc + 1
									paramName = "@" + wItem.Column + inc.ToString()
									query += " AND " + paramName
									cmd.Parameters.Add(paramName, wItem.BetweenEndValue)
									requiresParam = False
							End Select

							If requiresParam Then
								Dim dbCmd As IDbCommand = CType(cmd, IDbCommand)
								cmd.Parameters.Add(wItem.Param)
								wItem.Param.Value = wItem.Value
								p += 1
							End If

							first = False
							skipConjuction = False

						End If						 ' If wItem.IsDirty Then

					End If					  ' Is WhereParameter

				Next index

			End If

			If _groupBy.Length > 0 Then
				query += " GROUP BY " + _groupBy

				If Me._withRollup Then
					query += " WITH ROLLUP"
				End If
			End If

			If (_orderBy.Length > 0) Then
				query += " ORDER BY " + _orderBy
			End If

			cmd.CommandText = query
			Return cmd

		End Function

		Private Function D(ByVal text As String) As String

			Dim entity As FirebirdSqlEntity = CType(Me._entity, FirebirdSqlEntity)
			If entity._dialect = 3 Then
				text = """" + text + """"
			End If

			Return text

		End Function


	End Class

End Namespace

