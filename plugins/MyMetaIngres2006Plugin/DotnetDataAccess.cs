using System;
using System.Collections.Generic;
using System.Text;

namespace MyMeta.Plugins
{
    class DotnetDataAccess : IDataAccess
    {
        public System.Data.IDbConnection GetConnection()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public T ExecuteReader<T>(string sql, MapperDelegate<T> mapper)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
