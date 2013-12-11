using System;
using System.Data;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace MyMeta.Plugins.Emdb
{
    /// <summary>
    /// Dummy DB-Connection for MyMeta.Plugins.Xsd3b, required by MyGen api
    /// 
    /// (c) 2007 by dl3bak@qsl.net
    /// 
    /// MyGenXsd3b ("MyMeta.Plugins.Xsd3b.dll") is a provider plugin 
    ///     for MyGeneration that allows to use XSDFile s (*.xsd, *.xsd3b) 
    ///     as datasource instead of an online sqldatabase.
    /// 
    /// MyGeneration from http://www.mygenerationsoftware.com is a template 
    ///     driven sourcecodegenerator that you can use free of charge. 
    /// It uses the RelationalDatastructure of a sql database 
    ///         (ie Microsoft SQL, Oracle, IBM DB2, MySQL, PostgreSQL, 
    ///         Microsoft Access, FireBird, Interbase, SQLite, ...) 
    ///         and produces sourcecode (ie C#, VB.NET code,
    ///         SQL Stored Procedures, PHP, HTML, ...) 
    /// 
    /// There is an online repository with lots of templates available. 
    /// </summary>
	public class EmdbConnection : IDbConnection
	{
        #region connection cache
        private static DateTime lastModified = DateTime.MinValue;
        private static XDocument _db = null;
        private static String _connectionString = "";

        /// <summary>
        /// opening a connection might take a long time
        /// opening a 2.3 MB umlfile with 86 tables requires 87 Seconds on a 2GHz pc with 768MB ram
        /// 
        /// therefor the connection opens it only once until either 
        /// - the connectionstring changes
        /// - the file modificationdate changes
        /// </summary>
        private static XDocument GetEmdb(string connectionString)
        {
            if (_connectionString != connectionString || _db == null )
            {
                _connectionString = connectionString;
                _db = XDocument.Load(_connectionString);
                state = ConnectionState.Open;
            }
            return _db;
        }
        #endregion

        public EmdbConnection() 
		{
		}

        public EmdbConnection(string connectionString)
        {
            if (_connectionString != connectionString)
            {
                _db = GetEmdb(connectionString);
            }
        }

        #region IDbConnection Members

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return null;
        }

        public IDbTransaction BeginTransaction()
        {
            return null;
        }

        public void ChangeDatabase(string databaseName)
        {
        }

        public XDocument Db
        {
            get
            {
                if (_db == null)
                {
                    _db = GetEmdb(this.ConnectionString);
                }
                return _db;
            }
        }


        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    _db = null;
                }
            }
        }

        public int ConnectionTimeout
        {
            get { return 0; }
        }

        public IDbCommand CreateCommand()
        {
            return null;
        }

        public string Database
        {
            get 
            {
                var dbs = this.Db.Element("Databases").Elements("Database").ToList();
                
                return dbs[0].Attribute("Name").Value;
            }
        }

        public void Open()
        {
            try
            {
                if (state == ConnectionState.Closed)
                {
                    _db = GetEmdb(this.ConnectionString);
                    state = ConnectionState.Open;
                }
            }
            catch (Exception)
            {
                state = ConnectionState.Broken;
                throw;
            }
        }

        public void Close()
        {
            state = ConnectionState.Closed;
        }

        private static ConnectionState state = ConnectionState.Closed;
        public ConnectionState State
        {
            get { return state; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
