using System;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

namespace Zeus.Projects
{
	/// <summary>
	/// Summary description for InputItemCollection.
	/// </summary>
	public class ZeusModuleCollection : ICollection, IDictionary
	{
		private SortedList _hash = new SortedList();
        private ZeusModule _parentModule;
        private List<string> _filesChanged;

		public ZeusModuleCollection(ZeusModule parentModule) 
		{
			_parentModule = parentModule;
		}

		public ZeusModule this[string name] 
		{
			get 
			{
				if (_hash.ContainsKey(name)) 
					return _hash[name] as ZeusModule;
				else
					return null;
			}
			set 
			{
				Add(value);
			}
		}

		public void Add(ZeusModule module) 
		{
			module.SetParentModule(this._parentModule);
			this._hash[module.Name] = module;
		}

		public void Remove(ZeusModule module) 
		{
			if (_hash.ContainsValue(module)) 
				_hash.Remove(module.Name);
		}

		public bool Contains(string name) 
		{
			return _hash.ContainsKey(name);
		}

		public ZeusModule ParentModule
		{
			get 
			{
				return _parentModule;
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

		public void Execute(int timeout, ILog log) 
		{
			foreach (ZeusModule module in this) 
			{
                module.Execute(timeout, log);

                foreach (string file in module.SavedFiles)
                {
                    if (!SavedFiles.Contains(file)) SavedFiles.Add(file);
                }
			}
        }

        virtual public void BuildXML(XmlTextWriter xml)
        {
            if (this.Count > 0)
            {
                xml.WriteStartElement("modules");

                foreach (ZeusModule module in this.Values)
                {
                    module.BuildXML(xml);
                }

                xml.WriteEndElement();
            }
        }

        virtual public void BuildUserXML(XmlTextWriter xml)
        {
            if (this.Count > 0)
            {
                xml.WriteStartElement("modules");

                foreach (ZeusModule module in this.Values)
                {
                    module.BuildUserXML(xml);
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
