'==============================================================================
' MyGeneration.dOOdads
'
' BusinessEntity.vb
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

Imports System.IO
Imports System.Configuration
Imports System.Data
Imports System.Data.Common
Imports System.Xml.Serialization

Namespace MyGeneration.dOOdads

    Public MustInherit Class BusinessEntity

        Friend MustOverride Function CreateDynamicQuery(ByVal entity As BusinessEntity) As DynamicQuery
        Friend MustOverride Function CreateIDbCommand() As IDbCommand
        Friend MustOverride Function CreateIDbDataAdapter() As IDbDataAdapter
        Friend MustOverride Function CreateIDbConnection() As IDbConnection
        Friend MustOverride Function CreateIDataParameter(ByVal name As String, ByVal value As Object) As IDataParameter
        Friend MustOverride Function CreateIDataParameter() As IDataParameter
        Friend MustOverride Function ConvertIDbDataAdapter(ByVal dataAdapter As IDbDataAdapter) As DbDataAdapter

		Public ReadOnly Property EOF() As Boolean
			Get
				Return _EOF
			End Get
		End Property

        Protected Overridable Function _LoadFromRawSql(ByVal rawSql As String, ByVal ParamArray parameters() As Object) As IDbCommand
            Return Nothing
        End Function

        Friend Overridable Function RequiresSquareBrackets() As Boolean
            Return True
        End Function

        Public StringFormat As String = "MM/dd/yyyy"

        ' Some properties to allow for a little customization
        Public Overridable Property QuerySource() As String
            Get
                Return SchemaTableView + _querySource
            End Get
            Set(ByVal Value As String)
                _querySource = Value
            End Set
        End Property

        Public Overridable Property MappingName() As String
            Get
                Return _mappingName
            End Get
            Set(ByVal Value As String)
                _mappingName = Value
            End Set
        End Property

        Public Overridable Property SchemaGlobal() As String
            Get
                Return _schemaGlobal
            End Get
            Set(ByVal Value As String)
                _schemaGlobal = Value
                SchemaTableView = Value
                SchemaStoredProcedure = Value
            End Set
        End Property

        Public Overridable Property SchemaTableView() As String
            Get
                Return _tableViewSchema
            End Get
            Set(ByVal Value As String)
                _tableViewSchema = Value
            End Set
        End Property

        Public Overridable Property SchemaStoredProcedure() As String
            Get
                Return _storedProcSchema
            End Get
            Set(ByVal Value As String)
                _storedProcSchema = Value
            End Set
        End Property

        Protected Overridable Function GetInsertCommand() As IDbCommand
            Return Nothing
        End Function

        Protected Overridable Function GetUpdateCommand() As IDbCommand
            Return Nothing
        End Function

        Protected Overridable Function GetDeleteCommand() As IDbCommand
            Return Nothing
        End Function

        ' Use this to override Query.Load() as in emps.Query.Load()
        Protected Friend Overridable Sub OnQueryLoad(ByVal conjuction As String)

        End Sub

		Public Property NotRecommendedConnection() As System.Data.IDbConnection
			Get
				Return _notRecommendedConnection
			End Get
			Set(ByVal Value As System.Data.IDbConnection)
				_notRecommendedConnection = Value
			End Set
		End Property

		Public Property ConnectionString() As String
			Get
				Return _raw
			End Get
			Set(ByVal Value As String)
				_raw = Value
			End Set
		End Property

		Public Property ConnectionStringConfig() As String
			Get
				Return _config
			End Get
			Set(ByVal Value As String)
				_config = Value
			End Set
		End Property
		Public Shared Property DefaultConnectionStringConfig() As String
			Get
				Return BusinessEntity._defaultConfig
			End Get
			Set(ByVal Value As String)
				BusinessEntity._defaultConfig = Value
			End Set
		End Property

		Public Property Filter() As String
			Get
				Dim _filter As String = ""

				If Not _dataTable Is Nothing Then
					_filter = _dataTable.DefaultView.RowFilter
				End If

				Return _filter
			End Get

			Set(ByVal Value As String)

				If Not _dataTable Is Nothing Then
					_dataTable.DefaultView.RowFilter = Value
					Rewind()
				End If

			End Set
		End Property

		Public Property Sort() As String
			Get
				Dim _sort As String = ""

				If Not _dataTable Is Nothing Then
					_sort = _dataTable.DefaultView.Sort
				End If

				Return _sort
			End Get

			Set(ByVal Value As String)
				If Not _dataTable Is Nothing Then
					_dataTable.DefaultView.Sort = Value
					Rewind()
				End If
			End Set
		End Property

		Public Function AddColumn(ByVal name As String, ByVal dataType As System.Type) As DataColumn

			Dim dc As DataColumn = Nothing

			If Not _dataTable Is Nothing Then
				dc = New DataColumn(name, dataType)
				_dataTable.Columns.Add(dc)
			End If

			Return dc

		End Function

		Public Function RowState() As DataRowState

			If Not _dataTable Is Nothing AndAlso Not _dataRow Is Nothing Then
				Return _dataRow.RowState
			Else
				Return DataRowState.Detached
			End If

		End Function

		Public ReadOnly Property RowCount() As Integer
			Get
				Dim count As Integer = 0

				If Not _dataTable Is Nothing Then
					count = _dataTable.DefaultView.Count()
				End If

				Return count
			End Get
		End Property

		Public Sub Rewind()

			_dataRow = Nothing
			_enumerator = Nothing

			If Not _dataTable Is Nothing Then
				If (_dataTable.DefaultView.Count > 0) Then
					_enumerator = _dataTable.DefaultView.GetEnumerator()
					_enumerator.MoveNext()
					Dim rowView As DataRowView = CType(_enumerator.Current, DataRowView)
					_dataRow = rowView.Row
					_EOF = False
				Else
					_EOF = True
				End If
			Else
				_EOF = True
			End If

		End Sub

		Public Function MoveNext() As Boolean

			Dim moved As Boolean = False

			If Not _enumerator Is Nothing AndAlso _enumerator.MoveNext() Then
				Dim rowView As DataRowView = CType(_enumerator.Current, DataRowView)
				_dataRow = rowView.Row
				moved = True
			Else
				_EOF = True
			End If

			Return moved

		End Function

		Public Overridable Sub FlushData()

			_dataRow = Nothing
			_dataTable = Nothing
			_enumerator = Nothing
			_query = Nothing
			_canSave = True
			_EOF = True
		End Sub

		Public ReadOnly Property DefaultView() As DataView
			Get
				If Not _dataTable Is Nothing Then
					Return _dataTable.DefaultView
				Else
					Return Nothing
				End If
			End Get
		End Property

		' DataTable Property
		Protected Friend Property DataTable() As DataTable

			Get
				Return _dataTable
			End Get

			Set(ByVal Value As DataTable)

				_dataTable = Value
				_dataRow = Nothing

				If Not _dataTable Is Nothing Then
					Rewind()
				End If

			End Set

		End Property

		' DataRow Property
		Protected Friend Property DataRow() As DataRow

			Get
				Return _dataRow
			End Get

			Set(ByVal Value As DataRow)
				_dataRow = Value
			End Set

		End Property

