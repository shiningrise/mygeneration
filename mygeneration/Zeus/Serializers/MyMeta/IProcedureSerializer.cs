using System;
using MyMeta;
using Zeus;
using System.Text;

namespace TypeSerializer.MyMeta
{
	public class IProcedureSerializer : IDatabaseSerializer
	{
		public IProcedureSerializer() {}
		
		override public Type DataType
		{
			get { return typeof(IProcedure); }
		}

		override public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is IProcedure) 
			{
				IProcedure procedure = source as IProcedure;
				output.Append( base.Dissolve(procedure.Database) );
				output.Append( "|" );
				output.Append( ReadableSerialize(procedure.Name) );
			}

			return output.ToString();
		}

		override public object Reconstitute(string source)
		{
			string[] items = source.Split('|');
			
			IProcedure procedure = null;
			IDatabase database = null;

			if (items.Length >= 3) 
			{
				database = base.Reconstitute(items[0] + "|" + items[1]) as IDatabase;
				procedure = database.Procedures[ ReadableDeserialize(items[2]) ];
			}

			return procedure;
		}
	}
}
