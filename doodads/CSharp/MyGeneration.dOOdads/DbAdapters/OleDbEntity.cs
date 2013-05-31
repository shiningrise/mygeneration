//==============================================================================
// MyGeneration.dOOdads
//
// OleDbEntity.cs
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
using System.Data.OleDb;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// OleDbEntity is the Microsoft Access implementation of BusinessEntity
	/// </summary>
	public class OleDbEntity : BusinessEntity
	{
		public OleDbEntity()
		{

		}

		override internal DynamicQuery CreateDynamicQuery(BusinessEntity entity)
		{
			return new OleDbDynamicQuery(entity);
		}

		override internal IDataParameter CreateIDataParameter(string name, object value)
		{
			OleDbParameter p = new OleDbParameter();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		override internal IDataParameter CreateIDataParameter()
		{
			return new OleDbParameter();
		}

		override internal IDbCommand CreateIDbCommand()
		{
			return new OleDbCommand();
		}

		override internal IDbDataAdapter CreateIDbDataAdapter()
		{	
			return new OleDbDataAdapter();
		}

		override internal IDbConnection CreateIDbConnection()
		{
			return new OleDbConnection();
		}

		override internal DbDataAdapter ConvertIDbDataAdapter(IDbDataAdapter dataAdapter)
		{
			return (dataAdapter as OleDbDataAdapter) as DbDataAdapter;
		}

		#region @@IDENTITY Logic

		// Overloaded in the generated class
		public virtual string GetAutoKeyColumn()
		{
			return "";
		}
		
		// Called just before the Save() is truly executed
		override protected void HookupRowUpdateEvents(DbDataAdapter adapter)
		{
			// We only bother hooking up the event if we have an AutoKey
			if(this.GetAutoKeyColumn().Length > 0)
			{
				OleDbDataAdapter da = adapter as OleDbDataAdapter;
				da.RowUpdated += new OleDbRowUpdatedEventHandler(OnRowUpdated);
			}
		}

		// If it's an Insert we fetch the @@Identity value and stuff it in the proper column
		protected void OnRowUpdated(object sender, OleDbRowUpdatedEventArgs e)
		{
			try
			{
				if(e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
				{
					TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

					OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY");

					// We make sure we enlist in the ongoing transaction, otherwise, we 
					// would most likely deadlock
					txMgr.Enlist(cmd, this);
					object o = cmd.ExecuteScalar(); // Get the Identity Value
					txMgr.DeEnlist(cmd, this);

					if(o != null)
					{
						e.Row[this.GetAutoKeyColumn()] = o;
						e.Row.AcceptChanges();
					}
				}
			}
			catch {}
		}
		#endregion

		override internal IDbCommand _LoadFromRawSql(string rawSql, params object[] parameters)
		{
			int i = 0;
			string token  = "";
			string sIndex = "";
			string param  = "";

			OleDbCommand cmd = new OleDbCommand();

			foreach(object o in parameters)
			{
				sIndex = i.ToString();
				token = '{' + sIndex + '}';
				param = "@p" + sIndex;

				rawSql = rawSql.Replace(token, param);

				OleDbParameter p = new OleDbParameter(param, o);
				cmd.Parameters.Add(p);
				i++;
			}

			cmd.CommandText = rawSql;
			return cmd;
		}
	}
}
