using MyMeta;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyMeta.Plugins
{
	public class VisualFoxProPlugin : IMyMetaPlugin
	{
		private const string PROVIDER_KEY = @"VISUALFOXPRO";
		private const string PROVIDER_NAME = @"Visual FoxPro";
		private const string AUTHOR_INFO = @"Visual Fox Pro MyMeta plugin written by Timothy Moriarty.";
		private const string AUTHOR_URI = @"http://www.mygenerationsoftware.com/";
		private const string SAMPLE_CONNECTION = @"Provider=vfpoledb.1;data source=C:\Program Files\Microsoft Visual FoxPro OLE DB Provider\Samples\Northwind\northwind.dbc;";
		private IMyMetaPluginContext context = null;
        private static Dictionary<int, string> typeLookup = new Dictionary<int, string>()
        {
            {3, "Integer"}
            ,{5, "Double"}
            ,{6, "Currency"}
            ,{7, "Date"}
            ,{11, "Logical"}
            ,{128, "Varbinary"}
            ,{129, "Varchar"}
            ,{131, "Float"}
            ,{133, "Date"}
            ,{135, "Date Time"}
        };

        private static Dictionary<string, int> reverseTypeLookup = new Dictionary<string, int>()
        {
            {"integer", 3}
            ,{"double", 5}
            ,{"Currency", 6}
            ,{"logical", 11}
            ,{"varbinary", 128}
            ,{"varchar", 129}
            ,{"float", 131}
            ,{"date", 133}
            ,{"date time", 135}
            ,{"string", 129}
        };

        private const string DATA_TYPE_INTEGER = "Integer";
        private const string DATA_TYPE_DOUBLE = "Double";
        private const string DATA_TYPE_CURRENCY = "Currency";
        private const string DATA_TYPE_LOGICAL = "Logical";
        private const string DATA_TYPE_VARBINARY = "Varbinary";
        private const string DATA_TYPE_VARCHAR = "Varchar";
        private const string DATA_TYPE_FLOAT = "Float";
        private const string DATA_TYPE_DATE = "Date";
        private const string DATA_TYPE_DATE_TIME = "Date Time";

		private bool IsIntialized { get { return (context != null); } }

		public void Initialize(IMyMetaPluginContext context)
		{
			this.context = context;
		}

		public string ProviderName
		{
			get { return PROVIDER_NAME; }
		}

		public string ProviderUniqueKey
		{
			get { return PROVIDER_KEY; }
		}

		public string ProviderAuthorInfo
		{
			get { return AUTHOR_INFO; }
		}

		public Uri ProviderAuthorUri
		{
			get { return new Uri(AUTHOR_URI); }
		}

		public bool StripTrailingNulls
		{
			get { return false; }
		}

		public bool RequiredDatabaseName
		{
			get { return false; }
		}

		public string SampleConnectionString
		{
			get { return SAMPLE_CONNECTION; }
		}

		public IDbConnection NewConnection
		{
			get
			{
				if (IsIntialized)
					return new OleDbConnection(context.ConnectionString);
				else
					return null;
			}
		}

		public string DefaultDatabase
		{
			get
			{
				using (OleDbConnection c = this.NewConnection as OleDbConnection)
				{
					c.Open();
					string defaultDB = c.DataSource;
					c.Close();

					return defaultDB;
				}
			}
		}

		public DataTable Databases
		{
			get
			{
				using (OleDbConnection c = this.NewConnection as OleDbConnection)
				{
					c.Open();
					DataTable dt = context.CreateDatabasesDataTable();
					DataRow row = dt.NewRow();
					row["CATALOG_NAME"] = c.DataSource;
					row["DESCRIPTION"] = "The folder where the delimited text files reside.";
					dt.Rows.Add(row);
					c.Close();

					return dt;
				}
			}
		}

		public DataTable GetTables(string database)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                DataTable tableMetaData = c.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                c.Close();
                return tableMetaData;
            }
		}

		public DataTable GetViews(string database)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                DataTable viewMetaData = c.GetOleDbSchemaTable(OleDbSchemaGuid.Views, new object[] { null, null, null });
                c.Close();
                return viewMetaData;
            }
		}

		public DataTable GetProcedures(string database)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                DataTable meta = c.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, new object[] { null, null, null });;
                c.Close();
                return meta;
            }
		}

		public DataTable GetDomains(string database)
		{
			return new DataTable();
		}

		public DataTable GetProcedureParameters(string database, string procedure)
		{
            DataTable metaData = context.CreateParametersDataTable();
            this.ParseProcedureForParameters(metaData, procedure);
            metaData.AcceptChanges();
            return metaData;
		}

		public DataTable GetProcedureResultColumns(string database, string procedure)
		{
			return new DataTable();
		}

		public DataTable GetViewColumns(string database, string view)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                DataTable meta = c.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, view });
                this.SetDataTypes(c, database, view, meta);
                c.Close();
                return meta;
            }
		}

		public DataTable GetTableColumns(string database, string table)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                DataTable meta = c.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, table });
                this.SetDataTypes(c, database, table, meta);
                c.Close();
                return meta;
            }
		}

		public List<string> GetPrimaryKeyColumns(string database, string table)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                List<string> primaryKeys = new List<string>();
                c.Open();
                DataTable meta = c.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new object[] { null, null, table });

                foreach (DataRow row in meta.Rows)
                {
                    primaryKeys.Add(row["PK_NAME"].ToString());
                }
                c.Close();
                return primaryKeys;
            }
		}

		public List<string> GetViewSubViews(string database, string view)
		{
			return new List<string>();
		}

		public List<string> GetViewSubTables(string database, string view)
		{
			return new List<string>();
		}

		public DataTable GetTableIndexes(string database, string table)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                DataTable meta = c.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new object[] { null, null, null, null, table });
                c.Close();
                return meta;
            }
		}

		public DataTable GetForeignKeys(string database, string table)
		{
            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                DataTable meta = c.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new object[] { null, null, table });
                c.Close();
                return meta;
            }
		}

		public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
		{
			return null;
		}

        private void ParseProcedureForParameters(DataTable meta, string procedure)
        {
            DataTable procedures;
            string procedure_name = string.Empty;
            string procedure_text = string.Empty;

            using (OleDbConnection c = this.NewConnection as OleDbConnection)
            {
                c.Open();
                procedures = c.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, new object[] { null, null, null });;
                c.Close();
            }

            if (procedures != null)
            {
                procedures.PrimaryKey = new DataColumn[] { procedures.Columns["PROCEDURE_NAME"] };
                DataRow row = procedures.Rows.Find(procedure);

                if (row != null)
                {
                    procedure_text = row["PROCEDURE_DEFINITION"].ToString().ToLower();
                    procedure_name = row["PROCEDURE_NAME"].ToString();
                    System.Diagnostics.Debug.WriteLine(procedure_text);
                    System.Diagnostics.Debug.WriteLine(procedure_name);

                    Regex regex_procedure_text = new Regex(procedure_name + @"\(.*?\)");
                    Match match = regex_procedure_text.Match(procedure_text);

                    if (match.Success)
                    {
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


                        string parameter_clause = match.Value;
                        System.Diagnostics.Debug.WriteLine(parameter_clause);
                        parameter_clause = parameter_clause.Replace(")", string.Empty);
                        parameter_clause = parameter_clause.Replace(procedure_name.ToLower() + "(", string.Empty);

                        System.Diagnostics.Debug.WriteLine(parameter_clause);

                        string[] params_list = parameter_clause.Split(',');

                        for (int i = 0; i < params_list.Length; i++)
                        {
                            Regex regex_name_type = new Regex("as");
                            string[] nameAndType = regex_name_type.Split(params_list[i]);

                            foreach (string s in nameAndType)
                            {
                                System.Diagnostics.Debug.WriteLine(s);
                            }

                            if (nameAndType.Length == 2)
                            {
                                DataRow new_row = meta.NewRow();
                                new_row["PROCEDURE_CATALOG"] = string.Empty;
                                new_row["PROCEDURE_SCHEMA"] = string.Empty;
                                new_row["PROCEDURE_NAME"] = procedure_name;
                                new_row["PARAMETER_NAME"] = nameAndType[0].Trim();
                                new_row["ORDINAL_POSITION"] = i + 1;
                                new_row["PARAMETER_TYPE"] = reverseTypeLookup[nameAndType[1].Trim()];
                                new_row["PARAMETER_HASDEFAULT"] = false;
                                new_row["PARAMETER_DEFAULT"] = string.Empty;
                                new_row["IS_NULLABLE"] = true;
                                new_row["DATA_TYPE"] = reverseTypeLookup[nameAndType[1].Trim()];
                                new_row["CHARACTER_MAXIMUM_LENGTH"] = 0;
                                new_row["CHARACTER_OCTET_LENGTH"] = 0;
                                new_row["NUMERIC_PRECISION"] = 0;
                                new_row["DESCRIPTION"] = string.Empty;
                                new_row["TYPE_NAME"] = string.Empty;
                                new_row["LOCAL_TYPE_NAME"] = string.Empty;
                                meta.Rows.Add(new_row);
                            }
                        }
                    }
                }
            }
        }

		private void SetDataTypes(OleDbConnection conn, string database, string entityName, DataTable metaData)
		{
			if (!metaData.Columns.Contains("TYPE_NAME"))
			{
				metaData.Columns.Add("TYPE_NAME");
			}
			if (!metaData.Columns.Contains("TYPE_NAME_COMPLETE"))
			{
				metaData.Columns.Add("TYPE_NAME_COMPLETE");
			}


            foreach (DataRow row in metaData.Rows)
            {
                if (row["DATA_TYPE"] != DBNull.Value)
                {
                    int dataType = (int)row["DATA_TYPE"];
                    string typeName = typeLookup[dataType];
                    row["TYPE_NAME"] = typeName;
                    this.SetDataTypeComplete(conn, row, entityName, row["COLUMN_NAME"].ToString(), typeName);
                }
            }
		}

		private void SetDataTypeComplete(OleDbConnection conn, DataRow row, string entityName, string fieldName, string typeName)
		{
			if (typeName == "Varchar" || typeName == "Varbinary")
			{
				OleDbCommand command = new OleDbCommand();
				command.Connection = conn;
				command.CommandText = string.Format("SELECT FSIZE('{0}') FROM {1}", fieldName, entityName);
				command.CommandType = CommandType.Text;

				object o = command.ExecuteScalar();
				int fsize;

				if (o != null && int.TryParse(o.ToString(), out fsize))
				{
					row["TYPE_NAME_COMPLETE"] = string.Format("{0}({1})", typeName, fsize);
					System.Diagnostics.Debug.WriteLine(string.Format("{0}({1})", typeName, fsize));
				}
			}
			else
			{
				row["TYPE_NAME_COMPLETE"] = typeName;
			}
		}
	}
}