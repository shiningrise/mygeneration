using System;
using MyMeta;
using Zeus;
using System.Text;

namespace TypeSerializer.MyMeta
{
	public class IColumnSerializer : ITableSerializer
	{
		public IColumnSerializer() {}
		
		override public Type DataType
		{
			get { return typeof(IColumn); }
		}

		override public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is IColumn) 
			{
				IColumn column = source as IColumn;
				output.Append( base.Dissolve(column.Table) );
				output.Append( "|" );
				output.Append( ReadableSerialize(column.Name) );
			}

			return output.ToString();
		}

		override public object Reconstitute(string source)
		{
			string[] items = source.Split('|');
			
			IColumn column = null;
			ITable table = null;

			if (items.Length >= 4) 
			{
				table = base.Reconstitute(items[0] + "|" + items[1] + "|" + items[2]) as ITable;
				column = table.Columns[ ReadableDeserialize(items[3]) ];
			}

			return column;
		}
	}
}
