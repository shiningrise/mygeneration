using System;
using System.Collections.Specialized;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A generic interface shared by controls that hold a collection of data that can be bound to.
	/// </summary>
	public interface IGuiListControl
	{
		NameValueCollection Items { get; }
		void Clear();
		string this[string val] { get; set; }
		bool Sorted { get; set; }
		void BindData(object dataSource);
		void BindDataFromTable(object dataSource, string valueField, string textField);
	}
}
