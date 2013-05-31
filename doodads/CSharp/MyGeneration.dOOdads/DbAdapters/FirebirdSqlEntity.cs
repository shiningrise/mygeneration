//==============================================================================
// MyGeneration.dOOdads
//
// FirebirdSqlEntity.cs
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
using System.Data;
using System.Data.Common;

using FirebirdSql.Data.Firebird;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// FirebirdSqlEntity is the Firebird implementation of BusinessEntity
	/// </summary>
	public class FirebirdSqlEntity  : BusinessEntity
	{
		public FirebirdSqlEntity()
		{

		}

		override internal DynamicQuery CreateDynamicQuery(BusinessEntity entity)
		{
			return new FirebirdSqlDynamicQuery(entity);
		}

		override internal IDataParameter CreateIDataParameter(string name, object value)
		{
			FbParameter p = new FbParameter();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		override internal IDataParameter CreateIDataParameter()
		{
			return new FbParameter();
		}

		override internal IDbCommand CreateIDbCommand()
		{
			return new FbCommand();
		}

		override internal IDbDataAdapter CreateIDbDataAdapter()
		{	
			return new FbDataAdapter();
		}

		override internal IDbConnection CreateIDbConnection()
		{
			return new FbConnection();
		}

		override internal DbDataAdapter ConvertIDbDataAdapter(IDbDataAdapter dataAdapter)
		{
			return (dataAdapter as FbDataAdapter) as DbDataAdapter;
		}

		override public void AddNew()
		{
			if(this.DataTable == null)
			{
				this.LoadFromSql("SELECT * FROM " +  D(QuerySource) + " WHERE 1 = 0", null, CommandType.Text);
			}

			DataRow newRow = this.DataTable.NewRow();
			this.DataTable.Rows.Add(newRow);
			this.DataRow = newRow;
		}

		#region Dialect
		private string D(string text)
		{
			if(_dialect == 3)
			{
				text = '"' + text + '"';
			}

			return text;
		}

		protected int Dialect
		{
			get	{ return _dialect;	}
			set	{ _dialect = value;	}
		}

		internal int _dialect = 1;
		#endregion

		override internal IDbCommand _LoadFromRawSql(string rawSql, params object[] parameters)
		{
			int i = 0;
			string token  = "";
			string sIndex = "";
			string param  = "";

			FbCommand cmd = new FbCommand();

			foreach(object o in parameters)
			{
				sIndex = i.ToString();
				token = '{' + sIndex + '}';
				param = "@p" + sIndex;

				rawSql = rawSql.Replace(token, param);

				FbParameter p = new FbParameter(param, o);
				cmd.Parameters.Add(p);
				i++;
			}

			cmd.CommandText = rawSql;
			return cmd;
		}
	}
}
