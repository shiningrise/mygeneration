using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Zeus.UserInterface
{
	/// <summary>
	/// The GuiDataBinder is a utility class used to bind different data types to IGuiListControl's.
	/// </summary>
	/// <remarks>
	/// The GuiDataBinder is a utility class used to bind different data types to IGuiListControl's.
	/// </remarks>
	internal class GuiDataBinder
	{
		/// <summary>
		/// Creates a new instance of the GuiDataBinder.
		/// </summary>
		public GuiDataBinder() {}

		/// <summary>
		/// Binds a DataTable to an IGuiListControl. 
		/// </summary>
		/// <param name="control">The control to populate data in.</param>
		/// <param name="dataSource">The datatable</param>
		/// <param name="valueFieldName">The column name for the value</param>
		/// <param name="textFieldName">The column name for the text</param>
		public static void BindDataToListControl(IGuiListControl control, object dataSource, string valueFieldName, string textFieldName) 
		{
			control.Clear();

			if (dataSource is DataTable) 
			{
				DataTable source = dataSource as DataTable;

				if ( (source.Columns.IndexOf(valueFieldName) >= 0) && 
					(source.Columns.IndexOf(textFieldName) >= 0) )
				{
					foreach (DataRow row in source.Rows) 
					{
						control[row[valueFieldName].ToString()] = row[textFieldName].ToString();
					}
				}
			}
		}

		/// <summary>
		/// Binds a DataSource (ArrayList, Hashtable, NameValueCollection) to an IGuiListControl. 
		/// Will also bind any other object that implements IEnumerable and has properties
		/// named "ItemName" and "ItemValue".
		/// </summary>
		/// <param name="control">The control to populate data in.</param>
		/// <param name="dataSource">The data source object.</param>
		public static void BindDataToListControl(IGuiListControl control, object dataSource) 
		{
			control.Clear();

			if (dataSource is Hashtable) 
			{
				Hashtable source = dataSource as Hashtable;

				foreach (string key in source.Keys) 
				{
					control[key] = source[key].ToString();
				}
			}
			else if (dataSource is NameValueCollection) 
			{
				NameValueCollection source = dataSource as NameValueCollection;

				foreach (string key in source.Keys) 
				{
					control[key] = source[key];
				}
			}
			else if (dataSource is ArrayList) 
			{
				ArrayList source = dataSource as ArrayList;

				for (int i = 0; i < source.Count; i++) 
				{
					control[i.ToString()] = source[i].ToString();
				}
			}
			else if (dataSource is string[]) 
			{
				string[] source = dataSource as string[];

				for (int i = 0; i < source.Length; i++) 
				{
					control[i.ToString()] = source[i];
				}
			}
			else if (dataSource is object[]) 
			{
				object[] source = dataSource as object[];

				for (int i = 0; i < source.Length; i++) 
				{
					control[i.ToString()] = source[i].ToString();
				}
			}
			else if (dataSource is IDictionary) 
			{
				IDictionary dictionary = dataSource as IDictionary;
				foreach (object key in dictionary.Keys) 
				{
					control[key.ToString()] = dictionary[key].ToString();
				}
			}
			else if (dataSource is Scripting.Dictionary) 
			{
				Scripting.Dictionary dictionary = dataSource as Scripting.Dictionary;
				Array items = dictionary.Items() as Array;
				Array keys = dictionary.Keys() as Array;
				for (int i = 0; i < keys.Length; i++) 
				{
					control[keys.GetValue(i).ToString()] = items.GetValue(i).ToString();
				}
			}
			else if (dataSource is IEnumerable)
			{
				IEnumerable collection = dataSource as IEnumerable;

				PropertyInfo pinfoName, pinfoValue;
				string name, val;
				foreach (object item in collection) 
				{
					pinfoName = item.GetType().GetProperty("ItemValue");
					pinfoValue = item.GetType().GetProperty("ItemName");
					
					if ((pinfoName != null) && (pinfoValue != null)) 
					{
						name = pinfoName.GetGetMethod().Invoke(item, null) as String;
						val = pinfoValue.GetGetMethod().Invoke(item, null) as String;
						
						control.Items[val] = name;
					}
					else
					{
						break;
					}
				}
			}
		}
	}
}
