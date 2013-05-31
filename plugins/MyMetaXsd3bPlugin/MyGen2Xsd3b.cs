using System;
using System.Collections.Generic;
using System.Text;
using Dl3bak.Data.OleDB;
using Dl3bak.Data.Xsd3b;
using MyMeta;

namespace MyMeta.Plugins.Xsd3b
{
    /// <summary>
    /// This is the reverse-Provider of 
    /// 	MyMeta.Plugins.Xsd3b.dll providerplugin for MyGeneration
    /// 
    /// (c) 2007 by dl3bak@qsl.net
    /// 
    /// It uses MyMeta to produce *.xsd or *.xsd3b. You can use this
    /// 	api to save a databaseschema to an xml (*.xsd3b) file.
    /// 
    /// MyGenXsd3b ("MyMeta.Plugins.Xsd3b.dll") is a provider plugin 
    ///     for MyGeneration that allows to use XSDFile s (*.xsd, *.xsd3b) 
    ///     as datasource instead of an online sqldatabase.
    /// 
    /// MyGeneration from http://www.mygenerationsoftware.com is a template 
    ///     driven sourcecodegenerator that you can use free of charge. 
    /// It uses the RelationalDatastructure of a sql database 
    ///         (ie Microsoft SQL, Oracle, IBM DB2, MySQL, PostgreSQL, 
    ///         Microsoft Access, FireBird, Interbase, SQLite, ...) 
    ///         and produces sourcecode (ie C#, VB.NET code,
    ///         SQL Stored Procedures, PHP, HTML, ...) 
    /// 
    /// There is an online repository with lots of templates available. 
    /// </summary>
    public class MyGen2Xsd3b
    {
	    
        public static String GetXml(IDatabase srcDb)
        {
            SchemaXsd3bEx destSet = SchemaXsd3bEx.CreateNewSchemaXsd3b();
            Import(srcDb, destSet);
            return destSet.GetXml();
        }

        public static void Import(IDatabase srcDb, SchemaXsd3bEx destSet)
        {
            ImportDatabaseInfos(srcDb, destSet);
            ImportTables(srcDb, destSet);
            ImportProcs(srcDb, destSet);
            ImportViews(srcDb, destSet);
            ImportRelations(srcDb, destSet);
            System.Diagnostics.Trace.WriteLine(destSet.GetXml());
        }

        public static void ImportDatabaseInfos(IDatabase srcDb, SchemaXsd3bEx destSet)
        {
            SchemaDefinitionRow destDb = destSet.GetSchemaDefinitionRow();

            if (srcDb.Description.Length > 0)
                destDb.SchemaComment = srcDb.Description;
            destDb.SchemaAlias = srcDb.Alias;
            destDb.SchemaID = (srcDb.SchemaName.Length > 0) ? srcDb.SchemaName : srcDb.Name;
        }

        #region stored procedures
        public static void ImportProcs(IDatabase srcDb, SchemaXsd3bEx destSet)
        {
            foreach (IProcedure srcProc in srcDb.Procedures)
                ImportProc(srcProc, destSet.TableDefinition);
        }

        public static void ImportProc(IProcedure srcProc, TableDefinitionDataTable destTabs)
        {
            System.Diagnostics.Debug.WriteLine("Import Proc " + srcProc.Name);
            TableDefinitionRow destTab = destTabs.FindByTableName(srcProc.Name);
            if (destTab == null)
            {
                destTab = destTabs.NewTableDefinitionRow();
                destTab.TableName = srcProc.Name;
                destTabs.AddTableDefinitionRow(destTab);
            }
            ImportProc(srcProc, destTab);
        }

        public static void ImportProc(IProcedure srcProc, TableDefinitionRow destTab)
        {
            destTab.TableAlias = srcProc.Alias;
            if (srcProc.Description.Length > 0)
                destTab.TableComment = srcProc.Description;
            destTab.DBTableType = "PROCEDURE"; // srcProc.Type.ToString();
            destTab.OriginalName = srcProc.Name; // no guid GetOriginalName(srcProc.Guid, srcProc.Name);

            ImportParameters(srcProc.Parameters, destTab);
            ImportResultColumns(srcProc.ResultColumns, destTab);
        }

