using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;

namespace MyGeneration.dOOdads
{
	public class VisualFoxProEntity : BusinessEntity
	{
		public VisualFoxProEntity() { }

		override internal DynamicQuery CreateDynamicQuery(BusinessEntity entity)
		{
			return new VisualFoxProDynamicQuery(entity);
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

		// Overloaded in the generated class
		public virtual string GetAutoKeyColumn()
		{
			return "";
		}

		// Called just before the Save() is truly executed
		override protected void HookupRowUpdateEvents(DbDataAdapter adapter)
		{
			// We only bother hooking up the event if we have an AutoKey
			if (this.GetAutoKeyColumn().Length > 0)
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
				if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
				{
					TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

					OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY");

					// We make sure we enlist in the ongoing transaction, otherwise, we 
					// would most likely deadlock
					txMgr.Enlist(cmd, this);
					object o = cmd.ExecuteScalar(); // Get the Identity Value
					txMgr.DeEnlist(cmd, this);

					if (o != null)
					{
						e.Row[this.GetAutoKeyColumn()] = o;
						e.Row.AcceptChanges();
					}
				}
			}
			catch { }
		}

		override internal IDbCommand _LoadFromRawSql(string rawSql, params object[] parameters)
		{
			int i = 0;
			string token = "";
			string sIndex = "";
			string param = "";

			OleDbCommand cmd = new OleDbCommand();

			foreach (object o in parameters)
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