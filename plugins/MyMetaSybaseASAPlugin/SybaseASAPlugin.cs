using MyMeta;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace MyMeta.Plugins
{
	public class SybaseASAPlugin : IMyMetaPlugin
	{
		private const string PROVIDER_KEY = @"SYBASE_ASA";
		private const string PROVIDER_NAME = @"Sybase ASA ODBC";
		private const string AUTHOR_INFO = @"Sybase ASA ODBC MyMeta plugin written by Timothy Moriarty.";
		private const string AUTHOR_URI = @"http://www.mygenerationsoftware.com/";
		private const string SAMPLE_CONNECTION = @"Driver={Adaptive Server Anywhere 9.0};uid=USERNAME;pwd=PASSWORD;autostop=Yes;integrated=No;filedsn=C:\Program Files\DATABASE FOLDER\DATABASE.dsn;debug=No;disablemultirowfetch=No;compress=No";
		private IMyMetaPluginContext context;

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
					return new OdbcConnection(context.ConnectionString);
				else
					return null;
			}
		}

		public string DefaultDatabase
		{
			get
			{
				using (OdbcConnection c = this.NewConnection as OdbcConnection)
				{
					c.Open();
					string defaultDB = c.DataSource;
					c.Close();

					return defaultDB;
				}
			}
		}

		public DataTable Databases
		{
			get
			{
				OdbcConnection c = this.NewConnection as OdbcConnection;
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
			using (OdbcConnection c = this.NewConnection as OdbcConnection)
            {
				DataTable tables = context.CreateTablesDataTable();
				OdbcCommand sql = new OdbcCommand();
				sql.CommandText = "select * from sys.syscatalog where creator<>'SYS' and creator<>'dbo' and creator not like 'rs%'";
				sql.CommandType = CommandType.Text;
				sql.Connection = c;
				c.Open();
				using (OdbcDataReader reader = sql.ExecuteReader())
				{
					while (reader.Read())
					{
						DataRow row = tables.NewRow();
						row["TABLE_NAME"] = reader.GetString(1);
						row["TABLE_TYPE"] = reader.GetString(3);

						tables.Rows.Add(row);
					}
					reader.Close();
				}

				c.Close();
				tables.AcceptChanges();
				return tables;
            }
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
            using (OdbcConnection c = this.NewConnection as OdbcConnection)
            {
				DataTable columns = context.CreateColumnsDataTable();
				DataTable columnData = new DataTable();
				// Build the SQL statement
				StringBuilder sb = new StringBuilder("select sys.syscolumn.*, sys.sysdomain.* from sys.systable ");
				sb.Append(" join sys.syscolumn on sys.systable.table_id=sys.syscolumn.table_id ");
				sb.Append(" join sys.sysdomain on sys.syscolumn.domain_id=sys.sysdomain.domain_id where sys.systable.table_name='");
				sb.Append(table);
				sb.Append("' order by sys.syscolumn.column_id");
				OdbcCommand sql = new OdbcCommand();
				sql.CommandText = sb.ToString();
				sql.CommandType = CommandType.Text;
				sql.Connection = c;
				OdbcDataAdapter adapter = new OdbcDataAdapter(sql);
				c.Open();

				adapter.Fill(columnData);

				foreach (DataRow columnDataRow in columnData.Rows)
				{
					DataRow row = columns.NewRow();

					row["TABLE_NAME"] = table;
					row["COLUMN_NAME"] = columnDataRow["column_name"];
					row["ORDINAL_POSITION"] = columnDataRow["column_id"];

					string columnDefault = columnDataRow["default"].ToString();
					row["COLUMN_DEFAULT"] = columnDefault;
					row["COLUMN_HASDEFAULT"] = (columnDefault != string.Empty) ? true : false;

					row["IS_NULLABLE"] = (columnDataRow["nulls"].ToString() == "Y") ? true : false;
					row["CHARACTER_MAXIMUM_LENGTH"] = columnDataRow["width"];
					row["CHARACTER_OCTET_LENGTH"] = columnDataRow["width"];
					row["NUMERIC_PRECISION"] = columnDataRow["precision"];
					row["NUMERIC_SCALE"] = columnDataRow["scale"];

					row["DATA_TYPE"] = columnDataRow["domain_id"];
					row["TYPE_NAME"] = columnDataRow["domain_name"];
					row["TYPE_NAME_COMPLETE"] = columnDataRow["domain_name"];

					columns.Rows.Add(row);
				}

				
						//metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
						//metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
						//metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
						//metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));
						//metaData.Columns.Add("COLUMN_GUID", Type.GetType("System.Guid"));
						//metaData.Columns.Add("COLUMN_PROPID", Type.GetType("System.Int64"));
						//metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int64"));
						//metaData.Columns.Add("COLUMN_HASDEFAULT", Type.GetType("System.Boolean"));
						//metaData.Columns.Add("COLUMN_DEFAULT", Type.GetType("System.String"));
						//metaData.Columns.Add("COLUMN_FLAGS", Type.GetType("System.Int64"));
						//metaData.Columns.Add("IS_NULLABLE", Type.GetType("System.Boolean"));
						//metaData.Columns.Add("DATA_TYPE", Type.GetType("System.Int32"));
						//metaData.Columns.Add("TYPE_NAME", Type.GetType("System.String"));
						//metaData.Columns.Add("TYPE_NAME_COMPLETE", Type.GetType("System.String"));
						//metaData.Columns.Add("TYPE_GUID", Type.GetType("System.Guid"));
						//metaData.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.Int64"));
						//metaData.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.Int64"));
						//metaData.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.Int32"));
						//metaData.Columns.Add("NUMERIC_SCALE", Type.GetType("System.Int16"));
						//metaData.Columns.Add("DATETIME_PRECISION", Type.GetType("System.Int64"));
						//metaData.Columns.Add("CHARACTER_SET_CATALOG", Type.GetType("System.String"));
						//metaData.Columns.Add("CHARACTER_SET_SCHEMA", Type.GetType("System.String"));
						//metaData.Columns.Add("CHARACTER_SET_NAME", Type.GetType("System.String"));
						//metaData.Columns.Add("COLLATION_CATALOG", Type.GetType("System.String"));
						//metaData.Columns.Add("COLLATION_SCHEMA", Type.GetType("System.String"));
						//metaData.Columns.Add("COLLATION_NAME", Type.GetType("System.String"));
						//metaData.Columns.Add("DOMAIN_CATALOG", Type.GetType("System.String"));
						//metaData.Columns.Add("DOMAIN_SCHEMA", Type.GetType("System.String"));
						//metaData.Columns.Add("DOMAIN_NAME", Type.GetType("System.String"));
						//metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
						//metaData.Columns.Add("COLUMN_LCID", Type.GetType("System.Int32"));
						//metaData.Columns.Add("COLUMN_COMPFLAGS", Type.GetType("System.Int32"));
						//metaData.Columns.Add("COLUMN_SORTID", Type.GetType("System.Int32"));
						//metaData.Columns.Add("IS_COMPUTED", Type.GetType("System.Boolean"));
						//metaData.Columns.Add("IS_AUTO_KEY", Type.GetType("System.Boolean"));
						//metaData.Columns.Add("AUTO_KEY_SEED", Type.GetType("System.Int32"));
						//metaData.Columns.Add("AUTO_KEY_INCREMENT", Type.GetType("System.Int32"));

				c.Close();
				columns.AcceptChanges();
				return columns;
            }
		}

		public List<string> GetPrimaryKeyColumns(string database, string table)
		{
			using (OdbcConnection c = this.NewConnection as OdbcConnection)
			{
				List<string> primaryKeys = new List<string>();
				OdbcCommand sql = new OdbcCommand();
				sql.CommandText = "select sys.syscolumn.column_name from sys.systable join sys.syscolumn on sys.systable.table_id=sys.syscolumn.table_id where sys.systable.table_name='" + table + "' and sys.syscolumn.pkey='Y'";
				sql.CommandType = CommandType.Text;
				sql.Connection = c;
				c.Open();
				using (OdbcDataReader reader = sql.ExecuteReader())
				{
					while (reader.Read())
					{
						primaryKeys.Add(reader.GetString(0));
					}
					reader.Close();
				}

				c.Close();

				return primaryKeys;
			}
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
			using (OdbcConnection c = this.NewConnection as OdbcConnection)
            {
				return new DataTable();
            }
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
