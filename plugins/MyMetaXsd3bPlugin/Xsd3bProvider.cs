using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Dl3bak.Data.Xsd3b;
using Dl3bak.Data.OleDB;
using MyMeta.Plugins.Xsd3b;
using System.Data.OleDb;
using System.Reflection;
using System.Windows.Forms;
using Dl3bak.Data.Xsd3b.Plugin;
using System.IO;

namespace MyMeta.Plugins
{
    /// <summary>
    /// MyMeta.Plugins.Xsd3b.dll providerplugin for MyGeneration
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
    public class Xsd3bProvider : IMyMetaPlugin
    {
#if PLUGINS_FROM_SUBDIRS
        static Xsd3bProvider()
        {
            string mainPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string dllPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (mainPath != dllPath)
                AppDomain.CurrentDomain.AppendPrivatePath(dllPath);

        }
#endif
        #region IMyMetaPlugin Interface

        #region context information
        private IMyMetaPluginContext context;

        void IMyMetaPlugin.Initialize(IMyMetaPluginContext context)
        {
            TraceContext("Initialize(ConnectionString=" + context.ConnectionString + ")");
            this.context = context;
        }

        string IMyMetaPlugin.ProviderName
        {
            get { return @"Xsd3b (xml,xsd,uml,er)"; }
        }

        string IMyMetaPlugin.ProviderUniqueKey
        {
            get { return @"XSD3B"; }
        }

        string IMyMetaPlugin.ProviderAuthorInfo
        {
            get 
            { 
                return
@"Xsd3b is a provider and a consumer plugin for MyGeneration.

use xml,xsd,uml,er,xsd3b ... files as datasource instead of an online sqldatabase or create xml from any MyGeneration Datasource.

(c) 2007 by dl3bak@qsl.net

provider features: use xml based File as datasource instead of an online sqldatabase. New import fileformats are created through xsl.

consumer features: transform the databaseschema from any MyGeneration Datasource into an xml file, either microsofts version of *.xsd ('typed dataset') or the much simpler and more powerful *.xsd3b. This file can be used as a MyGeneration Datasource.

perspective: with the Xsd3b plugin you can use MyGeneration without the need to have an online database at codegeneration. If you already have 'dotnet typed datasets' (*.xsd) you can use MyGeneration to create additional sourcefiles."; 
            }
        }

        Uri IMyMetaPlugin.ProviderAuthorUri
        {
            get
            {
                string name = @"http://mygeneration.wiki.sourceforge.net/Xsd3b"; // getHelpFileUrl();
                if (name == null)
                    name = @"http://www.qsl.net/dl3bak/";

                return new Uri(name);
            }
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
            get 
            {
                string binDir = Path.GetDirectoryName(this.GetType().Assembly.Location);
                binDir = Path.Combine(Path.Combine(binDir, "Templates"), "xsd3b");
                string[] fileNames = Directory.GetFiles(binDir, "ExampleOracle2.xdm");
                if (fileNames.Length > 0)
                    return "FILETYPE=DATAMODELL;FILENAME=" + fileNames[0];

                fileNames = Directory.GetFiles(binDir, "Example*.*");
                if (fileNames.Length > 0)
                    return fileNames[0];

                return @"C:\Test\Northwind.xsd3b"; 
            }
        }

        IDbConnection IMyMetaPlugin.NewConnection
        {
            get
            {
                myConnection = null;
                return MyConnection; // create a new
            }
        }

        string IMyMetaPlugin.DefaultDatabase
        {
            get
            {
                return this.context.ConnectionString;
            }
        }
        #endregion

