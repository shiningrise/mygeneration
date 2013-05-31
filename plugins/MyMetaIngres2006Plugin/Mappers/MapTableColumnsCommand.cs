using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyMeta.Plugins
{
    class MapTableColumnsCommand : IMyMetaMapper<DataTable>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;
        private String _database;
        private String _table;

        public MapTableColumnsCommand(IMyMetaPluginContext context, IDataAccess dataAccess, String database, String table)
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
                @"SELECT TRIM(table_owner) AS table_owner, table_name, column_name, column_sequence, column_defaults, 
                column_default_val, column_nulls, column_datatype, column_length, column_scale
                FROM iicolumns 
                WHERE table_owner = '{0}' AND table_name = '{1}'
                ORDER BY column_sequence", tableNameParts[0], tableNameParts[1]);

            return _dataAccess.ExecuteReader<DataTable>(sql, new MapperDelegate<DataTable>(Mapper));
        }

        private DataTable Mapper(IDataReader reader)
        {
            DataTable metaData = _context.CreateColumnsDataTable();

            while (reader.Read())
            {
                DataRow row = metaData.NewRow();

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

                row["TABLE_CATALOG"] = _database;
                row["TABLE_SCHEMA"] = reader.GetString(0);
                row["TABLE_NAME"] = reader.GetString(1);
                row["COLUMN_NAME"] = reader.GetString(2).Trim();
                row["ORDINAL_POSITION"] = reader.GetInt32(3);
                row["COLUMN_HASDEFAULT"] = (reader.GetChar(4) == 'Y');

                if (!reader.IsDBNull(5))
                    row["COLUMN_DEFAULT"] = reader.GetString(5);

                row["IS_NULLABLE"] = (reader.GetChar(6) == 'Y');
                row["TYPE_NAME"] = reader.GetString(7).Trim();
                row["TYPE_NAME_COMPLETE"] = reader.GetString(7).Trim();
                row["CHARACTER_MAXIMUM_LENGTH"] = reader.GetInt32(8);
                row["NUMERIC_SCALE"] = reader.GetInt32(9);
                row["IS_COMPUTED"] = false;
                row["IS_AUTO_KEY"] = false;

                metaData.Rows.Add(row);
            }

            metaData.AcceptChanges();

            return metaData;
        }
    }
}