using System;
using Zeus.Data;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A Grid input control for attaining a table of input from the user. 
	/// </summary>
	/// <remarks>
	/// A Grid input control for attaining a table of input from the user. A SimpleTable class
	/// can be used to bind custom data to the grid. Other classes, such as DataTables and MyMeta collections 
	/// cas also be bound to the grid. In the sample code below, the default DataSource object, a SimpleTable,
	/// is being populated with names and addresses. A SimpleTable object can also be instatiated by calling
	/// input.CreateSimpleTable().
	/// </remarks>
	/// <example>
	/// Adding the GuiGrid to the GuiController (jscript)
	/// <code>
	/// 	var grid = ui.AddGrid("grid", "Enter some names and addresses!");
	/// 	grid.Height = 150;
	/// 	var table = grid.DataSource;
	/// 	table.Columns.Add("FirstName");
	/// 	table.Columns.Add("LastName");
	/// 	table.Columns.Add("Address");
	/// 	table.Rows.Item(0).Item(0) = "Justin";
	/// 	table.Rows.Item(0).Item(1) = "Greenwood";
	/// 	table.Rows.Item(0).Item(2) = "1111 Cordon Drive";
	/// 	var row = table.Rows.Add();
	/// 	row.Item("FirstName") = "Jeremy";
	/// 	row.Item("LastName") = "Smith";
	/// 	row.Item("Address") = "12345 Test Way";
	/// </code>
	/// </example>
	public class GuiGrid : GuiControl, IGuiGrid
	{
		private SimpleTable _dataSource = null;
		private int _height = 64;

		/// <summary>
		/// Creates a new instance of a GuiGrid control.
		/// </summary>
		public GuiGrid() {}

		/// <summary>
		/// Returns the SimpleTable that contains the grids data
		/// </summary>
		public override object Value 
		{
			get { return this._dataSource; }
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
		/// Returns the SimpleTable that contains the grids data
		/// </summary>
		public SimpleTable DataSource
		{
			get 
			{ 
				if (_dataSource == null) 
				{
					_dataSource = new SimpleTable();
				}

				return this._dataSource; 
			}
			set { this._dataSource = value; }
		}

		/// <summary>
		/// Binds a DataSource to this GuiGrid. Binding supports SimpleTables, DataTables, DataViews, and other IEnumerable classes.
		/// </summary>
		/// <param name="dataSource">The data source object to bind.</param>
		public void BindData(object dataSource)
        {
#if !HTML_HELP
            if (dataSource is SimpleTable)
				this.DataSource = dataSource as SimpleTable;
			else
				this.DataSource = SimpleTableTools.ConvertToSimpleTable(dataSource);
#endif
		}
	}
}