        #region Data access
        DataTable IMyMetaPlugin.Databases
        {
            get
            {
                TraceContext("Databases");

                DataTable metaData = new DataTable();
                SchemaXsd3bEx db = null;

                try
                {
                    metaData = context.CreateDatabasesDataTable();

                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    db = Xsd3b;

                    set(row,"CATALOG_NAME",db.GetSchemaDefinitionRow().SchemaID);
                    set(row,"DESCRIPTION",db.GetSchemaDefinitionRow().SchemaComment);
                }
                finally
                {
                }
                metaData.TableName = "Databases";
                TraceTable("Databases" , metaData);

                return metaData;
            }
        }

        #region Tables
        DataTable IMyMetaPlugin.GetTables(string databaseName)
        {
            TraceContext("GetTables: db=" + databaseName);
            DataTable metaData = new DataTable();
            SchemaXsd3bEx db = null;

            try
            {
                metaData = context.CreateTablesDataTable();

                db = Xsd3b;
                foreach (TableDefinitionRow tab in db.TableDefinition)
                {
                    if (tab.DBTableType.ToUpper().IndexOf("TABLE") >= 0)
                    {
                        DataRow row = metaData.NewRow();
                        metaData.Rows.Add(row);

                        set(row, "TABLE_NAME", tab.TableName);
                        set(row, "DESCRIPTION", tab.TableComment);
                    }
                }
            }
            finally
            {
            }
            metaData.TableName = "GetTables";
            TraceTable("GetTables-" + databaseName, metaData);

            return metaData;
        }

        public DataTable GetTableColumns(string databaseName, string tableName)
        {
            TraceContext("GetTableColumns: tab=" + tableName);
            DataTable metaData = new DataTable();
            try
            {
                metaData = context.CreateColumnsDataTable();

                CreateColumns(tableName, metaData,null);

            }
            finally
            {
            }

            metaData.TableName = "GetTableColumns";
            TraceTable("GetTableColumns-" + databaseName, metaData);
            return metaData;
        }

        private void CreateColumns(string tableName, DataTable metaData, String filter)
        {
            TableDefinitionRow tab = GetTable(tableName);

            int ORDINAL_POSITION = 0;
            foreach (FieldDefinitionRow c in tab.GetFieldDefinitionRows())
            {
                if ((filter == null) || (c.DBFieldType.ToUpper().IndexOf(filter) >= 0))
                {
                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    set(row, "TABLE_NAME", tab.TableName);
                    setAndCreate(row, "COLUMN_NAME", c.FieldName);
                    set(row, "ORDINAL_POSITION", ORDINAL_POSITION++);
                    set(row, "IS_NULLABLE", c.AllowDBNull);
                    set(row, "COLUMN_HASDEFAULT", (c.DefaultValue.Length > 0));
                    set(row, "COLUMN_DEFAULT", c.DefaultValue);
                    set(row, "IS_AUTO_KEY", c.AutoIncrement);
                    set(row, "AUTO_KEY_SEED", c.AutoIncrementSeed);
                    set(row, "AUTO_KEY_INCREMENT", c.AutoIncrementStep);

                    set(row, "TYPE_NAME", c.DataType);
                    // set(row,"TYPE_NAME_COMPLETE",GetDbDataTypeName(c)); //  .DataType);
                    SetDbDataTypeName(c, row); //  .DataType);

                    set(row, "NUMERIC_PRECISION", 0);
                    set(row, "NUMERIC_SCALE", 0);
                    set(row, "CHARACTER_MAXIMUM_LENGTH", c.GetDataTypStringLen());
                    set(row, "CHARACTER_OCTET_LENGTH", c.GetDataTypStringLen());
                    set(row, "DESCRIPTION", c.FieldComment);

                    // different names for ProcedureParameter instead of COLUMN
                    set(row, "PROCEDURE_NAME", tab.TableName);
                    set(row, "PARAMETER_NAME", c.FieldName);
                    set(row, "PARAMETER_HASDEFAULT", (c.DefaultValue.Length > 0));
                    set(row, "PARAMETER_DEFAULT", c.DefaultValue);
                    // PARAMETER_TYPE

                    // different names for ResultColumn instead of COLUMN
                    set(row, "NAME", c.FieldName);

                }
            }
        }