        #region Parameter
        public static void ImportParameters(IParameters srcParameters, TableDefinitionRow destTab)
        {
            nextPkNumber = 1;
            foreach (IParameter srcCol in srcParameters)
                ImportParameter(srcCol, destTab);
        }

        public static void ImportParameter(IParameter srcCol, TableDefinitionRow destTab)
        {
            DataTypeDefinitionRow dataType = GetType(srcCol, destTab.Table.DataSet.DataTypeDefinition);

            FieldDefinitionRow destField = destTab.FindFieldByAliasOrName(srcCol.Name); // destTab.Table.DataSet.FieldDefinition destTabs.FindByTableName(srcTab.Name);

            if (destField == null)
                destField = destTab.FindFieldByAliasOrName(srcCol.Alias);
            if (destField == null)
            {
                destField = destTab.AddFieldDefinitionRow(srcCol.Name, dataType.DataTypeName);
            }
            else
            {
                destField.DataTypeDefinitionRow = dataType;
            }

            destField.DBFieldType = "PROC_INPUT";
            destField.ReadOnly=true;

            destField.FieldAlias = srcCol.Alias;
            if (srcCol.Description.Length > 0)
                destField.FieldComment = srcCol.Description;
            destField.AllowDBNull = srcCol.IsNullable;
            System.Diagnostics.Debug.WriteLine(string.Format(
                "FeldName={4}.{3}, DataType={0}, DataTypeNameComplete={1}, DbTargetType={2}"
                , srcCol.DataType
                , srcCol.DataTypeNameComplete
                , srcCol.DbTargetType
                , srcCol.Name
                , destTab.TableName));
            /* destField.DataType = ???;
            srcCol.DataType; // int
            srcCol.DataTypeName;
            srcCol.DataTypeNameComplete;
            srcCol.DbTargetType;
             */


            // srcCol.ForeignKeys;
            if (srcCol.HasDefault)
                destField.DefaultValue = srcCol.Default;
            else
                destField.SetDefaultValueNull();
            destField.OriginalName = srcCol.Name;
            // destField.StringSize = srcCol.CharacterMaxLength; moved to DataTypeDefinition

            /** unsupported properties
            destField.FieldExpression = srcCol.???
            destField.NullValue = srcCol.??? // nullvaluereplacement
            destField.XmlFieldType = ???; // default is Attribute

            // srcCol.AllProperties;
            srcCol.CharacterOctetLength;
            srcCol.CharacterSetCatalog;
            srcCol.CharacterSetName;
            srcCol.CharacterSetSchema;
            srcCol.CompFlags;
            srcCol.Flags;
            srcCol.GlobalProperties;

            srcCol.DateTimePrecision; // int
            srcCol.HasDomain;
            srcCol.Domain; // IDomain
            srcCol.DomainCatalog;
            srcCol.DomainName;
            srcCol.DomainSchema;
            srcCol.LanguageType;
            srcCol.LCID;
            srcCol.NumericPrecision;
            srcCol.NumericScale;
            srcCol.Ordinal;
            srcCol.Properties;
            srcCol.PropID;
            srcCol.SortID;
            srcCol.TDSCollation;
            srcCol.TypeGuid;
            srcCol.UserDataXPath;

*/

        }
        private static DataTypeDefinitionRow GetType(IParameter srcCol, DataTypeDefinitionDataTable types)
        {
            // DataTypeName=Hyperlink, DataTypeNameComplete=Text (255)
            String dataTypeNameComplete = srcCol.DataTypeNameComplete
                .Replace(" ", "")
                .Replace("(", "_")
                .Replace(")", "_");

            String dataTypeName = dataTypeNameComplete;

            if (dataTypeNameComplete.Length == 0)
                dataTypeNameComplete = dataTypeName;
            if (dataTypeName.Length == 0)
                dataTypeName = dataTypeNameComplete;

            int baseTypeId = srcCol.DataType;

            if (baseTypeId == 0)
                baseTypeId = Mapping.GetOleDbTypeIDFromOleDbTypeName(dataTypeNameComplete);
            if (baseTypeId == 0)
                baseTypeId = Mapping.GetOleDbTypeIDFromOleDbTypeName(dataTypeName);

            // include System.XXXX
            DataTypeDefinitionRow baseType = types.IncludeDataTypeDefinition(Mapping.GetSystemTypeFromOleDBTypeID(baseTypeId), null);

            string oleTypeName = (baseTypeId != 0) ? Mapping.GetOleDBTypeNameFromID(baseTypeId) : null;
            if (oleTypeName != null)
                baseType = types.IncludeDataTypeDefinition("sql." + oleTypeName, baseType.DataTypeName);
            if (dataTypeName.Length > 0)
                baseType = types.IncludeDataTypeDefinition("sql." + dataTypeName, baseType.DataTypeName);

            if (srcCol.CharacterMaxLength > 0)
                dataTypeNameComplete += "_" + srcCol.CharacterMaxLength.ToString() + "_";

            baseType = types.IncludeDataTypeDefinition("app." + dataTypeNameComplete, baseType.DataTypeName);

            if (srcCol.CharacterMaxLength > 0)
                baseType.StringSize = srcCol.CharacterMaxLength;
            return baseType;
        }
        #endregion

