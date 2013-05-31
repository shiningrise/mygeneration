using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace MyMeta
{
	/// <summary>
	/// This interface allows all the collections here to be bound to 
	/// Name/Value collection type objects. with ease
	/// </summary>
	[GuidAttribute("8a05e01d-cccd-48f4-94f8-e84d91032b80"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface INameValueItem
	{
		string ItemName{ get; }
		string ItemValue{ get; }
	}
}