#Region "ToXml / FromXml"

		Public Overridable Function Serialize() As String
			Dim dataSet As DataSet = New DataSet
			dataSet.Tables.Add(_dataTable)
			Dim writer As StringWriter = New StringWriter
			Dim ser As XmlSerializer = New XmlSerializer(GetType(DataSet))
			ser.Serialize(writer, dataSet)
			dataSet.Tables.Clear
			Return writer.ToString
		End Function

		Public Overridable Sub Deserialize(ByVal xml As String)
			Dim dataSet As DataSet = New DataSet
			Dim reader As StringReader = New StringReader(xml)
			Dim ser As XmlSerializer = New XmlSerializer(GetType(DataSet))
			dataSet = CType(ser.Deserialize(reader), DataSet)
			Me.DataTable = dataSet.Tables(0)
			dataSet.Tables.Clear
		End Sub

		Public Overridable Function ToXml() As String

			Dim ds As DataSet = New DataSet
			ds.Tables.Add(_dataTable)
			Dim writer As StringWriter = New StringWriter
			ds.WriteXml(writer)
			ds.Tables.Clear()
			Return writer.ToString()

		End Function

		Public Overridable Function ToXml(ByVal mode As XmlWriteMode) As String

			Dim ds As DataSet = New DataSet
			ds.Tables.Add(_dataTable)
			Dim writer As StringWriter = New StringWriter
			ds.WriteXml(writer, mode)
			ds.Tables.Clear()
			Return writer.ToString()

		End Function

		Public Overridable Sub FromXml(ByVal xml As String)

			Dim ds As DataSet = New DataSet
			Dim reader As StringReader = New StringReader(xml)
			ds.ReadXml(reader)
			Me.DataTable = ds.Tables(0)
			ds.Tables.Clear()

		End Sub

		Public Overridable Sub FromXml(ByVal xml As String, ByVal mode As XmlReadMode)

			Dim ds As DataSet = New DataSet
			Dim reader As StringReader = New StringReader(xml)
			ds.ReadXml(reader, mode)
			Me.DataTable = ds.Tables(0)
			ds.Tables.Clear()

		End Sub

