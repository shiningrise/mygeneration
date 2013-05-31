using System;
using System.Data;
using Dl3bak.Data.Xsd3b;
using Dl3bak.Data.Xsd3b.Plugin;
using System.Collections.Specialized;
using System.IO;

namespace MyMeta.Plugins.Xsd3b
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
	public class Xsd3bConnection : IDbConnection
	{
        private String connectionString = "";
		public Xsd3bConnection() 
		{
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

        #region connection cache
        private static DateTime lastModified = DateTime.MinValue;
        private static String lastConnectString = "";
        private static SchemaXsd3bEx lastXsd3b = null;

        /// <summary>
        /// opening a connection might take a long time
        /// opening a 2.3 MB umlfile with 86 tables requires 87 Seconds on a 2GHz pc with 768MB ram
        /// 
        /// therefor the connection opens it only once until either 
        /// - the connectionstring changes
        /// - the file modificationdate changes
        /// </summary>
        private static SchemaXsd3bEx getXsd3b(string connectionString)
        {
        	return SchemaXsd3bEx.ReadXsd3b(connectionString,null);
        /* feature not completed yet, disabled
            if (connectionString != null)
            {
                ListDictionary parms = PlugInSupport.GetConnectParameters(connectionString);
                String fileName = parms[PlugInSupport.CONNECT_FILENAME].ToString();

                if ((lastXsd3b == null) 
                    || (connectionString.CompareTo(lastConnectString) != 0)
                    || (lastModified != System.IO.File.GetLastWriteTime(fileName)))
                {
                    string tempDir = Path.Combine(Environment.GetEnvironmentVariable("temp"), "tmpLastLoaded.xsd3b");

                    // hier gehts weiter

                    lastXsd3b = null; // reset if there is a exception in ReadXsd3b
                    lastXsd3b = SchemaXsd3bEx.ReadXsd3b(connectionString, null);
                    lastModified = System.IO.File.GetLastWriteTime(fileName);
                    lastConnectString = connectionString;                    
                }
            }
            return lastXsd3b;
            */
        }
        #endregion
        private SchemaXsd3bEx xsd3b = null;
        public SchemaXsd3bEx Xsd3b
        {
            get
            {
                if (xsd3b == null)
                {
                    xsd3b = getXsd3b(this.ConnectionString);
                    // xsd3b = SchemaXsd3bEx.ReadXsd3b(this.ConnectionString,null);
                }
                return xsd3b;
            }
        }


        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                xsd3b = null;
                connectionString = value;
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
            get { return null; }
        }

        public void Open()
        {

            try
            {
                xsd3b = getXsd3b(this.ConnectionString);
                state = ConnectionState.Open;
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

        ConnectionState state = ConnectionState.Closed;
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