        #region ResultColumn
        public static void ImportResultColumns(IResultColumns srcResultColumns, TableDefinitionRow destTab)
        {
            nextPkNumber = 1;
            foreach (IResultColumn srcCol in srcResultColumns)
                ImportResultColumn(srcCol, destTab);
        }

        public static void ImportResultColumn(IResultColumn srcCol, TableDefinitionRow destTab)
        {
            DataTypeDefinitionRow dataType = GetType(srcCol, destTab.Table.DataSet.DataTypeDefinition);

            FieldDefinitionRow destField = destTab.FindFieldByAliasOrName(srcCol.Name); // destTab.Table.DataSet.FieldDefinition destTabs.FindByTableName(srcTab.Name);
            
            if (destField == null)
                destField = destTab.FindFieldByAliasOrName(srcCol.Alias);
            if (destField == null)
            {
                destField = destTab.AddFieldDefinitionRow(srcCol.Name, dataType.DataTypeName);
            }
            else
            {
                destField.DataTypeDefinitionRow = dataType;
            }

            destField.DBFieldType = "PROC_RETURN";
            // destField.ReadOnly = true;

            //Exception on accessing srcCol.Alias
            destField.FieldAlias = srcCol.Alias;

            System.Diagnostics.Debug.WriteLine(string.Format(
                "FeldName={4}.{3}, DataTypeName={0}, DataTypeNameComplete={1}, DbTargetType={2}"
                , srcCol.DataTypeName
                , srcCol.DataTypeNameComplete
                , srcCol.DbTargetType
                , srcCol.Name
                , destTab.TableName));
            /* destField.DataType = ???;
            srcCol.DataType; // int
            srcCol.DataTypeName;
            srcCol.DataTypeNameComplete;
            srcCol.DbTargetType;
             */


            destField.SetDefaultValueNull();
            destField.OriginalName = srcCol.Name;

            /** unsupported properties
            destField.FieldExpression = srcCol.???
            destField.NullValue = srcCol.??? // nullvaluereplacement
            destField.XmlFieldType = ???; // default is Attribute

            // srcCol.AllProperties;
            srcCol.CharacterOctetLength;
            srcCol.CharacterSetCatalog;
            srcCol.CharacterSetName;
            srcCol.CharacterSetSchema;
            srcCol.CompFlags;
            srcCol.Flags;
            srcCol.GlobalProperties;

            srcCol.DateTimePrecision; // int
            srcCol.HasDomain;
            srcCol.Domain; // IDomain
            srcCol.DomainCatalog;
            srcCol.DomainName;
            srcCol.DomainSchema;
            srcCol.LanguageType;
            srcCol.LCID;
            srcCol.NumericPrecision;
            srcCol.NumericScale;
            srcCol.Ordinal;
            srcCol.Properties;
            srcCol.PropID;
            srcCol.SortID;
            srcCol.TDSCollation;
            srcCol.TypeGuid;
            srcCol.UserDataXPath;

*/

        }

