#if !IGNORE_SQLCE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyMeta;

using System.Data.SqlServerCe;

namespace MyMeta.Plugins
{
    public class SqlCePlugin : IMyMetaPlugin
	{
		#region IMyMetaPlugin Interface

		private IMyMetaPluginContext context;

        void IMyMetaPlugin.Initialize(IMyMetaPluginContext context)
        {
            this.context = context;
        }

        string IMyMetaPlugin.ProviderName
        {
            get { return @"Microsoft SQL CE"; }
        }

        string IMyMetaPlugin.ProviderUniqueKey
        {
            get { return @"SQLCE"; }
        }

        string IMyMetaPlugin.ProviderAuthorInfo
        {
            get { return @"Microsoft SQL CE Plugin Written by MyGeneration Software"; }
        }

        Uri IMyMetaPlugin.ProviderAuthorUri
        {
            get { return new Uri(@"http://www.mygenerationsoftware.com/"); }
        }

        bool IMyMetaPlugin.StripTrailingNulls
        {
            get { return false; }
        }

        bool IMyMetaPlugin.RequiredDatabaseName
        {
            get { return false; }
        }

        string IMyMetaPlugin.SampleConnectionString
        {
            get { return @"Data Source=C:\Program Files\Microsoft SQL Server Compact Edition\v3.1\SDK\Samples\Northwind.sdf"; }
        }

        IDbConnection IMyMetaPlugin.NewConnection
        {
            get
            {
                if (IsIntialized)
				{
                    SqlCeConnection cn = new SqlCeConnection(this.context.ConnectionString);
					return cn as IDbConnection;
				}
                else
                    return null;
            }
        }

        string IMyMetaPlugin.DefaultDatabase
        {
            get
            {
				return this.GetDatabaseName();
            }
        }

        DataTable IMyMetaPlugin.Databases
        {
            get
            {
				DataTable metaData = new DataTable();

				try
				{
					metaData = context.CreateDatabasesDataTable();

					DataRow row = metaData.NewRow();
					metaData.Rows.Add(row);

					row["CATALOG_NAME"] = GetDatabaseName();
				//	row["DESCRIPTION"]  = db.Description;
				}
				finally
				{

				}

				return metaData;
            }
        }

        DataTable IMyMetaPlugin.GetTables(string database)
        {
			DataTable metaData = new DataTable();

			try
			{
				metaData = context.CreateTablesDataTable();

                SqlCeConnection conn = new SqlCeConnection();
                conn.ConnectionString = context.ConnectionString;

                SqlCeCommand cmd = new SqlCeCommand();
                cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='TABLE'";
                cmd.Connection = conn;

                DataTable dt = new DataTable();
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);

				foreach(DataRow r in dt.Rows) 
				{ 
					DataRow row = metaData.NewRow();
					metaData.Rows.Add(row);

                    row["TABLE_CATALOG"] = r["TABLE_CATALOG"];
                    row["TABLE_SCHEMA"] = r["TABLE_SCHEMA"];
                    row["TABLE_NAME"] = r["TABLE_NAME"];

				}
			}
			finally
			{

			}

			return metaData;
        }

		DataTable IMyMetaPlugin.GetViews(string database)
		{
			DataTable metaData = new DataTable();

            //try
            //{
            //    metaData = context.CreateTablesDataTable();

            //    SqlCeConnection conn = new SqlCeConnection();
            //    conn.ConnectionString = context.ConnectionString;

            //    SqlCeCommand cmd = new SqlCeCommand();
            //    cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.VIEWS";
            //    cmd.Connection = conn;

            //    DataTable dt = new DataTable();
            //    SqlCeDataAdapter adapter = new SqlCeDataAdapter();
            //    adapter.SelectCommand = cmd;
            //    adapter.Fill(dt);

            //    foreach (DataRow r in dt.Rows)
            //    {
            //        DataRow row = metaData.NewRow();
            //        metaData.Rows.Add(row);

            //        row["TABLE_CATALOG"] = r["TABLE_CATALOG"];
            //        row["TABLE_SCHEMA"] = r["TABLE_SCHEMA"];
            //        row["TABLE_NAME"] = r["TABLE_NAME"];
            //    }
            //}
            //finally
            //{

            //}

            return metaData;
		}

        DataTable IMyMetaPlugin.GetProcedures(string database)
        {
            return new DataTable();
        }

        DataTable IMyMetaPlugin.GetDomains(string database)
        {
            return new DataTable();
        }

