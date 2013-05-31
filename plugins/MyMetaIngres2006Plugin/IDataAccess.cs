using System;
using System.Data;

namespace MyMeta.Plugins
{
    public delegate T MapperDelegate<T>(IDataReader reader);

    interface IDataAccess
    {
        IDbConnection GetConnection();
        T ExecuteReader<T>(String sql, MapperDelegate<T> mapper);
    }
}