#End Region

#Region "SQL Methods -- LoadFromSql, AddNew, MarkAsDeleted, Save"

		Protected Function LoadFromSql(ByVal sp As String, _
		  Optional ByRef Parameters As Specialized.ListDictionary = Nothing, _
		  Optional ByVal commandType As CommandType = CommandType.StoredProcedure) As Boolean

			Dim dataTable As dataTable = Nothing
			Dim loaded As Boolean = False

			Try

				dataTable = New dataTable(Me.MappingName)

				Dim cmd As IDbCommand = Me.CreateIDbCommand()
				cmd.CommandText = sp
				cmd.CommandType = commandType

				Dim p As IDataParameter

				If Not Parameters Is Nothing Then
					Dim param As DictionaryEntry

					For Each param In Parameters
						If Not TypeOf param.Key Is IDataParameter Then
							p = Me.CreateIDataParameter(CType(param.Key, String), param.Value)
						Else
							p = CType(param.Key, IDataParameter)
							p.Value = param.Value
						End If

						cmd.Parameters.Add(p)
					Next

				End If

				Dim da As IDbDataAdapter = Me.CreateIDbDataAdapter()
				da.SelectCommand = cmd

				Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

				txMgr.Enlist(cmd, Me)
				Dim adapter As DbDataAdapter = Me.ConvertIDbDataAdapter(da)
				adapter.Fill(dataTable)
				txMgr.DeEnlist(cmd, Me)

			Catch ex As Exception
				Throw ex
			Finally
				Me.DataTable = dataTable
				loaded = (Me.RowCount() > 0)
			End Try

			Return loaded

		End Function

		Protected Function LoadFromRawSql(ByVal rawSql As String, ByVal ParamArray parameters() As Object) As Boolean

			Dim dataTable As dataTable = Nothing
			Dim loaded As Boolean = False

			Try
				Dim cmd As IDbCommand = _LoadFromRawSql(rawSql, parameters)

				Dim da As IDbDataAdapter = Me.CreateIDbDataAdapter()
				da.SelectCommand = cmd

				Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

				dataTable = New dataTable(Me.MappingName)

				txMgr.Enlist(cmd, Me)
				Dim adapter As DbDataAdapter = Me.ConvertIDbDataAdapter(da)
				adapter.Fill(dataTable)
				txMgr.DeEnlist(cmd, Me)

			Catch ex As Exception
				Throw ex
			Finally
				Me.DataTable = dataTable
				loaded = (Me.RowCount() > 0)
			End Try

			Return loaded

		End Function

		Protected Function LoadFromSqlNoExec(ByVal sp As String, _
		  Optional ByRef Parameters As Specialized.ListDictionary = Nothing, _
		  Optional ByVal commandType As CommandType = CommandType.StoredProcedure, _
		  Optional ByVal commandTimeout As Integer = -1) As Integer

			Dim retValue As Integer = 0

			Try

				Dim cmd As IDbCommand = Me.CreateIDbCommand()
				cmd.CommandText = sp
				cmd.CommandType = commandType

				If commandTimeout > 0 Then
					cmd.CommandTimeout = commandTimeout
				End If

				Dim p As IDataParameter

				If Not Parameters Is Nothing Then
					Dim param As DictionaryEntry

					For Each param In Parameters
						If Not TypeOf param.Key Is IDataParameter Then
							p = Me.CreateIDataParameter(CType(param.Key, String), param.Value)
						Else
							p = CType(param.Key, IDataParameter)
							p.Value = param.Value
						End If

						cmd.Parameters.Add(p)
					Next

				End If

				Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

				txMgr.Enlist(cmd, Me)
				retValue = cmd.ExecuteNonQuery()
				txMgr.DeEnlist(cmd, Me)

			Catch ex As Exception
				Throw ex
			Finally
			End Try

			Return retValue

		End Function

		Protected Function LoadFromSqlScalar(ByVal sp As String, _
		  Optional ByRef Parameters As Specialized.ListDictionary = Nothing, _
		  Optional ByVal commandType As CommandType = CommandType.StoredProcedure, _
		  Optional ByVal commandTimeout As Integer = -1) As Object

			Dim retValue As Object = 0

			Try

				Dim cmd As IDbCommand = Me.CreateIDbCommand()
				cmd.CommandText = sp
				cmd.CommandType = commandType

				If commandTimeout > 0 Then
					cmd.CommandTimeout = commandTimeout
				End If

				Dim p As IDataParameter

				If Not Parameters Is Nothing Then
					Dim param As DictionaryEntry

					For Each param In Parameters
						If Not TypeOf param.Key Is IDataParameter Then
							p = Me.CreateIDataParameter(CType(param.Key, String), param.Value)
						Else
							p = CType(param.Key, IDataParameter)
							p.Value = param.Value
						End If

						cmd.Parameters.Add(p)
					Next

				End If

				Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

				txMgr.Enlist(cmd, Me)
				retValue = cmd.ExecuteScalar()
				txMgr.DeEnlist(cmd, Me)

			Catch ex As Exception
				Throw ex
			Finally
			End Try

			Return retValue

		End Function

		Protected Function LoadFromSqlReader(ByVal sp As String, _
		 Optional ByRef Parameters As Specialized.ListDictionary = Nothing, _
		 Optional ByVal commandType As CommandType = CommandType.StoredProcedure) As IDataReader

			Try

				Dim cmd As IDbCommand = Me.CreateIDbCommand()
				cmd.CommandText = sp
				cmd.CommandType = commandType

				Dim p As IDataParameter

				If Not Parameters Is Nothing Then
					Dim param As DictionaryEntry

					For Each param In Parameters
						If Not TypeOf param.Key Is IDataParameter Then
							p = Me.CreateIDataParameter(CType(param.Key, String), param.Value)
						Else
							p = CType(param.Key, IDataParameter)
							p.Value = param.Value
						End If

						cmd.Parameters.Add(p)
					Next

				End If

				cmd.Connection = Me.CreateIDbConnection()
				cmd.Connection.ConnectionString = Me.RawConnectionString
				cmd.Connection.Open()
				Return cmd.ExecuteReader(CommandBehavior.CloseConnection)

			Catch ex As Exception
				Throw ex
			Finally
			End Try

		End Function

		Public Overridable Sub AddNew()

			If _dataTable Is Nothing Then
				Me.LoadFromSql("SELECT * FROM [" + QuerySource() + "] WHERE 1=0", Nothing, CommandType.Text)
			End If

			Dim newRow As DataRow = _dataTable.NewRow()
			_dataTable.Rows.Add(newRow)
			_dataRow = newRow

		End Sub

		Public Overridable Sub MarkAsDeleted()

			If Not _dataRow Is Nothing Then
				_dataRow.Delete()
			End If

		End Sub

		Public Overridable Sub DeleteAll()

			If Not _dataTable Is Nothing Then
				Dim row As DataRow

				For Each row In _dataTable.Rows
					row.Delete()
				Next
			End If

		End Sub

		Public Overridable Sub Save()

			If _dataTable Is Nothing Then Return

			If _canSave = False Then Throw New Exception("Cannot call Save() after Query.AddResultColumn()")

			Dim txMgr As TransactionMgr = TransactionMgr.ThreadTransactionMgr()

			Try

				Dim row As DataRow

				Dim needToInsert As Boolean = False
				Dim needToUpdate As Boolean = False
				Dim needToDelete As Boolean = False

				' Do we have any data to save
				For Each row In _dataTable.Rows
					Select Case row.RowState
						Case DataRowState.Added
							needToInsert = True
						Case DataRowState.Modified
							needToUpdate = True
						Case DataRowState.Deleted
							needToDelete = True
					End Select
				Next

				If needToInsert Or needToUpdate Or needToDelete Then
					' Yes, we do

					Dim da As IDbDataAdapter = Me.CreateIDbDataAdapter()

					If needToInsert Then da.InsertCommand = GetInsertCommand()
					If needToUpdate Then da.UpdateCommand = GetUpdateCommand()
					If needToDelete Then da.DeleteCommand = GetDeleteCommand()

					txMgr.BeginTransaction()

					' We need to NOT call AcceptChanges() directly if the user 
					' has also called BeginTransaction()
					Dim txCount As Integer = txMgr.NestingCount
					If txCount > 1 Then txMgr.AddBusinessEntity(Me)

					If needToInsert Then txMgr.Enlist(da.InsertCommand, Me)
					If needToUpdate Then txMgr.Enlist(da.UpdateCommand, Me)
					If needToDelete Then txMgr.Enlist(da.DeleteCommand, Me)

					Dim Adapter As DbDataAdapter = Me.ConvertIDbDataAdapter(da)

					' Let's give folks a chance to do some custom work here
					Me.HookupRowUpdateEvents(Adapter)

					Adapter.Update(_dataTable)

					txMgr.CommitTransaction()

					' We started the transaction so we can call AcceptChanges() as 
					' we know that there's nobody else in the transaction
					If txCount = 1 Then Me.AcceptChanges()

					If needToInsert Then txMgr.DeEnlist(da.InsertCommand, Me)
					If needToUpdate Then txMgr.DeEnlist(da.UpdateCommand, Me)
					If needToDelete Then txMgr.DeEnlist(da.DeleteCommand, Me)

				End If

			Catch ex As Exception

				If Not txMgr Is Nothing Then txMgr.RollbackTransaction()
				Throw ex

			Finally

			End Try

		End Sub

		Protected Overridable Sub HookupRowUpdateEvents(ByVal adapter As DbDataAdapter)
			' Default is to do nothing, however, the OleDbEntity overrides this
			' to hookup the RowUpdated event in order to return the latest identity value
		End Sub

		Public Overridable Sub RejectChanges()
			If Not _dataTable Is Nothing Then _dataTable.RejectChanges()
		End Sub

		Public Overridable Sub AcceptChanges()
			If Not _dataTable Is Nothing Then _dataTable.AcceptChanges()
		End Sub

		Public Overridable Sub GetChanges()
			If Not _dataTable Is Nothing Then
				Me.DataTable = _dataTable.GetChanges()
			End If
		End Sub

		Public Overridable Sub GetChanges(ByVal states As DataRowState)
			If Not _dataTable Is Nothing Then
				Me.DataTable = _dataTable.GetChanges(states)
			End If
		End Sub