        DataTable IMyMetaPlugin.GetProcedureParameters(string database, string procedure)
        {
            return new DataTable();
        }

        DataTable IMyMetaPlugin.GetProcedureResultColumns(string database, string procedure)
        {
            return new DataTable();
        }

        DataTable IMyMetaPlugin.GetViewColumns(string database, string view)
        {

            DataTable metaData = new DataTable();

            try
            {
                metaData = context.CreateColumnsDataTable();

                SqlCeConnection conn = new SqlCeConnection();
                conn.ConnectionString = context.ConnectionString;

                SqlCeCommand cmd = new SqlCeCommand();
                cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" +
                    view + "'";
                cmd.Connection = conn;

                DataTable dt = new DataTable();
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);

                foreach (DataRow r in dt.Rows)
                {
                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    row["TABLE_CATALOG"] = r["TABLE_CATALOG"];
                    row["TABLE_SCHEMA"] = r["TABLE_SCHEMA"];
                    row["TABLE_NAME"] = r["TABLE_NAME"];
                    row["COLUMN_NAME"] = r["COLUMN_NAME"];
                    row["ORDINAL_POSITION"] = r["ORDINAL_POSITION"];
                    row["DESCRIPTION"] = r["DESCRIPTION"];

                    if (r["IS_NULLABLE"] != DBNull.Value)
                    {
                        string isNullable = (string)r["IS_NULLABLE"];
                        isNullable = isNullable.ToUpper();
                        row["IS_NULLABLE"] = (isNullable == "NO") ? false : true;
                    }

                    if ((bool)r["COLUMN_HASDEFAULT"])
                    {
                        row["COLUMN_HASDEFAULT"] = true;
                        row["COLUMN_DEFAULT"] = r["COLUMN_DEFAULT"];
                    }

                    if (r["AUTOINC_INCREMENT"] != DBNull.Value)
                    {
                        row["IS_AUTO_KEY"] = true;
                        row["AUTO_KEY_SEED"] = Convert.ToInt32(r["AUTOINC_SEED"]);
                        row["AUTO_KEY_INCREMENT"] = Convert.ToInt32(r["AUTOINC_INCREMENT"]);
                    }

                    string type = (string)r["DATA_TYPE"];
                    int charMax = 0;
                    short precision = 0;
                    short scale = 0;

                    if (r["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                    {
                        charMax = (int)r["CHARACTER_MAXIMUM_LENGTH"];
                    }

                    if (r["NUMERIC_PRECISION"] != DBNull.Value)
                    {
                        precision = (short)r["NUMERIC_PRECISION"];
                    }

                    if (r["NUMERIC_SCALE"] != DBNull.Value)
                    {
                        scale = (short)r["NUMERIC_SCALE"];
                    }

                    row["TYPE_NAME"] = type;
                    row["TYPE_NAME_COMPLETE"] = this.GetDataTypeNameComplete(type, charMax, precision, scale);

                    row["NUMERIC_PRECISION"] = precision;
                    row["NUMERIC_SCALE"] = scale;

                    row["CHARACTER_MAXIMUM_LENGTH"] = charMax;

                    row["IS_COMPUTED"] = (type == "timestamp") ? true : false;
                }
            }
            finally
            {

            }

            return metaData;
        }

