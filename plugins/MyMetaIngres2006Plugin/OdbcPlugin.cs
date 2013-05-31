using System;
using System.Collections.Generic;
using System.Data;

using MyMeta;

namespace MyMeta.Plugins
{
    public class OdbcPlugin : IMyMetaPlugin
    {
        private const string Name = @"Ingres 2006 (ODBC)";
        private const string AuthorInfo = @"Ingres 2006 MyMeta Plugin written by Julian Maughan";
        private const string AuthorUrl = @"http://www.codeplex.com/MyMetaPlugins";
        private const string UniqueKey = @"INGRES2006ODBC";
        private const string DefaultConnectionString = @"DSN=Ingres;SERVER=(LOCAL);DATABASE=demodb;SERVERTYPE=INGRES";

        private IMyMetaPluginContext _context;

        public void Initialize(IMyMetaPluginContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            this._context = context;
        }

        #region "Properties"

        public System.Data.DataTable Databases
        {
            get 
            {
                return new MapDatabasesCommand(_context, new OdbcDataAccess(_context.ConnectionString)).Execute();
            }
        }

        public string DefaultDatabase
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public System.Data.IDbConnection NewConnection
        {
            get 
            {
                if (_context != null)
                {
                    return new OdbcDataAccess(_context.ConnectionString).GetConnection();
                }
                else
                    return null;
            }
        }

        public string ProviderAuthorInfo
        {
            get { return AuthorInfo; }
        }

        public Uri ProviderAuthorUri
        {
            get { return new Uri(AuthorUrl); }
        }

        public string ProviderName
        {
            get { return Name; }
        }

        public string ProviderUniqueKey
        {
            get { return UniqueKey; }
        }

        public bool RequiredDatabaseName
        {
            get { return true; }
        }

        public string SampleConnectionString
        {
            get { return DefaultConnectionString; }
        }

        public bool StripTrailingNulls
        {
            get { return false; }
        }

        #endregion

        #region "Methods"

        public System.Data.DataTable GetTables(string database)
        {
            return new MapTableCommand(_context, new OdbcDataAccess(_context.ConnectionString)).Execute();
        }

        public System.Data.DataTable GetTableColumns(string database, string table)
        {
            return new MapTableColumnsCommand(_context, new OdbcDataAccess(_context.ConnectionString), database, table).Execute();
        }

        public System.Data.DataTable GetTableIndexes(string database, string table)
        {
            return new MapTableIndexesCommand(_context, new OdbcDataAccess(_context.ConnectionString), database, table).Execute();
        }

        public List<string> GetPrimaryKeyColumns(string database, string table)
        {
            return new MapPrimaryKeyColumns(_context, new OdbcDataAccess(_context.ConnectionString), database, table).Execute();
        }

        public System.Data.DataTable GetForeignKeys(string database, string table)
        {
            //TODO
            return _context.CreateForeignKeysDataTable();
        }

        public System.Data.DataTable GetProcedures(string database)
        {
            return new MapProceduresCommand(_context, new OdbcDataAccess(_context.ConnectionString), database).Execute();
        }

        public System.Data.DataTable GetProcedureParameters(string database, string procedure)
        {
            return new MapProcedureParametersCommand(_context, new OdbcDataAccess(_context.ConnectionString), database, procedure).Execute();
        }

        public System.Data.DataTable GetProcedureResultColumns(string database, string procedure)
        {
            //TODO
            return _context.CreateResultColumnsDataTable();
        }

        public System.Data.DataTable GetViews(string database)
        {
            //TODO
            return _context.CreateViewsDataTable();
        }

        public System.Data.DataTable GetViewColumns(string database, string view)
        {
            //TODO
            return new DataTable();
        }

        public List<string> GetViewSubTables(string database, string view)
        {
            //TODO
            return new List<string>();
        }

        public List<string> GetViewSubViews(string database, string view)
        {
            //TODO
            return new List<string>();
        }

        public System.Data.DataTable GetDomains(string database)
        {
            //TODO
            return _context.CreateDomainsDataTable();
        }

        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }

        #endregion

    }
}