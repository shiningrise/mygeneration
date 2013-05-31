using System;
using System.Collections;

namespace MyGeneration.CodeSmithConversion.Conversion.ApiMapping
{
	/// <summary>
	/// Summary description for ApiMap.
	/// </summary>
	public class ApiMapCollection : ICollection
	{
		private ArrayList apiMaps = new ArrayList();

		public ApiMapCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public ApiMap this[int i]
		{
			get 
			{
				return apiMaps[i] as ApiMap;
			}
			set 
			{
				apiMaps[i] = value;
			}
		}

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return apiMaps.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return apiMaps.Count;
			}
		}

		public void CopyTo(System.Array array, int index)
		{
			apiMaps.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return apiMaps.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return apiMaps.GetEnumerator();
		}

		#endregion
	}
}
