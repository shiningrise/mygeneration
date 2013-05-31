//==============================================================================
// MyGeneration.dOOdads
//
// MySql4Entity.cs
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
using MySql.Data.MySqlClient;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// VistaDBEntity is the VistaDB implementation of BusinessEntity
	/// </summary>
	public class MySql4Entity : BusinessEntity
	{
		public MySql4Entity()
		{

		}

		override internal DynamicQuery CreateDynamicQuery(BusinessEntity entity)
		{
			return new MySql4DynamicQuery(entity);
		}

		override internal IDataParameter CreateIDataParameter(string name, object value)
		{
			MySqlParameter p = new MySqlParameter();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		override internal IDataParameter CreateIDataParameter()
		{
			return new MySqlParameter();
		}

		override internal IDbCommand CreateIDbCommand()
		{
			return new MySqlCommand();
		}

		override internal IDbDataAdapter CreateIDbDataAdapter()
		{	
			return new MySqlDataAdapter();
		}

		override internal IDbConnection CreateIDbConnection()
		{
			return new MySqlConnection();
		}

		override internal DbDataAdapter ConvertIDbDataAdapter(IDbDataAdapter dataAdapter)
		{
			return (dataAdapter as MySqlDataAdapter) as DbDataAdapter;
		}

		override public void AddNew()
		{
			if(this.DataTable == null)
			{
				this.LoadFromSql("SELECT * FROM `" +  QuerySource + "` WHERE 1=0", null, CommandType.Text);
			}

			DataRow newRow = this.DataTable.NewRow();
			this.DataTable.Rows.Add(newRow);
			this.DataRow = newRow;
		}

		override internal IDbCommand _LoadFromRawSql(string rawSql, params object[] parameters)
		{
			int i = 0;
			string token  = "";
			string sIndex = "";
			string param  = "";

			MySqlCommand cmd = new MySqlCommand();

			foreach(object o in parameters)
			{
				sIndex = i.ToString();
				token = '{' + sIndex + '}';
				param = "?p" + sIndex;

				rawSql = rawSql.Replace(token, param);

				MySqlParameter p = new MySqlParameter(param, o);
				cmd.Parameters.Add(p);
				i++;
			}

			cmd.CommandText = rawSql;
			return cmd;
		}

		#region LastIdentity Logic

		// Overloaded in the generated class
		public virtual string GetAutoKeyColumns()
		{
			return "";
		}

		override protected void HookupRowUpdateEvents(DbDataAdapter adapter)
		{
			// We only bother hooking up the event if we have an AutoKey
			if(this.GetAutoKeyColumns().Length > 0)
			{
				MySqlDataAdapter da = adapter as MySqlDataAdapter;
				da.RowUpdated += new MySqlRowUpdatedEventHandler(OnRowUpdated);
			}
		}

		// If it's an Insert we fetch the @@Identity value and stuff it in the proper column
		protected void OnRowUpdated(object sender, MySqlRowUpdatedEventArgs e)
		{
			try
			{
				if(e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
				{
					TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

					string identityCol = this.GetAutoKeyColumns();

					MySqlCommand cmd = new MySqlCommand();

					cmd.CommandText = "SELECT LAST_INSERT_ID();";

					// We make sure we enlist in the ongoing transaction, otherwise, we 
					// would most likely deadlock
					txMgr.Enlist(cmd, this);
					object o = cmd.ExecuteScalar(); // Get the Identity Value
					txMgr.DeEnlist(cmd, this);

					if(o != null)
					{
						e.Row[identityCol] = o;
					}

					e.Row.AcceptChanges();
				}
			}
			catch {}
		}
		
		#endregion
	}
}