        #region datatype-mapping
        private const string sqlPrefix = "sql.";
        private const string systemPrefix = DataTypeMapping.prefixSystemDataType;

        private void SetDbDataTypeName(FieldDefinitionRow c, DataRow row)
        {
            DataTypeDefinitionRow fieldDataType = c.DataTypeDefinitionRow;
            string name;
            int typID = 0;

            if (fieldDataType != null)
            {
                #region find type "sql." that matches oledb
                DataTypeDefinitionRow datatyp = fieldDataType.FindBaseTypeByPrefix(sqlPrefix);

                while ((typID == 0) && (datatyp != null) && (datatyp != datatyp.DataTypeDefinitionRowParent))
                {
                    name = datatyp.DataTypeName.Substring(sqlPrefix.Length);
                    typID = Mapping.GetOleDbTypeIDFromOleDbTypeName(name);
                    if (typID == 0)
                    {
                        datatyp = datatyp.DataTypeDefinitionRowParent;
                        datatyp = datatyp.FindBaseTypeByPrefix(sqlPrefix);
                    }
                }
                #endregion

                #region if not found find typ "System." that has a mapping to oledb
                if (typID == 0)
                {
                    datatyp = fieldDataType.FindBaseTypeByPrefix(systemPrefix);

                    while ((typID == 0) && (datatyp != null) && (datatyp != datatyp.DataTypeDefinitionRowParent))
                    {
                        name = datatyp.DataTypeName.Substring(systemPrefix.Length);
                        typID = Mapping.GetOleDbTypeIDFromSystemTypeName(name);
                        if (typID == 0)
                        {
                            datatyp = datatyp.DataTypeDefinitionRowParent;
                            datatyp = datatyp.FindBaseTypeByPrefix(systemPrefix);
                        }
                    }
                }
                #endregion
                if (typID != 0)
                {
                    setAndCreate(row, "DATA_TYPE", typID);
                    set(row, "PARAMETER_TYPE", typID);
                    name = ((OleDbType)typID).ToString();
                }
                else
                {
                    name = fieldDataType.DataTypeName;
                }

            }
            else // else (fieldDataType === null)
            {
                name = c.DataType.Substring(systemPrefix.Length);
            }
            set(row, "TYPE_NAME", name);
            int len = c.GetDataTypStringLen();
            if (len > 0)
                name += "(" + len.ToString() + ")";
            set(row, "TYPE_NAME_COMPLETE", name);

        }

        #endregion

        List<string> IMyMetaPlugin.GetPrimaryKeyColumns(string databaseName, string tableName)
        {
            TraceContext("GetPrimaryKeyColumns: tab=" + tableName);
            List<string> primaryKeys = new List<string>();
            SchemaXsd3bEx db = null;

            try
            {
                db = Xsd3b;
                TableDefinitionRow tab = GetTable(tableName);
                foreach (FieldDefinitionRow c in tab.GetPKs())
                {
                    primaryKeys.Add(c.FieldName);
                }
            }
            finally
            {
            }

            return primaryKeys;
        }

        DataTable IMyMetaPlugin.GetTableIndexes(string databaseName, string tableName)
        {
            TraceContext("GetTableIndexes: tab=" + tableName);
            DataTable metaData = new DataTable();
            SchemaXsd3bEx db = null;

            try
            {
                metaData = context.CreateIndexesDataTable();

                db = Xsd3b;
                TableDefinitionRow tab = GetTable(tableName);

                CreateIndexesFromPKs(metaData, tab);

                CreateIndexesFromRelations(metaData, tab);

            }
            finally
            {
            }
            metaData.TableName = "GetTableIndexes";
            TraceTable("GetTableIndexes-" + databaseName, metaData);

            return metaData;
        }

