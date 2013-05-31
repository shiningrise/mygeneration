//==============================================================================
// MyGeneration.dOOdads
//
// DynamicQuery.cs
// Version 5.0
// Updated - 09/15/2005
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
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// DynamicQuery allows you to (without writing any stored procedures) query your database on the fly. All selection criteria are passed in
	/// via Parameters (SqlParameter, OleDbParameter) in order to prevent sql injection tecniques often attempted by hackers.  
	/// </summary>
	/// <example>
	/// VB.NET
	/// <code>
	/// Dim emps As New Employees
	///
    /// ' LastNames that have "A" anywher in them
    /// emps.Where.LastName.Value = "%A%"
    /// emps.Where.LastName.Operator = WhereParameter.Operand.Like_
	///
    /// ' Only return the EmployeeID and LastName
    /// emps.Query.AddResultColumn(Employees.ColumnNames.EmployeeID)
    /// emps.Query.AddResultColumn(Employees.ColumnNames.LastName)
	///
    /// ' Order by LastName 
    /// ' (you can add as many order by columns as you like by repeatedly calling this)
    /// emps.Query.AddOrderBy(Employees.ColumnNames.LastName, WhereParameter.Dir.ASC)
	///
    /// ' Bring back only distinct rows
    /// emps.Query.Distinct = True
	///
    /// ' Bring back the top 10 rows
    /// emps.Query.Top = 10
	///
    /// emps.Query.Load()</code>
	///	C#
	///	<code>
	/// Employees emps = new Employees();
	///
	/// // LastNames that have "A" anywher in them
	/// emps.Where.LastName.Value = "%A%";
	/// emps.Where.LastName.Operator = WhereParameter.Operand.Like;
	///
	/// // Only return the EmployeeID and LastName
	/// emps.Query.AddResultColumn(Employees.ColumnNames.EmployeeID);
	/// emps.Query.AddResultColumn(Employees.ColumnNames.LastName);
	///
	/// // Order by LastName 
	/// // (you can add as many order by columns as you like by repeatedly calling this)
	/// emps.Query.AddOrderBy(Employees.ColumnNames.LastName, WhereParameter.Dir.ASC);
	///
	/// // Bring back only distinct rows
	/// emps.Query.Distinct = true;
	///
	/// // Bring back the top 10 rows
	/// emps.Query.Top = 10;
	///
	/// emps.Query.Load();</code>
	/// </example>
	abstract public class DynamicQuery
	{
		/// <summary>
		/// Derived classes implement this, like SqlClientDynamicQuery and OleDbDynamicQuery to account for differences in DBMS systems.
		/// </summary>
		/// <param name="conjuction">The conjuction, "AND" or "OR"</param>
		/// <returns></returns>
		abstract protected IDbCommand _Load(string conjuction);	

		/// <summary>
		/// You never need to call this, just access your BusinessEntity.Query property.
		/// </summary>
		/// <param name="entity">Passed in via the BusinessEntity</param>
		public DynamicQuery(BusinessEntity entity)
		{
			this._entity = entity;
		}

		private IDbCommand _Load()
		{
			return _Load("AND");
		}


		/// <summary>
		/// Execute the Query and loads your BusinessEntity. The default conjuction between the WHERE parameters is "AND"
		/// </summary>
		/// <returns>True if at least one record was loaded</returns>
		public bool Load()
		{
			return Load("AND");
		}

		/// <summary>
		/// Execute the Query and loads your BusinessEntity. 
		/// You can pass in the conjustion that will be used between the WHERE parameters, either, "AND" or "OR". "AND" is the default.
		/// Also, if you need to be notified that this is being called override BusinessEntity.OnQueryLoad().
		/// </summary>
		/// <returns>True if at least one record was loaded</returns>
		public bool Load(string conjuction)
		{
			bool loaded  = false;
			DataTable dt = null;

			try
			{
				if(( _aggregateParameters == null || _aggregateParameters.Count <= 0)
					&& _resultColumns.Length <= 0 && _countAll == false)
				{
					this._entity._canSave = true;
				}

				this._entity.OnQueryLoad(conjuction);

				IDbCommand cmd  = _Load(conjuction);
				_lastQuery = cmd.CommandText;

				IDbDataAdapter da = this._entity.CreateIDbDataAdapter();
				da.SelectCommand = cmd;

				TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

				dt = new DataTable(_entity.MappingName);

				txMgr.Enlist(cmd, _entity);
				DbDataAdapter dbDataAdapter = this._entity.ConvertIDbDataAdapter(da);
				dbDataAdapter.Fill(dt);
				txMgr.DeEnlist(cmd, _entity);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				this._entity.DataTable = dt;
				loaded = (dt.Rows.Count > 0);
			}

			return loaded;
		}

		/// <summary>
		/// Builds the SQL Statement that it would use to execute the Query, however, it doesn't execute, just returns it as a string for debugging purposes.
		/// "AND" is the default conjuction.
		/// </summary>
		/// <returns>The SQL statement</returns>
		public string GenerateSQL()
		{
			return GenerateSQL("AND");
		}

		/// <summary>
		/// Contains the Query string from your last call to Query.Load(), this is useful for debugging purposes.
		/// </summary>
		/// <returns>The SQL statement</returns>
		public string LastQuery
		{
			get
			{
				return _lastQuery;
			}
		}

		/// <summary>
		/// Builds the SQL Statement that it would use to execute the Query, however, it doesn't execute, just returns it as a string for debugging purposes.
		/// You can pass in the conjustion that will be used between the WHERE parameters, either, "AND" or "OR". "AND" is the default.
		/// </summary>
		/// <returns>The SQL statement</returns>
		public string GenerateSQL(string conjuction) 
		{
			// This is for debugging purposes
			string sql = "";

			try
			{
				IDbCommand cmd = _Load(conjuction);
				sql = cmd.CommandText;
			}
			catch(Exception) {}

			return sql;
		}

		/// <summary>
		/// Executes the Query, however, your BusinessEntity will not be loaded with the data from the query, instead, a DataReader is returned that will allow you 
		/// to iterate over the data. The default conjuction is "AND" between WHERE parameters.
		/// </summary>
		/// <returns>The DataReader</returns>
		public IDataReader ReturnReader()
		{
			return ReturnReader("AND");
		}

		/// <summary>
		/// Executes the Query, however, your BusinessEntity will not be loaded with the data from the query, instead, a DataReader is returned that will allow you 
		/// to iterate over the data. You can pass in the conjustion that will be used between the WHERE parameters, either, "AND" or "OR". "AND" is the default.
		/// </summary>
		/// <returns>The DataReader</returns>
		public IDataReader ReturnReader(string conjuction)
		{
			try
			{
				IDbCommand cmd = _Load(conjuction);

				cmd.Connection = this._entity.CreateIDbConnection();
				cmd.Connection.ConnectionString = _entity.RawConnectionString;
				cmd.Connection.Open();
				return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// This will limit the number of rows returned, after sorting. Setting Top to 10 will return the top ten rows after sorting.
		/// </summary>
		public int Top
		{
			get
			{
				return _top;
			}
			set
			{
				_top = value;
			}
		}


		/// <summary>
		/// Setting Distinct = True will elimate duplicate rows from the data.
		/// </summary>
		public bool Distinct
		{
			get
			{
				return _distinct;
			}
			set
			{
				_distinct = value;
			}
		}


		/// <summary>
		/// If true, add a COUNT(*) Aggregate to the selected columns list.
		/// </summary>
		public bool CountAll
		{
			get
			{
				return _countAll;
			}
			set
			{
				_countAll = value;
				
				if(value)
				{
					// We don't allow Save() to succeed once they reduce the columns
					this._entity._canSave = false;
				}
			}
		}

		/// <summary>
		/// If CountAll is set to true, use this to add a user-friendly column name.
		/// </summary>
		public string CountAllAlias
		{
			get
			{
				return _countAllAlias;
			}
			set
			{
				_countAllAlias = value;
			}
		}

		/// <summary>
		/// If true, add WITH ROLLUP to the GROUP BY clause.
		/// </summary>
		/// <example>
		/// <code>
		/// prds.Query.WithRollup = true;
		/// </code>
		/// </example>
		public bool WithRollup
		{
			get
			{
				return _withRollup;
			}
			set
			{
				_withRollup = value;
			}
		}

		/// <summary>
		/// NOTE: This is called by the dOOdad framework and you should never call it. We reserve the right to remove or change this method.
		/// </summary>
		/// <param name="wItem">The WhereParameter</param>
		public void AddWhereParameter(WhereParameter wItem)
		{
			if(_whereParameters == null)
			{
				_whereParameters = new ArrayList();
			}

			_whereParameters.Add(wItem);
		}

		/// <summary>
		/// This represents the number of WhereParameters contained within the Query.
		/// </summary>
		public int ParameterCount
		{
			get
			{
				int count = 0;

				if(_whereParameters != null)
				{
					count = _whereParameters.Count;
				}

				return count;
			}
		}

		/// <summary>
		/// NOTE: This is called by the dOOdad framework and you should never call it. We reserve the right to remove or change this method.
		/// </summary>
		/// <param name="wItem">The AggregateParameter</param>
		public void AddAggregateParameter(AggregateParameter wItem)
		{
			if(_aggregateParameters == null)
			{
				_aggregateParameters = new ArrayList();
			}

			_aggregateParameters.Add(wItem);
			
			// We don't allow Save() to succeed once they reduce the columns
			this._entity._canSave = false;
		}

		/// <summary>
		/// This represents the number of Aggregates contained within the Query.
		/// </summary>
		public int AggregateCount
		{
			get
			{
				int count = 0;

				if(_aggregateParameters != null)
				{
					count = _aggregateParameters.Count;
				}

				return count;
			}
		}

		/// <overloads>
		/// Use this to have your Query order the data. If you want to order the data by two columns you will need to call this method twice.
		/// </overloads>
		/// <summary>
		/// Use this to have your Query order the data. If you want to order the data by two columns you will need to call this method twice.
		/// </summary>
		/// <param name="column">This should be an entry from your ColumnNames class</param>
		/// <param name="direction">Either Descending or Ascending</param>
		/// <example>
		/// <code>
		/// emps.Query.AddOrderBy(Employees.ColumnNames.LastName, WhereParameter.Dir.ASC)</code>
		/// </example>
		public virtual void AddOrderBy(string column, WhereParameter.Dir direction)
		{
			if( _orderBy.Length > 0)
			{
				_orderBy += ", ";
			}

			_orderBy += column;

			if(direction == WhereParameter.Dir.ASC)
				_orderBy += " ASC";
			else
				_orderBy += " DESC";
		}

		/// <summary>
		/// Overloaded to let your Query order the data by COUNT(*).
		/// Used with Query.CountAll set to true.
		/// Derived classes implement this, like SqlClientDynamicQuery and OleDbDynamicQuery
		/// to account for differences in DBMS systems.
		/// </summary>
		/// <param name="countAll">This should be entity.Query</param>
		/// <param name="direction">Either Descending or Ascending</param>
		/// <example>
		/// <code>
		/// emps.Query.AddOrderBy(emps.Query, WhereParameter.Dir.ASC)</code>
		/// </example>
		public virtual void AddOrderBy(DynamicQuery countAll, WhereParameter.Dir direction)
		{
		}

		/// <summary>
		/// Overloaded to support aggregates.
		/// Derived classes implement this, like SqlClientDynamicQuery and OleDbDynamicQuery
		/// to account for differences in DBMS systems.
		/// </summary>
		/// <param name="aggregate">This should be an entry from your Aggregate class</param>
		/// <param name="direction">Either Descending or Ascending</param>
		/// <example>
		/// <code>
		/// emps.Query.AddOrderBy(emps.Aggregate.CategoryID, WhereParameter.Dir.ASC)</code>
		/// </example>
		public virtual void AddOrderBy(AggregateParameter aggregate, WhereParameter.Dir direction)
		{
		}

		/// <overloads>
		/// Use this to have your Query group the data. If you want to group the data by two columns you will need to call this method twice.
		/// </overloads>
		/// <summary>
		/// Use this to have your Query group the data. If you want to group the data by two columns you will need to call this method twice.
		/// If you call AddGroupBy, ANSI SQL requires an AddGroupBy for each AddResultColumn that is not an aggregate. Check your DBMS docs.
		/// </summary>
		/// <param name="column">This should be an entry from your ColumnNames class</param>
		/// <example>
		/// <code>
		/// emps.Query.AddGroupBy(Employees.ColumnNames.City)</code>
		/// </example>
		public virtual void AddGroupBy(string column)
		{
			if( _groupBy.Length > 0)
			{
				_groupBy += ", ";
			}
	
			_groupBy += column;
		}
	
		/// <summary>
		/// Overloaded to support aggregates.
		/// Derived classes implement this, like SqlClientDynamicQuery and OleDbDynamicQuery
		/// to account for differences in DBMS systems.
		/// </summary>
		/// <param name="aggregate">This should be an entry from your Aggregate class</param>
		/// <example>
		/// <code>
		/// emps.Query.AddGroupBy(emps.Aggregate.City)</code>
		/// </example>
		public virtual void AddGroupBy(AggregateParameter aggregate)
		{
		}
	
		/// <summary>
		/// NOTE: This is called by the dOOdad framework and you should never call it. We reserve the right to remove or change this method.
		/// </summary>
		public void FlushWhereParameters()
		{
			if( _whereParameters != null)
			{
				_whereParameters.Clear();
			}

			_orderBy = string.Empty;
		}

		/// <summary>
		/// NOTE: This is called by the dOOdad framework and you should never call it. We reserve the right to remove or change this method.
		/// </summary>
		public void FlushAggregateParameters()
		{
			if( _aggregateParameters != null)
			{
				_aggregateParameters.Clear();
			}

			_countAll = false;
			_countAllAlias = string.Empty;
			_groupBy = string.Empty;
		}

		/// <summary>
		/// The default result set for Query.Load is all of the columns in your Table or View. Once you call AddResultColumn this changes to only
		/// the columns you have added via this method. For instance, if you call AddResultColumn twice then only two columns will be returned
		/// in your result set. 
		/// </summary>
		/// <param name="columnName">This should be an entry from your ColumnNames class.</param>
		/// <example>
		/// VB.NET
		/// <code>
		/// Public Sub FillComboBox()
		///
        /// 	Dim prds As New Products
		///
        /// 	' Note we only bring back these two columns for performance reasons, why bring back more?
        /// 	prds.Query.AddResultColumn(prds.ColumnNames.ProductID)
        /// 	prds.Query.AddResultColumn(prds.ColumnNames.ProductName)
		///
        /// 	' Sort
        /// 	prds.Query.AddOrderBy(prds.ColumnNames.ProductName, MyGeneration.dOOdads.WhereParameter.Dir.ASC)
		/// 	
        /// 	' Load it
        /// 	prds.Query.Load()
		/// 	
        /// 	' Bind it 
        /// 	Me.cmbBox.DisplayMember = prds.ColumnNames.ProductName
        /// 	Me.cmbBox.DataSource    = prds.DefaultView
        /// 	End Sub
        /// </code>
		/// C#
		/// <code>
		/// public void FillComboBox()
		/// {
		/// 	Products prds = new Products();
		///
		/// 	// Note we only bring back these two columns for performance reasons, why bring back more?
		/// 	prds.Query.AddResultColumn(Products.ColumnNames.ProductID);
		/// 	prds.Query.AddResultColumn(Products.ColumnNames.ProductName);
		///
		/// 	// Sort
		/// 	prds.Query.AddOrderBy(Products.ColumnNames.ProductName, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
		/// 	
		/// 	// Load it
		/// 	prds.Query.Load();
		///
		/// 	// Bind it (there no combo box in this code, see demo)
		/// 	Me.cmbBox.DisplayMember = prds.ColumnNames.ProductName
		/// 	Me.cmbBox.DataSource    = prds.DefaultView
		/// }
		/// </code>
		/// </example>
		public virtual void AddResultColumn(string columnName)
		{
			if(_resultColumns.Length > 0)
			{
				_resultColumns += ", ";
			}

			_resultColumns += columnName;

			// We don't allow Save() to succeed once they reduce the columns
			this._entity._canSave = false;
		}

		/// <summary>
		/// Calling this will set the result columns back to "all".
		/// </summary>
		public void ResultColumnsClear()
		{
			_resultColumns = string.Empty;
		}

		/// <summary>
		/// A Query has a default conjuction between WHERE parameters, this method lets you intermix those and alternate between AND/OR.
		/// </summary>
		/// <param name="conjuction"></param>
		public void AddConjunction(WhereParameter.Conj conjuction)
		{
			if(_whereParameters == null) 
			{
				_whereParameters = new ArrayList();
			}

			if(conjuction != WhereParameter.Conj.UseDefault)
			{
				if(conjuction == WhereParameter.Conj.And)
					_whereParameters.Add(" AND ");
				else
					_whereParameters.Add(" OR ");
			}
		}

		/// <summary>
		/// Used for advanced queries
		/// </summary>
		public void OpenParenthesis()
		{
			if(_whereParameters == null)
			{
				_whereParameters = new ArrayList();
			}

			_whereParameters.Add("(");
		}

		/// <summary>
		/// Used for advanced queries
		/// </summary>
		public void CloseParenthesis()
		{
			if(_whereParameters == null)
			{
				_whereParameters = new ArrayList();
			}

			_whereParameters.Add(")");
		}

		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected bool _distinct = false;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected int _top = -1;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected bool _countAll = false;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected string _countAllAlias = string.Empty;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected bool _withRollup = false;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected ArrayList _whereParameters = null;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected ArrayList _aggregateParameters = null;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected string _resultColumns = string.Empty;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected string _orderBy = string.Empty;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected string _groupBy = string.Empty;

		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected BusinessEntity _entity;
		/// <summary>
		/// Used by derived classes
		/// </summary>
		protected int inc = 0;

		private string _lastQuery = "";
	}
}

