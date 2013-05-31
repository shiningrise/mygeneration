using System;
using System.Collections.Specialized;

namespace Zeus.UserInterface
{
	/// <summary>
	/// A generic interface shared by controls that hold a collection of data that can be bound to.
	/// </summary>
	public interface IGuiListControl : IGuiControl
	{
		NameValueCollection Items { get; }
		void Clear();
		string this[string val] { get; set; }
		bool Sorted { get; set; }
		int Count { get; }
		int SelectedIndex { get; }
		int[] SelectedIndeces { get; }
		int IndexOf(string val);
		string GetValueAtIndex(int index);
		string GetTextAtIndex(int index);
		bool IsSelectedAtIndex(int index);
		void SelectAtIndex(int index);
		int Add(string val, string text);
		bool Contains(string val);
		void Select(string val);
		void BindData(object dataSource);
		void BindDataFromTable(object dataSource, string valueField, string textField);
	}
}