        private void CreateIndexesFromRelations(DataTable metaData, TableDefinitionRow tab)
        {
            foreach (RelationDefinitionRow rel in tab.GetChildRelationDefinitionRows())
            {
                int ordinal = 1;
                foreach (FieldRelationDefinitionRow f in rel.GetFieldRelationDefinitionRows())
                {
                    DataRow row = metaData.NewRow();
                    metaData.Rows.Add(row);

                    // <Indexes TABLE_NAME="Customers" INDEX_NAME="City" 
                    //      PRIMARY_KEY="false" UNIQUE="false" CLUSTERED="false" 
                    //      TYPE="1" FILL_FACTOR="100" INITIAL_SIZE="4096" 
                    //      NULLS="0" SORT_BOOKMARKS="false" AUTO_UPDATE="true" 
                    //      NULL_COLLATION="4" ORDINAL_POSITION="1" COLUMN_NAME="City" 
                    //      COLLATION="1" CARDINALITY="69" PAGES="1" INTEGRATED="true" />                        
                    set(row, "TABLE_CATALOG", DBNull.Value); // GetDatabaseName();
                    set(row, "TABLE_SCHEMA", DBNull.Value);
                    set(row, "TABLE_NAME", tab.TableName);
                    set(row, "INDEX_CATALOG", DBNull.Value);
                    set(row, "INDEX_NAME", rel.RelationName);

                    set(row, "PRIMARY_KEY", false);
                    set(row, "UNIQUE", false);
                    set(row, "ORDINAL_POSITION", ordinal++);
                    set(row, "COLUMN_NAME", f.ParentFieldName);

                    set(row, "DESCRIPTION", rel.RelationComment);
                }
            }
        }

        private void CreateIndexesFromPKs(DataTable destMetaData, TableDefinitionRow srcTab)
        {
            int ordinal = 1;
            foreach (FieldDefinitionRow field in srcTab.GetPKs())
            {
                DataRow row = destMetaData.NewRow();
                destMetaData.Rows.Add(row);

                // <Indexes TABLE_NAME="Customers" INDEX_NAME="PrimaryKey" 
                //      PRIMARY_KEY="true" UNIQUE="true" CLUSTERED="false" TYPE="1" FILL_FACTOR="100" INITIAL_SIZE="4096" NULLS="1" SORT_BOOKMARKS="false" AUTO_UPDATE="true" NULL_COLLATION="4" 
                //      ORDINAL_POSITION="1" COLUMN_NAME="CustomerID" COLLATION="1" CARDINALITY="97" PAGES="1" INTEGRATED="true" />
                set(row, "TABLE_CATALOG", DBNull.Value);
                set(row, "TABLE_SCHEMA", DBNull.Value);
                set(row, "TABLE_NAME", srcTab.TableName);
                set(row, "INDEX_CATALOG", DBNull.Value);
                set(row, "INDEX_NAME", "PrimaryKey");
                set(row, "PRIMARY_KEY", true);
                set(row, "UNIQUE", true);
                set(row, "ORDINAL_POSITION", ordinal++);
                set(row, "COLUMN_NAME", field.FieldName);
            }
        }

        DataTable IMyMetaPlugin.GetForeignKeys(string databaseName, string tableName)
        {
            TraceContext("GetForeignKeys: tab=" + tableName);
            DataTable metaData = new DataTable();
            SchemaXsd3bEx db = null;

            try
            {
                metaData = context.CreateForeignKeysDataTable();

                db = Xsd3b;
                TableDefinitionRow tab = GetTable(tableName);

                // FKs = tab.GetParentRelationDefinitionRows().GetFieldRelationDefinitionRows().
                foreach (RelationDefinitionRow rel in tab.GetParentRelationDefinitionRows())
                    AddForeignKey(metaData, rel);

                // bugfix 20071022 MyGen always have both parent-child and child-parent as FK
                foreach (RelationDefinitionRow rel in tab.GetChildRelationDefinitionRows())
                    AddForeignKey(metaData, rel);
            }
            finally
            {
            }
            metaData.TableName = "GetForeignKeys";
            TraceTable("GetForeignKeys-" + databaseName, metaData);

            return metaData;
        }

