using System;using System.Collections;
namespace Zeus.Data{	/// <summary>	/// The SimpleRowCollection is an enumerable collection of SimpleRows	/// </summary>	public class SimpleRowCollection : IEnumerable	{		protected ArrayList rows = new ArrayList();		protected SimpleTable table;				internal SimpleRowCollection(SimpleTable table)		{			this.table = table;		}
		public SimpleRow ByIndex(int rowIndex) 		{			return this[rowIndex];		}
		public SimpleRow this[int rowIndex] 		{			get 			{				if ((rowIndex >= 0) && (rowIndex < rows.Count)) 				{					return rows[rowIndex] as SimpleRow;				}				else if (rowIndex == rows.Count)
				{					SimpleRow row = this.Add();					return row;				}				else				{					throw new IndexOutOfRangeException("Index " + rowIndex + " is out of range. This SimpleTable has a row length of " + rows.Count + ".");				}			}		}
		public int Count 		{			get 			{				return this.rows.Count;			}		}
		public SimpleRow Add() 		{			SimpleRow newRow = new SimpleRow(table);			rows.Add(newRow);			return newRow;		}
		public void RemoveAt(int rowIndex) 		{			this.rows.RemoveAt(rowIndex);		}
		public void RemoveRange(int startIndex, int count) 		{			this.rows.RemoveRange(startIndex, count);		}		public virtual IEnumerator GetEnumerator()		{			return rows.GetEnumerator();		}	}}