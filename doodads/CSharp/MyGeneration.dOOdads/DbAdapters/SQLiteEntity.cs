//==============================================================================
// MyGeneration.dOOdads
//
// SQLiteEntity.cs
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
using Finisar.SQLite;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// VistaDBEntity is the VistaDB implementation of BusinessEntity
	/// </summary>
	public class SQLiteEntity : BusinessEntity
	{
		public SQLiteEntity()
		{

		}

		override internal DynamicQuery CreateDynamicQuery(BusinessEntity entity)
		{
			return new SQLiteDynamicQuery(entity);
		}

		override internal IDataParameter CreateIDataParameter(string name, object value)
		{
			SQLiteParameter p = new SQLiteParameter();
			p.ParameterName = name;
			p.Value = value;
			return p;
		}

		override internal IDataParameter CreateIDataParameter()
		{
			return new SQLiteParameter();
		}

		override internal IDbCommand CreateIDbCommand()
		{
			return new SQLiteCommand();
		}

		override internal IDbDataAdapter CreateIDbDataAdapter()
		{	
			return new SQLiteDataAdapter();
		}

		override internal IDbConnection CreateIDbConnection()
		{
			return new SQLiteConnection();
		}

		override internal DbDataAdapter ConvertIDbDataAdapter(IDbDataAdapter dataAdapter)
		{
			return (dataAdapter as SQLiteDataAdapter) as DbDataAdapter;
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
				SQLiteDataAdapter da = adapter as SQLiteDataAdapter;
				da.RowUpdated += new SQLiteRowUpdatedEventHandler(OnRowUpdated);
			}
		}

		// If it's an Insert we fetch the @@Identity value and stuff it in the proper column
		protected void OnRowUpdated(object sender, RowUpdatedEventArgs e)
		{
			try
			{
				if(e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
				{
					TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

					string[] identityCols = this.GetAutoKeyColumns().Split(';');

					SQLiteCommand cmd = new SQLiteCommand();

					foreach(string col in identityCols)
					{
						 cmd.CommandText = "SELECT last_insert_rowid()";

						// We make sure we enlist in the ongoing transaction, otherwise, we 
						// would most likely deadlock
						txMgr.Enlist(cmd, this);
						object o = cmd.ExecuteScalar(); // Get the Identity Value
						txMgr.DeEnlist(cmd, this);

						if(o != null)
						{
							e.Row[col] = o;
						}
					}

					e.Row.AcceptChanges();
				}
			}
			catch {}
		}
		
		#endregion
	}
}
