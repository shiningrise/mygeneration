Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.Text

Namespace MyGeneration.dOOdads
    Public Class VisualFoxProDynamicQuery
        Inherits DynamicQuery
        Public Sub New(ByVal entity As BusinessEntity)
            MyBase.New(entity)
        End Sub

        Public Overloads Overrides Sub AddOrderBy(ByVal column As String, ByVal direction As MyGeneration.dOOdads.WhereParameter.Dir)
            MyBase.AddOrderBy(column, direction)
        End Sub

        Public Overloads Overrides Sub AddOrderBy(ByVal countAll As DynamicQuery, ByVal direction As MyGeneration.dOOdads.WhereParameter.Dir)
            If countAll.CountAll Then
                MyBase.AddOrderBy("Count(*)", direction)
            End If
        End Sub

        Public Overloads Overrides Sub AddOrderBy(ByVal aggregate As AggregateParameter, ByVal direction As MyGeneration.dOOdads.WhereParameter.Dir)
            MyBase.AddOrderBy(GetAggregate(aggregate, False), direction)
        End Sub

        Public Overloads Overrides Sub AddGroupBy(ByVal column As String)
            MyBase.AddGroupBy(column)
        End Sub

        Public Overloads Overrides Sub AddGroupBy(ByVal aggregate As AggregateParameter)
            ' Access does not support aggregates in a GROUP BY 
            MyBase.AddGroupBy(GetAggregate(aggregate, False))
        End Sub

        Public Overloads Overrides Sub AddResultColumn(ByVal columnName As String)
            MyBase.AddResultColumn(columnName)
        End Sub

        Protected Function GetAggregate(ByVal wItem As AggregateParameter, ByVal withAlias As Boolean) As String
            Dim query As String = String.Empty

            Select Case wItem.[Function]
                Case AggregateParameter.Func.Avg
                    query += "Avg("
                    Exit Select
                Case AggregateParameter.Func.Count
                    query += "Count("
                    Exit Select
                Case AggregateParameter.Func.Max
                    query += "Max("
                    Exit Select
                Case AggregateParameter.Func.Min
                    query += "Min("
                    Exit Select
                Case AggregateParameter.Func.Sum
                    query += "Sum("
                    Exit Select
                Case AggregateParameter.Func.StdDev
                    query += "StDev("
                    Exit Select
                Case AggregateParameter.Func.Var
                    query += "Var("
                    Exit Select
            End Select

            If wItem.Distinct Then
                query += "DISTINCT "
            End If

            query += wItem.Column

            If withAlias AndAlso wItem.[Alias] <> String.Empty Then
                query += " AS " & wItem.[Alias]
            End If

            Return query
        End Function

        Protected Overloads Overrides Function _Load(Optional ByVal conjuction As String = "AND") As IDbCommand
            Dim hasColumn As Boolean = False
            Dim selectAll As Boolean = True
            Dim query As String

            query = "SELECT "

            If Me._distinct Then
                query += " DISTINCT "
            End If
            If Me._top >= 0 Then
                query += " TOP " & Me._top.ToString() & " "
            End If

            If Me._resultColumns.Length > 0 Then
                query += Me._resultColumns
                hasColumn = True
                selectAll = False
            End If

            If Me._countAll Then
                If hasColumn Then
                    query += ", "
                End If

                query += "Count(*)"

                If Me._countAllAlias <> String.Empty Then
                    query += " AS " & Me._countAllAlias
                End If

                hasColumn = True
                selectAll = False
            End If

            If _aggregateParameters IsNot Nothing AndAlso _aggregateParameters.Count > 0 Then
                Dim isFirst As Boolean = True

                If hasColumn Then
                    query += ", "
                End If

                Dim wItem As AggregateParameter

                For Each obj As Object In _aggregateParameters
                    wItem = TryCast(obj, AggregateParameter)

                    If wItem.IsDirty Then
                        If isFirst Then
                            query += GetAggregate(wItem, True)
                            isFirst = False
                        Else
                            query += ", " & GetAggregate(wItem, True)
                        End If
                    End If
                Next

                selectAll = False
            End If

            If selectAll Then
                query += "*"
            End If

            query += " FROM " & Me._entity.QuerySource

            Dim cmd As New OleDbCommand()

            If _whereParameters IsNot Nothing AndAlso _whereParameters.Count > 0 Then
                query += " WHERE "

                Dim first As Boolean = True

                Dim requiresParam As Boolean

                Dim wItem As WhereParameter
                Dim skipConjuction As Boolean = False

                Dim paramName As String
                Dim columnName As String

                For Each obj As Object In _whereParameters
                    ' Maybe we injected text or a WhereParameter 
                    If obj.[GetType]().ToString() = "System.String" Then
                        Dim text As String = TryCast(obj, String)
                        query += text

                        If text = "(" Then
                            skipConjuction = True
                        End If
                    Else
                        wItem = TryCast(obj, WhereParameter)

                        If wItem.IsDirty Then
                            If Not first AndAlso Not skipConjuction Then
                                If wItem.Conjuction <> WhereParameter.Conj.UseDefault Then
                                    If wItem.Conjuction = WhereParameter.Conj.[AND] Then
                                        query += " AND "
                                    Else
                                        query += " OR "
                                    End If
                                Else
                                    query += " " & conjuction & " "
                                End If
                            End If

                            requiresParam = True

                            columnName = wItem.Column
                            paramName = wItem.Column + (System.Threading.Interlocked.Increment(inc)).ToString()
                            wItem.Param.ParameterName = paramName

                            Select Case wItem.[Operator]
                                Case WhereParameter.Operand.Equal
                                    query += columnName & " = ? "
                                    Exit Select
                                Case WhereParameter.Operand.NotEqual
                                    query += columnName & " <> ? "
                                    Exit Select
                                Case WhereParameter.Operand.GreaterThan
                                    query += columnName & " > ? "
                                    Exit Select
                                Case WhereParameter.Operand.LessThan
                                    query += columnName & " < ? "
                                    Exit Select
                                Case WhereParameter.Operand.LessThanOrEqual
                                    query += columnName & " <= ? "
                                    Exit Select
                                Case WhereParameter.Operand.GreaterThanOrEqual
                                    query += columnName & " >= ? "
                                    Exit Select
                                Case WhereParameter.Operand.[Like]
                                    query += columnName & " LIKE ? "
                                    Exit Select
                                Case WhereParameter.Operand.NotLike
                                    query += columnName & " NOT LIKE ? "
                                    Exit Select
                                Case WhereParameter.Operand.IsNull
                                    query += columnName & " IS NULL "
                                    requiresParam = False
                                    Exit Select
                                Case WhereParameter.Operand.IsNotNull
                                    query += columnName & " IS NOT NULL "
                                    requiresParam = False
                                    Exit Select
                                Case WhereParameter.Operand.[In]
                                    query += (columnName & " IN (") + wItem.Value.ToString() & ") "
                                    requiresParam = False
                                    Exit Select
                                Case WhereParameter.Operand.NotIn
                                    query += (columnName & " NOT IN (") + wItem.Value.ToString() & ") "
                                    requiresParam = False
                                    Exit Select
                                Case WhereParameter.Operand.Between

                                    query += columnName & " BETWEEN ? AND ? "
                                    Me.AddParameter(cmd, paramName, wItem.BetweenBeginValue)

                                    paramName = wItem.Column + (System.Threading.Interlocked.Increment(inc)).ToString()
                                    Me.AddParameter(cmd, paramName, wItem.BetweenEndValue)
                                    requiresParam = False
                                    Exit Select
                            End Select

                            If requiresParam Then
                                Dim dbCmd As IDbCommand = TryCast(cmd, IDbCommand)
                                dbCmd.Parameters.Add(wItem.Param)
                                wItem.Param.Value = wItem.Value
                            End If

                            first = False
                            skipConjuction = False
                        End If
                    End If
                Next
            End If

            If _groupBy.Length > 0 Then
                query += " GROUP BY " & _groupBy

                If Me._withRollup Then
                    query += " WITH ROLLUP"
                End If
            End If

            If _orderBy.Length > 0 Then
                query += " ORDER BY " & _orderBy
            End If

            cmd.CommandText = query
            Return cmd
        End Function

        Private Sub AddParameter(ByVal cmd As OleDbCommand, ByVal paramName As String, ByVal data As Object)
#If (VS2005) Then
            cmd.Parameters.AddWithValue(paramName, data)
#Else
            cmd.Parameters.Add(paramName, data)
#End If

        End Sub
    End Class
End Namespace