        private static DataTypeDefinitionRow GetType(IResultColumn srcCol, DataTypeDefinitionDataTable types)
        {
            // DataTypeName=Hyperlink, DataTypeNameComplete=Text (255)
            String dataTypeNameComplete = srcCol.DataTypeNameComplete
                .Replace(" ", "")
                .Replace("(", "_")
                .Replace(")", "_");
            String dataTypeName = srcCol.DataTypeName
                .Replace(" ", "")
                .Replace("(", "_")
                .Replace(")", "_");

            if (dataTypeNameComplete.Length == 0)
                dataTypeNameComplete = dataTypeName;
            if (dataTypeName.Length == 0)
                dataTypeName = dataTypeNameComplete;

            int baseTypeId = srcCol.DataType;

            if (baseTypeId == 0)
                baseTypeId = Mapping.GetOleDbTypeIDFromOleDbTypeName(dataTypeNameComplete);
            if (baseTypeId == 0)
                baseTypeId = Mapping.GetOleDbTypeIDFromOleDbTypeName(dataTypeName);

            // include System.XXXX
            DataTypeDefinitionRow baseType = types.IncludeDataTypeDefinition(Mapping.GetSystemTypeFromOleDBTypeID(baseTypeId), null);

            string oleTypeName = (baseTypeId != 0) ? Mapping.GetOleDBTypeNameFromID(baseTypeId) : null;
            if (oleTypeName != null)
                baseType = types.IncludeDataTypeDefinition("sql." + oleTypeName, baseType.DataTypeName);
            if (dataTypeName.Length > 0)
                baseType = types.IncludeDataTypeDefinition("sql." + dataTypeName, baseType.DataTypeName);

            baseType = types.IncludeDataTypeDefinition("app." + dataTypeNameComplete, baseType.DataTypeName);

            return baseType;
        }
        #endregion
	

        #endregion
        #region views
        public static void ImportViews(IDatabase srcDb, SchemaXsd3bEx destSet)
        {
            foreach (IView srcView in srcDb.Views)
                ImportView(srcView, destSet.TableDefinition);
        }

        public static void ImportView(IView srcView, TableDefinitionDataTable destTabs)
        {
            System.Diagnostics.Debug.WriteLine("Import View " + srcView.Name);
            TableDefinitionRow destTab = destTabs.FindByTableName(srcView.Name);
            if (destTab == null)
            {
                destTab = destTabs.NewTableDefinitionRow();
                destTab.TableName = srcView.Name;
                destTabs.AddTableDefinitionRow(destTab);
            }
            ImportView(srcView, destTab);
        }

        public static void ImportView(IView srcView, TableDefinitionRow destTab)
        {
            destTab.TableAlias = srcView.Alias;
            if (srcView.Description.Length > 0)
                destTab.TableComment = srcView.Description;
            destTab.DBTableType = srcView.Type;
            destTab.OriginalName = GetOriginalName(srcView.Guid, srcView.Name);

            ImportFields(srcView.Columns, destTab);
        }
        #endregion

        #region tables
        public static void ImportTables(IDatabase srcDb, SchemaXsd3bEx destSet)
        {
            foreach (ITable srcTab in srcDb.Tables)
                ImportTable(srcTab, destSet.TableDefinition);
        }

        public static void ImportTable(ITable srcTab, TableDefinitionDataTable destTabs)
        {
            TableDefinitionRow destTab = destTabs.FindByTableName(srcTab.Name);
            if (destTab == null)
            {
                destTab = destTabs.NewTableDefinitionRow();
                destTab.TableName = srcTab.Name;
                destTabs.AddTableDefinitionRow(destTab);
            }
            ImportTable(srcTab, destTab);
        }

        private static string GetOriginalName(Guid guid, String name)
        {
            if ((guid != null) && (guid != Guid.Empty))  // && (srcTab.Guid != DBNull.Value))
                return guid.ToString();
            return name;
        }

