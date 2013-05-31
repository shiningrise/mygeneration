using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

using Zeus.UserInterface;
using Zeus.Data;

namespace Zeus
{
	/// <summary>
	/// The ZeusInput object contains key/value pairs with any input provided by either the interface 
	/// code segment, or the application. 
	/// </summary>
	/// <remarks>
	/// The ZeusInput object is a data container, similar to a hashtable, that contains all of the input 
	/// data for use in the template code. Along with the user supplied input, it contains application 
	/// variables. The application variables MyGeneration populates before executing a template are the following:
	/// <list type="table">
	/// <item><term>__version</term><description>The MyGeneration Application Version.</description></item>
	/// <item><term>__defaultTemplatePath</term><description>The location of templates that should be displayed in the template browser.</description></item>
	/// <item><term>__defaultOutputPath</term><description>The default output path.</description></item>
	/// <item><term>__dbDriver</term><description>The default database driver.</description></item>
	/// <item><term>__dbConnectionString</term><description>The default database connection string.</description></item>
	/// <item><term>__domainOverride</term><description>True or false based on the Domain Override setting in the default Application Settings.</description></item>
	/// <item><term>__dbTarget</term><description>The selected DB Target</description></item>
	/// <item><term>__dbTargetMappingFileName</term><description>The DB Targets file path.</description></item>
	/// <item><term>__dbLanguageMappingFileName</term><description>The Language Mapping file path.</description></item>
	/// <item><term>__language</term><description>The selected language.</description></item>
	/// <item><term>__userMetaDataFileName</term><description>The User Meta-Data file path.</description></item>	/// </list>
	/// The same instance of the ZeusInput object remains in the template context through both the Interface Code Segment 
	/// and Template Body segment. 
	/// </remarks>
	/// <example>
	/// VBScript:
	/// <code>
	/// Dim databaseName 
	/// Dim tableNames
	/// databaseName = input.Item("cmbDatabase")
	/// Set tableNames = input.Item("lstTables")
	/// </code>
	/// </example>
	/// <example>
	/// JScript:
	/// <code>
	/// var databaseName = input.Item("cmbDatabase"); 
	/// var tablenames = input.Item("lstTables"); 
	/// </code>
	/// </example>
	/// <example>
	/// CSharp:
	/// <code>
	/// string databaseName = input["cmbDatabase"].ToString(); 
	/// ArrayList tablenames = input["lstTables"] as ArrayList; 
	/// </code>
	/// </example>
	public class ZeusInput : IZeusInput
	{
		private Hashtable _invars = new Hashtable();

		/// <summary>
		/// Creates a new ZeusInput object.
		/// </summary>
		public ZeusInput() {}

		/// <summary>
		/// Sets or retrieves the item that belongs to the passed in key.
		/// </summary>
		public object this[string key] 
		{
			get { return _invars[key]; }
			set { _invars[key] = value; }
		}

		/// <summary>
		/// Removes the item that belongs to the passed in key.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(object key) 
		{
			_invars.Remove(key.ToString());
		}

		/// <summary>
		/// Returns true if the passed in key exists.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>True if the passed in key exists.</returns>
		public bool Contains(object key) 
		{
			return _invars.ContainsKey(key.ToString());
		}

		/// <summary>
		/// Adds all of the items from the passed in collection.
		/// Supported collection types are, for example, HashTable, GuiController, and NameValueCollection
		/// </summary>
		/// <param name="collection"></param>
		public void AddItems(object collection) 
		{
			if (collection is GuiController)
			{
				GuiController controller = collection as GuiController;
				foreach (GuiControl control in controller) 
				{
					if (control.IsDataControl) 
					{
						this._invars[control.ID] = control.Value;
					}
				}
			}
			else if (collection is Hashtable)
			{
				Hashtable hash = collection as Hashtable;
				foreach (object key in hash.Keys) 
				{
					this._invars[key] = hash[key];
				}
			}
			else if (collection is NameValueCollection)
			{
				NameValueCollection nvc = collection as NameValueCollection;
				foreach (string key in nvc.Keys) 
				{
					this._invars[key] = nvc[key];
				}
			}
#if !HTML_HELP
			else if (collection is InputItemCollection)
			{
				InputItemCollection iic = collection as InputItemCollection;
				foreach (InputItem item in iic) 
				{
					this._invars[item.VariableName] = item.DataObject;
				}
			}
#endif
		}

		/// <summary>
		/// Returns an ICollection of all the keys from the ZeusInput hash
		/// </summary>
		public ICollection Keys
		{
			get 
			{
				return this._invars.Keys;
			}
		}

		/// <summary>
		/// Returns an ICollection of all the values from the ZeusInput hash
		/// </summary>
		public ICollection Values
		{
			get 
			{
				return this._invars.Values;
			}
		}

		/// <summary>
		/// Returns true if the array of keys all exist in the ZeusInput hash
		/// </summary>
		public bool ContainsKeys(object[] keys) 
		{
			bool haskeys = true;
			foreach (object key in keys) 
			{
				if (!_invars.ContainsKey(key.ToString()) ) 
				{
					haskeys = false;
					break;
				}
			}
			return haskeys;
		}

		/// <summary>
		/// Returns a new instance of a serializable SimpleTable collection.
		/// </summary>
		/// <returns>A new instance of a serializable SimpleTable collection.</returns>
		public SimpleTable CreateSimpleTable() 
		{
			return new SimpleTable();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exclude/>
		public string __Variables
		{
			get 
			{
				StringBuilder sb = new StringBuilder();
				foreach (string key in this._invars.Keys) 
				{
					sb.Append(key);
					sb.Append(": (");
					sb.Append(this._invars[key].GetType().FullName);
					sb.Append(") ");
					sb.Append(this._invars[key].ToString());
					sb.Append("\r\n");
				}

				return sb.ToString();
			}
		}
	}
}