using System;
using MyMeta;
using Zeus;
using System.Text;

namespace TypeSerializer.MyMeta
{
	public class ITableSerializer : IDatabaseSerializer
	{
		public ITableSerializer() {}
		
		override public Type DataType
		{
			get { return typeof(ITable); }
		}

		override public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is ITable) 
			{
				ITable table = source as ITable;
				output.Append( base.Dissolve(table.Database) );
				output.Append( "|" );
				output.Append( ReadableSerialize(table.Name) );
			}

			return output.ToString();
		}

		override public object Reconstitute(string source)
		{
			string[] items = source.Split('|');
			
			ITable table = null;
			IDatabase database = null;

			if (items.Length >= 3) 
			{
				database = base.Reconstitute(items[0] + "|" + items[1]) as IDatabase;
				table = database.Tables[ ReadableDeserialize(items[2]) ];
			}

			return table;
		}
	}
}
