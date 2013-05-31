using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyMeta.Plugins
{
    class MapProceduresCommand : IMyMetaMapper<DataTable>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;
        private string _database;

        public MapProceduresCommand(IMyMetaPluginContext context, IDataAccess dataAccess, string database)
        {
            _context = context;
            _dataAccess = dataAccess;
            _database = database;
        }

        public System.Data.DataTable Execute()
        {
            String sql =
                @"SELECT DISTINCT TRIM(procedure_owner) AS procedure_owner, TRIM(procedure_owner) + '.' + procedure_name AS procedure_name
                FROM iiprocedures 
                WHERE system_use = 'U' 
                ORDER BY procedure_name";

            return _dataAccess.ExecuteReader<DataTable>(sql, new MapperDelegate<DataTable>(Mapper));
        }

        private DataTable Mapper(IDataReader reader)
        {
            DataTable metaData = _context.CreateProceduresDataTable();

            while (reader.Read())
            {
                DataRow row = metaData.NewRow();

                //metaData.Columns.Add("PROCEDURE_CATALOG", Type.GetType("System.String"));
                //metaData.Columns.Add("PROCEDURE_SCHEMA", Type.GetType("System.String"));
                //metaData.Columns.Add("PROCEDURE_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("PROCEDURE_TYPE", Type.GetType("System.Int16"));
                //metaData.Columns.Add("PROCEDURE_DEFINITION", Type.GetType("System.String"));
                //metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
                //metaData.Columns.Add("DATE_CREATED", Type.GetType("System.DateTime"));
                //metaData.Columns.Add("DATE_MODIFIED", Type.GetType("System.DateTime"));

                row["PROCEDURE_CATALOG"] = _database;
                row["PROCEDURE_SCHEMA"] = reader.GetString(0).Trim();
                row["PROCEDURE_NAME"] = reader.GetString(1).Trim();

                metaData.Rows.Add(row);
            }

            metaData.AcceptChanges();

            return metaData;
        }
    }
}
