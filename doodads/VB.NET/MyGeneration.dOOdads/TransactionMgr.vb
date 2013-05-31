'==============================================================================
' MyGeneration.dOOdads
'
' TransactionMgr.vb
' Version 5.1
' Updated - 11/17/2005
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

Imports System
Imports System.Data
Imports System.Configuration
Imports System.Threading
Imports System.Diagnostics
Imports System.Collections

Namespace MyGeneration.dOOdads

	Public Class TransactionMgr

		Protected Sub New()

		End Sub

		Public ReadOnly Property NestingCount() As Integer
			Get
				Return Me.txCount
			End Get
		End Property

		Public ReadOnly Property HasBeenRolledBack() As Boolean
			Get
				Return Me.hasRolledBack
			End Get
		End Property

		Public Sub BeginTransaction()

			If hasRolledBack Then Throw New Exception("Transaction Rolledback")

			txCount = txCount + 1

		End Sub

		Public Sub CommitTransaction()

			If hasRolledBack Then Throw New Exception("Transaction Rolledback")

			txCount = txCount - 1

			If txCount = 0 Then
				Dim tx As Transaction

				For Each tx In Me.transactions.Values
					tx.sqlTx.Commit()
                    tx.Dispose()
				Next

				Me.transactions.Clear()

				If Not Me.objectsInTransaction Is Nothing Then
					Try
						Dim entity As BusinessEntity
						For Each entity In Me.objectsInTransaction
							entity.AcceptChanges()
						Next
					Catch
					End Try

					Me.objectsInTransaction = Nothing
				End If
			End If

		End Sub

		Public Sub RollbackTransaction()

			If Not hasRolledBack And txCount > 0 Then
				Dim tx As Transaction

				For Each tx In Me.transactions.Values
					tx.sqlTx.Rollback()
                    tx.Dispose()
				Next

				Me.transactions.Clear()
				Me.txCount = 0
				Me.objectsInTransaction = Nothing
			End If

		End Sub

		Public Sub Enlist(ByVal cmd As IDbCommand, ByVal Entity As BusinessEntity)

			If txCount = 0 Or Not Entity._notRecommendedConnection Is Nothing Then
				cmd.Connection = CreateSqlConnection(Entity)
			Else

				Dim connStr As String = Entity._config
				If Not Entity._raw = "" Then connStr = Entity._raw

				Dim tx As Transaction = CType(Me.transactions(connStr), Transaction)

                If tx Is Nothing Then
                    Dim sqlConn As IDbConnection = CreateSqlConnection(Entity)
                    tx = New Transaction
                    tx.sqlConnection = sqlConn
                    If Not _isolationLevel = IsolationLevel.Unspecified Then
                        tx.sqlTx = sqlConn.BeginTransaction(_isolationLevel)
                    Else
                        tx.sqlTx = sqlConn.BeginTransaction()
                    End If

                    Me.transactions(connStr) = tx
                End If

                cmd.Connection = tx.sqlConnection
                cmd.Transaction = tx.sqlTx
			End If

		End Sub

		Public Sub DeEnlist(ByVal cmd As IDbCommand, ByVal Entity As BusinessEntity)

			If Not Entity._notRecommendedConnection Is Nothing Then
				cmd.Connection = Nothing
			Else
				' We do nothing basically if there is a transaction going on
				If txCount = 0 Then
					cmd.Connection.Dispose()
				End If
			End If

		End Sub

		' Called internally by BusinessEntity
		Friend Sub AddBusinessEntity(ByVal entity As BusinessEntity)

			If Me.objectsInTransaction Is Nothing Then
				Me.objectsInTransaction = New ArrayList
			End If

			Me.objectsInTransaction.Add(entity)
		End Sub

		Private Function CreateSqlConnection(ByVal entity As BusinessEntity) As IDbConnection

			Dim cn As IDbConnection

			If Not entity._notRecommendedConnection Is Nothing Then
				' This is assumed to be open
				cn = entity._notRecommendedConnection
			Else
				cn = entity.CreateIDbConnection()

				If Not entity._raw = "" Then
					cn.ConnectionString = entity._raw
				Else
#If (VS2005) Then
					cn.ConnectionString = ConfigurationManager.AppSettings(entity._config)
#Else
					cn.ConnectionString = ConfigurationSettings.AppSettings(entity._config)
#End If
				End If

				cn.Open()
			End If

			Return cn

		End Function

		' We might have multple transactions going at the same time.
		' There's one per connnection string
        Private Class Transaction
            Implements IDisposable

            Public sqlTx As IDbTransaction = Nothing
            Public sqlConnection As IDbConnection = Nothing

            Public Sub Dispose() Implements System.IDisposable.Dispose
                If (Not sqlConnection Is Nothing) Then
                    sqlConnection.Close()
                    sqlConnection.Dispose()
                End If

                If (Not sqlTx Is Nothing) Then
                    sqlTx.Dispose()
                End If
            End Sub
        End Class

        Private transactions As New Hashtable
        Private txCount As Integer = 0
        Private hasRolledBack As Boolean = False

        ' Used to control AcceptChanges()
        Friend objectsInTransaction As ArrayList = Nothing

#Region "Shared"
        Public Shared Function ThreadTransactionMgr() As TransactionMgr

            Dim txMgr As TransactionMgr = Nothing

            Dim obj As Object = Thread.GetData(txMgrSlot)

            If Not obj Is Nothing Then
                txMgr = CType(obj, TransactionMgr)
            Else
                txMgr = New TransactionMgr
                Thread.SetData(txMgrSlot, txMgr)
            End If

            Return txMgr

        End Function

        Public Shared Sub ThreadTransactionMgrReset()

            Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

            Try
                If txMgr.txCount > 0 AndAlso txMgr.hasRolledBack = False Then
                    txMgr.RollbackTransaction()
                End If
            Catch ex As Exception
                ' At this point we're not worried about a failure
            End Try

            Thread.SetData(txMgrSlot, Nothing)
        End Sub

        Public Shared Property IsolationLevel() As IsolationLevel
            Get
                Return _isolationLevel
            End Get
            Set(ByVal Value As IsolationLevel)
                _isolationLevel = Value
            End Set
        End Property

        Private Shared _isolationLevel As IsolationLevel = IsolationLevel.Unspecified
        Private Shared txMgrSlot As LocalDataStoreSlot = Thread.AllocateDataSlot()
#End Region

    End Class

End Namespace