        DataTable IMyMetaPlugin.GetTableColumns(string database, string table)
        {
            DataTable metaData = new DataTable();

            try
            {
                metaData = context.CreateColumnsDataTable();

                SqlCeConnection conn = new SqlCeConnection();
                conn.ConnectionString = context.ConnectionString;

                SqlCeCommand cmd = new SqlCeCommand();
                cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" +
                    table + "'";
                cmd.Connection = conn;

                DataTable dt = new DataTable();
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);

                foreach (DataRow r in dt.Rows)
                {
                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    row["TABLE_CATALOG"] = r["TABLE_CATALOG"];
                    row["TABLE_SCHEMA"] = r["TABLE_SCHEMA"];
                    row["TABLE_NAME"] = r["TABLE_NAME"];
                    row["COLUMN_NAME"] = r["COLUMN_NAME"];
                    row["ORDINAL_POSITION"] = r["ORDINAL_POSITION"];
                    row["DESCRIPTION"] = r["DESCRIPTION"];

                    if (r["IS_NULLABLE"] != DBNull.Value)
                    {
                        string isNullable = (string)r["IS_NULLABLE"];
                        isNullable = isNullable.ToUpper();
                        row["IS_NULLABLE"] = (isNullable == "NO") ? false : true;
                    }

                    if ((bool)r["COLUMN_HASDEFAULT"])
                    {
                        row["COLUMN_HASDEFAULT"] = true;
                        row["COLUMN_DEFAULT"] = r["COLUMN_DEFAULT"];
                    }

                    if (r["AUTOINC_INCREMENT"] != DBNull.Value)
                    {
                        row["IS_AUTO_KEY"] = true;
                        row["AUTO_KEY_SEED"] = Convert.ToInt32(r["AUTOINC_SEED"]);
                        row["AUTO_KEY_INCREMENT"] = Convert.ToInt32(r["AUTOINC_INCREMENT"]);
                    }

                    string type = (string)r["DATA_TYPE"];
                    int charMax = 0;
                    short precision = 0;
                    short scale = 0;

                    if (r["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                    {
                        charMax = (int)r["CHARACTER_MAXIMUM_LENGTH"];
                    }

                    if (r["NUMERIC_PRECISION"] != DBNull.Value)
                    {
                        precision = (short)r["NUMERIC_PRECISION"];
                    }

                    if (r["NUMERIC_SCALE"] != DBNull.Value)
                    {
                        scale = (short)r["NUMERIC_SCALE"];
                    }

                    row["TYPE_NAME"] = type;
                    row["TYPE_NAME_COMPLETE"] = this.GetDataTypeNameComplete(type, charMax, precision, scale);

                    row["NUMERIC_PRECISION"] = precision;
                    row["NUMERIC_SCALE"] = scale;

                    row["CHARACTER_MAXIMUM_LENGTH"] = charMax;

                    row["IS_COMPUTED"] = (type == "timestamp") ? true : false;
                }
            }
            finally
            {

            }

            return metaData;
        }

        List<string> IMyMetaPlugin.GetPrimaryKeyColumns(string database, string table)
        {
			List<string> primaryKeys = new List<string>();

            try
            {
                SqlCeConnection conn = new SqlCeConnection();
                conn.ConnectionString = context.ConnectionString;

                SqlCeCommand cmd = new SqlCeCommand();

                cmd.CommandText =
"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.INDEXES where Table_Name='" + table + "' AND PRIMARY_KEY=1";
                cmd.Connection = conn;

                DataTable dt = new DataTable();
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    primaryKeys.Add((string)row["COLUMN_NAME"]);

                }
            }
            catch { }

