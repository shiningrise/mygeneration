using System;
using System.Text;
using System.Collections;

namespace Zeus.Serializers
{
	/// <summary>
	/// Summary description for SerializeHashtable.
	/// </summary>
	public class SerializeHashtable : CollectionSerializer, ITypeSerializer
	{
		public SerializeHashtable() {}

		public Type DataType
		{
			get
			{
				return typeof(Hashtable);
			}
		}

		public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is Hashtable) 
			{
				Hashtable ht = source as Hashtable;
				object val;
				string s_key, s_val;

				foreach (object key in ht.Keys) 
				{
					val = ht[key];

					s_key = key.GetType().FullName + "|";
					s_val = val.GetType().FullName + "|";

					if (MasterSerializer.IsSimpleType(key)) 
					{
						s_key += "H|";
						s_key += ReadableSerialize( MasterSerializer.Dissolve(key) );
					}
					else
					{
						s_key += "B|";
						s_key += ConvertUTF8ToBytes(MasterSerializer.Dissolve(key));
					}

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

					output.Append(s_key);
					output.Append(",");
					output.Append(s_val);
					output.Append(";");
				}
			}

			return output.ToString();
		}

		public object Reconstitute(string source)
		{
			Hashtable ht = new Hashtable();
			
			string[] pairs = source.Split(';');
			string[] pair, typeset;
			string s_key, s_val;
			object key, val;
			Type type;

			//------------------------
			// 'B' = Base64
			// 'H' = Human Readable
			//------------------------
			string mode = "B";

			foreach (string strpair in pairs) 
			{
				pair = strpair.Split(',');
				if (pair.Length == 2) 
				{
					key = null; 
					val = null;

					s_key = pair[0];
					s_val = pair[1];

					typeset = s_key.Split('|');
					mode = "B";
					if (typeset.Length == 3) 
					{
						mode = typeset[1];
					}

					if (typeset.Length >= 2) 
					{
						type = Type.GetType(typeset[0]);
						if (mode == "B") 
						{
							key = MasterSerializer.Reconstitute( type, ConvertBytesToUTF8( typeset[typeset.Length - 1] ) );
						}
						else 
						{
							key = MasterSerializer.Reconstitute( type, ReadableDeserialize( typeset[typeset.Length - 1] ) );
						}
					}

					typeset = s_val.Split('|');
					mode = "B";
					if (typeset.Length == 3) 
					{
						mode = typeset[1];
					}

					if (typeset.Length >= 2) 
					{
						type = Type.GetType(typeset[0]);
						if (mode == "B") 
						{
							val = MasterSerializer.Reconstitute( type, ConvertBytesToUTF8( typeset[typeset.Length - 1] ) );
						}
						else 
						{
							val = MasterSerializer.Reconstitute( type, ReadableDeserialize( typeset[typeset.Length - 1] ) );
						}
					}

					if ((key != null) && (val != null))
					{
						ht[key] = val;
					}
				}
			}

			return ht;
		}
	}
}