#End Region

		Public ReadOnly Property Query() As DynamicQuery
			Get
				If _query Is Nothing Then
					_query = Me.CreateDynamicQuery(Me)
				End If

				Return _query
			End Get
		End Property

		Friend ReadOnly Property RawConnectionString() As String
			Get
				Dim connStr As String
				If Not _raw = "" Then
					connStr = _raw
				Else
#If (VS2005) Then
					connStr = ConfigurationManager.AppSettings(_config)
#Else
					connStr = ConfigurationSettings.AppSettings(_config)
#End If
				End If
				Return connStr
			End Get
		End Property

#Region "Row Accessors"

		' Accessing a column that is null will throw an exception, use this
		Public Function IsColumnNull(ByVal columnName As String) As Boolean
			IsColumnNull = _dataRow.IsNull(columnName)
		End Function

		' Set a column to SQL Null
		Public Sub SetColumnNull(ByVal columnName As String)
			_dataRow(columnName) = DBNull.Value
		End Sub

		' Set a column to a value, used only for extra fields
		Public Sub SetColumn(ByVal columnName As String, ByVal Value As Object)
			_dataRow(columnName) = Value
		End Sub

		' Use for extra fields brought back via a View
		Public Function GetColumn(ByVal columnName As String) As Object
			Return _dataRow(columnName)
		End Function

		' Guid (uniqueidentfier)
		Protected Function GetGuid(ByVal columnName As String) As Guid
			If _dataRow.IsNull(columnName) Then
				Return Guid.Empty
			Else
				Return CType(_dataRow(columnName), Guid)
			End If
		End Function

		Protected Sub SetGuid(ByVal columnName As String, ByVal data As Guid)
			If (Guid.Empty.Equals(data)) Then
				_dataRow(columnName) = DBNull.Value
			Else
				_dataRow(columnName) = data
			End If
		End Sub

		' Boolean (bit)
		Protected Function GetBoolean(ByVal columnName As String) As Boolean
			Return CType(_dataRow(columnName), Boolean)
		End Function

		Protected Sub SetBoolean(ByVal columnName As String, ByVal data As Boolean)
			_dataRow(columnName) = data
		End Sub

		' String (nvarchar, varchar, nchar, char, text)
		Protected Function GetString(ByVal columnName As String) As String
			If _dataRow.IsNull(columnName) Then
				Return String.Empty
			Else
				Return CType(_dataRow(columnName), String)
			End If
		End Function

		Protected Sub SetString(ByVal columnName As String, ByVal data As String)
			If (0 = data.Length) Then
				_dataRow(columnName) = DBNull.Value
			Else
				_dataRow(columnName) = data
			End If
		End Sub

		' Integer (int)
		Protected Function GetInteger(ByVal columnName As String) As Integer
			Return CType(_dataRow(columnName), Integer)
		End Function

		Protected Sub SetInteger(ByVal columnName As String, ByVal data As Integer)
			_dataRow(columnName) = data
		End Sub

		' UInt64 (System.UInt64)
		Protected Function GetUInt64(ByVal columnName As String) As System.UInt64
			Return CType(_dataRow(columnName), System.UInt64)
		End Function

		Protected Sub SetUInt64(ByVal columnName As String, ByVal data As System.UInt64)
			_dataRow(columnName) = data
		End Sub

		' UInt32 (System.UInt32)
		Protected Function GetUInt32(ByVal columnName As String) As System.UInt32
			Return CType(_dataRow(columnName), System.UInt32)
		End Function

		Protected Sub SetUInt32(ByVal columnName As String, ByVal data As System.UInt32)
			_dataRow(columnName) = data
		End Sub

		' UInt16 (System.UInt16)
		Protected Function GetUInt16(ByVal columnName As String) As System.UInt16
			Return CType(_dataRow(columnName), System.UInt16)
		End Function

		Protected Sub SetUInt16(ByVal columnName As String, ByVal data As System.UInt16)
			_dataRow(columnName) = data
		End Sub

		' Long (bigint)
		Protected Function GetLong(ByVal columnName As String) As Long
			Return CType(_dataRow(columnName), Long)
		End Function

		Protected Sub SetLong(ByVal columnName As String, ByVal data As Long)
			_dataRow(columnName) = data
		End Sub

		' Short (int, smallint)
		Protected Function GetShort(ByVal columnName As String) As Short
			Return CType(_dataRow(columnName), Short)
		End Function

		Protected Sub SetShort(ByVal columnName As String, ByVal data As Short)
			_dataRow(columnName) = data
		End Sub

		' DateTime (datetime)
		Protected Function GetDateTime(ByVal columnName As String) As DateTime
			Return CType(_dataRow(columnName), DateTime)
		End Function

		Protected Sub SetDateTime(ByVal columnName As String, ByVal data As DateTime)
			_dataRow(columnName) = data
		End Sub

		' TimeSpan (?)
		Protected Function GetTimeSpan(ByVal columnName As String) As TimeSpan
			Return CType(_dataRow(columnName), TimeSpan)
		End Function

		Protected Sub SetTimeSpan(ByVal columnName As String, ByVal data As TimeSpan)
			_dataRow(columnName) = data
		End Sub

		' Decimal (decimal, money)
		Protected Function GetDecimal(ByVal columnName As String) As Decimal
			Return CType(_dataRow(columnName), Decimal)
		End Function

		Protected Sub SetDecimal(ByVal columnName As String, ByVal data As Decimal)
			_dataRow(columnName) = data
		End Sub

		' Single (real)
		Protected Function GetSingle(ByVal columnName As String) As Single
			Return CType(_dataRow(columnName), Single)
		End Function

		Protected Sub SetSingle(ByVal columnName As String, ByVal data As Single)
			_dataRow(columnName) = data
		End Sub

		' Byte (real)
		Protected Function GetByte(ByVal columnName As String) As Byte
			Return CType(_dataRow(columnName), Byte)
		End Function

		Protected Sub SetByte(ByVal columnName As String, ByVal data As Byte)
			_dataRow(columnName) = data
		End Sub

		' Double (float)
		Protected Function GetDouble(ByVal columnName As String) As Double
			Return CType(_dataRow(columnName), Double)
		End Function

		Protected Sub SetDouble(ByVal columnName As String, ByVal data As Double)
			_dataRow(columnName) = data
		End Sub

		' Byte() (varbinary, image) 
		Protected Function GetByteArray(ByVal columnName As String) As Byte()
			Return CType(_dataRow(columnName), Byte())
		End Function

		Protected Sub SetByteArray(ByVal columnName As String, ByVal data As Byte())
			_dataRow(columnName) = data
		End Sub

		' Object 
		Protected Function GetObject(ByVal columnName As String) As Object
			Return CType(_dataRow(columnName), Object)
		End Function

		Protected Sub SetObject(ByVal columnName As String, ByVal data As Object)
			_dataRow(columnName) = data
		End Sub

