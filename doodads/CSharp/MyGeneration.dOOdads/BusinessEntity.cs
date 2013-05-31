//==============================================================================
// MyGeneration.dOOdads
//
// BusinessEntity.cs
// Version 5.1
// Updated - 11/17/2005
//------------------------------------------------------------------------------
// Copyright 2004, 2005 by MyGeneration Software.
// All Rights Reserved.
//
// Permission to use, copy, modify, and distribute this software and its 
// documentation for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appear in all copies and that 
// both that copyright notice and this permission notice appear in 
// supporting documentation. 
//
// MYGENERATION SOFTWARE DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS 
// SOFTWARE, INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY 
// AND FITNESS, IN NO EVENT SHALL MYGENERATION SOFTWARE BE LIABLE FOR ANY 
// SPECIAL, INDIRECT OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES 
// WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, 
// WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER 
// TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE 
// OR PERFORMANCE OF THIS SOFTWARE. 
//==============================================================================

using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml.Serialization;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// BusinessEntity provides the bulk of the logic for dOOdads. Your generated class will ultimately inherit from this class.
	/// See our website at http://www.mygenerationsoftware.com/dOOdads/dOOdads.aspx for more information.
	/// </summary>
	abstract public class BusinessEntity
	{		
		abstract internal DynamicQuery   CreateDynamicQuery(BusinessEntity entity);
		abstract internal IDbCommand     CreateIDbCommand();
		abstract internal IDbDataAdapter CreateIDbDataAdapter();
		abstract internal IDbConnection  CreateIDbConnection();	
		abstract internal IDataParameter CreateIDataParameter(string name, object value);
		abstract internal IDataParameter CreateIDataParameter();
		abstract internal DbDataAdapter  ConvertIDbDataAdapter(IDbDataAdapter dataAdapter);

		/// <summary>
		/// This is overloaded by each specific Entity type such as SqlClientEntity
		/// </summary>
		/// <param name="rawSql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		virtual  internal IDbCommand _LoadFromRawSql(string rawSql, params object[] parameters)
		{
			return null;
		}

		/// <summary>
		/// This is used by GetDateTimeAsString, for more informaton see "about DateTimeFormatInfo class" in the Visual Studio Help. The default is "MM/dd/yyyy". 
		/// </summary>
		/// The chosen default is "MM/dd/yyyy" which is very similiar to:
		/// <remarks>
		/// <code>
		/// DateTimeFormatInfo myDTFI = new CultureInfo( "en-US", false ).DateTimeFormat;
		/// string StringFormat = myDTFI.ShortDatePattern;</code>
		/// However, that format is "M/d/yyyy" which does not zero pad the month or day, the author chose this as the default. 
		/// You can override this default by simply setting this property. It is recommended that you set this in your objects
		/// constructor, however you can set it at will if you like. See <see cref="DateTimeFormatInfo"/>
		/// </remarks>
		public string StringFormat = "MM/dd/yyyy";

		/// <summary>
		/// Used as the source when Query.Load is called. "SELECT * FROM [Query.Source] WHERE ...". You can change this before calling
		/// Query.Load to bring back extra columns in your BusinessEntity from a View. You will however have to access those extra 
		/// columns with <see cref="GetColumn"/>.
		/// </summary>
		virtual public string QuerySource
		{ 
			get { return SchemaTableView + _querySource; }
			set { _querySource = value; }
		}

		/// <summary>
		/// This can used as your DataGridTableStyle.MappingName, and is the name of your DataTable 
		/// </summary>
		virtual public string MappingName
		{
			get { return _mappingName; }
			set { _mappingName = value; }
		}

		/// <summary>
		/// Setting this is a quick way to set both <see cref="SchemaTableView"/> and <see cref="SchemaStoredProcedure"/>. at the same time.
		/// Setting this property does nothing more than assign both SchemaTableView and SchemaStoredProcedure, vary rarely will you need to
		/// set SchemaTableView to one schema and SchemaStoredProcedure to a different schema. 
		/// </summary>
		/// NOTE: The period is required, as in "HR." 
		virtual public string SchemaGlobal
		{ 
			get { return _schemaGlobal; }
			set 
			{ 
				_schemaGlobal = value;
				SchemaTableView = value;
				SchemaStoredProcedure = value;	
			}
		}

		/// <summary>
		/// This is the schema for the Table or View that will be accessed via Query.Load() and AddNew().  For instance, if you set this to 
		/// "HR." then Query.Load() will use "HR.EMPLOYEES" instead of just "EMPLOYEES". See <see cref="SchemaGlobal"/>.
		/// </summary>
		virtual public string SchemaTableView
		{ 
			get { return _tableViewSchema; }
			set { _tableViewSchema = value;}
		}

		/// <summary>
		/// This is the schema for the stored procedures that will be accessed by the dOOdad.  For instance, if you set this to 
		/// "HR." then when you do an update your stored procedure will be prefaced by "HR.MyStoredProc" See <see cref="SchemaGlobal"/>.
		/// </summary>
		virtual public string SchemaStoredProcedure
		{ 
			get { return _storedProcSchema; }
			set { _storedProcSchema = value;}
		}

		/// <summary>
		/// Called by BusinessEntity but implemented in your generated class by MyGeneration during template generation.
		/// </summary>
		/// <returns>The command required for an INSERT statement, the code for this is generated for you.</returns>
		virtual protected IDbCommand GetInsertCommand()
		{
			return null;
		}

		/// <summary>
		/// Called by BusinessEntity but implemented in your generated class by MyGeneration during template generation.
		/// </summary>
		/// <returns>The command required for an UPDATE statement, the code for this is generated for you.</returns>
		virtual protected IDbCommand GetUpdateCommand()
		{
			return null;
		}

		/// <summary>
		/// Called by BusinessEntity but implemented in your generated class by MyGeneration during template generation.
		/// </summary>
		/// <returns>The command required for an DELETE statement, the code for this is generated for you.</returns>
		virtual protected IDbCommand GetDeleteCommand()
		{
			return null;
		}


		/// <summary>
		/// This is called when DynamicQuery.Load() is called, this occurs when code like this is executed:  emps.Query.Load(), 
		/// by default this method does nothing. When overriding you don't use the internal keyword.
		/// </summary>
		/// <param name="conjuction">The conjuction as passed into Query.Load()</param>
		virtual protected internal void OnQueryLoad(string conjuction)
		{

		}

		/// <summary>
		/// The method was added for a very specific reason. You are discouraged from using this method.
		/// </summary>
		virtual public IDbConnection NotRecommendedConnection
		{
			get
			{
				return this._notRecommendedConnection;
			}
			set
			{
				this._notRecommendedConnection = value;
			}
		}

		/// <summary>
		/// The default connection string for dOOdads is assumed to come from your app.config or web.config file. This property
		/// is for assigning a raw connection string to your entity, typically, the config file is used however. If you do choose
		/// to set your entity's connection string using this property we recommend you do so in it's constructor.
		/// </summary>
		/// <example>
		/// The default connection string is stored like this:
		/// <code>
		/// &lt;configuration&gt;
		///   &lt;appSettings&gt;
		///     &lt;add key="dbConnection" value="User ID=sa;Password=griffinski;Initial Catalog=Northwind;Data Source=localhost"/&gt;
		///   &lt;/appSettings&gt;
		/// &lt;/configuration&gt;
		/// </code>
		/// </example>
		virtual public string ConnectionString
		{
			get
			{
				return _raw;
			}
			set
			{
				_raw = value;
			}
		}

		/// <summary>
		/// Typically, applications use only a single database, dOOdads assumes that your connection string is registered in your
		/// applications app.config or web.config file under the key "dbConnection". You can use whatever key you desire and override
		/// dOOdads default of "dbConnection" with this property.
		/// </summary>
		/// <example>
		/// The default connection string is stored like this:
		/// <code>
		/// &lt;configuration&gt;
		///   &lt;appSettings&gt;
		///     &lt;add key="dbConnection" value="User ID=sa;Password=griffinski;Initial Catalog=Northwind;Data Source=localhost"/&gt;
		///     &lt;add key="dbHumanResources" value="User ID=sa;Password=;Initial Catalog=DBHR677;Data Source=PLQ99C"/&gt;
		///   &lt;/appSettings&gt;
		/// &lt;/configuration&gt;
		/// </code>
		/// </example>
		virtual public string ConnectionStringConfig
		{
			get
			{
				return _config;
			}
			set
			{
				_config = value;
			}
		}

		/// <summary>
		/// dOOdads assumes that your connection string is registered in your applications app.config or web.config file under
		/// the key "dbConnection". You can change the system wide default from "dbConnection" to whatever you desire however, remember
		/// this is static and effects all dOOdad objects. To override an individual objects setting see ConnectionStringConfig.
		/// </summary>
		/// <example>
		/// The DefaultConnectionStringConfig is set like this:
		/// <code>
		/// 
		/// BusinessEntity.DefaultConnectionStringConfig = "SiteSqlServer"
		/// 
		/// &lt;configuration&gt;
		///   &lt;appSettings&gt;
		///     &lt;add key="SiteSqlServer" value="User ID=sa;Password=griffinski;Initial Catalog=Northwind;Data Source=localhost"/&gt;
		///   &lt;/appSettings&gt;
		/// &lt;/configuration&gt;
		/// </code>
		/// </example>
		static public string DefaultConnectionStringConfig
		{
			get
			{
				return BusinessEntity._defaultConfig;
			}
			set
			{
				BusinessEntity._defaultConfig = value;
			}
		}

		/// <summary>
		/// After loading your BusinessEntity you can filter (temporary hide) rows. To disable the filter set this property to String.empty.
		/// After filter using Iteration via MoveNext will properly respect any filter you have in place. See also <see cref="Sort"/>.
		/// </summary>
		/// <example>
		/// For a detailed explanation see the RowFilter property on ADO.NET's DataView.RowFilter property.
		/// <code>
		/// emps.Filter = "City = 'Berlin'";
		/// </code>
		/// </example>
		public string Filter
		{
			get
			{
				string _filter = "";

				if(_dataTable != null)
				{
					_filter = _dataTable.DefaultView.RowFilter;
				}

				return _filter;
			}

			set
			{
				if(_dataTable != null)
				{
					_dataTable.DefaultView.RowFilter = value;
					Rewind();
				}
			}
		}

		/// <summary>
		/// After loading your BusinessEntity you can sort the rows. To disable the sort set this property to String.empty.
		/// After settign Sort iteration via MoveNext will properly respect the sort order. See also <see cref="Filter"/>.
		/// </summary>
		/// <example>
		/// For a detailed explanation see the Sort property on ADO.NET's DataView.Sort property.
		/// <code>
		/// addresses.Sort = "State, ZipCode DESC";
		/// </code>
		/// </example>
		public string Sort
		{
			get
			{
				string _sort = "";

				if(_dataTable != null)
				{
					_sort = _dataTable.DefaultView.Sort;
				}

				return _sort;
			}

			set
			{
				if(_dataTable != null)
				{
					_dataTable.DefaultView.Sort = value;
					Rewind();
				}
			}	
		}

		/// <summary>
		/// After loading your BusinessEntity you can add custom columns, this is typically done to create a calculated column, however, it can
		/// be used to add a column just to hold state, it will not be saved to the database of course.
		/// </summary>
		/// <param name="name">The name of the Column</param>
		/// <param name="dataType">Use Type.GetType() as in Type.GetType("System.String")</param>
		/// <returns>The newly created DataColumn</returns>
		/// <example>
		/// VB.NET
		/// <code>
		/// Dim emps As New Employees
		/// If emps.LoadAll() Then
		///
		///    Dim col As DataColumn = emps.AddColumn("FullName", Type.GetType("System.String"))
		///    col.Expression = Employees.ColumnNames.LastName + "+ ', ' + " + Employees.ColumnNames.FirstName
		///
		///    Dim fullName As String
		///
		///    Do
		///        fullName = CType(emps.GetColumn("FullName"), String)
		///    Loop Until Not emps.MoveNext
		///    
		///
		/// End If
		/// </code>
		/// C#
		/// <code>
		/// Employees emps = new Employees();
		///	if(emps.LoadAll())
		///	{
		///		DataColumn col = emps.AddColumn("FullName", Type.GetType("System.String"));
		///		col.Expression = Employees.ColumnNames.LastName + "+ ', ' + " + Employees.ColumnNames.FirstName;
		///
		///		string fullName;
		///
		///		do
		///			fullName = emps.GetColumn("FullName") as string;
		///		while(emps.MoveNext());
		///	}
		/// </code>
		/// </example>
		public DataColumn AddColumn(string name, System.Type dataType)
		{
			DataColumn dc = null;

			if(_dataTable != null)
			{
				dc = new DataColumn(name, dataType);
				_dataTable.Columns.Add(dc);
			}

			return dc;
		}

		/// <summary>
		/// You can use this to determine the rowstate of a given row in your BusinessEntity, examples are Added, Deleted, Modified.
		/// </summary>
		/// <returns>The row state of the current row in your BusinessEntity</returns>
		public DataRowState RowState()
		{
			if(_dataTable != null && _dataRow != null)
				return _dataRow.RowState;
			else
				return DataRowState.Detached;
		}

		/// <summary>
		/// The number of rows in your BusinessEntity or Zero if none.
		/// </summary>
		public int RowCount
		{
			get
			{
				int count = 0;

				if(_dataTable != null)
				{
					count = _dataTable.DefaultView.Count;
				}

				return count;
			}
		}

		/// <summary>
		/// Resets the interation process back to the first row.
		/// </summary>
		/// <example>
		/// VB.NET
		/// <code>
		/// Dim emps As New Employees
		/// If emps.LoadAll() Then
		///
		///    Dim lastName As String
		///
		///    ' Iteration walks the DataTable.DefaultView, see the FilterAndSort
		///    ' sample for further clarification.
		///    Do
		///        lastName = emps.LastName
		///    Loop Until Not emps.MoveNext
		///
		///    emps.Rewind()
		///
		///    Do
		///        lastName = emps.LastName
		///    Loop Until Not emps.MoveNext
		///
		/// End If
		/// </code>
		/// C#
		/// <code>
		/// Employees emps = new Employees();
		/// if(emps.LoadAll())
		/// {
		///		string lastName;
		///
		///		// Iteration walks the DataTable.DefaultView, see the FilterAndSort
		///		// sample for further clarification.
		///		do
		///			lastName = emps.LastName;
		///		while(emps.MoveNext());
		///
		///		emps.Rewind();
		///
		///		do
		///			lastName = emps.LastName;
		///		while(emps.MoveNext());
		/// } 
		/// </code>
		/// </example>
		public void Rewind()
		{
			_dataRow = null;
			_enumerator = null;

			if(_dataTable != null)
			{
				if(_dataTable.DefaultView.Count > 0) 
				{
					_enumerator = _dataTable.DefaultView.GetEnumerator();
					_enumerator.MoveNext();
					DataRowView rowView  = (DataRowView)_enumerator.Current;
					_dataRow = rowView.Row;
					_EOF = false;
				}
				else
				{
					_EOF = true;
				}
			}
			else
			{
				_EOF = true;
			}
		}

		/// <summary>
		/// After loading your BusinessEntity it is automatically position on the first row. Thus, you need to call MoveNext as shown in the following 
		/// example.
		/// </summary>
		/// <example>
		/// VB.NET
		/// <code>
		/// Dim emps As New Employees
		/// If emps.LoadAll() Then
		///
		///    Dim lastName As String
		///
		///    ' Iteration walks the DataTable.DefaultView, see the FilterAndSort
		///    ' sample for further clarification.
		///    Do
		///        lastName = emps.LastName
		///    Loop Until Not emps.MoveNext
		///
		///    emps.Rewind()
		///
		///    Do
		///        lastName = emps.LastName
		///    Loop Until Not emps.MoveNext
		///
		/// End If
		/// </code>
		/// C#
		/// <code>
		/// Employees emps = new Employees();
		/// if(emps.LoadAll())
		/// {
		///		string lastName;
		///
		///		// Iteration walks the DataTable.DefaultView, see the FilterAndSort
		///		// sample for further clarification.
		///		do
		///			lastName = emps.LastName;
		///		while(emps.MoveNext());
		///
		///		emps.Rewind();
		///
		///		do
		///			lastName = emps.LastName;
		///		while(emps.MoveNext());
		/// } 
		/// </code>
		/// </example>
		public bool MoveNext()
		{
			bool moved = false;

			if( _enumerator != null && _enumerator.MoveNext())
			{
				DataRowView rowView  = (DataRowView)_enumerator.Current;
				_dataRow = rowView.Row;
				moved = true;
			}
			else
			{
				_EOF = true;
			}

			return moved;
		}

		/// <summary>
		/// True if MoveNext() moves past the last row or
		/// there are 0 rows.
		/// </summary>
		/// <example>
		/// C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.LoadAll();
		/// 
		/// while(!emps.EOF)
		/// {
		/// 	// logic goes here ...
		/// 	emps.MoveNext();
		/// }
		/// </code>
		/// VB.NET
		/// <code>
		/// Dim emps as Employees = New Employees
		/// emps.LoadAll()
		///  
		/// While Not emps.EOF 
		///		' logic goes here
		///	    emps.MoveNext()
		///	End While    
		/// </code>
		/// </example>
		public bool EOF
		{
			get
			{
				return _EOF;
			}
		}

		/// <summary>
		/// Thus method flushes the Data in the DataTable and nulls out the Current Row, including any DynamicQuery Data.
		/// You can call FlushData() and than reload an object.
		/// </summary>
		public virtual void FlushData()
		{
			_dataRow = null;
			_dataTable = null;
			_enumerator = null;
			_query = null;
			_canSave = true;
			_EOF = true;
		}

		/// <summary>
		/// This is an ADO.NET DataView, both <see cref="Filter"/> and <see cref="Sort"/> effect the DefaultView, also Interation using 
		/// MoveNext and Rewind iterate over the DefaultView. When binding to controls such as DataGrid's and ComboBoxes bind to this property.
		/// </summary>
		public DataView DefaultView
		{
			get
			{
				if(_dataTable != null)
					return _dataTable.DefaultView;
				else
					return null;
			}
		}

		/// <summary>
		/// This is the ADO.NET DataTable, it holds the data for your BusinessEntity. It is protected so your derived class can have access
		/// to it but the consumers of your BusinessEntity cannot. Exposing this publically is not a good idea as your data would be able
		/// to be modified without going through you business logic.
		/// </summary>
		internal protected DataTable DataTable
		{
			get
			{
				return _dataTable;
			}

			set
			{
				_dataTable = value;
				_dataRow = null;

				if(_dataTable != null)
				{
					Rewind();
				}
			}												 
		}

		/// <summary>
		/// This is the ADO.NET DataRow, it holds the data for your BusinessEntity. It is protected so your derived class can have access
		/// to it but the consumers of your BusinessEntity cannot. Exposing this publically is not a good idea as your data would be able
		/// to be modified without going through you business logic.
		/// </summary>
		internal protected DataRow DataRow
		{
			get
			{
				return _dataRow;
			}

			set
			{
				_dataRow = value;
			}												 
		}

		#region ToXml / FromXml

		/// <summary>
		/// This method will allow you to save the contents within the embedded DataTable to XML.
		/// It is saved as a DataSet with Schema, data, and Rowstate as a DiffGram.
		/// You can load this data into another entity of the same type via Deserialize. 
		/// Call <see cref="GetChanges"/> before calling Serialize to serialize only the modified data.
		/// Also <see cref="Deserialize"/>
		/// </summary>
		/// <returns>The XML</returns>
		///	<example>
		///	VB.NET
		///	<code>
		///	Dim emps As New Employees
		/// emps.Query.Load()              ' emps.RowCount = 200
		/// emps.FirstName = "Griffinski"  ' Change first row
		/// emps.GetChanges()              ' emps.RowCount now = 1 
		/// Dim xml As String = emps.Serialize()
		/// 
		/// ' Now reload that single record into a new Employees object and Save it
		/// Dim empsClone As New Employees
		/// empsClone.Deserialize(xml)
		/// empsClone.Save()
		///	</code>
		///	C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.LoadAll();                // emps.RowCount = 200
		/// emps.LastName = "Griffinski";  // Change first row
		/// emps.GetChanges();             // emps.RowCount now = 1 
		/// string str = emps.Serialize();
		/// 
		/// // Now reload that single record into a new Employees object and Save it
		/// Employees empsClone = new Employees();
		/// empsClone.Deserialize(str);
		/// empsClone.Save();
		/// </code> 
		///	</example>
		virtual public string Serialize()
		{
			DataSet dataSet = new DataSet(); 
			dataSet.Tables.Add( _dataTable ); 
			StringWriter writer = new StringWriter(); 
			XmlSerializer ser = new XmlSerializer(typeof(DataSet));
			ser.Serialize(writer, dataSet);
			dataSet.Tables.Clear();
			return writer.ToString(); 
		}

		/// <summary>
		/// Reload the contents obtained from a previous call to <see cref="Serialize"/>.
		/// </summary>
		/// <param name="xml">The string to reload</param>
		///	<example>
		///	VB.NET
		///	<code>
		///	Dim emps As New Employees
		/// emps.Query.Load()              ' emps.RowCount = 200
		/// emps.FirstName = "Griffinski"  ' Change first row
		/// emps.GetChanges()              ' emps.RowCount now = 1 
		/// Dim xml As String = emps.Serialize()
		/// 
		/// ' Now reload that single record into a new Employees object and Save it
		/// Dim empsClone As New Employees
		/// empsClone.Deserialize(xml)
		/// empsClone.Save()
		///	</code>
		///	C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.LoadAll();                // emps.RowCount = 200
		/// emps.LastName = "Griffinski";  // Change first row
		/// emps.GetChanges();             // emps.RowCount now = 1 
		/// string str = emps.Serialize();
		/// 
		/// // Now reload that single record into a new Employees object and Save it
		/// Employees empsClone = new Employees();
		/// empsClone.Deserialize(str);
		/// empsClone.Save();
		/// </code> 
		///	</example>
		virtual public void Deserialize(string xml)
		{
			DataSet dataSet = new DataSet(); 
			StringReader reader = new StringReader(xml);
			XmlSerializer ser = new XmlSerializer(typeof(DataSet));
			dataSet = (DataSet)ser.Deserialize(reader);
			this.DataTable =  dataSet.Tables[0];
			dataSet.Tables.Clear();
		}

		/// <summary>
		/// This method will allow you to save the contents within the embedded DataTable only to XML.
		/// You can load this data into another entity of the same type via FromXml. 
		/// Call <see cref="GetChanges"/> before calling ToXml to serialize only the modified data.
		/// Also <see cref="FromXml"/>
		/// </summary>
		/// <returns>The XML</returns>
		///	<example>
		///	VB.NET
		///	<code>
		///	Dim emps As New Employees
		/// emps.Query.Load()              ' emps.RowCount = 200
		/// emps.FirstName = "Griffinski"  ' Change first row
		/// emps.GetChanges()              ' emps.RowCount now = 1 
		/// 
		/// ' Now reload that single record into a new Employees object and Save it
		/// Dim xml As String = emps.ToXml()
		/// Dim empsClone As New Employees
		/// empsClone.FromXml(xml)
		/// empsClone.Save()
		///	</code>
		///	C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.LoadAll();                // emps.RowCount = 200
		/// emps.LastName = "Griffinski";  // Change first row
		/// emps.GetChanges();             // emps.RowCount now = 1 
		/// 
		/// // Now reload that single record into a new Employees object and Save it
		/// string str = emps.ToXml();
		/// Employees empsClone = new Employees();
		/// empsClone.FromXml(str);
		/// empsClone.Save();
		/// </code> 
		///	</example>
		virtual public string ToXml()
		{
			DataSet dataSet = new DataSet(); 
			dataSet.Tables.Add( _dataTable ); 
			StringWriter writer = new StringWriter(); 
			dataSet.WriteXml(writer); 
			dataSet.Tables.Clear();
			return writer.ToString(); 
		}

		/// <summary>
		/// This method will allow you to save the contents within the embedded DataTable only to XML.
		/// You have better control of what gets serialized via the mode parameter.
		/// Call <see cref="GetChanges"/> before calling ToXml to serialize only the modified data.
		/// Also <see cref="FromXml"/>.
		/// </summary>
		/// <param name="mode">See the .NET enum XmlWriteMode for more help.</param>
		/// <returns></returns>
		///	<example>
		///	VB.NET
		///	<code>
		///	Dim emps As New Employees
		/// emps.Query.Load()              ' emps.RowCount = 200
		/// emps.FirstName = "Griffinski"  ' Change first row
		/// emps.GetChanges()              ' emps.RowCount now = 1 
		/// 
		/// ' Now reload that single record into a new Employees object and Save it
		/// Dim xml As String = emps.ToXml()
		/// Dim empsClone As New Employees
		/// empsClone.FromXml(xml)
		/// empsClone.Save()
		///	</code>
		///	C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.LoadAll();                // emps.RowCount = 200
		/// emps.LastName = "Griffinski";  // Change first row
		/// emps.GetChanges();             // emps.RowCount now = 1 
		/// 
		/// // Now reload that single record into a new Employees object and Save it
		/// string str = emps.ToXml();
		/// Employees empsClone = new Employees();
		/// empsClone.FromXml(str);
		/// empsClone.Save();
		/// </code> 
		///	</example>
		virtual public string ToXml(XmlWriteMode mode)
		{
			DataSet dataSet = new DataSet(); 
			dataSet.Tables.Add( _dataTable ); 
			StringWriter writer = new StringWriter(); 
			dataSet.WriteXml(writer, mode);
			dataSet.Tables.Clear();
			return writer.ToString(); 
		}

		/// <summary>
		/// Reload the contents obtained from a previous call to <see cref="ToXml"/>.
		/// </summary>
		/// <param name="xml">The string to reload</param>
		///	<example>
		///	VB.NET
		///	<code>
		///	Dim emps As New Employees
        /// emps.Query.Load()              ' emps.RowCount = 200
        /// emps.FirstName = "Griffinski"  ' Change first row
        /// emps.GetChanges()              ' emps.RowCount now = 1 
		/// 
		/// ' Now reload that single record into a new Employees object and Save it
		/// Dim xml As String = emps.ToXml()
        /// Dim empsClone As New Employees
        /// empsClone.FromXml(xml)
        /// empsClone.Save()
		///	</code>
		///	C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.LoadAll();                // emps.RowCount = 200
		/// emps.LastName = "Griffinski";  // Change first row
		/// emps.GetChanges();             // emps.RowCount now = 1 
		/// 
		/// // Now reload that single record into a new Employees object and Save it
		/// string str = emps.ToXml();
		/// Employees empsClone = new Employees();
		/// empsClone.FromXml(str);
		/// empsClone.Save();
		/// </code> 
		///	</example>
		virtual public void FromXml(string xml)
		{
			DataSet dataSet = new DataSet(); 
			StringReader reader = new StringReader(xml);
			dataSet.ReadXml(reader);
			this.DataTable =  dataSet.Tables[0];
			dataSet.Tables.Clear();
		}

		/// <summary>
		/// Reload the contents obtained from a previous call to <see cref="ToXml"/>. Use the mode
		/// parameter for finer control.
		/// </summary>
		/// <param name="xml">The string to reload</param>
		/// <param name="mode">See the .NET XmlReadMode enum for more help.</param>
		///	<example>
		///	VB.NET
		///	<code>
		///	Dim emps As New Employees
		/// emps.Query.Load()              ' emps.RowCount = 200
		/// emps.FirstName = "Griffinski"  ' Change first row
		/// emps.GetChanges()              ' emps.RowCount now = 1 
		/// 
		/// ' Now reload that single record into a new Employees object and Save it
		/// Dim xml As String = emps.ToXml()
		/// Dim empsClone As New Employees
		/// empsClone.FromXml(xml)
		/// empsClone.Save()
		///	</code>
		///	C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.LoadAll();                // emps.RowCount = 200
		/// emps.LastName = "Griffinski";  // Change first row
		/// emps.GetChanges();             // emps.RowCount now = 1 
		/// 
		/// // Now reload that single record into a new Employees object and Save it
		/// string str = emps.ToXml();
		/// Employees empsClone = new Employees();
		/// empsClone.FromXml(str);
		/// empsClone.Save();
		/// </code> 
		///	</example>
		virtual public void FromXml(string xml, XmlReadMode mode)
		{
			DataSet dataSet = new DataSet(); 
			StringReader reader = new StringReader(xml);
			dataSet.ReadXml(reader, mode);
			this.DataTable =  dataSet.Tables[0];
			dataSet.Tables.Clear();
		}

		#endregion

		#region SQL Methods -- LoadFromSql, AddNew, MarkAsDeleted, Save

		/// <summary>
		/// Use LoadFromSql to load you BusinessEntity from a custom stored procedure. The generated method in your 
		/// BusinessEntity called 'LoadByPrimaryKey' uses this method.
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <returns>True if at least one row was loaded</returns>
		protected bool LoadFromSql(string sp)
		{
			return this.LoadFromSql(sp, null, CommandType.StoredProcedure);
		}

		/// <summary>
		/// This method allows you to also pass in paramters and thier values.
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see the coding samples for this method</param>
		/// <returns>True if at least one row was loaded</returns>
		/// <example>
		/// <code>
		/// Public Sub CallCustomProcUsingSqlParameter(ByVal EmployeeID As Integer)
		///
		///		' Uses SqlParameters from generated entity
		///		Dim Parameters As ListDictionary = New ListDictionary
		///		Parameters.Add(Me.Parameters.EmployeeID, EmployeeID)
		///
		///		' This loads the data for this object
		///		Me.LoadFromSql("proc_GetSpecialEmployees", Parameters)
		///
		///	End Sub
		/// </code>
		/// or
		/// <code>
		/// Public Sub CallCustomProc(ByVal EmployeeID As Integer)
		///
		///		' Doesn't use a SqlParameter: 
		///		' Only do this when there isn't one in your Parameters collection !!
		///		Dim Parameters As ListDictionary = New ListDictionary
		///		Parameters.Add(Me.ColumnNames.EmployeeID, EmployeeID)
		///
		///		' This loads the data for this object
		///		Me.LoadFromSql("proc_GetSpecialEmployees", Parameters)
		///
		///	End Sub
		/// </code>
		/// </example>
		protected bool LoadFromSql(string sp, ListDictionary Parameters)
		{
			return this.LoadFromSql(sp, Parameters, CommandType.StoredProcedure);
		}

		/// <summary>
		/// This method allows you to pass in direct sql.
		/// </summary>
		/// <param name="sp">This can be a stored procedure, a table, or direct sql</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see the coding samples for this method</param>
		/// <param name="commandType">This property determines the type being passed in the "sp" parameter</param>
		/// <returns>True if at least one row was loaded</returns>
		protected bool LoadFromSql(string sp, ListDictionary Parameters, CommandType commandType)
		{
			DataTable dataTable = null;
			bool loaded  = false;

			try
			{
				dataTable = new DataTable(this.MappingName);

				IDbCommand cmd = this.CreateIDbCommand();
				cmd.CommandText = sp;
				cmd.CommandType = commandType;

				IDataParameter p;

				if( Parameters != null)
				{
					foreach(DictionaryEntry param in Parameters)
					{
						p = param.Key as IDataParameter;

						if(null == p)
						{
							p = this.CreateIDataParameter((string)param.Key, param.Value);
						}
						else
						{
							p.Value = param.Value;
						}

						cmd.Parameters.Add(p);
					}	
				}

				IDbDataAdapter da = this.CreateIDbDataAdapter();

				da.SelectCommand = cmd;

				TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

				txMgr.Enlist(cmd, this);
				DbDataAdapter dba = ConvertIDbDataAdapter(da);
				dba.Fill(dataTable);
				txMgr.DeEnlist(cmd, this);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.DataTable = dataTable;
				loaded = (this.RowCount > 0);
			}

			return loaded;
		}

		/// <summary>
		/// LoadFromRawSql provides a quick and easy way (easier than say LoadFromSql) to execute a raw sql statement. All
		/// values passed in via parameters will be passed in via actual SQL Parameters to prevent
		/// SQL injection techniques, you can pass in any number of values via {0}, {1} and so on ...
		/// </summary>
		/// <param name="rawSql"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <example>
		/// <code>
		/// public class Employees : _Employees
		/// {
		/// 	public bool MySpecialLoad()
		/// 	{
		/// 		// Load All Employees with an 'o' in the last name
		/// 		return base.LoadFromRawSql("Select * from Employees where LastName LIKE {0}", "%o%");
		/// 	}
		/// }
		/// </code>
		/// </example> 
		protected bool LoadFromRawSql(string rawSql, params object[] parameters)
		{
			bool loaded  = false;
			DataTable dt = null;

			try
			{
				IDbCommand cmd  = _LoadFromRawSql(rawSql, parameters);

				IDbDataAdapter da = this.CreateIDbDataAdapter();
				da.SelectCommand = cmd;

				TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

				dt = new DataTable(this.MappingName);

				txMgr.Enlist(cmd, this);
				DbDataAdapter dbDataAdapter = this.ConvertIDbDataAdapter(da);
				dbDataAdapter.Fill(dt);
				txMgr.DeEnlist(cmd, this);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				this.DataTable = dt;
				loaded = (dt.Rows.Count > 0);
			}

			return loaded;
		}

		/// <summary>
		/// Use LoadFromSqlNoExec to execute a stored procedure, this method does not load data into your BusinessEntity.
		/// </summary>
		/// <param name="sp">The name of the stored procedure</param>
		/// <returns>The number of rows affected</returns> 
		protected int LoadFromSqlNoExec(string sp)
		{
			return this.LoadFromSqlNoExec(sp, null, CommandType.StoredProcedure, -1);
		}

		/// <summary>
		/// This method allows you to pass in parameters and their values. See <see cref="LoadFromSql"/>
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see the coding samples for this method</param>
		/// <returns>The number of rows affected</returns> 
		protected int LoadFromSqlNoExec(string sp, ListDictionary Parameters)
		{
			return this.LoadFromSqlNoExec(sp, Parameters, CommandType.StoredProcedure, -1);
		}

		/// <summary>
		/// This method allows you to pass in direct sql and to control the timeout value.
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see <see cref="LoadFromSql"/></param>
		/// <param name="commandType">This property determines the type being passed in the "sp" parameter</param>
		/// <param name="commandTimeout">-1 is standard database timeout</param>
		/// <returns>The number of rows affected</returns> 
		protected int LoadFromSqlNoExec(string sp, 
			ListDictionary Parameters, 
			CommandType commandType,
			int commandTimeout)
		{
			int retValue = 0;

			try
			{
				IDbCommand cmd = this.CreateIDbCommand();
				cmd.CommandText = sp;
				cmd.CommandType = commandType;

				if(commandTimeout > 0)
				{
					cmd.CommandTimeout = commandTimeout;					
				}

				IDataParameter p;

				if( Parameters != null)
				{
					foreach(DictionaryEntry param in Parameters)
					{
						p = param.Key as IDataParameter;

						if(null == p)
						{
							p = this.CreateIDataParameter((string)param.Key, param.Value);
						}
						else
						{
							p.Value = param.Value;
						}

						cmd.Parameters.Add(p);
					}	
				}

				TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

				txMgr.Enlist(cmd, this);
				retValue = cmd.ExecuteNonQuery();
				txMgr.DeEnlist(cmd, this);
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return retValue;
		}

		/// <summary>
		/// Use LoadFromSqlScalar to execute a stored procedure, this method does not load data into your BusinessEntity.
		/// </summary>
		/// <param name="sp">The name of the stored procedure</param>
		/// <returns>The first column of the first row in the resultset.</returns> 
		protected object LoadFromSqlScalar(string sp)
		{
			return this.LoadFromSqlScalar(sp, null, CommandType.StoredProcedure, -1);
		}

		/// <summary>
		/// This method allows you to pass in parameters and their values. See <see cref="LoadFromSqlScalar"/>
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see the coding samples for this method</param>
		/// <returns>The first column of the first row in the resultset.</returns> 
		protected object LoadFromSqlScalar(string sp, ListDictionary Parameters)
		{
			return this.LoadFromSqlScalar(sp, Parameters, CommandType.StoredProcedure, -1);
		}

		/// <summary>
		/// This method allows you to pass in direct sql and to control the timeout value.
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see <see cref="LoadFromSql"/></param>
		/// <param name="commandType">This property determines the type being passed in the "sp" parameter</param>
		/// <param name="commandTimeout">-1 is standard database timeout</param>
		/// <returns>The first column of the first row in the resultset.</returns> 
		protected object LoadFromSqlScalar(string sp, 
			ListDictionary Parameters, 
			CommandType commandType,
			int commandTimeout)
		{
			object retValue = 0;

			try
			{
				IDbCommand cmd = this.CreateIDbCommand();
				cmd.CommandText = sp;
				cmd.CommandType = commandType;

				if(commandTimeout > 0)
				{
					cmd.CommandTimeout = commandTimeout;					
				}

				IDataParameter p;

				if( Parameters != null)
				{
					foreach(DictionaryEntry param in Parameters)
					{
						p = param.Key as IDataParameter;

						if(null == p)
						{
							p = this.CreateIDataParameter((string)param.Key, param.Value);
						}
						else
						{
							p.Value = param.Value;
						}

						cmd.Parameters.Add(p);
					}	
				}

				TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

				txMgr.Enlist(cmd, this);
				retValue = cmd.ExecuteScalar();
				txMgr.DeEnlist(cmd, this);
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return retValue;
		}

		/// <summary>
		/// LoadFromSqlReader does not load data into your BusinessEntity
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <returns>The IDataReader (SqlDataReader, OleDbDataReader)</returns>
		protected IDataReader LoadFromSqlReader(string sp)
		{
			return this.LoadFromSqlReader(sp, null, CommandType.StoredProcedure);
		}

		/// <summary>
		/// This version allows you to pass in Parameters and thier values
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see <see cref="LoadFromSql"/></param>
		/// <returns>The IDataReader (SqlDataReader, OleDbDataReader)</returns>
		protected IDataReader LoadFromSqlReader(string sp, ListDictionary Parameters)
		{
			return this.LoadFromSqlReader(sp, Parameters, CommandType.StoredProcedure);
		}

		/// <summary>
		/// This version allow you to use direct sql.
		/// </summary>
		/// <param name="sp">This must be a stored procedure</param>
		/// <param name="Parameters">Two types of key/value pairs are allowed, see <see cref="LoadFromSql"/></param>
		/// <param name="commandType">This property determines the type being passed in the "sp" parameter</param>
		/// <returns>The IDataReader (SqlDataReader, OleDbDataReader)</returns>
		protected IDataReader LoadFromSqlReader(string sp, ListDictionary Parameters, CommandType commandType)
		{
			try
			{
				IDbCommand cmd = this.CreateIDbCommand();
				cmd.CommandText = sp;
				cmd.CommandType = commandType;

				IDataParameter p;

				if( Parameters != null)
				{
					foreach(DictionaryEntry param in Parameters)
					{
						p = param.Key as IDataParameter;

						if(null == p)
						{
							p = this.CreateIDataParameter((string)param.Key, param.Value);
						}
						else
						{
							p.Value = param.Value;
						}

						cmd.Parameters.Add(p);
					}	
				}

				cmd.Connection = this.CreateIDbConnection();
				cmd.Connection.ConnectionString = this.RawConnectionString;
				cmd.Connection.Open();
				return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// AddNew is how you add a new record to your BusinessEntity, when saved this row will be an INSERT. 
		/// All Identity columns and calculated columns are present and available after calling Save()
		/// </summary>
		/// <example>
		/// VB.NET
		/// <code>
		/// Dim emps As New Employees
		/// emps.AddNew()
		/// emps.FirstName = "Jimmy"
		/// emps.LastName = "Coder"
		/// emps.Save()
		/// 
		/// ' Notice how upon return from Save the EmployeeID property is ready and waiting for us
		/// ' In order for this to work your DBMS must support OUTPUT parameters.
		/// Dim empID As Integer 
		/// empID = emps.EmployeeID</code>
		/// C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.AddNew();
		/// emps.FirstName = "Jimmy";
		/// emps.LastName = "Coder";
		/// emps.Save();
		/// 
		/// // Notice how upon return from Save the EmployeeID property is ready and waiting for us
		/// // In order for this to work your DBMS must support OUTPUT parameters.
		/// int empID = emps.EmployeeID;</code>
		/// </example>
		virtual public void AddNew()
		{
			if(_dataTable == null)
			{
				this.LoadFromSql("SELECT * FROM [" +  QuerySource + "] WHERE 1=0", null, CommandType.Text);
			}

			DataRow newRow = _dataTable.NewRow();
			_dataTable.Rows.Add(newRow);
			_dataRow = newRow;
		}

		/// <summary>
		/// MarkAsDeleted does as it name suggests, marks the current row as deleted. You still must call <see cref="Save"/> to actually delete the row.
		/// See also <see cref="DeleteAll"/>
		/// </summary>
		virtual public void MarkAsDeleted()
		{
			if(_dataRow != null)
			{
				_dataRow.Delete();
			}
		}

		/// <summary>
		/// DeleteAll does as it name suggests, marks ALL of the rows as deleted. You still must call <see cref="Save"/> to actually delete the rows.
		/// This method, DeleteAll, should really be called MarkAllAsDeleted but we were afraid it might accidentally be chosen via intellisense, see also <see cref="MarkAsDeleted"/>
		/// </summary>
		virtual public void DeleteAll()
		{
			if(_dataTable != null)
			{
				DataRow row;

				DataRowCollection rows = _dataTable.Rows;
				for(int i = 0; i < rows.Count; i++)
				{
					row = rows[i];
					row.Delete();
				}
			}
		}

		/// <summary>
		/// Save is really a bulk save, you can call <see cref="AddNew"/> to insert a row, call <see cref="MarkAsDeleted"/> to mark a row for deletion, 
		/// and modify other rows through properties and then call Save. All of the changes will be save in this one call, and by default they are
		/// protected by a Transaction.
		/// </summary>
		/// <example>
		/// VB.NET
		/// <code>
		/// Dim emps As New Employees
		/// emps.AddNew()
		/// emps.FirstName = "Jimmy"
		/// emps.LastName = "Coder"
		/// emps.Save()
		/// 
		/// ' Notice how upon return from Save the EmployeeID property is ready and waiting for us
		/// ' In order for this to work your DBMS must support OUTPUT parameters.
		/// Dim empID As Integer 
		/// empID = emps.EmployeeID</code>
		/// C#
		/// <code>
		/// Employees emps = new Employees();
		/// emps.AddNew();
		/// emps.FirstName = "Jimmy";
		/// emps.LastName = "Coder";
		/// emps.Save();
		/// 
		/// // Notice how upon return from Save the EmployeeID property is ready and waiting for us
		/// // In order for this to work your DBMS must support OUTPUT parameters.
		/// int empID = emps.EmployeeID;</code>
		/// </example>
		virtual public void Save()
		{
			if(_dataTable == null) return;

			if(!_canSave) throw new Exception("Cannot call Save() after Query.AddResultColumn()");

			TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

			try
			{
				bool needToInsert  = false;
				bool needToUpdate  = false;
				bool needToDelete  = false;

				DataRow row;
				DataRowCollection rows = _dataTable.Rows;
				for(int i = 0; i < rows.Count; i++)
				{
					row = rows[i];

					switch(row.RowState)
					{
						case DataRowState.Added:
							needToInsert = true;
							break;
						case DataRowState.Modified:
							needToUpdate = true;
							break;
						case DataRowState.Deleted:
							needToDelete = true;
							break;
					}
				}

				if( needToInsert || needToUpdate || needToDelete)
				{
					// Yes, we do
					IDbDataAdapter da = this.CreateIDbDataAdapter();

					if(needToInsert) da.InsertCommand = GetInsertCommand();
					if(needToUpdate) da.UpdateCommand = GetUpdateCommand();
					if(needToDelete) da.DeleteCommand = GetDeleteCommand();

					txMgr.BeginTransaction();

					// We need to NOT call AcceptChanges() directly if the user 
					// has also called BeginTransaction()
					int txCount = txMgr.NestingCount;
					if(txCount > 1) txMgr.AddBusinessEntity(this);

					if(needToInsert) txMgr.Enlist(da.InsertCommand, this);
					if(needToUpdate) txMgr.Enlist(da.UpdateCommand, this);
					if(needToDelete) txMgr.Enlist(da.DeleteCommand, this);

					DbDataAdapter dbDataAdapter = ConvertIDbDataAdapter(da);

					// Let's give folks a chance to do some custom work here
					this.HookupRowUpdateEvents(dbDataAdapter);

					dbDataAdapter.Update(_dataTable);

					txMgr.CommitTransaction();

					// We started the transaction so we can call AcceptChanges() as 
					// we know that there's nobody else in the transaction
					if(txCount == 1) this.AcceptChanges();

					if(needToInsert) txMgr.DeEnlist(da.InsertCommand, this);
					if(needToUpdate) txMgr.DeEnlist(da.UpdateCommand, this);
					if(needToDelete) txMgr.DeEnlist(da.DeleteCommand, this);
				}
			}
			catch(Exception ex)
			{
				if( txMgr != null) txMgr.RollbackTransaction();
				throw ex;
			}
		}

		/// <summary>
		/// If you need to trap DbDataAdapter.RowUpdated or DbDataAdapter.RowUpdating events, overload this method.
		/// </summary>
		/// <example>
		/// The OleDbEntity does something similiar to the below code snippet to return identity columns.
		/// <code>
		/// override protected void HookupRowUpdateEvents(DbDataAdapter adapter)
		/// {
		///     OleDbDataAdapter da = adapter as OleDbDataAdapter;
		///     da.RowUpdated += new OleDbRowUpdatedEventHandler(OnRowUpdated);
	    /// }
		/// </code>
		/// </example>
		/// <param name="adapter"></param>
		virtual protected void HookupRowUpdateEvents(DbDataAdapter adapter)
		{
			// Default is to do nothing, however, the OleDbEntity overrides this
			// to hookup the RowUpdated event in order to return the latest identity value
		}

		/// <summary>
		/// Call this to revert your business entity back to it's original state, same as DataTable.RejectChanges()
		/// </summary>
		virtual public void RejectChanges()
		{
			if( _dataTable != null) _dataTable.RejectChanges();
		}

		/// <summary>
		/// This is called after a successful call to Save(), all rows marked as Added, Modified or Deleted are changed
		/// to Unchanged. Deleted rows, of course, are gone after calling AcceptChanges, same as DataTable.AcceptChanges()
		/// </summary>
		virtual public void AcceptChanges()
		{
			if( _dataTable != null) _dataTable.AcceptChanges();
		}

		/// <summary>
		/// After calling this method your BusinessEntity will contain only rows that have been
		/// Added, Modified, or Deleted. See <see cref="ToXml"/>.
		/// </summary>
		virtual public void GetChanges()
		{
			if( _dataTable != null)
			{
				this.DataTable = _dataTable.GetChanges();
			}
		}

		/// <summary>
		/// After calling this method your BusinessEntity will contain only those row that match the value
		/// of the rowStates parameter. See <see cref="ToXml"/>.
		/// </summary>
		/// <param name="rowStates"></param>
		virtual public void GetChanges(System.Data.DataRowState rowStates)
		{
			if( _dataTable != null)
			{
				this.DataTable = _dataTable.GetChanges(rowStates);
			}
		}

		#endregion

		/// <summary>
		/// This property returns the DynamicQuery object which will allow you to build complex queries using your BusinessEntity.
		/// </summary>
		public DynamicQuery Query
		{
			get
			{
				if(_query == null)
				{
					_query = this.CreateDynamicQuery(this);
				}

				return _query;
			}
		}

		internal string RawConnectionString
		{
			get
			{
				string connStr;

				if (_raw != "")
					connStr = _raw;
				else
#if(VS2005)
					connStr = ConfigurationManager.AppSettings[_config];
#else
					connStr = ConfigurationSettings.AppSettings[_config];
#endif
                
				return connStr;
			}
		}

		#region Row Accessors

		/// <summary>
		/// Except for "string" and "Guid" properties all other properties will throw an exception if you call thier "get" accessors.
		/// You can head off the exception by calling this method.
		/// </summary>
		/// <param name="columnName">This should be one of your "Columns" property</param>
		/// <returns>True if the column is DBNull.Value</returns>
		/// <example>
		/// VB.NET
		/// <code>
		/// Dim emps As Employees
		/// If emps.LoadByPrimaryKey(42) Then
		///    If emps.IsColumnNull(emps.ColumnNames.Photo) Then
		///        ' The Photo column is null
		///    End If
		/// End If</code>
		/// C#
		/// <code>
		/// Employees emps = new Employees();
		/// if(emps.LoadByPrimaryKey(42))
		/// {
		///		if(emps.IsColumnNull(Employees.ColumnNames.Photo))
		///		{
		///	        // The Photo column is null
		///		}
		/// }</code>
		/// </example>
		public bool IsColumnNull(string columnName)
		{
			return _dataRow.IsNull(columnName);
		}

		/// <summary>
		/// Use this method to set a column to DBNull.Value which will translate to NULL in your DBMS system.
		/// </summary>
		/// <param name="columnName">The name of the column. Use your ColumnNames like this, Employees.ColumnNames.Photo</param>
		virtual public void SetColumnNull(string columnName)
		{
			_dataRow[columnName] = DBNull.Value;
		}

		/// <summary>
		/// This is the typeless version, this method should only be used for columns that you added via <see cref="AddColumn"/> or to access
		/// extra columns brought back by changing your <see cref="QuerySource"/> to a SQL View.
		/// </summary>
		/// <param name="columnName">The name of the column, "MyColumn"</param>
		/// <param name="Value">The value to set the column to</param>
		public void SetColumn(string columnName, object Value)
		{
			_dataRow[columnName] = Value;
		}

		/// <summary>
		/// This is the typeless version, this method should only be used for columns that you added via <see cref="AddColumn"/> or to access
		/// extra columns brought back by changing your <see cref="QuerySource"/> to a SQL View.
		/// </summary>
		/// <param name="columnName">The name of the column, "MyColumn"</param>
		/// <returns>The value, you will have to typecast it to the proper type.</returns>
		public object GetColumn(string columnName)
		{
			return _dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected Guid GetGuid(string columnName)
		{
			if(_dataRow.IsNull(columnName))
				return Guid.Empty;
			else
				return (Guid)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void SetGuid(string columnName, Guid data)
		{
			if( Guid.Empty.Equals(data)) 
				_dataRow[columnName] = DBNull.Value;
			else
				_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected bool Getbool(string columnName)
		{
			return (bool)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setbool(string columnName, bool data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string Getstring(string columnName)
		{
			if(_dataRow.IsNull(columnName))
				return String.Empty;
			else
				return (string)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setstring(string columnName, string data)
		{
			if(0 == data.Length) 
				_dataRow[columnName] = DBNull.Value;
			else
				_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected int Getint(string columnName)
		{
			return (int)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setint(string columnName, int data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected uint Getuint(string columnName)
		{
			return (uint)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setuint(string columnName, uint data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected long Getlong(string columnName)
		{
			return (long)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setlong(string columnName, long data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected ulong Getulong(string columnName)
		{
			return (ulong)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setulong(string columnName, ulong data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected short Getshort(string columnName)
		{
			return (short)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setshort(string columnName, short data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected ushort Getushort(string columnName)
		{
			return (ushort)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setushort(string columnName, ushort data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected DateTime GetDateTime(string columnName)
		{
			return (DateTime)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void SetDateTime(string columnName, DateTime data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected TimeSpan GetTimeSpan(string columnName)
		{
			return (TimeSpan)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void SetTimeSpan(string columnName, TimeSpan data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected decimal Getdecimal(string columnName)
		{
			return (decimal)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setdecimal(string columnName, decimal data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected float Getfloat(string columnName)
		{
			return (float)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setfloat(string columnName, float data)
		{
			_dataRow[columnName] = data;
		}


		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected double Getdouble(string columnName)
		{
			return (double)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setdouble(string columnName, double data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected byte Getbyte(string columnName)
		{
			return (byte)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setbyte(string columnName, byte data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected sbyte Getsbyte(string columnName)
		{
			return (sbyte)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setsbyte(string columnName, sbyte data)
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected object Getobject(string columnName)
		{
			return (object)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		protected void Setobject(string columnName, object data)
		{
			_dataRow[columnName] = data;
		}

		#endregion

		#region Array Row Accessors

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected byte[] GetByteArray(string columnName)
		{
			return _dataRow[columnName] as byte[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="byteArray">The Value</param>
		protected void SetByteArray(string columnName, byte[] byteArray)
		{
			_dataRow[columnName] = byteArray;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected bool[] GetboolArray(string columnName)
		{
			return _dataRow[columnName] as bool[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetboolArray(string columnName, bool[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string[] GetstringArray(string columnName)
		{
			return _dataRow[columnName] as string[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetstringArray(string columnName, string[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected int[] GetintArray(string columnName)
		{
			return _dataRow[columnName] as int[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetintArray(string columnName, int[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected long[] GetlongArray(string columnName)
		{
			return _dataRow[columnName] as long[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetlongArray(string columnName, long[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected short[] GetshortArray(string columnName)
		{
			return _dataRow[columnName] as short[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetshortArray(string columnName, short[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected DateTime[] GetDateTimeArray(string columnName)
		{
			return _dataRow[columnName] as DateTime[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetDateTimeArray(string columnName, DateTime[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected decimal[] GetdecimalArray(string columnName)
		{
			return _dataRow[columnName] as decimal[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetdecimalArray(string columnName, decimal[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected float[] GetfloatArray(string columnName)
		{
			return _dataRow[columnName] as float[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetfloatArray(string columnName, float[] data )
		{
			_dataRow[columnName] = data;
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected double[] GetdoubleArray(string columnName)
		{
			return _dataRow[columnName] as double[];
		}

		/// <summary>
		/// Used by the Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param> 
		/// <returns>The value</returns>
		protected void SetdoubleArray(string columnName, double[] data )
		{
			_dataRow[columnName] = data;
		}

		#endregion

		#region String Row Accessors
		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetGuidAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected Guid SetGuidAsString(string columnName, string data)
		{
			return new Guid(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetboolAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected bool SetboolAsString(string columnName, string data)
		{
			return Convert.ToBoolean(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetstringAsString(string columnName)
		{
			return (string)_dataRow[columnName];
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected string SetstringAsString(string columnName, string data)
		{
			return data;
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetintAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected int SetintAsString(string columnName, string data)
		{
			return Convert.ToInt32(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetuintAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected uint SetuintAsString(string columnName, string data)
		{
			return Convert.ToUInt32(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetlongAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected long SetlongAsString(string columnName, string data)
		{
			return Convert.ToInt64(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetshortAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected short SetshortAsString(string columnName, string data)
		{
			return Convert.ToInt16(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetushortAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected ushort SetushortAsString(string columnName, string data)
		{
			return Convert.ToUInt16(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetDateTimeAsString(string columnName)
		{
			return ((DateTime)_dataRow[columnName]).ToString(this.StringFormat);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected DateTime SetDateTimeAsString(string columnName, string data)
		{
			return Convert.ToDateTime(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetTimeSpanAsString(string columnName)
		{
			return ((TimeSpan)_dataRow[columnName]).ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected TimeSpan SetTimeSpanAsString(string columnName, string data)
		{
			return TimeSpan.Parse(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetdecimalAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected decimal SetdecimalAsString(string columnName, string data)
		{
			return Convert.ToDecimal(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetfloatAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected Single SetfloatAsString(string columnName, string data)
		{
			return Convert.ToSingle(data);
		}


		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetdoubleAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected double SetdoubleAsString(string columnName, string data)
		{
			return Convert.ToDouble(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetbyteAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected byte SetbyteAsString(string columnName, string data)
		{
			return Convert.ToByte(data);
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <returns>The value</returns>
		protected string GetsbyteAsString(string columnName)
		{
			return _dataRow[columnName].ToString();
		}

		/// <summary>
		/// Used by the String Properties in your generated class. 
		/// </summary>
		/// <param name="columnName">One of the named for your ColumnNames class</param>
		/// <param name="data">The Value</param>
		/// <returns>The Strong Type</returns>
		protected sbyte SetsbyteAsString(string columnName, string data)
		{
			return Convert.ToSByte(data);
		}
		#endregion

		// Private Variables
		protected DataRow _dataRow = null;
		private DataTable _dataTable = null;
		private IEnumerator _enumerator = null;

		private static string _defaultConfig = "dbConnection";
		internal string _config = BusinessEntity._defaultConfig;
		internal IDbConnection _notRecommendedConnection = null;
		internal string _raw = "";
		internal bool   _canSave = true;

		private DynamicQuery _query;

		private string _querySource = "";
		private string _schemaGlobal = "";
		private string _tableViewSchema = "";
		private string _storedProcSchema = "";
		private string _mappingName = "";
		private bool _EOF = true;
	}
}
