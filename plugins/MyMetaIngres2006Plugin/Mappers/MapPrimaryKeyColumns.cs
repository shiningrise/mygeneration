using System;
using System.Collections.Generic;
using System.Data;

namespace MyMeta.Plugins
{
    class MapPrimaryKeyColumns : IMyMetaMapper<List<string>>
    {
        private IMyMetaPluginContext _context;
        private IDataAccess _dataAccess;
        private String _database;
        private String _table;

        public MapPrimaryKeyColumns(IMyMetaPluginContext context, IDataAccess dataAccess, String database, String table)
        {
            _context = context;
            _dataAccess = dataAccess;
            _database = database;
            _table = table;
        }

        public List<string> Execute()
        {
            string[] tableNameParts = _table.Split(new char[] { '.' });

            String sql = String.Format(
                @"SELECT column_name
                FROM iiconstraints c
                INNER JOIN iiconstraint_indexes ci on c.constraint_name = ci.constraint_name
                INNER JOIN iiindex_columns ic on ci.index_name = ic.index_name
                WHERE c.schema_name = '{0}' AND c.table_name = '{1}'
                AND c.constraint_type = 'P'
                ORDER BY ic.key_sequence", tableNameParts[0], tableNameParts[1]);

            return _dataAccess.ExecuteReader<List<string>>(sql, new MapperDelegate<List<string>>(Mapper));
        }

        private List<string> Mapper(IDataReader reader)
        {
            List<string> metaData = new List<string>();

            while (reader.Read())
            {
                metaData.Add(reader.GetString(0).Trim());
            }

            return metaData;

        }
    }
}
