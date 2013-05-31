//==============================================================================
// MyGeneration.dOOdads
//
// AggregateParameter.cs
// Version 5.0
// Updated - 10/08/2005
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

using System.Collections;
using System.Data;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// This class is dynamcially created when you add an AggregateParameter to your BusinessEntity's DynamicQuery (See the BusinessEntity.Query Property).
	/// </summary>
	/// <remarks>
	/// Aggregate and GROUP BY Feature Support by DBMS:
	/// <code>
	///                 MS    MS    My    SQ    Vista Fire  Post
	/// Feature         SQL   Acces SQL   Lite  DB    bird  gre   Oracle  Ads
	/// --------------- ----- ----- ----- ----- ----- ----- ----- ------ -----
	/// Avg              Y     Y     Y     Y     Y     Y     Y     Y       Y
	/// Count            Y     Y     Y     Y     Y     Y     Y     Y       Y
	/// Max              Y     Y     Y     Y     Y     Y     Y     Y       Y
	/// Min              Y     Y     Y     Y     Y     Y     Y     Y       Y
	/// Sum              Y     Y     Y     Y     Y     Y     Y     Y       Y
	/// StdDev           Y     Y     Y     -     Y     -     Y    (4)      -
	/// Var              Y     Y     Y     -     -     -     Y     Y       -
	/// Aggregate in
	///   ORDER BY       Y     Y    (1)    Y    (3)    Y     Y     Y       Y
	///   GROUP BY       -     -     -     Y    (3)    Y     Y     Y       -
	/// WITH ROLLUP      Y     -    (2)    -     Y     -     -     Y       -
	/// COUNT(DISTINCT)  Y     -     Y     -     Y     Y     Y     Y       Y
	/// 
	/// Notes:
	///   (1) - MySQL - accepts an aggregate's alias in an
	///         ORDER BY clause.
	///   (2) - MySQL - WITH ROLLUP and ORDER BY are mutually
	///         exclusive
	///   (3) - VistaDB - will not ORDER BY or GROUP BY 'COUNT(*)' 
	///         the rest works fine.   
	///   (4) - Uses TRUNC(STDDEV(column),10) to avoid overflow errors
	///   
	/// </code>
	/// This will be the extent of your use of the AggregateParameter class, this class is mostly used by the dOOdad architecture, not the programmer.
	/// <code>
	/// prds  = new Products();
	///
	/// // To include a COUNT(*) with NULLs included
	/// prds.Query.CountAll = true;
	/// prds.Query.CountAllAlias = "Product Count";
	///
	/// // To exclude NULLs in the COUNT for a column
	/// prds.Aggregate.UnitsInStock.Function = AggregateParameter.Func.Count;
	/// prds.Aggregate.UnitsInStock.Alias = "With Stock";
	///
	/// // To have two aggregates for the same column, use a tearoff
	/// AggregateParameter ap = prds.Aggregate.TearOff.UnitsInStock;
	/// ap.Function = AggregateParameter.Func.Sum;
	/// ap.Alias = "Total Units";
	///
	/// prds.Aggregate.ReorderLevel.Function = AggregateParameter.Func.Avg;
	/// prds.Aggregate.ReorderLevel.Alias = "Avg Reorder";
	///
	/// prds.Aggregate.UnitPrice.Function = AggregateParameter.Func.Min;
	/// prds.Aggregate.UnitPrice.Alias = "Min Price";
	///
	/// ap = prds.Aggregate.TearOff.UnitPrice;
	/// ap.Function = AggregateParameter.Func.Max;
	/// ap.Alias = "Max Price";
	///
	/// // If you have no aggregates or AddResultColumns,
	/// // Then the query defaults to SELECT *
	/// // If you have an aggregate and no AddResultColumns,
	/// // Then only aggregates are reurned in the query.
	/// prds.Query.AddResultColumn(Products.ColumnNames.CategoryID);
	/// prds.Query.AddResultColumn(Products.ColumnNames.Discontinued);
	///
	/// // If you have an Aggregate, ANSI SQL requires an AddGroupBy
	/// // for each AddResultColumn. Check your DBMS docs.
	/// prds.Query.AddGroupBy(Products.ColumnNames.CategoryID);
	/// prds.Query.AddGroupBy(Products.ColumnNames.Discontinued);
	///
	/// prds.Query.AddOrderBy(Products.ColumnNames.Discontinued, WhereParameter.Dir.ASC);
	/// 
	/// // You can use aggregates in AddOrderBy by
	/// // referencing either the entity AggregateParameter or a tearoff
	/// // You must create the aggregate before using it here.
	/// prds.Query.AddOrderBy(prds.Aggregate.UnitsInStock, WhereParameter.Dir.DESC);
	/// 
	/// // Load it.
	/// prds.Query.Load();
	/// </code>
	/// </remarks>
	public class AggregateParameter
	{
		/// <summary>
		/// The aggregate function used by Aggregate.Function
		/// </summary>
		public enum Func
		{
			/// <summary>
			/// Average
			/// </summary>
			Avg = 1,
			/// <summary>
			/// Count
			/// </summary>
			Count,
			/// <summary>
			/// Maximum
			/// </summary>
			Max,
			/// <summary>
			/// Minimum
			/// </summary>
			Min,
			/// <summary>
			/// Standard Deviation
			/// </summary>
			StdDev,
			/// <summary>
			/// Variance
			/// </summary>
			Var,
			/// <summary>
			/// Sum
			/// </summary>
			Sum
		};

		/// <summary>
		/// This is only called by dOOdads architecture.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="param"></param>
		public AggregateParameter(string column, IDataParameter param)
		{
			this._column = column;
			this._alias = column;
			this._distinct = false;
			this._param = param;
			this._function = AggregateParameter.Func.Sum;
		}

		/// <summary>
		/// Used to determine if the AggregateParameter has a value
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}
		}

		/// <summary>
		/// The column in the BusinessEntity that this AggregateParameter is going to query against. 
		/// </summary>
		public string Column
		{
			get
			{
				return _column;
			}
		}

		/// <summary>
		/// The actual database Parameter 
		/// </summary>
		public IDataParameter Param
		{
			get
			{
				return _param;
			}
		}

		/// <summary>
		/// The value that will be placed into the Parameter
		/// </summary>
		public object Value
		{
			get
			{
				return _value;
			}

			set
			{
				_value = value;
				_isDirty = true;
			}
		}

		/// <summary>
		/// The type of aggregate function desired.
		/// Avg, Count, Min, Max, Sum, StdDev, or Var.
		/// (See AggregateParameter.Func enumeration.)
		/// </summary>
		public Func Function
		{
			get
			{
				return _function;
			}

			set
			{
				_function = value;
				_isDirty = true;
			}
		}

		/// <summary>
		/// The user-friendly name of the aggregate column
		/// </summary>
		public string Alias
		{
			get
			{
				return _alias;
			}

			set
			{
				_alias = value;
				_isDirty = true;
			}
		}

		/// <summary>
		/// If true, then use (DISTINCT columnName) in the aggregate.
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
				_isDirty = true;
			}
		}

		private object _value = null;
		private IDataParameter _param;
		private string _column;
		private Func _function = AggregateParameter.Func.Sum;
		private string _alias = string.Empty;
		private bool _isDirty = false;
		private bool _distinct = false;
	}
}
