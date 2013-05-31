using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MyMeta;

namespace MyMetaPluginTestTool
{
    public class MyMetaPluginTests
    {
        private static DataTable _databases;

        public static void Test(IMyMetaTestContext criteria)
        {
            IMyMetaPlugin plugin = null;

            try
            {
                plugin = dbRoot.Plugins[criteria.ProviderType] as IMyMetaPlugin;

                IMyMetaPluginContext context = new MyMetaPluginContext(plugin.ProviderUniqueKey, criteria.ConnectionString);

                plugin.Initialize(context);
                using (IDbConnection conn = plugin.NewConnection)
                {
                    conn.Open();
                    conn.Close();
                }
                criteria.AppendLog("Connection Test Successful.");
            }
            catch (Exception ex)
            {
                criteria.AppendLog("Error testing connection", ex);
            }

            _databases = null;
            TestDatabases(criteria, plugin);
            TestTables(criteria, plugin);
            TestViews(criteria, plugin);
            TestProcedures(criteria, plugin);
            _databases = null;
        }

        private static DataTable GetDatabases(IMyMetaTestContext criteria, IMyMetaPlugin plugin)
        {
            if (_databases == null)
            {
                _databases = plugin.Databases;
                if (criteria.DefaultDatabaseOnly)
                {
                    List<DataRow> rowsToDelete = new List<DataRow>();
                    string defaultDb = plugin.DefaultDatabase;
                    if (!string.IsNullOrEmpty(defaultDb))
                    {
                        defaultDb = defaultDb.Trim();
                        foreach (DataRow dbRow in _databases.Rows)
                        {
                            string dbname = dbRow["CATALOG_NAME"].ToString();
                            if (dbname != defaultDb) rowsToDelete.Add(dbRow);
                        }
                    }
                    if (rowsToDelete.Count != (_databases.Rows.Count - 1))
                    {
                        rowsToDelete.Clear();
                        for (int i = 1; i < _databases.Rows.Count; i++) rowsToDelete.Add(_databases.Rows[i]);
                    }

                    foreach (DataRow dbRow in rowsToDelete) _databases.Rows.Remove(dbRow);
                }
            }
            return _databases;
        }

        private static void TestDatabases(IMyMetaTestContext criteria, IMyMetaPlugin plugin)
        {
            if (!criteria.HasErrors)
            {
                try
                {
                    DataTable dt = GetDatabases(criteria, plugin);
                    criteria.AppendLog(dt.Rows.Count + " databases found through Plugin.");

                }
                catch (Exception ex)
                {
                    criteria.AppendLog("Plugin Databases Error", ex);
                }
            }
        }

