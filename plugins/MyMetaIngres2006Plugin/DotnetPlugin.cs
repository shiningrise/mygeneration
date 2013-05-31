using System;
using System.Collections.Generic;
using System.Data;

using MyMeta;

namespace MyMeta.Plugins
{

#if DEBUG

    public class DotnetPlugin : IMyMetaPlugin
    {
        private const string Name = @"Ingres 2006 (.NET Provider)";
        private const string AuthorInfo = @"Ingres 2006 MyMeta Plugin written by Julian Maughan";
        private const string AuthorUrl = @"http://www.codeplex.com/MyMetaPlugins";
        private const string UniqueKey = @"INGRES2006DOTNET";
        private const string DefaultConnectionString = @"DSN=Ingres;SERVER=(LOCAL);DATABASE=demodb;SERVERTYPE=INGRES";

        private IMyMetaPluginContext _context;
        private DataAccess _dataAccess;

        public void Initialize(IMyMetaPluginContext context)
        {
            if (context == null)
            { throw new ArgumentNullException("context"); }

            this._context = context;
            this._dataAccess = new DataAccess(context.ConnectionString);
        }

    #region "Properties"

        public System.Data.DataTable Databases
        {
            get
            {
                return new MapDatabasesCommand(_context).Execute();
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
                    return new DataAccess(_context.ConnectionString).GetConnection();
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
            return new MapTableCommand(_context).Execute();
        }

        public System.Data.DataTable GetTableColumns(string database, string table)
        {
            return new MapTableColumnsCommand(_context, database, table).Execute();
        }

        public System.Data.DataTable GetTableIndexes(string database, string table)
        {
            return new MapTableIndexesCommand(_context, database, table).Execute();
        }

        public List<string> GetPrimaryKeyColumns(string database, string table)
        {
            return new MapPrimaryKeyColumns(_context, database, table).Execute();
        }

        public System.Data.DataTable GetForeignKeys(string database, string table)
        {
            return new MapForeignKeys(_context, database, table).Execute();
        }

        public System.Data.DataTable GetProcedures(string database)
        {
            return _context.CreateProceduresDataTable();
        }

        public System.Data.DataTable GetProcedureParameters(string database, string procedure)
        {
            return _context.CreateParametersDataTable();
        }

        public System.Data.DataTable GetProcedureResultColumns(string database, string procedure)
        {
            return _context.CreateResultColumnsDataTable();
        }

        public System.Data.DataTable GetViews(string database)
        {
            return _context.CreateViewsDataTable();
        }

        public System.Data.DataTable GetViewColumns(string database, string view)
        {
            return new DataTable();
        }

        public List<string> GetViewSubTables(string database, string view)
        {
            return new List<string>();
        }

        public List<string> GetViewSubViews(string database, string view)
        {
            return new List<string>();
        }

        public System.Data.DataTable GetDomains(string database)
        {
            return _context.CreateDomainsDataTable();
        }

        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }

        #endregion
    }

#endif

}
