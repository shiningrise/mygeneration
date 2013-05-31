using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using MyMeta;

namespace MyMeta.Plugins
{
    public class DelimitedTextPlugin : IMyMetaPlugin
    {
        private const string PROVIDER_KEY = @"DELIMITEDTEXT";
        private const string PROVIDER_NAME = @"Delimited Text";
        private const string AUTHOR_INFO = @"Delimited Text MyMeta plugin written by Justin Greenwood.";
        private const string AUTHOR_URI = @"http://www.mygenerationsoftware.com/";
        private const string SAMPLE_CONNECTION = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\TxtFilesFolder\;Extended Properties='text;HDR=Yes;FMT=Delimited'";
        private IMyMetaPluginContext context = null;

        private bool IsIntialized { get { return (context != null); } }

        public void Initialize(IMyMetaPluginContext context)
        {
            this.context = context;
        }

        public string ProviderName
        {
            get { return PROVIDER_NAME; }
        }

        public string ProviderUniqueKey
        {
            get { return PROVIDER_KEY; }
        }

        public string ProviderAuthorInfo
        {
            get { return AUTHOR_INFO; }
        }

        public Uri ProviderAuthorUri
        {
            get { return new Uri(AUTHOR_URI); }
        }

        public bool StripTrailingNulls
        {
            get { return false; }
        }

        public bool RequiredDatabaseName
        {
            get { return false; }
        }

        public string SampleConnectionString
        {
            get { return SAMPLE_CONNECTION; }
        }

        public IDbConnection NewConnection
        {
            get
            {
                if (IsIntialized) 
                    return new OleDbConnection(context.ConnectionString);
                else
                    return null;
            }
        }

        public string DefaultDatabase
        {
            get
            {
                OleDbConnection c = this.NewConnection as OleDbConnection;
                c.Open();
                string defaultDB = c.DataSource;
                c.Close();

                return defaultDB;
            }
        }

        public DataTable Databases
        {
            get
            {
                OleDbConnection c = this.NewConnection as OleDbConnection;
                c.Open();
                DataTable dt = context.CreateDatabasesDataTable();
                DataRow row = dt.NewRow();
                row["CATALOG_NAME"] = c.DataSource;
                row["DESCRIPTION"] = "The folder where the delimited text files reside.";
                dt.Rows.Add(row);
                c.Close();

                return dt;
            }
        }

        public DataTable GetTables(string database)
        {
                OleDbConnection c = this.NewConnection as OleDbConnection;
                c.Open();
                DataTable dt = c.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                c.Close();
                return dt;
        }

        public DataTable GetViews(string database)
        {
            return new DataTable();
        }

        public DataTable GetProcedures(string database)
        {
            return new DataTable();
        }

        public DataTable GetDomains(string database)
        {
            return new DataTable();
        }

        public DataTable GetProcedureParameters(string database, string procedure)
        {
            return new DataTable();
        }

        public DataTable GetProcedureResultColumns(string database, string procedure)
        {
            return new DataTable();
        }

        public DataTable GetViewColumns(string database, string view)
        {
            return new DataTable();
        }

        public DataTable GetTableColumns(string database, string table)
        {
            OleDbConnection c = this.NewConnection as OleDbConnection;
            c.Open();
            DataTable dt = c.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, table });
            c.Close();
            return dt;
        }

        public List<string> GetPrimaryKeyColumns(string database, string table)
        {
            return new List<string>();
        }

        public List<string> GetViewSubViews(string database, string view)
        {
            return new List<string>();
        }

        public List<string> GetViewSubTables(string database, string view)
        {
            return new List<string>();
        }

        public DataTable GetTableIndexes(string database, string table)
        {
            return new DataTable();
        }

        public DataTable GetForeignKeys(string database, string table)
        {
            return new DataTable();
        }

        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }
    }
}
