using System;
using System.Text;
using System.Collections;

namespace Zeus.Serializers
{
	/// <summary>
	/// Summary description for SerializeArrayList.
	/// </summary>
	public class SerializeArrayList : CollectionSerializer, ITypeSerializer
	{
		public SerializeArrayList()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public Type DataType
		{
			get
			{
				return typeof(ArrayList);
			}
		}

		public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is ArrayList) 
			{
				ArrayList al = source as ArrayList;
				string s_val;
				foreach (object val in al) 
				{
					s_val = val.GetType().FullName + "|";
					if (MasterSerializer.IsSimpleType(val)) 
					{
						s_val += "H|";
						s_val += ReadableSerialize( MasterSerializer.Dissolve(val) );
					}
					else
					{
						s_val += "B|";
						s_val += ConvertUTF8ToBytes(MasterSerializer.Dissolve(val));
					}

					output.Append(s_val);
					output.Append(";");
				}
			}

			return output.ToString();
		}

		public object Reconstitute(string source)
		{
			ArrayList al = new ArrayList();
			
			string[] sets = source.Split(';');
			string[] typeset;
			string s_val;

			//------------------------
			// 'B' = Base64
			// 'H' = Human Readable
			//------------------------
			string mode = "B";
			object val;
			Type type;

			foreach (string strval in sets) 
			{
				val = null;
				s_val = strval;

				typeset = s_val.Split('|');
				
				if (typeset.Length == 3) 
				{
					mode = typeset[1];
				}
				else 
				{
					mode = "B";
				}
				
				if (typeset.Length >= 2) 
				{
					type = Type.GetType(typeset[0]);

					if (mode == "B") 
					{
						val = MasterSerializer.Reconstitute(type, ConvertBytesToUTF8(typeset[typeset.Length - 1]));
					}
					else 
					{
						val = MasterSerializer.Reconstitute( type, ReadableDeserialize( typeset[typeset.Length - 1] ) );
					}
				}

				if (val != null)
				{
					al.Add(val);
				}
			}

			return al;
		}
	}
}
