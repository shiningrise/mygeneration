using System;
namespace Zeus.Data{	/// <summary>	/// The SimpleColumn class represents a single column in a SimpleTable.	/// </summary>	public class SimpleColumn	{		private string _name = string.Empty;		internal SimpleColumn(string name)		{			_name = name;		}
		public string Name 
		{			get 
			{				return _name;			}		}	}}
