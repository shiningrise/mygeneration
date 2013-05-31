using System;using System.Collections;
namespace Zeus.Data{	/// <summary>	/// The SimpleColumnCollection is an enumerable collection of SimpleColumns	/// </summary>	public class SimpleColumnCollection : IEnumerable	{		protected ArrayList columns = new ArrayList();		protected SimpleTable table;
		internal SimpleColumnCollection(SimpleTable table)		{			this.table = table;		}
		public SimpleColumn this[int columnIndex] 		{			get 			{				if ((columnIndex >= 0) && (columnIndex < columns.Count)) 				{					return columns[columnIndex] as SimpleColumn;				}				else				{					throw new IndexOutOfRangeException("Index " + columnIndex + " is out of range. This SimpleTable has a column length of " + columns.Count + ".");				}			}			set 			{				if ((columnIndex >= 0) && (columnIndex < columns.Count)) 				{					columns[columnIndex] = value;				}				else if (columnIndex == columns.Count)				{					columns.Add(value);				}				else				{					throw new IndexOutOfRangeException("Index " + columnIndex + " is out of range. This SimpleTable has a column length of " + columns.Count + ".");				}			}		}		public bool Contains(string name) 
		{			return (IndexOf(name) >= 0) ;		}		public int Count 		{			get 			{				return this.columns.Count;			}		}		public int IndexOf(string columnName) 		{			int index = -1;			for (int i=0; i < columns.Count; i++) 
			{				SimpleColumn c = this.columns[i] as SimpleColumn;				if (c.Name == columnName) 
				{
					index = i;					break;
				}			}			return index;		}
		public void Remove(string columnName) 		{			int index = columns.IndexOf(columnName);			this.RemoveAt(index);		}
		public void RemoveAt(int columnIndex) 		{			if ((columnIndex > -1) && (columnIndex < columns.Count)) 			{				columns.RemoveAt(columnIndex);				OnAlterColumn(ColumnEventType.Remove, columnIndex);			}		}
		public void Add(string columnName) 		{			if (!this.Contains(columnName)) 
			{				columns.Add(new SimpleColumn(columnName));				OnAlterColumn(ColumnEventType.Add, columns.Count-1);			}		}
		public void Insert(int insertIndex, string columnName) 		{			if (!this.Contains(columnName)) 
			{				columns.Insert(insertIndex, new SimpleColumn(columnName));				OnAlterColumn(ColumnEventType.Insert, insertIndex);			}		}		public virtual IEnumerator GetEnumerator()		{			return this.columns.GetEnumerator();		}		internal virtual void OnAlterColumn(ColumnEventType type, int index) 		{			if (AlterColumn != null) 			{				AlterColumn(type, index);			}		}				internal event SimpleColumnEventHandler AlterColumn;	}
	internal delegate void SimpleColumnEventHandler(ColumnEventType type, int index);
	internal enum ColumnEventType	{		Remove = 0,		Insert,		Add,		Clear	}}