        public static void ImportTable(ITable srcTab, TableDefinitionRow destTab)
        {
            destTab.TableAlias = srcTab.Alias;
            if (srcTab.Description.Length > 0)
                destTab.TableComment = srcTab.Description;
            destTab.DBTableType = srcTab.Type;
            destTab.OriginalName = GetOriginalName(srcTab.Guid,srcTab.Name);

            ImportFields(srcTab.Columns, destTab);
        }

        private static int nextPkNumber = 1;
        public static void ImportFields(IColumns srcColumns, TableDefinitionRow destTab)
        {
            nextPkNumber = 1;
            foreach(IColumn srcCol in srcColumns)
                ImportField(srcCol, destTab);
        }

        private static DataTypeDefinitionRow GetType(IColumn srcCol, DataTypeDefinitionDataTable types)
        {
            // DataTypeName=Hyperlink, DataTypeNameComplete=Text (255)
            String dataTypeNameComplete = srcCol.DataTypeNameComplete
                .Replace(" ", "")
                .Replace("(", "_")
                .Replace(")", "_");
            String dataTypeName = srcCol.DataTypeName
                .Replace(" ", "")
                .Replace("(", "_")
                .Replace(")", "_");

            if (dataTypeNameComplete.Length == 0)
                dataTypeNameComplete = dataTypeName;
            if (dataTypeName.Length == 0)
                dataTypeName = dataTypeNameComplete;

            int baseTypeId = srcCol.DataType;

            if (baseTypeId == 0)
                baseTypeId = Mapping.GetOleDbTypeIDFromOleDbTypeName(dataTypeNameComplete);
            if (baseTypeId == 0)
                baseTypeId = Mapping.GetOleDbTypeIDFromOleDbTypeName(dataTypeName);

            // include System.XXXX
            DataTypeDefinitionRow baseType = types.IncludeDataTypeDefinition(Mapping.GetSystemTypeFromOleDBTypeID(baseTypeId), null);

            string oleTypeName = (baseTypeId != 0) ? Mapping.GetOleDBTypeNameFromID(baseTypeId) : null;
            if (oleTypeName != null)
                baseType = types.IncludeDataTypeDefinition("sql." + oleTypeName, baseType.DataTypeName);
            if (dataTypeName.Length > 0)
                baseType = types.IncludeDataTypeDefinition("sql." + dataTypeName, baseType.DataTypeName);

            if (srcCol.CharacterMaxLength > 0)
                dataTypeNameComplete += "_" + srcCol.CharacterMaxLength.ToString() + "_";

            baseType = types.IncludeDataTypeDefinition("app." + dataTypeNameComplete, baseType.DataTypeName);

            if (srcCol.CharacterMaxLength > 0)
                baseType.StringSize = srcCol.CharacterMaxLength;
            return baseType;
        }

