//==============================================================================
// MyGeneration.dOOdads
//
// TransactionMgr.cs
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
using System.Data;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.Collections;

namespace MyGeneration.dOOdads
{
	/// <summary>
	/// TransactionMgr is used to seemlessly enroll BusinessEntity's  into a transaction. TransactionMgr uses
	/// ADO.NET transactions and therefore is not a distributed transaction as you would get with COM+. You only have
	/// to use TransactionMgr if two or more BusinessEntity's need to be saved as a transaction.  The BusinessEntity.Save
	/// method is already protected by a transaction.
	/// </summary>
	/// <remarks>
	///	Transaction Rules:
	/// <list type="bullet">
	///		<item>Your transactions paths do not have to be pre-planned. At any time you can begin a transaction</item>
	///		<item>You can nest BeginTransaction/CommitTransaction any number of times as long as they are sandwiched appropriately</item>
	///		<item>Once RollbackTransaction is called the transaction is doomed, nothing can be committed even it is attempted.</item>
	///		<item>Transactions are stored in the Thread Local Storage.</item>
	///	</list>
	/// Transactions are stored in the Thread Local Storage or
	/// TLS. This way the API isn't intrusive, ie, forcing you
	/// to pass a SqlConnection around everywhere.  There is one
	/// thing to remember, once you call RollbackTransaction you will
	/// be unable to commit anything on that thread until you
	/// call ThreadTransactionMgrReset().
	/// 
	/// In an ASP.NET application each page is handled by a thread
	/// that is pulled from a thread pool. Thus, you need to clear
	/// out the TLS (thread local storage) before your page begins
	/// execution. The best way to do this is to create a base page
	/// that inhertis from System.Web.UI.Page and clears the state
	/// like this:	
	///	</remarks>
	///	<example>
	/// VB.NET
	/// <code>
	/// Dim tx As TransactionMgr
	/// tx = TransactionMgr.ThreadTransactionMgr()
	/// 
	/// Try
	/// 	tx.BeginTransaction()
	/// 	emps.Save()
	/// 	prds.Save()
	/// 	tx.CommitTransaction()
	/// Catch ex As Exception
	/// 	tx.RollbackTransaction()
	/// 	tx.ThreadTransactionMgrReset()
	/// End Try
	/// </code>
	/// C#
	/// <code>
	/// TransactionMgr tx = TransactionMgr.ThreadTransactionMgr();
	/// 
	/// try
	/// {
	/// 	tx.BeginTransaction();
	/// 	emps.Save();
	/// 	prds.Save();
	/// 	tx.CommitTransaction();
	/// }
	/// catch(Exception ex)
	/// {
	/// 	tx.RollbackTransaction();
	/// 	tx.ThreadTransactionMgrReset();
	/// }
	/// </code>
	/// </example>
	public class TransactionMgr
	{
		/// <summary>
		/// You cannot new an instance of the TransactionMgr class, see the static method <see cref="ThreadTransactionMgr"/>
		/// </summary>
		protected TransactionMgr()
		{

		}

		/// <summary>
		/// Returns the number of outstanding calls to <see cref="BeginTransaction"/> without subsequent calls to 
		/// <see cref="CommitTransaction"/>
		/// </summary>
		public int NestingCount
		{
			get
			{
				return this.txCount;
			}
		}

		/// <summary>
		/// True if <see cref="RollbackTransaction"/> has been called on this thread. 
		/// </summary>
		public bool HasBeenRolledBack
		{
			get
			{
				return hasRolledBack;
			}
		}

		/// <summary>
		/// BeginTransaction should always be a followed by a call to CommitTransaction if all goes well, or
		/// RollbackTransaction if problems are detected.  BeginTransaction() can be nested any number of times
		/// as long as each call is unwound with a call to CommitTransaction().
		/// </summary>
		public void BeginTransaction()
		{
			if( hasRolledBack) throw new Exception("Transaction Rolledback");

			txCount = txCount + 1;
		}