        private void AddForeignKey(DataTable metaData, RelationDefinitionRow rel)
        {
            int ordinal = 1;
            foreach (FieldRelationDefinitionRow f in rel.GetFieldRelationDefinitionRows())
            {
                DataRow row = metaData.NewRow();
                metaData.Rows.Add(row);

                set(row, "PK_TABLE_CATALOG", DBNull.Value); // GetDatabaseName();
                set(row, "PK_TABLE_SCHEMA", DBNull.Value);
                set(row, "FK_TABLE_CATALOG", DBNull.Value);
                set(row, "FK_TABLE_SCHEMA", DBNull.Value);
                set(row, "FK_TABLE_NAME", rel.ChildTableName);
                set(row, "PK_TABLE_NAME", rel.ParentTableName);
                set(row, "ORDINAL", ordinal++);
                set(row, "FK_NAME", rel.RelationName);
                set(row, "PK_NAME", "PrimaryKey");
                set(row, "PK_COLUMN_NAME", f.ParentFieldName);
                set(row, "FK_COLUMN_NAME", f.ChildFieldName);

                set(row, "UPDATE_RULE", Xsd3bRule2OleRule(rel.UpdateRule)); //  relInfo.UpdateIntegrity;
                set(row, "DELETE_RULE", Xsd3bRule2OleRule(rel.DeleteRule));
            }
        }

        private static string Xsd3bRule2OleRule(string action)
        {
            if (action.CompareTo("None") == 0)
                return "NO ACTION";
            return action.ToUpper();
            // oledb: CASCADE,NO ACTION, ???,???
            // xsd3b: 'Cascade', 'None', 'SetDefault' or 'SetNull'
        }
        #endregion

        #region Views
        DataTable IMyMetaPlugin.GetViews(string databaseName)
        {
            TraceContext("GetViews: db=" + databaseName);
            DataTable metaData = new DataTable();
            SchemaXsd3bEx db = null;

            try
            {
                metaData = context.CreateViewsDataTable();

                db = Xsd3b;
                foreach (TableDefinitionRow tab in db.TableDefinition)
                {
                    if (tab.DBTableType.ToUpper().IndexOf("VIEW") >= 0)
                    {
                        DataRow row = metaData.NewRow();
                        metaData.Rows.Add(row);

                        set(row, "TABLE_NAME", tab.TableName);
                        set(row, "DESCRIPTION", tab.TableComment);
                    }
                }
            }
            finally
            {
            }
            metaData.TableName = "GetViews";
            TraceTable("GetViews-" + databaseName, metaData);

            return metaData;
        }

        DataTable IMyMetaPlugin.GetViewColumns(string databaseName, string viewName)
        {
            TraceContext("GetViewColumns: view=" + viewName);
            DataTable metaData = new DataTable();
            try
            {
                metaData = context.CreateColumnsDataTable();

                CreateColumns(viewName, metaData, null);

            }
            finally
            {
            }

            metaData.TableName = "GetViewColumns";
            TraceTable("GetViewColumns-" + databaseName, metaData);
            return metaData;
        }

        List<string> IMyMetaPlugin.GetViewSubViews(string databaseName, string viewName)
        {
            TraceContext("GetViewSubViews: viewName=" + viewName);
            return new List<string>();
        }

        List<string> IMyMetaPlugin.GetViewSubTables(string databaseName, string viewName)
        {
            TraceContext("GetViewSubTables: viewName=" + viewName);
            return new List<string>();
        }
        #endregion