        public static void ImportField(IColumn srcCol, TableDefinitionRow destTab)
        {
            DataTypeDefinitionRow dataType = GetType(srcCol, destTab.Table.DataSet.DataTypeDefinition);

            FieldDefinitionRow destField = destTab.FindFieldByAliasOrName(srcCol.Name); // destTab.Table.DataSet.FieldDefinition destTabs.FindByTableName(srcTab.Name);

            if (destField == null)
                destField = destTab.FindFieldByAliasOrName(srcCol.Alias);
            if (destField == null)
            {
                destField = destTab.AddFieldDefinitionRow(srcCol.Name, dataType.DataTypeName);
            }
            else
            {
                destField.DataTypeDefinitionRow = dataType;
            }

            // destField.DBFieldType = "TAB_FIELD"; // default value
            destField.FieldAlias = srcCol.Alias;
            if (srcCol.Description.Length > 0)
                destField.FieldComment = srcCol.Description;
            destField.AllowDBNull = srcCol.IsNullable;
            destField.AutoIncrement = srcCol.IsAutoKey;
            if (srcCol.IsAutoKey)
            {
                destField.AutoIncrementSeed = srcCol.AutoKeySeed;
                destField.AutoIncrementStep = srcCol.AutoKeyIncrement;
            }
            System.Diagnostics.Debug.WriteLine(string.Format(
                "FeldName={4}.{3}, DataTypeName={0}, DataTypeNameComplete={1}, DbTargetType={2}"
                , srcCol.DataTypeName
                , srcCol.DataTypeNameComplete
                , srcCol.DbTargetType
                , srcCol.Name
                , destTab.TableName));
            /* destField.DataType = ???;
            srcCol.DataType; // int
            srcCol.DataTypeName;
            srcCol.DataTypeNameComplete;
            srcCol.DbTargetType;
             */


            // srcCol.ForeignKeys;
            if (srcCol.HasDefault)
                destField.DefaultValue = srcCol.Default;
            else
                destField.SetDefaultValueNull();
            destField.OriginalName = GetOriginalName(srcCol.Guid, srcCol.Name);
            destField.ReadOnly = srcCol.IsComputed; //  || srcCol.???
            // destField.StringSize = srcCol.CharacterMaxLength; moved to DataTypeDefinition
            destField.Unique = srcCol.IsAutoKey; // || ??? the only PK or the only FK
            if (srcCol.IsInPrimaryKey)
            {
                if (srcCol.Ordinal > 0)
                    destField.PrimaryKeyNumber = srcCol.Ordinal;
                else
                    destField.PrimaryKeyNumber = nextPkNumber++;
            }

            /** unsupported properties
            destField.FieldExpression = srcCol.???
            destField.NullValue = srcCol.??? // nullvaluereplacement
            destField.XmlFieldType = ???; // default is Attribute

            // srcCol.AllProperties;
            srcCol.CharacterOctetLength;
            srcCol.CharacterSetCatalog;
            srcCol.CharacterSetName;
            srcCol.CharacterSetSchema;
            srcCol.CompFlags;
            srcCol.Flags;
            srcCol.GlobalProperties;

            srcCol.DateTimePrecision; // int
            srcCol.HasDomain;
            srcCol.Domain; // IDomain
            srcCol.DomainCatalog;
            srcCol.DomainName;
            srcCol.DomainSchema;
            srcCol.LanguageType;
            srcCol.LCID;
            srcCol.NumericPrecision;
            srcCol.NumericScale;
            srcCol.Ordinal;
            srcCol.Properties;
            srcCol.PropID;
            srcCol.SortID;
            srcCol.TDSCollation;
            srcCol.TypeGuid;
            srcCol.UserDataXPath;

*/

        }
        #endregion

        #region relations
        public static void ImportRelations(IDatabase srcDb, SchemaXsd3bEx destSet)
        {
            TableDefinitionDataTable destTabs = destSet.TableDefinition;
            foreach (ITable srcTab in srcDb.Tables)
            {
                ImportChildRelations(srcTab, destTabs);
            }
        }

        public static void ImportChildRelations(ITable srcTab, TableDefinitionDataTable destTabs)
        {
            foreach (IForeignKey srcFK in srcTab.ForeignKeys)
            {
                if (srcTab == srcFK.PrimaryTable)   // use only childrelations
                {
                    TableDefinitionRow childTab = destTabs.FindByTableName(srcFK.ForeignTable.Name);
                    TableDefinitionRow parentTab = destTabs.FindByTableName(srcFK.PrimaryTable.Name);
                    if ((childTab == null) || (parentTab == null))
                        return; // both tables are not included

                    String relationName =
                    parentTab.Table.DataSet.RelationDefinition.CreateUniqueRelationName(srcFK.Name, childTab.TableName, parentTab.TableName);

                    RelationDefinitionRow curRelation = parentTab.InsertNewRelation(relationName, childTab.TableName);

                    IColumns parentColums = srcFK.PrimaryColumns;
                    IColumns childColums = srcFK.ForeignColumns;

                    FieldRelationDefinitionRow curFieldRelation = null;

                    for (int colNo = 0; colNo < parentColums.Count; colNo++)
                    {
                        curFieldRelation = curRelation.InsertNewFieldRelation(parentColums[colNo].Name, childColums[colNo].Name);
                    }
                } // if (srcTab == srcFK.PrimaryTable)   // use only childrelations
            } // foreach (IForeignKey srcFK in srcTab.ForeignKeys)

        }
        #endregion

    }
}
