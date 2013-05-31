using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using MyMeta;

namespace MyMeta.Plugins
{
    public class SybaseASEPlugin : IMyMetaPlugin, IMyMetaPluginExt, IDisposable
    {
        private OleDbConnection _connection;
        private IMyMetaPluginContext context;

        public void Initialize(IMyMetaPluginContext context)
        {
            this.context = context;
        }

        public string ProviderName
        {
            get { return @"Sybase ASE"; }
        }

        public string ProviderUniqueKey
        {
            get { return @"SYBASEASE"; }
        }

        public string ProviderAuthorInfo
        {
            get { return @"Sybase ASE MyMeta Plugin Written by komma8komma1"; }
        }

        public Uri ProviderAuthorUri
        {
            get { return new Uri(@"http://www.mygenerationsoftware.com/"); }
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
            get { return @"Provider=ASEOLEDB;Data Source=GREENMACHINE:5000;Catalog=pubs2;User Id=sa;Password=;"; }
        }

        public IDbConnection NewConnection
        {
            get
            {
                if (IsIntialized)
                {
                    OleDbConnection cn = new OleDbConnection(this.context.ConnectionString);
                    return cn as IDbConnection;
                }
                else
                    return null;
            }
        }

        public OleDbConnection OpenConnection
        {
            get
            {
                if ((_connection != null) && (_connection.State != ConnectionState.Open))
                {
                    _connection.Dispose();
                    _connection = null;
                }

                if (_connection == null)
                {
                    _connection = NewConnection as OleDbConnection;
                    _connection.Open();
                }

                return _connection;
            }
        }

        public string DefaultDatabase
        {
            get
            {
                return this.GetDatabaseName();
            }
        }

        public DataTable Databases
        {
            get
            {
                OleDbConnection cn = this.OpenConnection;

                DataTable dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs, new Object[] { null });
                /*if (HasDefaultDefined)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["CATALOG_NAME"].ToString() != DefaultDatabase) 
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }*/
                return dt;
            }
        }

