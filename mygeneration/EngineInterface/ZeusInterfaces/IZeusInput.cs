using System;
using System.Collections;

namespace Zeus
{
	public interface IZeusInput
	{
		object this[string key] { get; set; }
		ICollection Keys { get; }
		ICollection Values { get; }
		void Remove(object key);
		bool Contains(object key); 
		void AddItems(object collection);
		bool ContainsKeys(object[] keys); 
		string __Variables { get; }
	}
}
