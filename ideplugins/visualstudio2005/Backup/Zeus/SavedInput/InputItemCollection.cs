using System;
using System.Collections;
using System.Xml;

namespace Zeus
{
	/// <summary>
	/// Summary description for InputItemCollection.
	/// </summary>
	public class InputItemCollection : ICollection, IDictionary
	{
		private SortedList _hash = new SortedList();

		public InputItemCollection() {}

		public InputItemCollection(IZeusInput input) 
		{
			this.Add(input);
		}

		public InputItem this[string varName] 
		{
			get 
			{
				if (_hash.ContainsKey(varName)) 
					return _hash[varName] as InputItem;
				else
					return null;
			}
			set 
			{
				_hash[varName] = value;
			}
		}

		public void CopyTo(IDictionary hash) 
		{
			foreach (string key in this.Keys) 
			{
				hash[key] = this[key].DataObject;
			}
		}

		public InputItemCollection Copy() 
		{
			InputItemCollection copy = new InputItemCollection();
			foreach (string key in this.Keys) 
			{
				copy.Add(this[key]);
			}
			return copy;
		}

		public void Add(InputItem item) 
		{
			this._hash[item.VariableName] = item;
		}

		public void Add(IZeusInput input) 
		{
			foreach (string varName in input.Keys) 
			{
				InputItem item;
				try 
				{
					item = new InputItem(varName, input[varName]);
					_hash[varName] = new InputItem(varName, input[varName]);
				}
				catch (Exception ex)
				{
					// This will happen if an unserializable type is in the input collection. 
					_hash[varName] = ex.Message;
				}
			}
		}

		public void Remove(InputItem item) 
		{
			_hash.Remove(item.VariableName);
		}

		public bool Contains(string varName) 
		{
			return _hash.ContainsKey(varName);
		}

		public void BuildXML(XmlTextWriter xml) 
		{
			if (this.Count > 0) 
			{
				xml.WriteStartElement("items");
				foreach (InputItem item in this._hash.Values) 
				{
					item.BuildXML(xml);
				}
				xml.WriteEndElement();
			}
		}

		#region ICollection Members
		public bool IsSynchronized
		{
			get { return _hash.IsSynchronized; }
		}

		public int Count
		{
			get { return _hash.Count; }
		}

		public void CopyTo(Array array, int index)
		{
			_hash.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get { return _hash.SyncRoot; }
		}
		#endregion

		#region IDictionary Members
		public bool IsReadOnly
		{
			get { return _hash.IsReadOnly; }
		}

		IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
		{
			return _hash.GetEnumerator();
		}

		object System.Collections.IDictionary.this[object key]
		{
			get { return _hash[key]; }
			set { _hash[key] = value; }
		}

		public void Remove(object key)
		{
			_hash.Remove(key);
		}

		bool System.Collections.IDictionary.Contains(object key)
		{
			return _hash.Contains(key);
		}

		public void Clear()
		{
			_hash.Clear();
		}

		public ICollection Values
		{
			get { return _hash.Values; }
		}

		void System.Collections.IDictionary.Add(object key, object value)
		{
			_hash.Add(key, value);
		}

		public ICollection Keys
		{
			get { return _hash.Keys; }
		}

		public bool IsFixedSize
		{
			get { return _hash.IsFixedSize; }
		}
		#endregion

		#region IEnumerable Members
		public IEnumerator GetEnumerator()
		{
			return _hash.Values.GetEnumerator();
		}
		#endregion
	}
}
