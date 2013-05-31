using System;
using System.Collections.Generic;
using System.Data;

namespace MyMeta.Plugins
{
    class MapForeignKeys : IMyMetaMapper<DataTable>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;
        private String _database;
        private String _table;

        public MapForeignKeys(IMyMetaPluginContext context, IDataAccess dataAccess, String database, String table)
        {
            _context = context;
            _dataAccess = dataAccess;
            _database = database;
            _table = table;
        }

        public System.Data.DataTable Execute()
        {
            //TODO Define SQL for MapForeignKeys.Execute()
            String sql = String.Format("", _table);

            return _dataAccess.ExecuteReader<DataTable>(sql, new MapperDelegate<DataTable>(Mapper));
        }

        private DataTable Mapper(IDataReader reader)
        {
            DataTable metaData = _context.CreateForeignKeysDataTable();

            while (reader.Read())
            {
                DataRow row = metaData.NewRow();

                //metaData.Columns.Add("PK_TABLE_CATALOG", Type.GetType("System.String"));
                //metaData.Columns.Add("PK_TABLE_SCHEMA", Type.GetType("System.String"));
                //metaData.Columns.Add("PK_TABLE_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("PK_COLUMN_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("PK_COLUMN_GUID", Type.GetType("System.Guid"));
                //metaData.Columns.Add("PK_COLUMN_PROPID", Type.GetType("System.Int64"));
                //metaData.Columns.Add("FK_TABLE_CATALOG", Type.GetType("System.String"));
                //metaData.Columns.Add("FK_TABLE_SCHEMA", Type.GetType("System.String"));
                //metaData.Columns.Add("FK_TABLE_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("FK_COLUMN_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("FK_COLUMN_GUID", Type.GetType("System.Guid"));
                //metaData.Columns.Add("FK_COLUMN_PROPID", Type.GetType("System.Int64"));
                //metaData.Columns.Add("ORDINAL", Type.GetType("System.Int64"));
                //metaData.Columns.Add("UPDATE_RULE", Type.GetType("System.String"));
                //metaData.Columns.Add("DELETE_RULE", Type.GetType("System.String"));
                //metaData.Columns.Add("PK_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("FK_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("DEFERRABILITY", Type.GetType("System.Int16"));
                
                row["PK_TABLE_CATALOG"] = _database;
                row["PK_TABLE_SCHEMA"] = reader.GetString(0).Trim();
                row["PK_TABLE_NAME"] = reader.GetString(1).Trim();
                row["PK_COLUMN_NAME"] = _database;
                row["FK_TABLE_CATALOG"] = reader.GetString(2).Trim();
                row["FK_TABLE_SCHEMA"] = reader.GetString(3).Trim();
                row["FK_TABLE_NAME"] = (reader.GetInt32(7) == 1);
                row["FK_COLUMN_NAME"] = (reader.GetString(4).Trim() == "U");
                row["PK_NAME"] = reader.GetInt16(6);
                row["FK_NAME"] = reader.GetString(5).Trim();

                metaData.Rows.Add(row);
            }

            metaData.AcceptChanges();

            return metaData;
        }

    }
}