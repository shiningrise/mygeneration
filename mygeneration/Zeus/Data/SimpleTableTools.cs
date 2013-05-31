using System;
using System.Data;
using System.Collections;
using System.Reflection;

namespace Zeus.Data
{
	public enum SimpleTableJoinType 
	{
		InnerJoin,
		OuterJoin,
		CrossJoin,
		Exclude,
		Union
	}

	/// <summary>
	/// Summary description for SimpleTableTools.
	/// </summary>
	public class SimpleTableTools
	{
		public SimpleTableTools()
		{

		}

		public void FilterTable(SimpleTable table, string filterExpression) 
		{
			//TODO: finish filter functionality later! definitely not a top priority
		}

		public void SortTable(SimpleTable table, string sortExpression) 
		{
			//TODO: finish sort functionality later! definitely not a top priority
		}

		public SimpleTable JoinTables(SimpleTableJoinType joinType, SimpleTable table1, SimpleTable table2, string filterExpression) 
		{
			//TODO: finish join functionality later! definitely not a top priority
			SimpleTable newtable = new SimpleTable();
			return newtable;
		}

		private static SimpleTable BuildSimpleTable(IEnumerable collection)
		{
			SimpleTable newtable = new SimpleTable();
			PropertyInfo[] props;
			object obj = null;
			SimpleRow newrow = null;
			
			int idx = 0;
			foreach (object item in collection) 
			{
				props = item.GetType().GetProperties();
				newrow = newtable.Rows.Add();

				foreach (PropertyInfo prop in props) 
				{
					if (idx == 0) 
					{
						if (prop.CanRead &&
							((prop.PropertyType == typeof(String)) ||
							(prop.PropertyType == typeof(Char)) ||
							(prop.PropertyType == typeof(Byte)) ||
							(prop.PropertyType == typeof(Int64)) ||
							(prop.PropertyType == typeof(Int32)) ||
							(prop.PropertyType == typeof(Int16)) ||
							(prop.PropertyType == typeof(UInt64)) ||
							(prop.PropertyType == typeof(UInt32)) ||
							(prop.PropertyType == typeof(UInt16)) ||
							(prop.PropertyType == typeof(Boolean)) ||
							(prop.PropertyType == typeof(Decimal)) ||
							(prop.PropertyType == typeof(Double)) ||
							(prop.PropertyType == typeof(DateTime)) ||
							(prop.PropertyType == typeof(TimeSpan)))
							) 
						{
							newtable.Columns.Add(prop.Name);
						}
					}
					
					if (newtable.Columns.IndexOf(prop.Name) != -1) 
					{
						obj = prop.GetGetMethod().Invoke(item, null);
						newrow[prop.Name] = obj;
					}
				}

				idx++;
			}

			return newtable;
		}

		private static SimpleTable BuildSimpleTable(DataTable table)
		{
			SimpleTable newtable = new SimpleTable();

			foreach (DataColumn column in table.Columns) 
			{
				newtable.Columns.Add(column.ColumnName);
			}

			foreach (DataRow row in table.Rows) 
			{
				object[] items = row.ItemArray;
				SimpleRow newrow = newtable.Rows.Add();

				for (int i = 0; i < items.Length; i++) 
				{
					object item = items[i];
					if (item != DBNull.Value)
					{
						newrow[i] = item;
					}
				}
			}

			return newtable;
		}

		private static SimpleTable BuildSimpleTable(DataView view)
		{
			SimpleTable newtable = new SimpleTable();

			foreach (DataColumn column in view.Table.Columns) 
			{
				newtable.Columns.Add(column.ColumnName);
			}

			foreach (DataRowView row in view) 
			{
				object[] items = row.Row.ItemArray;
				SimpleRow newrow = newtable.Rows.Add();

				for (int i = 0; i < items.Length; i++) 
				{
					object item = items[i];
					if (item != DBNull.Value)
					{
						newrow[i] = item;
					}
				}
			}

			return newtable;
		}

		private static SimpleTable BuildSimpleTable(IDataReader reader)
		{
			SimpleTable newtable = new SimpleTable();

			for (int i = 0; i < reader.FieldCount; i++) 
			{
				newtable.Columns.Add(reader.GetName(i));
			}

			while (reader.Read()) 
			{
				SimpleRow newrow = newtable.Rows.Add();

				for (int i = 0; i < reader.FieldCount; i++) 
				{
					object item = reader[i];
					
					if (item != DBNull.Value)
					{
						newrow[i] = item;
					}
				}
			}

			return newtable;
		}
	
		private static SimpleTable BuildSimpleTable(object sourceObject)
		{
			SimpleTable newtable = null;

			if (sourceObject is DataTable) 
			{
				newtable = BuildSimpleTable(sourceObject as DataTable);
			}
			else if (sourceObject is DataView) 
			{
				newtable = BuildSimpleTable(sourceObject as DataView);
			}
			else if (sourceObject is IDataReader) 
			{
				newtable = BuildSimpleTable(sourceObject as IDataReader);
			}
			else if (sourceObject is IEnumerable) 
			{
				newtable = BuildSimpleTable(sourceObject as IEnumerable);
			}
			else 
			{
				newtable = new SimpleTable();
			}
			return newtable;
		}

		public static SimpleTable ConvertToSimpleTable(object dataSource)
		{
			return BuildSimpleTable(dataSource);
		}

		public static DataTable ConvertToDataTable(SimpleTable table)
		{
			DataTable newtable = new DataTable();

			foreach (SimpleColumn column in table.Columns) 
			{
				newtable.Columns.Add(column.Name);
			}

			foreach (SimpleRow row in table.Rows) 
			{
				DataRow newrow = newtable.NewRow();

				foreach (SimpleColumn column in table.Columns) 
				{
					newrow[column.Name] = row[column.Name];
				}

				newtable.Rows.Add(newrow);
			}

			return newtable;
		}
	}
}