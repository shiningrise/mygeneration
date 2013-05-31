using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MyMeta
{
	[GuidAttribute("9cafbed9-46e2-48f5-89ce-80b24a12c639"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IProviderTypes
	{
		[DispId(0)]
		IProviderType this[object index] { get; }

		// ICollection
		bool IsSynchronized { get; }
		int Count { get; }
		void CopyTo(System.Array array, int index);
		object SyncRoot { get; }

		// IEnumerable
		[DispId(-4)]
		IEnumerator GetEnumerator();
	}
}


