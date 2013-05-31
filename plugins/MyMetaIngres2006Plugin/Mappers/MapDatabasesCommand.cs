using System;
using System.Collections.Generic;
using System.Data;

namespace MyMeta.Plugins
{
    class MapDatabasesCommand : IMyMetaMapper<DataTable>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;

        public MapDatabasesCommand(IMyMetaPluginContext context, IDataAccess dataAccess)
        {
            _context = context;
            _dataAccess = dataAccess;
        }
        
        public DataTable Execute()
        {
            String sql = "SELECT dbmsinfo('database')";

            return _dataAccess.ExecuteReader<DataTable>(sql, new MapperDelegate<DataTable>(Mapper));            
        }

        private DataTable Mapper(IDataReader reader)
        {
            DataTable metaData = _context.CreateDatabasesDataTable();

            while (reader.Read())
            {
                DataRow row = metaData.NewRow();

                //table.Columns.Add("CATALOG_NAME", Type.GetType("System.String"));
                //table.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
                //table.Columns.Add("SCHEMA_NAME", Type.GetType("System.String"));
                //table.Columns.Add("SCHEMA_OWNER", Type.GetType("System.String"));
                //table.Columns.Add("DEFAULT_CHARACTER_SET_CATALOG", Type.GetType("System.String"));
                //table.Columns.Add("DEFAULT_CHARACTER_SET_SCHEMA", Type.GetType("System.String"));
                //table.Columns.Add("DEFAULT_CHARACTER_SET_NAME", Type.GetType("System.String"));

                row["CATALOG_NAME"] = reader.GetString(0).Trim();

                metaData.Rows.Add(row);
            }

            metaData.AcceptChanges();

            return metaData;
        }
    }
}