			return primaryKeys;
        }

        List<string> IMyMetaPlugin.GetViewSubViews(string database, string view)
        {
            return new List<string>();
        }

        List<string> IMyMetaPlugin.GetViewSubTables(string database, string view)
        {
            return new List<string>();
        }

        DataTable IMyMetaPlugin.GetTableIndexes(string database, string table)
        {
			DataTable metaData = new DataTable();

            try
            {
                metaData = context.CreateIndexesDataTable();

                SqlCeConnection conn = new SqlCeConnection();
                conn.ConnectionString = context.ConnectionString;

                SqlCeCommand cmd = new SqlCeCommand();
                cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.INDEXES WHERE TABLE_NAME='" +
                    table + "'";
                cmd.Connection = conn;

                DataTable dt = new DataTable();
                SqlCeDataAdapter adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);

                foreach (DataRow r in dt.Rows)
                {
                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    row["TABLE_NAME"] = r["TABLE_NAME"];
                    row["INDEX_NAME"] = r["INDEX_NAME"];
                    row["UNIQUE"] = r["UNIQUE"];
                    row["CLUSTERED"] = r["CLUSTERED"];
                    row["AUTO_UPDATE"] = r["AUTO_UPDATE"];
                    row["SORT_BOOKMARKS"] = r["SORT_BOOKMARKS"];
                    row["FILTER_CONDITION"] = r["FILTER_CONDITION"];
                    row["NULL_COLLATION"] = r["NULL_COLLATION"];
                    row["INITIAL_SIZE"] = r["INITIAL_SIZE"];
                    row["CARDINALITY"] = Convert.ToDecimal(r["INITIAL_SIZE"]);
                    row["COLLATION"] = r["COLLATION"];
                    row["COLUMN_NAME"] = r["COLUMN_NAME"];
                    row["FILL_FACTOR"] = r["FILL_FACTOR"];
                    row["AUTO_UPDATE"] = r["AUTO_UPDATE"];
                    row["PRIMARY_KEY"] = r["PRIMARY_KEY"];
                    row["NULLS"] = r["NULLS"];
                    row["ORDINAL_POSITION"] = r["ORDINAL_POSITION"];
                }
            }
            catch { }

			return metaData;
        }

        DataTable IMyMetaPlugin.GetForeignKeys(string database, string table)
        {
			DataTable metaData = new DataTable();

            try
            {
                metaData = context.CreateForeignKeysDataTable();

                LoadForeignKeysPartOne(metaData, table);
                LoadForeignKeysPartTwo(metaData, table);
            }
            catch { }

			return metaData;
        }

        private void LoadForeignKeysPartOne(DataTable metaData, string table)
        {
            SqlCeConnection conn = new SqlCeConnection();
            conn.ConnectionString = context.ConnectionString;

            SqlCeCommand cmd = new SqlCeCommand();
            cmd.CommandText =
"SELECT tc.*, rc.UPDATE_RULE, rc.DELETE_RULE, rc.UNIQUE_CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc " +
"JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc ON tc.CONSTRAINT_NAME = rc.CONSTRAINT_NAME " +
"WHERE tc.CONSTRAINT_TYPE='FOREIGN KEY' AND tc.TABLE_NAME = '" + table + "'";

            cmd.Connection = conn;

            DataTable dt = new DataTable();
            SqlCeDataAdapter adapter = new SqlCeDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            foreach (DataRow fk in dt.Rows)
            {
                //---------------------------------------
                // Get the Primary Key and Columns
                //---------------------------------------
                cmd = new SqlCeCommand();
                cmd.CommandText =
"SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME='" + fk["UNIQUE_CONSTRAINT_NAME"] + "'";
                cmd.Connection = conn;

                DataTable pCols = new DataTable();
                adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(pCols);

                //---------------------------------------
                // Get the Foreign Key Columns
                //---------------------------------------
                cmd = new SqlCeCommand();
                cmd.CommandText =
"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = '" + fk["CONSTRAINT_NAME"] + "'";
                cmd.Connection = conn;

                DataTable fCols = new DataTable();
                adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(fCols);

                for (int i = 0; i < pCols.Rows.Count; i++)
                {
                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    DataRow pRow = pCols.Rows[i];
                    DataRow fRow = fCols.Rows[i];

                    // The main Information ...
                    row["PK_TABLE_CATALOG"] = DBNull.Value;
                    row["PK_TABLE_SCHEMA"] = DBNull.Value;
                    row["FK_TABLE_CATALOG"] = DBNull.Value;
                    row["FK_TABLE_SCHEMA"] = DBNull.Value;
                    row["FK_TABLE_NAME"] = fk["TABLE_NAME"];
                    row["PK_TABLE_NAME"] = pRow["TABLE_NAME"];
                    row["ORDINAL"] = 0;
                    row["FK_NAME"] = fk["CONSTRAINT_NAME"];
                    row["UPDATE_RULE"] = fk["UPDATE_RULE"];
                    row["DELETE_RULE"] = fk["DELETE_RULE"];

                    bool isDeferrable = (bool)fk["IS_DEFERRABLE"];
                    bool initiallyDeferred = (bool)fk["INITIALLY_DEFERRED"];

                    if (isDeferrable)
                    {
                        row["DEFERRABILITY"] = initiallyDeferred ? 1 : 2;
                    }
                    else
                    {
                        row["DEFERRABILITY"] = 3;
                    }

                    row["PK_NAME"] = pRow["CONSTRAINT_NAME"];
                    row["PK_COLUMN_NAME"] = pRow["COLUMN_NAME"];
                    row["FK_COLUMN_NAME"] = fRow["COLUMN_NAME"];
                }
            }
        }

        private void LoadForeignKeysPartTwo(DataTable metaData, string table)
        {
            // Get primary key name
            SqlCeConnection conn = new SqlCeConnection();
            conn.ConnectionString = context.ConnectionString;

            SqlCeCommand cmd = new SqlCeCommand();
            cmd.CommandText = "SELECT INDEX_NAME FROM INFORMATION_SCHEMA.INDEXES WHERE TABLE_NAME='" + table + "' AND PRIMARY_KEY=1";
            cmd.Connection = conn;

            string pkName = "";

            try
            {
                conn.Open();
                pkName = (string)cmd.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }

            // Got it
            
            conn = new SqlCeConnection();
            conn.ConnectionString = context.ConnectionString;

            cmd = new SqlCeCommand();
            cmd.CommandText =
"SELECT tc.*, rc.UPDATE_RULE, rc.DELETE_RULE, rc.UNIQUE_CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc " +
"JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc ON tc.CONSTRAINT_NAME = rc.CONSTRAINT_NAME " +
"WHERE tc.CONSTRAINT_TYPE='FOREIGN KEY' AND rc.UNIQUE_CONSTRAINT_NAME = '" + pkName + "'";

            cmd.Connection = conn;

            DataTable dt = new DataTable();
            SqlCeDataAdapter adapter = new SqlCeDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            foreach (DataRow fk in dt.Rows)
            {
                //---------------------------------------
                // Get the Primary Key and Columns
                //---------------------------------------
                cmd = new SqlCeCommand();
                cmd.CommandText =
"SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME='" + fk["UNIQUE_CONSTRAINT_NAME"] + "'";
                cmd.Connection = conn;

                DataTable pCols = new DataTable();
                adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(pCols);

                //---------------------------------------
                // Get the Foreign Key Columns
                //---------------------------------------
                cmd = new SqlCeCommand();
                cmd.CommandText =
"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = '" + fk["CONSTRAINT_NAME"] + "'";
                cmd.Connection = conn;

                DataTable fCols = new DataTable();
                adapter = new SqlCeDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(fCols);

                for (int i = 0; i < pCols.Rows.Count; i++)
                {
                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    DataRow pRow = pCols.Rows[i];
                    DataRow fRow = fCols.Rows[i];

                    // The main Information ...
                    row["PK_TABLE_CATALOG"] = DBNull.Value;
                    row["PK_TABLE_SCHEMA"] = DBNull.Value;
                    row["FK_TABLE_CATALOG"] = DBNull.Value;
                    row["FK_TABLE_SCHEMA"] = DBNull.Value;
                    row["FK_TABLE_NAME"] = fk["TABLE_NAME"];
                    row["PK_TABLE_NAME"] = pRow["TABLE_NAME"];
                    row["ORDINAL"] = 0;
                    row["FK_NAME"] = fk["CONSTRAINT_NAME"];
                    row["UPDATE_RULE"] = fk["UPDATE_RULE"];
                    row["DELETE_RULE"] = fk["DELETE_RULE"];

                    bool isDeferrable = (bool)fk["IS_DEFERRABLE"];
                    bool initiallyDeferred = (bool)fk["INITIALLY_DEFERRED"];

                    if (isDeferrable)
                    {
                        row["DEFERRABILITY"] = initiallyDeferred ? 1 : 2;
                    }
                    else
                    {
                        row["DEFERRABILITY"] = 3;
                    }

                    row["PK_NAME"] = pRow["CONSTRAINT_NAME"];
                    row["PK_COLUMN_NAME"] = pRow["COLUMN_NAME"];
                    row["FK_COLUMN_NAME"] = fRow["COLUMN_NAME"];
                }
            }
        }

        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }

		#endregion

		#region Internal Methods

        private bool IsIntialized 
		{ 
			get 
			{ 
				return (context != null); 
			} 
		}

		public string GetDatabaseName()
		{
            SqlCeConnection cn = new SqlCeConnection();
            cn.ConnectionString = context.ConnectionString;

			string dbName = cn.DataSource;
			int index = dbName.LastIndexOfAny(new char[]{'\\'});
			if (index >= 0)
			{
				dbName = dbName.Substring(index + 1);
			}
			return dbName;
		}

		public string GetFullDatabaseName()
		{
            SqlCeConnection cn = new SqlCeConnection(this.context.ConnectionString);
            return cn.DataSource;
		}

		#endregion

        #region Other Methods

        private string GetDataTypeNameComplete(string dataType, int charMax, short precision, short scale)
        {
            //if (this.dbRoot.DomainOverride)
            //{
            //    if (this.HasDomain)
            //    {
            //        if (this.Domain != null)
            //        {
            //            return this.Domain.DataTypeNameComplete;
            //        }
            //    }
            //}

            switch (dataType)
            {
                case "binary":
                case "char":
                case "nchar":
                case "nvarchar":
                case "varchar":
                case "varbinary":

                    return dataType + "(" + charMax + ")";

                case "decimal":
                case "numeric":

                    return dataType + "(" + precision + "," + scale + ")";

                default:

                    return dataType;
            }
        }

        #endregion

    }
}
#endif
