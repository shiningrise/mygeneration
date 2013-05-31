using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyMeta.Plugins
{
    class MapTableCommand : IMyMetaMapper<DataTable>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;

        public MapTableCommand(IMyMetaPluginContext context, IDataAccess dataAccess)
        {
            _context = context;
            _dataAccess = dataAccess;
        }

        public DataTable Execute()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT TRIM(table_owner) AS table_owner, TRIM(table_owner) + '.' + table_name AS table_name, create_date, alter_date FROM iitables ");

            if (_context.IncludeSystemEntities == false)
                sql.Append("WHERE system_use = 'U' ");

            sql.Append("ORDER BY table_name");

            return _dataAccess.ExecuteReader<DataTable>(sql.ToString(), new MapperDelegate<DataTable>(Mapper));
        }

        private DataTable Mapper(IDataReader reader)
        {
            DataTable metaData = _context.CreateTablesDataTable();

            while (reader.Read())
            {
                DataRow row = metaData.NewRow();

                //metaData.Columns.Add("TABLE_CATALOG", Type.GetType("System.String"));
                //metaData.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
                //metaData.Columns.Add("TABLE_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("TABLE_TYPE", Type.GetType("System.String"));
                //metaData.Columns.Add("TABLE_GUID", Type.GetType("System.Guid"));
                //metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
                //metaData.Columns.Add("TABLE_PROPID", Type.GetType("System.Int64"));
                //metaData.Columns.Add("DATE_CREATED", Type.GetType("System.DateTime"));
                //metaData.Columns.Add("DATE_MODIFIED", Type.GetType("System.DateTime"));

                row["TABLE_SCHEMA"] = reader.GetString(0).Trim();
                row["TABLE_NAME"] = reader.GetString(1).Trim();
                row["DATE_CREATED"] = reader.GetDateTime(2);
                row["DATE_MODIFIED"] = reader.GetDateTime(3);

                metaData.Rows.Add(row);
            }

            metaData.AcceptChanges();

            return metaData;
        }
    }
}
