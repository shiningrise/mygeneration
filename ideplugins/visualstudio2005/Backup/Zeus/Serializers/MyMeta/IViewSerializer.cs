using System;
using MyMeta;
using Zeus;
using System.Text;

namespace TypeSerializer.MyMeta
{
	public class IViewSerializer : IDatabaseSerializer
	{
		public IViewSerializer() {}
		
		override public Type DataType
		{
			get { return typeof(IView); }
		}

		override public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is IView) 
			{
				IView view = source as IView;
				output.Append( base.Dissolve(view.Database) );
				output.Append( "|" );
				output.Append( ReadableSerialize(view.Name) );
			}

			return output.ToString();
		}

		override public object Reconstitute(string source)
		{
			string[] items = source.Split('|');
			
			IView view = null;
			IDatabase database = null;

			if (items.Length >= 3) 
			{
				database = base.Reconstitute(items[0] + "|" + items[1]) as IDatabase;
				view = database.Views[ ReadableDeserialize(items[2]) ];
			}

			return view;
		}
	}
}
