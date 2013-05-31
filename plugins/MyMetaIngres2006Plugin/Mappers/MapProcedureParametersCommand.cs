using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyMeta.Plugins
{
    class MapProcedureParametersCommand : IMyMetaMapper<DataTable>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;
        private string _database;
        private string _procedure;

        public MapProcedureParametersCommand(IMyMetaPluginContext context, IDataAccess dataAccess, string database, string procedure)
        {
            _context = context;
            _dataAccess = dataAccess;
            _database = database;
            _procedure = procedure;
        }

        public System.Data.DataTable Execute()
        {
            string[] procedureNameParts = _procedure.Split(new char[] { '.' });

            String sql = String.Format(
                @"SELECT TRIM(procedure_owner) AS procedure_owner, procedure_name, param_name, param_sequence, 
                param_datatype, param_length, param_scale, param_nulls, param_defaults, param_default_val 
                FROM iiproc_params 
                WHERE procedure_owner = '{0}' AND procedure_name = '{1}' 
                ORDER BY param_sequence", procedureNameParts[0], procedureNameParts[1]);

            return _dataAccess.ExecuteReader<DataTable>(sql, new MapperDelegate<DataTable>(Mapper));
        }

        private DataTable Mapper(IDataReader reader)
        {
            DataTable metaData = _context.CreateParametersDataTable();

            while (reader.Read())
            {
                DataRow row = metaData.NewRow();

                //metaData.Columns.Add("PROCEDURE_CATALOG", Type.GetType("System.String"));
                //metaData.Columns.Add("PROCEDURE_SCHEMA", Type.GetType("System.String"));
                //metaData.Columns.Add("PROCEDURE_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("PARAMETER_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("ORDINAL_POSITION", Type.GetType("System.Int32"));
                //metaData.Columns.Add("PARAMETER_TYPE", Type.GetType("System.Int32"));
                //metaData.Columns.Add("PARAMETER_HASDEFAULT", Type.GetType("System.Boolean"));
                //metaData.Columns.Add("PARAMETER_DEFAULT", Type.GetType("System.String"));
                //metaData.Columns.Add("IS_NULLABLE", Type.GetType("System.Boolean"));
                //metaData.Columns.Add("DATA_TYPE", Type.GetType("System.Int32"));
                //metaData.Columns.Add("CHARACTER_MAXIMUM_LENGTH", Type.GetType("System.Int64"));
                //metaData.Columns.Add("CHARACTER_OCTET_LENGTH", Type.GetType("System.Int64"));
                //metaData.Columns.Add("NUMERIC_PRECISION", Type.GetType("System.Int32"));
                //metaData.Columns.Add("NUMERIC_SCALE", Type.GetType("System.Int16"));
                //metaData.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
                //metaData.Columns.Add("TYPE_NAME", Type.GetType("System.String"));
                //metaData.Columns.Add("LOCAL_TYPE_NAME", Type.GetType("System.String"));

                row["PROCEDURE_CATALOG"] = _database;
                row["PROCEDURE_SCHEMA"] = reader.GetString(0).Trim();
                row["PROCEDURE_NAME"] = reader.GetString(1).Trim();
                row["PARAMETER_NAME"] = reader.GetString(2).Trim();
                row["ORDINAL_POSITION"] = reader.GetInt32(3);
                row["PARAMETER_HASDEFAULT"] = (reader.GetString(8) == "Y");

                if (!reader.IsDBNull(9))
                    row["PARAMETER_DEFAULT"] = reader.GetString(9).Trim();

                row["IS_NULLABLE"] = (reader.GetString(7) == "Y");
                row["CHARACTER_MAXIMUM_LENGTH"] = reader.GetInt32(5);
                row["NUMERIC_SCALE"] = reader.GetInt32(6);
                row["TYPE_NAME"] = reader.GetString(4).Trim();
                row["LOCAL_TYPE_NAME"] = reader.GetString(4).Trim();

                metaData.Rows.Add(row);
            }

            metaData.AcceptChanges();

            return metaData;
        }
    }
}
