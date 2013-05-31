//==============================================================================
// MyGeneration.dOOdads
//
// SqlClientEntity.cs
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
using System.Data.SqlClient;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// SqlClientEntity is the Microsoft SQL implementation of BusinessEntity
	/// </summary>
	public class SqlClientEntity : BusinessEntity
	{
		public SqlClientEntity()
		{

		}

		override internal DynamicQuery CreateDynamicQuery(BusinessEntity entity)
		{
			return new SqlClientDynamicQuery(entity);
		}

		override internal IDataParameter CreateIDataParameter(string name, object value)
		{
			SqlParameter p = new SqlParameter();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		override internal IDataParameter CreateIDataParameter()
		{
			return new SqlParameter();
		}

		override internal IDbCommand CreateIDbCommand()
		{
			return new SqlCommand();
		}

		override internal IDbDataAdapter CreateIDbDataAdapter()
		{	
			return new SqlDataAdapter();
		}

		override internal IDbConnection CreateIDbConnection()
		{
			return new SqlConnection();
		}

		override internal DbDataAdapter ConvertIDbDataAdapter(IDbDataAdapter dataAdapter)
		{
			return (dataAdapter as SqlDataAdapter) as DbDataAdapter;
		}

		override internal IDbCommand _LoadFromRawSql(string rawSql, params object[] parameters)
		{
			int i = 0;
			string token  = "";
			string sIndex = "";
			string param  = "";

			SqlCommand cmd = new SqlCommand();

			foreach(object o in parameters)
			{
				sIndex = i.ToString();
				token = '{' + sIndex + '}';
				param = "@p" + sIndex;

				rawSql = rawSql.Replace(token, param);

				SqlParameter p = new SqlParameter(param, o);
				cmd.Parameters.Add(p);
				i++;
			}

			cmd.CommandText = rawSql;
			return cmd;
		}
	}
}
