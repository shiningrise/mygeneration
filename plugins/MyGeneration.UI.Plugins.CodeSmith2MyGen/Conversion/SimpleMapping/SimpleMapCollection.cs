using System;
using System.Collections;

namespace MyGeneration.CodeSmithConversion.Conversion.SimpleMapping
{
	/// <summary>
	/// Summary description for ApiMap.
	/// </summary>
	public class SimpleMapCollection : ICollection
	{
		private ArrayList simpleMaps = new ArrayList();

		public SimpleMapCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public SimpleMap this[int i]
		{
			get 
			{
				return simpleMaps[i] as SimpleMap;
			}
			set 
			{
				simpleMaps[i] = value;
			}
		}

		public int Add(SimpleMap map)
		{
			return simpleMaps.Add(map);
		}

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return simpleMaps.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return simpleMaps.Count;
			}
		}

		public void CopyTo(System.Array array, int index)
		{
			simpleMaps.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return simpleMaps.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return simpleMaps.GetEnumerator();
		}

		#endregion
	}
}