#End Region

#Region "String Row Accessors"

		Protected Function GetGuidAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetGuidAsString(ByVal columnName As String, ByVal data As String) As Guid
			Return New Guid(data)
		End Function

		'-----------------

		Protected Function GetBooleanAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetBooleanAsString(ByVal columnName As String, ByVal data As String) As Boolean
			Return Convert.ToBoolean(data)
		End Function

		'-----------------

		Protected Function GetStringAsString(ByVal columnName As String) As String
			Return CType(_dataRow(columnName), String)
		End Function

		Protected Function SetStringAsString(ByVal columnName As String, ByVal data As String) As String
			Return data
		End Function

		'-----------------

		Protected Function GetIntegerAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetIntegerAsString(ByVal columnName As String, ByVal data As String) As Integer
			Return Convert.ToInt32(data)
		End Function

		'-----------------

		Protected Function GetUInt64AsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetUInt64AsString(ByVal columnName As String, ByVal data As String) As System.UInt64
			Return Convert.ToUInt64(data)
		End Function

		'-----------------

		Protected Function GetUInt32AsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetUInt32AsString(ByVal columnName As String, ByVal data As String) As System.UInt32
			Return Convert.ToUInt32(data)
		End Function

		'-----------------

		Protected Function GetUInt16AsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetUInt16AsString(ByVal columnName As String, ByVal data As String) As System.UInt16
			Return Convert.ToUInt16(data)
		End Function

		'-----------------

		Protected Function GetLongAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetLongAsString(ByVal columnName As String, ByVal data As String) As Long
			Return Convert.ToInt64(data)
		End Function

		'-----------------

		Protected Function GetShortAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetShortAsString(ByVal columnName As String, ByVal data As String) As Short
			Return Convert.ToInt16(data)
		End Function

		'-----------------

		Protected Function GetDateTimeAsString(ByVal columnName As String) As String
			Return CType(_dataRow(columnName), DateTime).ToString(Me.StringFormat)
		End Function

		Protected Function SetDateTimeAsString(ByVal columnName As String, ByVal data As String) As DateTime
			Return Convert.ToDateTime(data)
		End Function

		'-----------------

		Protected Function GetTimeSpanAsString(ByVal columnName As String) As String
			Return CType(_dataRow(columnName), TimeSpan).ToString()
		End Function

		Protected Function SetTimeSpanAsString(ByVal columnName As String, ByVal data As String) As TimeSpan
			Return TimeSpan.Parse(data)
		End Function

		'-----------------

		Protected Function GetDecimalAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetDecimalAsString(ByVal columnName As String, ByVal data As String) As Decimal
			Return Convert.ToDecimal(data)
		End Function

		'-----------------

		Protected Function GetSingleAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetSingleAsString(ByVal columnName As String, ByVal data As String) As Single
			Return Convert.ToSingle(data)
		End Function

		'-----------------

		Protected Function GetByteAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetByteAsString(ByVal columnName As String, ByVal data As String) As Byte
			Return Convert.ToByte(data)
		End Function

		'-----------------

		Protected Function GetDoubleAsString(ByVal columnName As String) As String
			Return _dataRow(columnName).ToString()
		End Function

		Protected Function SetDoubleAsString(ByVal columnName As String, ByVal data As String) As Double
			Return Convert.ToDouble(data)
		End Function

		'-----------------

#End Region

		' Private Variables
		Private _dataRow As DataRow = Nothing
		Private _dataTable As DataTable = Nothing
		Private _enumerator As IEnumerator = Nothing

		Private Shared _defaultConfig As String = "dbConnection"
		Friend _config As String = BusinessEntity._defaultConfig
		Friend _notRecommendedConnection As IDbConnection = Nothing
		Friend _raw As String = ""
		Friend _canSave As Boolean = True

		Private _query As DynamicQuery

		Private _querySource As String
		Private _schemaGlobal As String = ""
		Private _tableViewSchema As String = ""
		Private _storedProcSchema As String = ""
		Private _mappingName As String = ""
		Private _EOF As Boolean = True

	End Class

End Namespace
