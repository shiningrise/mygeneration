using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace Zeus
{
	/// <summary>
	/// Summary description for InputItemCollection.
	/// </summary>
	public class SavedTemplateInputCollection : ICollection, IDictionary
	{
        private SortedList _hash = new SortedList();
        private List<string> _filesChanged;
        private ApplyOverrideDataDelegate _applyOverrideDataDelegate;

        public SavedTemplateInputCollection() { }

        public ApplyOverrideDataDelegate ApplyOverrideDataDelegate
        {
            set
            {
                if (_applyOverrideDataDelegate != value)
                {
                    _applyOverrideDataDelegate = value;
                    foreach (SavedTemplateInput item in this._hash.Values)
                    {
                        item.ApplyOverrideDataDelegate = _applyOverrideDataDelegate;
                    }
                }
            }
        }

		public SavedTemplateInput this[string objectName] 
		{
			get 
			{
                if (_hash.ContainsKey(objectName))
                {
                    SavedTemplateInput item = _hash[objectName] as SavedTemplateInput;
                    item.ApplyOverrideDataDelegate = this._applyOverrideDataDelegate;
                    return item;
                }
                else
                    return null;
			}
			set
            {
                value.ApplyOverrideDataDelegate = this._applyOverrideDataDelegate;
				_hash[objectName] = value;
			}
        }

        public List<string> SavedFiles
        {
            get
            {
                if (_filesChanged == null) _filesChanged = new List<string>();
                return this._filesChanged;
            }
        }

		public void Add(SavedTemplateInput item) 
		{
            item.ApplyOverrideDataDelegate = this._applyOverrideDataDelegate;
			this._hash[item.SavedObjectName] = item;
		}

		public void Remove(SavedTemplateInput item) 
		{
			Remove(item.SavedObjectName);
		}

		public void Remove(string key) 
		{
			if (_hash.ContainsKey(key)) 
				_hash.Remove(key);
		}

		public bool Contains(string objectName) 
		{
			return _hash.ContainsKey(objectName);
		}

		public void Execute(int timeout, ILog log) 
		{
			foreach (SavedTemplateInput item in this) 
			{
                item.Execute(timeout, log);

                foreach (string file in item.SavedFiles)
                {
                    if (!SavedFiles.Contains(file)) SavedFiles.Add(file);
                }
			}
		}

		public void BuildXML(XmlTextWriter xml) 
		{
			if (this.Count > 0) 
			{
				xml.WriteStartElement("objects");

				foreach (SavedTemplateInput item in this.Values) 
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
			get
            {
                SavedTemplateInput item = _hash[key] as SavedTemplateInput;
                item.ApplyOverrideDataDelegate = this._applyOverrideDataDelegate;
                return item;
            }
			set 
            {
                SavedTemplateInput item = value as SavedTemplateInput;
                item.ApplyOverrideDataDelegate = this._applyOverrideDataDelegate;
                _hash[key] = item; 
            }
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
            SavedTemplateInput item = value as SavedTemplateInput;
            item.ApplyOverrideDataDelegate = this._applyOverrideDataDelegate;

            _hash.Add(key, item);
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