		/// <summary>
		/// The final call to CommitTransaction commits the transaction to the database, BeginTransaction and
		/// CommitTransaction calls can be nested, <see cref="BeginTransaction"/>
		/// </summary>
		public void CommitTransaction()
		{
			if(hasRolledBack) throw new Exception("Transaction Rolledback");

			txCount = txCount - 1;

			if(txCount == 0)
			{
				foreach(Transaction tx in this.transactions.Values)
				{
                    tx.sqlTx.Commit();
                    tx.Dispose();
				}

				this.transactions.Clear();

				if(this.objectsInTransaction != null)
				{
					try
					{
						foreach(BusinessEntity entity in this.objectsInTransaction)
						{
							entity.AcceptChanges();
						}
					} 
					catch {}

					this.objectsInTransaction = null;
				}
			}
		}

		/// <summary>
		/// RollbackTransaction dooms the transaction regardless of nested calls to BeginTransaction. Once this method is called
		/// nothing can be done to commit the transaction.  To reset the thread state a call to <see cref="ThreadTransactionMgrReset"/> must be made.
		/// You must call 
		/// </summary>
		public void RollbackTransaction()
		{
			if(false == hasRolledBack && txCount > 0)
			{
				foreach(Transaction tx in this.transactions.Values)
				{
					tx.sqlTx.Rollback();
                    tx.Dispose();
				}

                this.transactions.Clear();
                this.txCount = 0;
				this.objectsInTransaction = null;
			}
		}

		/// <summary>
		/// Enlist by the dOOdads architecture when a IDbCommand (SqlCommand is an IDbCommand). The command may or may not be enrolled 
		/// in a transaction depending on whether or not BeginTransaction has been called. Each call to Enlist must be followed by a
		/// call to <see cref="DeEnlist"/>.
		/// </summary>
		/// <param name="cmd">Your SqlCommand, OleDbCommand, etc ...</param>
		/// <param name="entity">Your business entity, in C# use 'this', VB.NET use 'Me'.</param>
		/// <example>
		/// C#
		/// <code>
		/// txMgr.Enlist(cmd, this);
		/// cmd.ExecuteNonQuery();
		/// txMgr.DeEnlist(cmd, this);
		/// </code>
		/// VB.NET
		/// <code>
		/// txMgr.Enlist(cmd, Me)
		/// cmd.ExecuteNonQuery()
		/// txMgr.DeEnlist(cmd, Me)
		/// </code>
		/// </example>
		public void Enlist(IDbCommand cmd, BusinessEntity entity)
		{
			if(txCount == 0 || entity._notRecommendedConnection != null)
			{
				// NotRecommendedConnections never play in dOOdad transactions
				cmd.Connection = CreateSqlConnection(entity);
			}
			else
			{
				string connStr = entity._config;
				if(entity._raw != "") connStr = entity._raw;

				Transaction tx = this.transactions[connStr] as Transaction;

				if(tx == null)
				{
                    IDbConnection sqlConn = CreateSqlConnection(entity);
                    
                    tx = new Transaction();
					tx.sqlConnection = sqlConn;

					if(_isolationLevel != IsolationLevel.Unspecified)
					{
						tx.sqlTx = sqlConn.BeginTransaction(_isolationLevel);
					}
					else
					{
						tx.sqlTx = sqlConn.BeginTransaction();
					}
					this.transactions[connStr] = tx;
				}

                cmd.Connection = tx.sqlConnection;
				cmd.Transaction = tx.sqlTx;
			}
		}

