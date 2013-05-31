using System;
using System.Collections;

namespace Zeus
{
	/// <summary>
	/// Summary description for BasicLog.
	/// </summary>
	public class ZeusSimpleLog : ILog
	{

		private ArrayList _items = new ArrayList();
		private ArrayList _exceptions = new ArrayList();

		public ZeusSimpleLog() {}

		public void Write(Exception ex)
		{
			string x = ex.GetType().FullName + " - " + ex.Message;
			_items.Add(x);
			OnLogEntryAdded(x);
			_exceptions.Add(ex);
		}

		public void Write(string format, params object[] args)
		{
			string x = string.Format(format, args);
			OnLogEntryAdded(x);
			_items.Add(x);
		}

		public void Write(string text)
		{
			string x = text;
			OnLogEntryAdded(x);
			_items.Add(x);
		}

		public string[] LogItems 
		{
			get 
			{
				return (string[])_items.ToArray(typeof(string));
			}
		}

		public Exception[] Exceptions 
		{
			get 
			{
				return (Exception[])_exceptions.ToArray(typeof(Exception));
			}
		}

		public bool HasExceptions 
		{
			get 
			{
				return (_exceptions.Count > 0);
			}
		}
	
		public event EventHandler LogEntryAdded;

		private void OnLogEntryAdded(string entry) 
		{
			if (LogEntryAdded != null) 
			{
				LogEntryAdded(entry, EventArgs.Empty);
			}
		}
	}
}
