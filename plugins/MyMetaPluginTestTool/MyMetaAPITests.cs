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
    public class MyMetaAPITests
    {

        public static void Test(IMyMetaTestContext criteria)
        {
            dbRoot root = null;

            if (!criteria.HasErrors)
            {
                root = new dbRoot();
                try
                {
                    root.Connect(criteria.ProviderType, criteria.ConnectionString);
                    root.ShowDefaultDatabaseOnly = true;
                    criteria.AppendLog("MyMeta dbRoot Connection Successful.");
                }
                catch (Exception ex)
                {
                    criteria.AppendLog("Error connecting to dbRoot", ex);
                }
            }

            TestDatabases(criteria, root);
            TestTables(criteria, root);
            TestViews(criteria, root);
            TestProcedures(criteria, root);
            TestSQL(criteria, root);
        }

        private static void TestSQL(IMyMetaTestContext criteria, dbRoot root)
        {
            IDbConnection conn = root.PluginSpecificData(root.DriverString, "internalconnection") as IDbConnection;
            if (conn != null)
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from TEST_DATATYPES";
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        try
                        {
                            string name = reader.GetName(i);
                            string dtname = reader.GetDataTypeName(i);
                            string dtname2 = reader.GetFieldType(i).Name;
                            criteria.AppendLog(name + "\t=\t" + dtname + "\t=\t" + dtname2);
                        }
                        catch { }
                    }
                }
            }
        }

        private static void TestDatabases(IMyMetaTestContext criteria, dbRoot root)
        {
            string garbage;
            
            if (!criteria.HasErrors)
            {
                try
                {
                    foreach (IDatabase db in root.Databases)
                    {
                        garbage = db.Name;
                        garbage = db.SchemaName;
                        garbage = db.SchemaOwner;
                    }

                    criteria.AppendLog(root.Databases.Count + " databases traversed successfully through MyMeta.");
                }
                catch (Exception ex)
                {
                    
                    criteria.AppendLog("Error traversing databases through MyMeta", ex);
                }
            }
        }

        private static void TestTables(IMyMetaTestContext criteria, dbRoot root)
        {
            string garbage;

            if (criteria.IncludeTables && !criteria.HasErrors)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        foreach (ITable tbl in db.Tables)
                        {
                            garbage = tbl.Name;
                            garbage = tbl.Schema;
                            garbage = tbl.DateCreated.ToString();
                            garbage = tbl.DateModified.ToString();
                            garbage = tbl.Description;
                            garbage = tbl.Guid.ToString();
                            garbage = tbl.PropID.ToString();
                            garbage = tbl.Type;
                        }

                        criteria.AppendLog(db.Tables.Count + " tables in database " + db.Name + " traversed successfully through MyMeta.");

                    }
                    catch (Exception ex)
                    {

                        criteria.AppendLog("Error traversing tables in database " + db.Name + " through MyMeta", ex);
                    }
                }
            }

            if (!criteria.HasErrors)
            {
                foreach (IDatabase db in root.Databases)
                {
                    foreach (ITable table in db.Tables)
                    {
                        if (criteria.IncludeTableColumns)
                        {
                            try
                            {
                                foreach (IColumn column in table.Columns)
                                {
                                    garbage = column.AutoKeyIncrement.ToString();
                                    garbage = column.AutoKeySeed.ToString();
                                    garbage = column.CharacterMaxLength.ToString();
                                    garbage = column.CharacterOctetLength.ToString();
                                    garbage = column.CharacterSetCatalog;
                                    garbage = column.CharacterSetName;
                                    garbage = column.CharacterSetSchema;
                                    garbage = column.CompFlags.ToString();
                                    garbage = column.DataType.ToString();
                                    garbage = column.DataTypeName;
                                    garbage = column.DataTypeNameComplete;
                                    garbage = column.DateTimePrecision.ToString();
                                    garbage = column.DbTargetType;
                                    garbage = column.Default;
                                    garbage = column.Description;
                                    garbage = column.DomainCatalog;
                                    garbage = column.DomainName;
                                    garbage = column.DomainSchema;
                                    garbage = column.Flags.ToString();
                                    garbage = column.Guid.ToString();
                                    garbage = column.HasDefault.ToString();
                                    garbage = column.HasDomain.ToString();
                                    garbage = column.IsAutoKey.ToString();
                                    garbage = column.IsComputed.ToString();
                                    garbage = column.IsInForeignKey.ToString();
                                    garbage = column.IsInPrimaryKey.ToString();
                                    garbage = column.IsNullable.ToString();
                                    garbage = column.LanguageType;
                                    garbage = column.LCID.ToString();
                                    garbage = column.Name;
                                    garbage = column.NumericPrecision.ToString();
                                    garbage = column.NumericScale.ToString();
                                    garbage = column.Ordinal.ToString();
                                    garbage = column.PropID.ToString();
                                    garbage = column.SortID.ToString();
                                    //garbage = column.TDSCollation.ToString(); -- Null means empty?
                                    garbage = column.TypeGuid.ToString();
                                }
                                criteria.AppendLog(table.Columns.Count + " table columns in database " + db.Name + "." + table.Name + " traversed successfully through MyMeta.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Error traversing table columns in " + db.Name + "." + table.Name + " through MyMeta", ex);
                            }
                        }

                        if (criteria.IncludeTableOther)
                        {
                            try
                            {
                                foreach (IForeignKey fk in table.ForeignKeys)
                                {
                                    garbage = fk.Deferrability;
                                    garbage = fk.DeleteRule;
                                    garbage = fk.Name;
                                    garbage = fk.PrimaryKeyName;
                                    garbage = fk.UpdateRule;
                                    garbage = fk.ForeignColumns.Count.ToString();
                                    garbage = fk.PrimaryColumns.Count.ToString();
                                    garbage = fk.ForeignTable.Name;
                                    garbage = fk.PrimaryTable.Name;
                                }

                                criteria.AppendLog(table.ForeignKeys.Count + " FKs in table " + db.Name + "." + table.Name + " traversed successfully through MyMeta.");

                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Error traversing FKs in table " + db.Name + "." + table.Name + " through MyMeta", ex);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="root"></param>
        /// <param name="criteria.HasErrors"></param>
        private static void TestViews(IMyMetaTestContext criteria, dbRoot root)
        {
            string garbage;

            if (criteria.IncludeViews && !criteria.HasErrors)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        foreach (IView view in db.Views)
                        {

                            garbage = view.Name;
                            garbage = view.Schema;
                            garbage = view.DateCreated.ToString();
                            garbage = view.DateModified.ToString();
                            garbage = view.Description;
                            garbage = view.Guid.ToString();
                            garbage = view.PropID.ToString();
                            garbage = view.Type;
                            garbage = view.IsUpdateable.ToString();
                            garbage = view.CheckOption.ToString();
                            garbage = view.ViewText;
                        }

                        criteria.AppendLog(db.Views.Count + " views in database " + db.Name + " traversed successfully through MyMeta.");

                    }
                    catch (Exception ex)
                    {
                        
                        criteria.AppendLog("Error traversing views in database " + db.Name + " through MyMeta", ex);
                    }
                }
            }
      
            if (!criteria.HasErrors)
            {
                foreach (IDatabase db in root.Databases)
                {
                    foreach (IView view in db.Views)
                    {
                        if (criteria.IncludeViewColumns)
                        {
                            try
                            {
                                foreach (IColumn column in view.Columns)
                                {
                                    garbage = column.AutoKeyIncrement.ToString();
                                    garbage = column.AutoKeySeed.ToString();
                                    garbage = column.CharacterMaxLength.ToString();
                                    garbage = column.CharacterOctetLength.ToString();
                                    garbage = column.CharacterSetCatalog;
                                    garbage = column.CharacterSetName;
                                    garbage = column.CharacterSetSchema;
                                    garbage = column.CompFlags.ToString();
                                    garbage = column.DataType.ToString();
                                    garbage = column.DataTypeName;
                                    garbage = column.DataTypeNameComplete;
                                    garbage = column.DateTimePrecision.ToString();
                                    garbage = column.DbTargetType;
                                    garbage = column.Default;
                                    garbage = column.Description;
                                    garbage = column.DomainCatalog;
                                    garbage = column.DomainName;
                                    garbage = column.DomainSchema;
                                    garbage = column.Flags.ToString();
                                    garbage = column.Guid.ToString();
                                    garbage = column.HasDefault.ToString();
                                    garbage = column.HasDomain.ToString();
                                    garbage = column.IsAutoKey.ToString();
                                    garbage = column.IsComputed.ToString();
                                    garbage = column.IsInForeignKey.ToString();
                                    garbage = column.IsInPrimaryKey.ToString();
                                    garbage = column.IsNullable.ToString();
                                    garbage = column.LanguageType;
                                    garbage = column.LCID.ToString();
                                    garbage = column.Name;
                                    garbage = column.NumericPrecision.ToString();
                                    garbage = column.NumericScale.ToString();
                                    garbage = column.Ordinal.ToString();
                                    garbage = column.PropID.ToString();
                                    garbage = column.SortID.ToString();
                                    //garbage = column.TDSCollation.ToString(); -- Null means empty?
                                    garbage = column.TypeGuid.ToString();
                                }
                                criteria.AppendLog(view.Columns.Count + " view columns in database " + db.Name + "." + view.Name + " traversed successfully through MyMeta.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Error traversing view columns in " + db.Name + "." + view.Name + " through MyMeta", ex);
                            }
                        }

                        if (criteria.IncludeViewOther)
                        {
                            try
                            {
                                foreach (ITable subtable in view.SubTables)
                                {
                                    garbage = subtable.Name;
                                    garbage = subtable.Schema;
                                    garbage = subtable.DateCreated.ToString();
                                    garbage = subtable.DateModified.ToString();
                                    garbage = subtable.Description;
                                    garbage = subtable.Guid.ToString();
                                    garbage = subtable.PropID.ToString();
                                    garbage = subtable.Type;
                                }
                                criteria.AppendLog(view.SubTables.Count + " view sub-tables in database " + db.Name + "." + view.Name + " traversed successfully through MyMeta.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Error traversing view sub-tables in " + db.Name + "." + view.Name + " through MyMeta", ex);
                            }

                            try
                            {
                                foreach (IView subview in view.SubViews)
                                {
                                    garbage = subview.Name;
                                    garbage = subview.Schema;
                                    garbage = subview.DateCreated.ToString();
                                    garbage = subview.DateModified.ToString();
                                    garbage = subview.Description;
                                    garbage = subview.Guid.ToString();
                                    garbage = subview.PropID.ToString();
                                    garbage = subview.Type;
                                    garbage = subview.IsUpdateable.ToString();
                                    garbage = subview.CheckOption.ToString();
                                    garbage = subview.ViewText;
                                }
                                criteria.AppendLog(view.SubViews.Count + " view sub-views in database " + db.Name + "." + view.Name + " traversed successfully through MyMeta.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Error traversing view sub-views in " + db.Name + "." + view.Name + " through MyMeta", ex);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="root"></param>
        /// <param name="criteria.HasErrors"></param>
        private static void TestProcedures(IMyMetaTestContext criteria, dbRoot root)
        {
            string garbage;

            if (criteria.IncludeProcedures && !criteria.HasErrors)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        foreach (IProcedure procedure in db.Procedures)
                        {
                            garbage = procedure.Name;
                            garbage = procedure.Schema;
                            garbage = procedure.DateCreated.ToString();
                            garbage = procedure.DateModified.ToString();
                            garbage = procedure.Description;
                            garbage = procedure.ProcedureText;
                            garbage = procedure.Type.ToString();
                        }

                        criteria.AppendLog(db.Procedures.Count + " procedures in database " + db.Name + " traversed successfully through MyMeta.");

                    }
                    catch (Exception ex)
                    {
                        
                        criteria.AppendLog("Error traversing procedures in database " + db.Name + " through MyMeta", ex);
                    }
                }
            }

            if (!criteria.HasErrors)
            {
                foreach (IDatabase db in root.Databases)
                {
                    foreach (IProcedure procedure in db.Procedures)
                    {
                        if (criteria.IncludeParameters)
                        {
                            try
                            {
                                foreach (IParameter parameter in procedure.Parameters)
                                {
                                    garbage = parameter.CharacterMaxLength.ToString();
                                    garbage = parameter.CharacterOctetLength.ToString();
                                    garbage = parameter.DataType.ToString();
                                    garbage = parameter.DataTypeNameComplete;
                                    garbage = parameter.DbTargetType;
                                    garbage = parameter.Default;
                                    garbage = parameter.Description;
                                    garbage = parameter.Direction.ToString();
                                    garbage = parameter.LocalTypeName.ToString();
                                    garbage = parameter.HasDefault.ToString();
                                    garbage = parameter.IsNullable.ToString();
                                    garbage = parameter.LanguageType;
                                    garbage = parameter.Name;
                                    garbage = parameter.NumericPrecision.ToString();
                                    garbage = parameter.NumericScale.ToString();
                                    garbage = parameter.Ordinal.ToString();
                                    garbage = parameter.TypeName.ToString();
                                }
                                criteria.AppendLog(procedure.Parameters.Count + " procedure parameters in " + db.Name + "." + procedure.Name + " traversed successfully through MyMeta.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Error traversing procedure parameters in " + db.Name + "." + procedure.Name + " through MyMeta", ex);
                            }
                        }

                        if (criteria.IncludeProcOther)
                        {
                            try
                            {
                                foreach (IResultColumn column in procedure.ResultColumns)
                                {
                                    garbage = column.DataType.ToString();
                                    garbage = column.DataTypeName;
                                    garbage = column.DataTypeNameComplete;
                                    garbage = column.DbTargetType;
                                    garbage = column.LanguageType;
                                    garbage = column.Name;
                                    garbage = column.Ordinal.ToString();
                                }
                                criteria.AppendLog(procedure.ResultColumns.Count + " procedure result columns in " + db.Name + "." + procedure.Name + " traversed successfully through MyMeta.");
                            }
                            catch (Exception ex)
                            {

                                criteria.AppendLog("Error traversing procedure result columns in " + db.Name + "." + procedure.Name + " through MyMeta", ex);
                            }
                        }
                    }
                }
            }
        }
    }
}