		/// <summary>
		/// Each call to Enlist must be followed eventually by a call to DeEnlist.  
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="entity"></param>
		/// <example>
		/// C#
		/// <code>
		/// txMgr.Enlist(cmd, this);
		/// cmd.ExecuteNonQuery();
		/// txMgr.DeEnlist(cmd, this); 
		/// </code>
		/// VB.NET
		/// <code>>
		/// txMgr.Enlist(cmd, Me)
		/// cmd.ExecuteNonQuery()
		/// txMgr.DeEnlist(cmd, Me)
		/// </code>
		/// </example>
		public void DeEnlist(IDbCommand cmd, BusinessEntity entity)
		{
			if(entity._notRecommendedConnection != null)
			{
				// NotRecommendedConnection never play in dOOdad transactions
				cmd.Connection = null;
			}
			else
			{
				if(txCount == 0)
				{
					cmd.Connection.Dispose();
				}
			}
		}

		/// <summary>
		/// Called internally by BusinessEntity
		/// </summary>
		/// <param name="entity"></param>
		internal void AddBusinessEntity(BusinessEntity entity)
		{
			if(this.objectsInTransaction == null)
			{
				this.objectsInTransaction = new ArrayList();
			}

			this.objectsInTransaction.Add(entity);
		}

		private IDbConnection CreateSqlConnection(BusinessEntity entity)
		{
			IDbConnection cn;

			if(entity._notRecommendedConnection != null)
			{
				// This is assumed to be open
				cn = entity._notRecommendedConnection;
			}
			else
			{
				cn = entity.CreateIDbConnection();

				if(entity._raw != "")
					cn.ConnectionString = entity._raw;
				else
#if(VS2005)
					cn.ConnectionString = ConfigurationManager.AppSettings[entity._config];
#else
                    cn.ConnectionString = ConfigurationSettings.AppSettings[entity._config];
#endif

				cn.Open();
			}

			return cn;
		}

		// We might have multple transactions going at the same time.
		// There's one per connnection string
		private class Transaction : IDisposable
        {
            public IDbTransaction sqlTx = null;
            public IDbConnection sqlConnection = null;

            #region IDisposable Members
            public void Dispose()
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }

                if (sqlTx != null)
                {
                    sqlTx.Dispose();
                }
            }
            #endregion
        }

        private Hashtable transactions = new Hashtable();
		private int txCount = 0;
		private bool hasRolledBack  = false;

		// Used to control AcceptChanges()
		internal ArrayList objectsInTransaction = null;

		#region "static"
		/// <summary>
		/// This static method is how you obtain a reference to the TransactionMgr. You cannot call "new" on TransactionMgr.
		/// If a TransactionMgr doesn't exist on the current thread, one is created and returned to you.
		/// </summary>
		/// <returns>The one and only TransactionMgr for this thread.</returns>
		public static TransactionMgr ThreadTransactionMgr()
		{
			TransactionMgr txMgr = null;

			object obj = Thread.GetData(txMgrSlot);

			if(obj != null)
			{
				txMgr = (TransactionMgr)obj;
			}
			else
			{
				txMgr = new TransactionMgr();
				Thread.SetData(txMgrSlot, txMgr);
			}

			return txMgr;
		}

		/// <summary>
		/// This must be called after RollbackTransaction or no futher database activity will happen successfully on the current thread.
		/// </summary>
		public static void ThreadTransactionMgrReset()
		{
			TransactionMgr txMgr = TransactionMgr.ThreadTransactionMgr();

			try
			{
				if(txMgr.txCount > 0 && txMgr.hasRolledBack == false)
				{
					txMgr.RollbackTransaction();
				}
			}
			catch {}

			Thread.SetData(txMgrSlot, null);
		}

		/// <summary>
		/// This is the Transaction's strength. The default is "IsolationLevel.Unspecified, the strongest is "IsolationLevel.Serializable" which is what
		/// is recommended for serious enterprize level projects.
		/// </summary>
		public static IsolationLevel IsolationLevel
		{
			get
			{
				return _isolationLevel;
			}

			set
			{
				_isolationLevel = value;
			}
		}

        private static IsolationLevel _isolationLevel = IsolationLevel.Unspecified;
		private static LocalDataStoreSlot txMgrSlot = Thread.AllocateDataSlot();
		#endregion

	}
}
