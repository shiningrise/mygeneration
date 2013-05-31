using System;
namespace Zeus.Data{	/// <summary>	/// The SimpleTable class is a script friendly, serializable, tablular data structure that	/// is made for MyGeneration DataBinding. The GuiGrid control can bind to a SimpleTable, and 	/// because it's serializable, it can be saved into zeus project files.	/// </summary>	public class SimpleTable	{		public SimpleRowCollection rows;		public SimpleColumnCollection columns;
		/// <summary>
		/// Creates a new SimpleTable
		/// </summary>
		public SimpleTable()		{			rows = new SimpleRowCollection(this);			columns = new SimpleColumnCollection(this);		}		/// <summary>
		/// A collection of SimpleRows
		/// </summary>		public SimpleRowCollection Rows 		{			get 			{				return rows;			}		}		/// <summary>
		/// A collection of SimpleColumns
		/// </summary>		public SimpleColumnCollection Columns 		{			get 			{				return columns;			}		}	}}