        #region Procedures
        DataTable IMyMetaPlugin.GetProcedures(string databaseName)
        {
            TraceContext("GetProcedures: db=" + databaseName);
            DataTable metaData = new DataTable();
            SchemaXsd3bEx db = null;

            try
            {
                metaData = context.CreateProceduresDataTable();

                db = Xsd3b;
                foreach (TableDefinitionRow tab in db.TableDefinition)
                {
                    if (tab.DBTableType.ToUpper().IndexOf("PROC") >= 0) // PROCEDURE, SYSTEM PROCEDURE
                    {
                        DataRow row = metaData.NewRow();
                        metaData.Rows.Add(row);

                        set(row, "PROCEDURE_NAME", tab.TableName);
                        set(row, "DESCRIPTION", tab.TableComment);

                    }
                }
            }
            finally
            {
            }
            metaData.TableName = "GetProcedures";
            TraceTable("GetProcedures-" + databaseName, metaData);

            return metaData;
        }

        DataTable IMyMetaPlugin.GetProcedureParameters(string databaseName, string procedureName)
        {
            TraceContext("GetProcedureParameters: proc=" + procedureName);
            DataTable metaData = new DataTable();
            try
            {
                metaData = context.CreateParametersDataTable();

                CreateColumns(procedureName, metaData, "_IN"); // *_IN* = PROC_INPUT or PROC_IN_OUT
                // PROC_OUTPUT, PROC_IN_OUT, PROC_RETURN

            }
            finally
            {
            }

            metaData.TableName = "GetProcedureParameters";
            TraceTable("GetProcedureParameters-" + databaseName, metaData);
            return metaData;
        }

        DataTable IMyMetaPlugin.GetProcedureResultColumns(string databaseName, string procedureName)
        {
            TraceContext("GetProcedureResultColumns: proc=" + procedureName);
            DataTable metaData = new DataTable();
            try
            {
                metaData = context.CreateResultColumnsDataTable();

                CreateColumns(procedureName, metaData, "_RET"); // *_RET* = PROC_RETURN
                CreateColumns(procedureName, metaData, "_OUT"); // *_OUT* = PROC_OUTPUT, PROC_IN_OUT
            }
            finally
            {
            }

            metaData.TableName = "GetProcedureResultColumns";
            TraceTable("GetProcedureResultColumns-" + databaseName, metaData);
            return metaData;
        }
        #endregion
        #region unsupported api Domains
        DataTable IMyMetaPlugin.GetDomains(string databaseName)
        {
            TraceContext("GetDomains: db=" + databaseName);
            return new DataTable();
        }


        #endregion
        #endregion

        #endregion

        #region Internal Methods

        private bool IsIntialized
        {
            get
            {
                return (context != null);
            }
        }

        #endregion

        private Xsd3bConnection myConnection = null;

        private Xsd3bConnection MyConnection
        {
            get
            {
                if (myConnection == null)
                {
                    this.myConnection = new Xsd3bConnection();
                    this.myConnection.ConnectionString = this.context.ConnectionString;                    
                }

                return myConnection;

            }
        }

        protected SchemaXsd3bEx Xsd3b
        {
            get
            {
                return MyConnection.Xsd3b;
            }
        }

        private TableDefinitionRow GetTable(string tableName)
        {
            TableDefinitionRow tab = Xsd3b.TableDefinition.FindByTableName(tableName);
            return tab;
        }

        private void setAndCreate(DataRow row, string fieldName, Object value)
        {
            DataColumn col = row.Table.Columns[fieldName];
            if (col == null)
                col = row.Table.Columns.Add(fieldName);

            if (col != null)
                row[col] = value;
        }
        private void set(DataRow row, string fieldName, Object value)
        {
            // row["TABLE_CATALOG"] = DBNull.Value; // GetDatabaseName();
            DataColumn col = row.Table.Columns[fieldName];
            if (col != null)
                row[col] = value;
        }


