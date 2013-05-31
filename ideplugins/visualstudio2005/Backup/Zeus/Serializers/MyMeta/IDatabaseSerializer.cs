using System;
using MyMeta;
using Zeus;
using System.Text;

namespace TypeSerializer.MyMeta
{
	public class IDatabaseSerializer : MyMetaSerializer
	{
		public IDatabaseSerializer() {}
		
		override public Type DataType
		{
			get { return typeof(IDatabase); }
		}

		override public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is IDatabase) 
			{
				IDatabase database = source as IDatabase;
				output.Append( base.Dissolve(database.Root) );
				output.Append( "|" );
				output.Append( ReadableSerialize(database.Name) );
			}

			return output.ToString();
		}

		override public object Reconstitute(string source)
		{
			string[] items = source.Split('|');
			
			IDatabase database = null;

			if (items.Length >= 2) 
			{
				dbRoot myMeta = base.Reconstitute(items[0]) as dbRoot;
				database = myMeta.Databases[ ReadableDeserialize(items[1]) ];
			}

			return database;
		}
	}
}