        public DataTable GetTables(string database)
        {

            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            DataTable dt1 = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { database, null, null, "TABLE" });
            if (context.IncludeSystemEntities)
            {
                DataTable dt2 = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { database, null, null, "SYSTEM TABLE" });
                foreach (DataRow r in dt2.Rows) dt1.ImportRow(r);
            }

            return dt1;
        }

        public DataTable GetViews(string database)
        {
            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Views, new Object[] { database, null, null });
        }

        public DataTable GetProcedures(string database)
        {

            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, new Object[] { database, null, null });
        }

        public DataTable GetDomains(string database)
        {
            return this.context.CreateDomainsDataTable();
        }

        public DataTable GetProcedureParameters(string database, string procedure)
        {
            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Procedure_Parameters, new Object[] { database, null, procedure });
        }

        public DataTable GetProcedureResultColumns(string database, string procedure)
        {
            DataTable dt = this.context.CreateResultColumnsDataTable();

            /*OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            try
            {
                OleDbCommand cmd = cn.CreateCommand();
                cmd.CommandText = "exec " + procedure + ";";
                OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);

                DataTable metaData = reader.GetSchemaTable();
                if (metaData != null)
                {
                    foreach (DataRow metaRow in metaData.Rows)
                    {
                        // NumericScale NumericPrecision ColumnSize ColumnOrdinal DataTypeName ColumnName
                        DataRow row = dt.NewRow();
                        row["COLUMN_NAME"] = metaRow["ColumnName"];
                        row["ORDINAL_POSITION"] = metaRow["ColumnOrdinal"];
                        row["TYPE_NAME"] = metaRow["DataTypeName"];
                        row["TYPE_NAME_COMPLETE"] = metaRow["DataTypeName"];
                        //row["DATA_TYPE"] = 0;
                        dt.Rows.Add(row);
                    }
                }
            }
            catch { }*/


            return dt;
        }

        public DataTable GetViewColumns(string database, string view)
        {
            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            DataTable meta = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { database, null, view });
            SetDataTypes(cn, database, view, meta, true);
            return meta;
        }

        public DataTable GetTableColumns(string database, string table)
        {
            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            DataTable meta = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { database, null, table });
            SetDataTypes(cn, database, table, meta, false);//COLUMN_NAME
            return meta;
        }

        public DataTable GetProviderTypes()
        {
            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, DefaultDatabase);
            return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Provider_Types, null);
        }

        public List<string> GetPrimaryKeyColumns(string database, string table)
        {
            List<string> fieldNames = new List<string>();

            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            DataTable dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new Object[] { database, null, null, null, table });
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToBoolean(row["PRIMARY_KEY"]))
                {
                    fieldNames.Add(row["COLUMN_NAME"].ToString());
                }
            }

            return fieldNames;
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
            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new Object[] { database, null, null, null, table });
        }

        public DataTable GetForeignKeys(string database, string tableName)
        {
            OleDbConnection cn = this.OpenConnection;
            InitDatabase(cn, database);
            DataTable fks = context.CreateForeignKeysDataTable();
            StringBuilder sql = new StringBuilder();

            for (int i = 1; i < 7; i++)
            {
                if (i > 1) sql.Append("\r\nunion\r\n");
                sql.Append(@"SELECT 
                    USER_NAME(OT.uid) as FK_TABLE_SCHEMA,
                    db_name() as FK_TABLE_CATALOG,
					OBJECT_NAME(R.tableid) as FK_TABLE_NAME,  
					COL_NAME(R.tableid,R.fokey").Append(i).Append(@",DB_ID(R.frgndbname)) as FK_COLUMN_NAME,
                    USER_NAME(O.uid) as PK_TABLE_SCHEMA,
                    db_name() as PK_TABLE_CATALOG,   
					OBJECT_NAME(R.reftabid) as PK_TABLE_NAME, 
					COL_NAME(R.reftabid,R.refkey").Append(i).Append(@",DB_ID(R.pmrydbname)) as PK_COLUMN_NAME,
					OBJECT_NAME(R.constrid) as CONSTRAINT_NAME,
					OBJECT_NAME(R.constrid) as FK_NAME,
					").Append(i).Append(@" as ORDINAL,
                    OI.name as PK_NAME
				FROM sysreferences R, sysobjects O, sysobjects OT, sysindexes OI
                WHERE R.reftabid=O.id 
                    AND R.tableid=OT.id  
                    AND R.reftabid=O.id 
                    AND R.reftabid=OI.id 
                    AND OI.keycnt > 0
                    AND R.refkey").Append(i).Append(@" > 0
                    AND R.pmrydbname IS NULL
				    AND (OBJECT_NAME(R.tableid) = '").Append(tableName).Append(@"'
                        OR OBJECT_NAME(R.reftabid) = '").Append(tableName).Append(@"')");
            }

            OleDbCommand cmd = cn.CreateCommand();
            cmd.CommandText = sql.ToString();
            OleDbDataAdapter adapt = new OleDbDataAdapter(cmd);
            adapt.Fill(fks);

            return fks;
        }

        private void SetDataTypes(OleDbConnection conn, string database, string entityName, DataTable metaData, bool isView)
        {
            string sql = @"SELECT C.colid, C.name, C.usertype, C.scale, C.prec as precision, 
							C.length, CONVERT(bit,(C.status & 0x08)) as CanBeNull, T.name as datatype, M.text as RowDefault, CONVERT(bit,(C.status & 0x80)) as IsIdentity
					FROM syscolumns C, sysobjects O, systypes T, syscomments M  
					WHERE O.name='" + entityName + @"'
						AND O.type='" + (isView ? "V" : "U") + @"' 
						AND C.id = O.id 
						AND C.usertype*=T.usertype
						AND M.id=*C.cdefault
					ORDER by C.colid";

            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            OleDbDataAdapter adapt = new OleDbDataAdapter(cmd);
            DataTable extraInfo = new DataTable();
            adapt.Fill(extraInfo);

            string schema = null;
            Dictionary<string, DataRow> metaHash = new Dictionary<string, DataRow>();
            foreach (DataRow row in metaData.Rows)
            {
                if (schema == null) schema = row["TABLE_SCHEMA"].ToString();
                metaHash.Add(row["COLUMN_NAME"].ToString(), row);
            }
            if (schema == null) schema = "dbo";

            if (!metaData.Columns.Contains("TYPE_NAME")) metaData.Columns.Add("TYPE_NAME");
            if (!metaData.Columns.Contains("TYPE_NAME_COMPLETE")) metaData.Columns.Add("TYPE_NAME_COMPLETE");
            if (!metaData.Columns.Contains("IS_AUTO_KEY"))
            {
                DataColumn col = new DataColumn("IS_AUTO_KEY", typeof(bool));
                col.DefaultValue = false;
                metaData.Columns.Add(col);
            }
            foreach (DataRow extraRow in extraInfo.Rows)
            {
                string colName = extraRow["name"].ToString();

                /*
                C.name, C.usertype, C.scale, C.prec as precision, 
                C.length, CONVERT(bit,(C.status & 0x08)) as CanBeNull, 
                T.name as datatype, M.text as RowDefault, CONVERT(bit,(C.status & 0x80)) as IsIdentity
                */

                string dataType = extraRow["datatype"].ToString();
                int usertype = Convert.ToInt32(extraRow["usertype"]);

                int? length = null, scale = null, precision = null;
                if (extraRow["length"] != DBNull.Value) length = Convert.ToInt32(extraRow["length"]);
                if (extraRow["scale"] != DBNull.Value) scale = Convert.ToInt32(extraRow["scale"]);
                if (extraRow["precision"] != DBNull.Value) precision = Convert.ToInt32(extraRow["precision"]);

                bool canBeNull = Convert.ToBoolean(extraRow["CanBeNull"]);
                string rowDefault = extraRow["RowDefault"].ToString();
                bool isIdentity = Convert.ToBoolean(extraRow["IsIdentity"]);
                bool hasDefault = (extraRow["RowDefault"] == DBNull.Value);
                int ordinal = Convert.ToInt32(extraRow["colid"]);

                DataRow row;
                if (metaHash.ContainsKey(colName))
                {
                    row = metaHash[colName];
                }
                else
                {
                    row = metaData.NewRow();

                    row["TABLE_SCHEMA"] = schema;
                    row["TABLE_NAME"] = dataType;
                    row["COLUMN_NAME"] = colName;
                    row["ORDINAL_POSITION"] = ordinal;
                    row["IS_NULLABLE"] = canBeNull;
                    row["DATA_TYPE"] = usertype;
                    if (length.HasValue) row["CHARACTER_MAXIMUM_LENGTH"] = length;
                    if (length.HasValue) row["CHARACTER_OCTET_LENGTH"] = length;
                    if (scale.HasValue) row["NUMERIC_SCALE"] = scale;
                    if (precision.HasValue) row["NUMERIC_PRECISION"] = precision;

                    metaData.Rows.Add(row);
                }

                row["TYPE_NAME"] = dataType;
                row["TYPE_NAME_COMPLETE"] = dataType;
                row["COLUMN_HASDEFAULT"] = hasDefault;
                row["COLUMN_DEFAULT"] = rowDefault;
                row["IS_AUTO_KEY"] = hasDefault;
            }
        }


        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }

        private bool IsIntialized { get { return (context != null); } }

        private string GetDatabaseName()
        {
            string dbname = string.Empty;
            string connstr = this.context.ConnectionString;
            int len = 8, idx = connstr.IndexOf("Catalog=", StringComparison.CurrentCultureIgnoreCase);
            if (idx < 0)
            {
                len = 9;
                idx = connstr.IndexOf("Catalog =", StringComparison.CurrentCultureIgnoreCase);
            }

            if (idx >= 0)
            {
                dbname = connstr.Substring(idx + len);
                idx = dbname.IndexOf(";", StringComparison.CurrentCultureIgnoreCase);
                if (idx > 0) dbname = dbname.Substring(0, idx);
            }
            return dbname;
        }

        private bool HasDefaultDefined
        {
            get
            {
                if ((this.context.ConnectionString.IndexOf("Catalog=", StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (this.context.ConnectionString.IndexOf("Catalog = ", StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    return true;
                }
                return false;
            }
        }

        private void InitDatabase(OleDbConnection connection, string dbName)
        {
            if (string.IsNullOrEmpty(dbName) || connection == null) return;

            if (connection.State != ConnectionState.Open)
            {
                string newconnstr, connstr = connection.ConnectionString;
                int len = 8, idx = connstr.IndexOf("Catalog=", StringComparison.CurrentCultureIgnoreCase);
                if (idx < 0)
                {
                    len = 9;
                    idx = connstr.IndexOf("Catalog =", StringComparison.CurrentCultureIgnoreCase);
                }

                if (idx < 0)
                {
                    newconnstr = connstr;
                    if (!connstr.EndsWith(";")) newconnstr += ";";
                    newconnstr += "Catalog=" + dbName;
                }
                else
                {
                    idx += len;
                    newconnstr = connstr.Substring(0, idx) + dbName;

                    int end = connstr.IndexOf(";", idx);
                    if (end > 0) newconnstr += connstr.Substring(end);
                }

                connection.ConnectionString = newconnstr;
                connection.Open();
            }

            OleDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = "use [" + dbName + "];";
            cmd.ExecuteNonQuery();
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            catch { }
        }

        #endregion

        #region IMyMetaPluginExt Members

        public void ChangeDatabase(IDbConnection conn, string dbName)
        {
            if (conn is OleDbConnection)
            {
                OleDbConnection connection = conn as OleDbConnection;
                if (connection.State != ConnectionState.Open) connection.Open();

                OleDbCommand cmd = connection.CreateCommand();
                cmd.CommandText = "use [" + dbName + "];";
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetProviderTypes(string database)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