        #region debug support
        [System.Diagnostics.Conditional("DEBUG")]
        private void TraceTable(string context, DataTable metaData)
        {
            DataSet ds = new DataSet(string.Format("{0} - tab:{1}", context, metaData.TableName));
            ds.Tables.Add(metaData);
            Dl3bak.Data.DatasetUtil.SetColMapping(metaData, MappingType.Attribute);
            // System.Diagnostics.Debug.WriteLine(ds.GetXmlSchema());
            System.Diagnostics.Debug.WriteLine(ds.GetXml());
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private void TraceContext(string msg)
        {
            System.Diagnostics.Debug.WriteLine("invoked " + msg);            
        }

        #endregion

        #region IMyMetaPlugin Members

        /// <summary>
        /// eve 20070626: This method was added to MyMeta.dll-1.2.0.0 that ships with MyGeneration.exe-1.2.0.7
        /// It was not part of MyMeta.dll-1.2.0.0 that ships with MyGeneration.exe-1.2.0.2
        /// </summary>
        /// <param name="myMetaObject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            if (string.Compare("CanBrowseDatabase", key, true) == 0)
                return Dl3bak.Data.Xsd3b.Plugin.PlugInSupport.GetPluginDescripton();
            if (string.Compare("BrowseDatabase", key, true) == 0)
                return this.browseDatabase();

            return null;
        }

        private static OpenFileDialog dlg = null;
        private object browseDatabase()
        {
            if (dlg == null)
            {
                dlg = new OpenFileDialog();
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.DereferenceLinks = true;
                if (null != getHelpFileUrl())
                {
                    dlg.HelpRequest += new EventHandler(onFileOpenHelprequest);
                    dlg.ShowHelp = true;
                }
                string allDatamodellTypes = ""
                    + "Extendet Schema (*.xsd3b;*.xsd3b.xml)|*.xsd3b;*.xsd3b.xml|"
                    + PlugInSupport.GetSupportedImportFileTypes();
                string dataModellExtensions = getSupportedFilter(allDatamodellTypes);
                dlg.Filter = ""
                    + allDatamodellTypes
                    + "Datamodell ("
                    + getShortFilter(dataModellExtensions)
                    + ")|" + dataModellExtensions + "|"
                    + "All files (*.*)|*.*";
                dlg.AddExtension = true;
                dlg.ValidateNames = true;
                dlg.SupportMultiDottedExtensions = true;
                dlg.Title = "Xsd3b: Open Datamodell";
            }
            PlugInSupport.SetSelection(dlg, MyConnection.ConnectionString);
            // dlg.FileName = MyConnection.ConnectionString;
            // dlg.FilterIndex = filterIndexLoad + DLG_INDEX_OFFSET;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                return PlugInSupport.GetConnectionString(dlg);
                // return dlg.FileName;
            }

            return null;
        }

        private string getShortFilter(string dataModellExtensions)
        {
            StringBuilder res = new StringBuilder();
            string[] types = dataModellExtensions.Split(';');
            foreach(string name in types)
                if (!name.ToLower().EndsWith(".xml"))
                    res.Append(name).Append(";");
            res.Append("*.xml");
            return res.ToString();
        }

        private string getSupportedFilter(string allDatamodellTypes)
        {
            string[] types = allDatamodellTypes.Split('|');
            StringBuilder res = new StringBuilder(types[1]);
            for (int i = 3; i < types.Length; i += 2)
                res.Append(";").Append(types[i]);
            return res.ToString();
        }

        void onFileOpenHelprequest(object sender, EventArgs e)
        {
            Help.ShowHelp(null, getHelpFileUrl(), HelpNavigator.Topic
                , "Xsd3bOpenModellFileDialog.htm"); // "");
        }

        private string getHelpFileUrl()
        {
            string binDir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string[] fileNames = Directory.GetFiles(binDir, "*xsd3b*.chm");
            if (fileNames.Length > 0)
                return fileNames[0];

            return null;
        }

        #endregion
    }
}
