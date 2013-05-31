using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace MyMeta.Plugins
{
    class OdbcDataAccess : IDataAccess
    {
        private String _connectionString;

        // ctor
        public OdbcDataAccess(String connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            return new OdbcConnection(_connectionString) as IDbConnection;
        }
        
        public T ExecuteReader<T>(String sql, MapperDelegate<T> mapper)
        {
            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand(sql, connection))
                {
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        return mapper(reader);
                    }
                }
            }
        }
    }
}