        private static void TestTables(IMyMetaTestContext criteria, IMyMetaPlugin plugin)
        {
            DataTable dbDt = null;

            if (!criteria.HasErrors)
            {
                try
                {
                    dbDt = GetDatabases(criteria, plugin);
                }
                catch { dbDt = new DataTable(); }
            }

            if (criteria.IncludeTables && !criteria.HasErrors)
            {
                foreach (DataRow dbRow in dbDt.Rows)
                {
                    string dbname = dbRow["CATALOG_NAME"].ToString();
                    try
                    {
                        DataTable dt = plugin.GetTables(dbname);
                        criteria.AppendLog(dt.Rows.Count + " tables in database " + dbname + " found through Plugin.");
                    }
                    catch (Exception ex)
                    {
                        
                        criteria.AppendLog("Plugin tables error in database " + dbname, ex);
                    }
                }
            }

            if (!criteria.HasErrors)
            {
                foreach (DataRow dbRow in dbDt.Rows)
                {
                    string dbname = dbRow["CATALOG_NAME"].ToString();
                    DataTable tblDt = plugin.GetTables(dbname);
                    foreach (DataRow tblRow in tblDt.Rows)
                    {
                        string tblname = tblRow["TABLE_NAME"].ToString();

                        if (criteria.IncludeTableColumns)
                        {
                            try
                            {
                                DataTable dt = plugin.GetTableColumns(dbname, tblname);
                                criteria.AppendLog(dt.Rows.Count + " columns in table " + dbname + "." + tblname + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin table column error in " + dbname + "." + tblname, ex);
                            }
                        }

                        if (criteria.IncludeTableOther)
                        {
                            try
                            {
                                List<string> pks = plugin.GetPrimaryKeyColumns(dbname, tblname);
                                criteria.AppendLog(pks.Count + " PK columns in table " + dbname + "." + tblname + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin table PK column error in " + dbname + "." + tblname, ex);
                            }

                            try
                            {
                                DataTable dt = plugin.GetForeignKeys(dbname, tblname);
                                criteria.AppendLog(dt.Rows.Count + " FKs in table " + dbname + "." + tblname + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin table FK error in " + dbname + "." + tblname, ex);
                            }

                            try
                            {
                                DataTable dt = plugin.GetTableIndexes(dbname, tblname);
                                criteria.AppendLog(dt.Rows.Count + " indexes in table " + dbname + "." + tblname + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin table index error in " + dbname + "." + tblname, ex);
                            }
                        }
                    }
                }
            }
        }

        private static void TestViews(IMyMetaTestContext criteria, IMyMetaPlugin plugin)
        {
            DataTable dbDt = null;
            if (!criteria.HasErrors)
            {
                try
                {
                    dbDt = GetDatabases(criteria, plugin);
                }
                catch { dbDt = new DataTable(); }
            }

            if (criteria.IncludeViews && !criteria.HasErrors)
            {
                foreach (DataRow dbRow in dbDt.Rows)
                {
                    string dbname = dbRow["CATALOG_NAME"].ToString();
                    try
                    {
                        DataTable dt = plugin.GetViews(dbname);
                        criteria.AppendLog(dt.Rows.Count + " views in database " + dbname + " found through Plugin.");
                    }
                    catch (Exception ex)
                    {
                        
                        criteria.AppendLog("Plugin views error in database " + dbname, ex);
                    }
                }
            }

            if (!criteria.HasErrors)
            {
                foreach (DataRow dbRow in dbDt.Rows)
                {
                    string dbname = dbRow["CATALOG_NAME"].ToString();
                    DataTable viewDt = plugin.GetViews(dbname);
                    foreach (DataRow viewRow in viewDt.Rows)
                    {
                        string viewname = viewRow["TABLE_NAME"].ToString();
                        if (criteria.IncludeViews)
                        {
                            try
                            {
                                DataTable dt = plugin.GetViewColumns(dbname, viewname);
                                criteria.AppendLog(dt.Rows.Count + " columns in view " + dbname + "." + viewname + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin view column error in " + dbname + "." + viewname, ex);
                            }
                        }
                        if (criteria.IncludeViewOther)
                        {
                            try
                            {
                                List<string> list = plugin.GetViewSubTables(dbname, viewname);
                                criteria.AppendLog(list.Count + " sub-tables in view " + dbname + "." + viewname + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin view sub-tables error in " + dbname + "." + viewname, ex);
                            }

                            try
                            {
                                List<string> list = plugin.GetViewSubViews(dbname, viewname);
                                criteria.AppendLog(list.Count + " sub-views in view " + dbname + "." + viewname + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin view sub-views error in " + dbname + "." + viewname, ex);
                            }
                        }
                    }
                }
            }
        }

        private static void TestProcedures(IMyMetaTestContext criteria, IMyMetaPlugin plugin)
        {
            
            DataTable dbDt = null;
            if (!criteria.HasErrors)
            {
                try
                {
                    dbDt = GetDatabases(criteria, plugin);
                }
                catch { dbDt = new DataTable(); }
            }

            if (criteria.IncludeProcedures && !criteria.HasErrors)
            {
                foreach (DataRow dbRow in dbDt.Rows)
                {
                    string dbname = dbRow["CATALOG_NAME"].ToString();
                    try
                    {
                        DataTable dt = plugin.GetProcedures(dbname);
                        criteria.AppendLog(dt.Rows.Count + " procedures in database " + dbname + " found through Plugin.");
                    }
                    catch (Exception ex)
                    {
                        
                        criteria.AppendLog("Plugin procedures error in database " + dbname, ex);
                    }
                }
            }

            if (!criteria.HasErrors)
            {
                foreach (DataRow dbRow in dbDt.Rows)
                {
                    string dbname = dbRow["CATALOG_NAME"].ToString();
                    DataTable procDt = plugin.GetProcedures(dbname);
                    foreach (DataRow procRow in procDt.Rows)
                    {
                        string procedurename = procRow["PROCEDURE_NAME"].ToString();

                        if (criteria.IncludeParameters)
                        {
                            try
                            {
                                DataTable dt = plugin.GetProcedureParameters(dbname, procedurename);
                                criteria.AppendLog(dt.Rows.Count + " parameters in procedure " + dbname + "." + procedurename + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin procedure parameter error in " + dbname + "." + procedurename, ex);
                            }
                        }

                        if (criteria.IncludeProcOther)
                        {
                            try
                            {
                                DataTable dt = plugin.GetProcedureResultColumns(dbname, procedurename);
                                criteria.AppendLog(dt.Rows.Count + " result columns in procedure " + dbname + "." + procedurename + " found through Plugin.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Plugin procedure result columns error in " + dbname + "." + procedurename, ex);
                            }
                        }
                    }
                }
            }
        }
    }
}
