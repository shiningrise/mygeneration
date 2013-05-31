using System;
using System.Collections.Generic;
using System.Data;

namespace MyMeta.Plugins
{
    class MapTableIndexesCommand : IMyMetaMapper<DataTable>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;
        private String _database;
        private String _table;

        public MapTableIndexesCommand(IMyMetaPluginContext context, IDataAccess dataAccess, String database, String table)
        {
            _context = context;
            _dataAccess = dataAccess;
            _database = database;
            _table = table;
        }

        public DataTable Execute()
        {
            string[] tableNameParts = _table.Split(new char[] { '.' });

            String sql = String.Format(
                @"SELECT i.base_owner, i.base_name, i.index_owner, i.index_name, i.unique_rule, ic.column_name, ic.key_sequence, CASE WHEN c.constraint_name = i.index_name THEN 1 ELSE 0 END AS is_primary_key
                FROM iiindexes i
                INNER JOIN iiindex_columns ic ON i.index_name = ic.index_name
                LEFT JOIN iiconstraints c ON i.index_name = c.constraint_name AND constraint_type = 'P'
                WHERE i.base_owner = '{0}' 
                AND i.base_name = '{0}'", tableNameParts[0], tableNameParts[1]);

            return _dataAccess.ExecuteReader<DataTable>(sql, new MapperDelegate<DataTable>(Mapper));
        }

        private DataTable Mapper(IDataReader reader)
        {
            DataTable metaData = _context.CreateIndexesDataTable();

            while (reader.Read())
            {
                DataRow row = metaData.NewRow();

                //metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
                //metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
                //metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("INDEX_CATALOG", Type.GetType("System.String"));
                //metaData.Columns.Add("INDEX_SCHEMA", Type.GetType("System.String"));
                //metaData.Columns.Add("INDEX_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("PRIMARY_KEY", Type.GetType("System.Boolean"));
                //metaData.Columns.Add("UNIQUE", Type.GetType("System.Boolean"));
                //metaData.Columns.Add("CLUSTERED", Type.GetType("System.Boolean"));
                //metaData.Columns.Add("TYPE", Type.GetType("System.Int32"));
                //metaData.Columns.Add("FILL_FACTOR", Type.GetType("System.Int32"));
                //metaData.Columns.Add("INITIAL_SIZE", Type.GetType("System.Int32"));
                //metaData.Columns.Add("NULLS", Type.GetType("System.Int32"));
                //metaData.Columns.Add("SORT_BOOKMARKS", Type.GetType("System.Boolean"));
                //metaData.Columns.Add("AUTO_UPDATE", Type.GetType("System.Boolean"));
                //metaData.Columns.Add("NULL_COLLATION", Type.GetType("System.Int32"));
                //metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int64"));
                //metaData.Columns.Add("COLUMN_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("COLUMN_GUID", Type.GetType("System.Guid"));
                //metaData.Columns.Add("COLUMN_PROPID", Type.GetType("System.Int64"));
                //metaData.Columns.Add("COLLATION", Type.GetType("System.Int16"));
                //metaData.Columns.Add("CARDINALITY", Type.GetType("System.Decimal"));
                //metaData.Columns.Add("PAGES", Type.GetType("System.Int32"));
                //metaData.Columns.Add("FILTER_CONDITION", Type.GetType("System.String"));
                //metaData.Columns.Add("INTEGRATED", Type.GetType("System.Boolean"));

                row["TABLE_CATALOG"] = _database;
                row["TABLE_SCHEMA"] = reader.GetString(0).Trim();
                row["TABLE_NAME"] = reader.GetString(1).Trim();
                row["INDEX_CATALOG"] = _database;
                row["INDEX_SCHEMA"] = reader.GetString(2).Trim();
                row["INDEX_NAME"] = reader.GetString(3).Trim();
                row["PRIMARY_KEY"] = (reader.GetInt32(7) == 1);
                row["UNIQUE"] = (reader.GetString(4).Trim() == "U");
                row["ORDINAL_POSITION"] = reader.GetInt16(6);
                row["COLUMN_NAME"] = reader.GetString(5).Trim();

                metaData.Rows.Add(row);
            }

            metaData.AcceptChanges();

            return metaData;
        }

    }
}