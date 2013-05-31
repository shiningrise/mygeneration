//==============================================================================
// MyGeneration.dOOdads
//
// WhereParameter.cs
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

using System.Collections;
using System.Data;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// This class is dynamcially created when you add a WhereParameter to your BusiinessEntity's DynamicQuery (See the BusinessEntity.Query Property).
	/// </summary>
	/// <remarks>
	/// This will be the extent of your use of the WhereParameter class, this class is mostly used by the dOOdad architecture, not the programmer.
	/// <code>
	/// emps.Where.LastName.Value = "%A%";
    /// emps.Where.LastName.Operator = WhereParameter.Operand.Like;
	/// </code>
	/// </remarks>
	public class WhereParameter
	{
		/// <summary>
		/// The type of comparison this parameter shoud use
		/// </summary>
		public enum Operand
		{
			/// <summary>
			/// Equal Comparison
			/// </summary>
			Equal = 1,
			/// <summary>
			/// Not Equal Comparison
			/// </summary>
			NotEqual,
			/// <summary>
			/// Greater Than Comparison
			/// </summary>
			GreaterThan,
			/// <summary>
			/// Greater Than or Equal Comparison
			/// </summary>
			GreaterThanOrEqual,
			/// <summary>
			/// Less Than Comparison
			/// </summary>
			LessThan,
			/// <summary>
			/// Less Than or Equal Comparison
			/// </summary>
			LessThanOrEqual,
			/// <summary>
			/// Like Comparison, "%s%" does it have an 's' in it? "s%" does it begin with 's'?
			/// </summary>
			Like,
			/// <summary>
			/// Is the value null in the database
			/// </summary>
			IsNull,
			/// <summary>
			/// Is the value non-null in the database
			/// </summary>
			IsNotNull,
			/// <summary>
			/// Is the value between two parameters? see <see cref="BetweenBeginValue"/> and <see cref="BetweenEndValue"/>. 
			/// Note that Between can be for other data types than just dates.
			/// </summary>
			Between,
			/// <summary>
			/// Is the value in a list, ie, "4,5,6,7,8"
			/// </summary>
			In,
			/// <summary>
			/// NOT in a list, ie not in, "4,5,6,7,8"
			/// </summary>
			NotIn,
			/// <summary>
			/// Not Like Comparison, "%s%", anything that does not it have an 's' in it.
			/// </summary>
			NotLike
		};

		/// <summary>
		/// The direction used by DynamicQuery.AddOrderBy
		/// </summary>
		public enum Dir
		{
			/// <summary>
			/// Ascending
			/// </summary>
			ASC = 1,
			/// <summary>
			/// Descending
			/// </summary>
			DESC
		};

		/// <summary>
		/// The conjuction used between WhereParameters
		/// </summary>
		public enum Conj
		{
			/// <summary>
			/// WhereParameters are joined via "And"
			/// </summary>
			And = 1,
			/// <summary>
			/// WhereParameters are joined via "Or"
			/// </summary>
			Or,
			/// <summary>
			/// WhereParameters are used via the default passed into DynamicQuery.Load.
			/// </summary>
			UseDefault
		};

		/// <summary>
		/// This is only called by dOOdads architecture.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="param"></param>
		public WhereParameter(string column, IDataParameter param)
		{
			this._column = column;
			this._param = param;
			this._operator = Operand.Equal;
		}

		/// <summary>
		/// Used to determine if the WhereParameters has a value
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}
		}

		/// <summary>
		/// The column in the BusinessEntity that this WhereParameter is going to query against. 
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
		/// The type of comparison desired
		/// </summary>
		public Operand Operator
		{
			get
			{
				return _operator;
			}

			set
			{
				_operator = value;
				_isDirty = true;
			}
		}

		/// <summary>
		/// The type of conjuction to use, "AND" or "OR"
		/// </summary>
		public Conj Conjuction
		{
			get
			{
				return _conjuction;
			}

			set
			{
				_conjuction = value;
				_isDirty = true;
			}
		}

		/// <summary>
		/// Used when use the Operand.Between comparison
		/// </summary>
		public object BetweenBeginValue
		{
			get
			{
				return _betweenBegin;
			}

			set
			{
				_betweenBegin = value;
				_isDirty = true;
			}
		}

		/// <summary>
		/// Used when use the Operand.Between comparison
		/// </summary>
		public object BetweenEndValue
		{
			get
			{
				return _betweenEnd;
			}

			set
			{
				_betweenEnd = value;
				_isDirty = true;
			}
		}

		private Operand _operator;
		private Conj _conjuction = Conj.UseDefault;
		private object _value = null;
		private string _column;
		private IDataParameter _param;
		private bool _isDirty = false;

		private object _betweenBegin = null;
		private object _betweenEnd = null;
	}
}
