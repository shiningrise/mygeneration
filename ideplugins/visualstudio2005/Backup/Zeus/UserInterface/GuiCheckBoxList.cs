using System;
using System.Collections;
using System.Collections.Specialized;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A CheckBox List control. You can bind ArrayList, NameValueCollection, and Hashtable 
	/// objects to the GuiCheckBoxList along with IEnumerable objects with both ItemName 
	/// and ItemValue properties (using reflection). 
	/// </summary>
	/// <remarks>
	/// A CheckBox List control. You can bind ArrayList, NameValueCollection, and Hashtable 
	/// objects to the GuiCheckBoxList along with IEnumerable objects with both ItemName 
	/// and ItemValue properties (using reflection). 
	/// </remarks>
	/// <example>
	/// Binding the MyMeta collection to the GuiCheckBoxList (jscript)
	/// <code>
	/// var db = MyMeta.Databases.Item(sDatabaseName);
	/// 
	/// var chklstTables = ui.AddCheckBoxList("chklstTables", "Select tables.");
	/// chklstTables.Height = 150;
	/// chklstTables.BindData(db.Tables);
	/// </code>
	/// </example>
	/// <example>
	/// Binding an ArrayList the GuiCheckBoxList (csharp). Note: The ArrayList index is bound to the item value.
	/// <code>
	/// GuiCheckBoxList chklstFruit = ui.AddCheckBoxList("chklstFruit", "Select fruit:");
	///  
	/// ArrayList al = new ArrayList();
	/// al.Add("Apples");
	/// al.Add("Oranges");
	/// al.Add("Pears");
	/// 
	/// chklstFruit.BindData(al);
	/// </code>
	/// </example>
	/// <example>
	/// Selecting items in a GuiCheckBoxList (jscript)
	/// <code>
	/// var db = MyMeta.Databases.Item(sDatabaseName);
	/// 
	/// var chklstTables = ui.AddCheckBoxList("chklstTables", "Select tables.");
	/// lstTables.Height = 150;
	/// lstTables.BindData(db.Tables);
	/// lstTables.IsMultiSelect = true;
	/// 
	/// // Select all items
	/// lstTables.SelectAll();
	/// 
	/// // Clear Selections
	/// lstTables.ClearSelected();
	/// 
	/// // Select specific items
	/// lstTables.Select("tblUsers");
	/// lstTables.Select("tblRoles");
	///
	/// // Another way to select items
	/// lstTables.SelectedItems.Add("tblMaps");
	/// lstTables.SelectedItems.Add("tblWeapons");
	/// 
	/// // Clear All Items and selections
	/// lstTables.Clear();
	/// </code>
	/// </example>
	/// <example>
	/// Manually adding Items to the GuiCheckBoxList (CSharp)
	/// <code>
	/// GuiCheckBoxList chklstVeggies = ui.AddCheckBoxList("chklstVeggies", "Select veggies.");
	/// lstVeggies["a"] = "Asparagus";
	/// lstVeggies["c"] = "Carrot";
	/// lstVeggies["t"] = "Tomato";
	/// </code>
	/// </example>
	public class GuiCheckBoxList : GuiControl, IGuiCheckBoxList, IGuiListControl
	{
		private bool _sorted = false;
		private int _height = 64;
		private ArrayList _selectedItems = new ArrayList();
		private NameValueCollection _items = new NameValueCollection();
		private ArrayList _onclickEvents = new ArrayList();

		/// <summary>
		/// Creates a new GuiCheckBoxList control.
		/// </summary>
		public GuiCheckBoxList() {}

		/// <summary>
		/// Returns an ArrayList of selected items
		/// </summary>
		public override object Value 
		{
			get { return this._selectedItems; }
		}

		/// <summary>
		/// Returns the Text for the item with the given val.
		/// </summary>
		public string this[string val]
		{
			get
			{
				return this._items[val];
			}
			set 
			{
				this._items[val] = value;
			}
		}

		/// <summary>
		/// If set to true, the checkbox list is sorted.
		/// </summary>
		public bool Sorted
		{
			get
			{
				return this._sorted;
			}
			set 
			{
				this._sorted = true;
			}
		}

		/// <summary>
		/// The item count.
		/// </summary>
		public int Count 
		{
			get
			{
				return this._items.Count;
			}
		}

		/// <summary>
		/// The first selected index of this list control.
		/// </summary>
		public int SelectedIndex 
		{
			get
			{
				int[] indeces = this.SelectedIndeces;
				if (indeces.Length > 0) 
				{
					return SelectedIndeces[0];
				}
				else 
				{
					return -1;
				}
			}
		}

		/// <summary>
		/// The selected indeces of this list control.
		/// </summary>
		public int[] SelectedIndeces 
		{
			get
			{
				ArrayList indeces = new ArrayList();
				foreach (object obj in this.SelectedItems) 
				{
					int i = this.IndexOf(obj.ToString());
					if (i >= 0)
					{
						indeces.Add( this.IndexOf(obj.ToString()) );
					}
				}

				return indeces.ToArray(typeof(Int32)) as Int32[];
			}
		}

		/// <summary>
		/// Returns the index of the given value. If it's nor found, it returns -1.
		/// </summary>
		/// <param name="val">An item value.</param>
		/// <returns>The index of the given value. If it's nor found, it returns -1.</returns>
		public int IndexOf(string val) 
		{
			int index = -1;

			for (int i=0; i < this._items.Keys.Count; i++) 
			{
				string key = _items.Keys[i];
				if (key == val) 
				{
					index = i;
					break;
				}
			}

			return index;
		}

		/// <summary>
		/// Gets the value at the given index
		/// </summary>
		/// <param name="index">The item index.</param>
		/// <returns>The item value at the passed in index</returns>
		public string GetValueAtIndex(int index) 
		{
			if ((index >= 0) && (index < this._items.Keys.Count))
			{
				return _items.Keys[index];
			}

			return string.Empty;
		}

		/// <summary>
		/// Returns the item text at the passed in index.
		/// </summary>
		/// <param name="index">The item index.</param>
		/// <returns>The item text at the passed in index</returns>
		public string GetTextAtIndex(int index) 
		{
			if ((index >= 0) && (index < this._items.Keys.Count))
			{
				return _items[ _items.Keys[index] ];
			}

			return string.Empty;
		}

		/// <summary>
		/// Adds a new item
		/// </summary>
		/// <param name="val">The item value or key</param>
		/// <param name="text">The item text.</param>
		/// <returns>The index at which the value was added.</returns>
		public int Add(string val, string text) 
		{
			this.Items.Add(val, text);
			
			return (this.Count-1);
		}

		/// <summary>
		/// Removes all listitems from the GuiCheckBoxList.
		/// </summary>
		public void Clear() 
		{
			this._selectedItems.Clear();
			this._items.Clear();
		}

		/// <summary>
		/// Clears all selected items.
		/// </summary>
		public void ClearSelected() 
		{
			this._selectedItems.Clear();
		}

		/// <summary>
		/// Returns true is the item at the given index is selected.
		/// </summary>
		/// <param name="index">The items index.</param>
		/// <returns>Returns true is the item at the given index is selected.</returns>
		public bool IsSelectedAtIndex(int index) 
		{
			bool isselected = false;
			if ((index < this.Count) && (index >= 0))
				isselected = this.SelectedItems.Contains(GetValueAtIndex(index));

			return isselected;
		}

		/// <summary>
		/// Selects the item and the given index
		/// </summary>
		/// <param name="index">The items index.</param>
		public void SelectAtIndex(int index) 
		{
			if ((index < this.Count) && (index >= 0))
				this.Select( GetValueAtIndex(index) );
		}

		/// <summary>
		/// Selects all items in the checkbox list.
		/// </summary>
		public void SelectAll() 
		{
			this.SelectedItems.Clear();
			this.SelectedItems.AddRange( this.Items.AllKeys );
		}

		/// <summary>
		/// Returns true if the specified value is in this checkbox list
		/// </summary>
		public bool Contains(string val) 
		{
			foreach (string key in this.Items.Keys) 
			{
				if (key == val) return true;
			}
			return false;
		}

		/// <summary>
		/// Selects the item with the specified value
		/// </summary>
		public void Select(string val) 
		{
			if ( this.Contains(val) )
			{
				this.SelectedItems.Add(val);
			}
		}

		/// <summary>
		/// The NameValueCollection containing all of the items displayed in the GuiCheckBoxList
		/// </summary>
		public NameValueCollection Items
		{
			 get { return this._items; }
		}
		
		/// <summary>
		/// Returns an ArrayList of selected items.
		/// </summary>
		public ArrayList SelectedItems 
		{
			get { return this._selectedItems; }
		}

		/// <summary>
		/// Sets or gets the height of the control.
		/// </summary>
		public override int Height 
		{
			get { return this._height; }
			set { this._height = value; }
		}

		/// <summary>
		/// Binds a DataSource (ArrayList, Hashtable, NameValueCollection) to this GuiCheckBoxList. 
		/// Will also bind data from any object that implements IEnumerable and has properties
		/// named "ItemName" and "ItemValue".
		/// </summary>
		/// <param name="dataSource">The data source object.</param>
		public void BindData(object dataSource) 
		{
			GuiDataBinder.BindDataToListControl(this, dataSource);
		}

		/// <summary>
		/// Binds a DataTable (dataSource) to the GuiCheckBoxList. The valueField 
		/// will be bound the the value of each list item, and the textField 
		/// will be bound to the visible text of each list item.
		/// </summary>
		/// <param name="dataSource">The source DataTable to bind to the GuiCheckBoxList</param>
		/// <param name="valueField">Identifies the column to pull the list items value from.</param>
		/// <param name="textField">Identifies the column to pull the list items text from.</param>
		public void BindDataFromTable(object dataSource, string valueField, string textField) 
		{
			GuiDataBinder.BindDataToListControl(this, dataSource, valueField, textField);
		}
		
		/// <summary>
		/// Attaches the functionName to the eventType event.  
		/// The GuiCheckBoxList supports the onchange event.
		/// </summary>
		/// <param name="eventType">The type of event to attach.</param>
		/// <param name="functionName">Name of the function to be called when the event fires.
		/// The functionName should have the signature in the Gui code: 
		/// "eventHandlerFunctionName(GuiCheckBoxList control)"</param>
		public override void AttachEvent(string eventType, string functionName)
		{
			if (eventType.ToLower() == "onchange")
			{
				this._onclickEvents.Add(functionName);
            }
            else
            {
                base.AttachEvent(eventType, functionName);
            }
		}

		/// <summary>
		/// This returns true if there are any event handlers for the passed in eventType.
		/// </summary>
		/// <param name="eventType">The type of event to check for.</param>
		/// <returns>Returns true if there are any event handlers of the given type.</returns>
		public override bool HasEventHandlers(string eventType)
		{
			if (eventType == "onchange") 
			{
				return (this._onclickEvents.Count > 0);
			}
			else
			{
				return base.HasEventHandlers(eventType);
			}
		}

		/// <summary>
		///  This returns all of the event handler functions that have been assigned 
		///  to this object for the passed in eventType. 
		/// </summary>
		/// <param name="eventType">The type of event to return function names for.</param>
		/// <returns>All of the event handler functions that have been assigned 
		///  to this object for the given eventType</returns>
		public override string[] GetEventHandlers(string eventType)
		{
			if (eventType == "onchange") 
			{
				return this._onclickEvents.ToArray(typeof(String)) as String[];
			}
			else
			{
				return base.GetEventHandlers(eventType);
			}
		}
	}
}