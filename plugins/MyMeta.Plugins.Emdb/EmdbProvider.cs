using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using MyMeta;
using System.Xml.Linq;
using System.Linq;

namespace MyMeta.Plugins.Emdb
{
    public class EmdbProvider : IMyMetaPlugin
    {
        private const string PROVIDER_KEY = @"EMDB";
        private const string PROVIDER_NAME = @"Entity Manager db";
        private const string AUTHOR_INFO = @"Entity Manager db MyMeta plugin written by Justin Greenwood.";
        private const string AUTHOR_URI = @"http://www.cnblogs.com/shiningrise/";
        private const string SAMPLE_CONNECTION = @"c:\XmlMyMeta.em";
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

        string IMyMetaPlugin.SampleConnectionString
        {
            get { return SAMPLE_CONNECTION; }
        }

        public IDbConnection NewConnection
        {
            get
            {
                if (IsIntialized)
                    return new EmdbConnection(context.ConnectionString);
                else
                    return null;
            }
        }

        public string DefaultDatabase
        {
            get
            {
                var c = this.NewConnection as EmdbConnection;
                c.Open();
                string defaultDB = c.Database;
                c.Close();

                return defaultDB;
            }
        }

        public DataTable Databases
        {
            get
            {
                var c = this.NewConnection as EmdbConnection;
                c.Open();
                var server = c.Db;
                var query = from db in server.Element("Databases").Elements("Database") select db;

                DataTable dt = context.CreateDatabasesDataTable();
                foreach (var db in query)
                {
                    DataRow row = dt.NewRow();
                    row["CATALOG_NAME"] = db.Attribute("Name").Value;
                    row["DESCRIPTION"] = db.Attribute("Description").Value;
                    dt.Rows.Add(row);
                }

                c.Close();

                return dt;
            }
        }

        public DataTable GetTables(string database)
        {
            var c = this.NewConnection as EmdbConnection;
            c.Open();
            var server = c.Db;
            var db = (from d in server.Element("Databases").Elements("Database") where d.Attribute("Name").Value == database select d).First();
            var tables = db.Elements("Table").ToList();
            DataTable dt = context.CreateTablesDataTable();
            foreach (var table in tables)
            {
                DataRow row = dt.NewRow();
                row["DESCRIPTION"] = table.Attribute("Description").Value;
                row["TABLE_NAME"] = table.Attribute("TableName").Value;
                dt.Rows.Add(row);
            }
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

        public DataTable GetTableColumns(string databaseName, string tableName)
        {
            var conn = this.NewConnection as EmdbConnection;
            conn.Open();
            var server = conn.Db;
            var db = (from d in server.Element("Databases").Elements("Database") where d.Attribute("Name").Value == databaseName select d).First();
            var table = (from t in db.Elements("Table") where t.Attribute("TableName").Value == tableName select t ).First();
            var columns = (from c1 in table.Elements("Column") select c1).ToList();

            DataTable dt = context.CreateColumnsDataTable();
            foreach (var c in columns)
            {
                DataRow row = dt.NewRow();

                set(row, "TABLE_NAME", table.Attribute("TableName").Value);
                set(row, "COLUMN_NAME", c.Attribute("FieldName").Value);
                //set(row, "ORDINAL_POSITION", ORDINAL_POSITION++);
                //set(row, "IS_NULLABLE", c.AllowDBNull);
                //set(row, "COLUMN_HASDEFAULT", (c.DefaultValue.Length > 0));
                //set(row, "COLUMN_DEFAULT", c.DefaultValue);
                //set(row, "IS_AUTO_KEY", c.AutoIncrement);
                //set(row, "AUTO_KEY_SEED", c.AutoIncrementSeed);
                //set(row, "AUTO_KEY_INCREMENT", c.AutoIncrementStep);

                //set(row, "TYPE_NAME", c.DataType);
                // set(row,"TYPE_NAME_COMPLETE",GetDbDataTypeName(c)); //  .DataType);
                //SetDbDataTypeName(c, row); //  .DataType);

                set(row, "NUMERIC_PRECISION", 0);
                set(row, "NUMERIC_SCALE", 0);
                //set(row, "CHARACTER_MAXIMUM_LENGTH", c.GetDataTypStringLen());
                //set(row, "CHARACTER_OCTET_LENGTH", c.GetDataTypStringLen());

                set(row, "DESCRIPTION", c.Attribute("Description").Value); //c.FieldComment);//Description

                // different names for ProcedureParameter instead of COLUMN
                //set(row, "PROCEDURE_NAME", tab.TableName);
                //set(row, "PARAMETER_NAME", c.FieldName);
                //set(row, "PARAMETER_HASDEFAULT", (c.DefaultValue.Length > 0));
                //set(row, "PARAMETER_DEFAULT", c.DefaultValue);
                // PARAMETER_TYPE

                // different names for ResultColumn instead of COLUMN
                set(row, "NAME", c.Attribute("FieldName").Value);

                //row["DESCRIPTION"] = column.Attribute("Description").Value;
                //row["TABLE_NAME"] = column.Attribute("FieldName").Value;

                dt.Rows.Add(row);
            }
            conn.Close();
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


        private void set(DataRow row, string fieldName, Object value)
        {
            // row["TABLE_CATALOG"] = DBNull.Value; // GetDatabaseName();
            DataColumn col = row.Table.Columns[fieldName];
            if (col != null)
                row[col] = value;
        }
    }
}
