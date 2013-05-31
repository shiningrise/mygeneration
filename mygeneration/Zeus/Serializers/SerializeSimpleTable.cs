using System;
using System.Text;
using System.Collections;
using Zeus.Data;

namespace Zeus.Serializers
{
	/// <summary>
	/// Summary description for SerializeHashtable.
	/// </summary>
	public class SerializeSimpleTable : CollectionSerializer, ITypeSerializer
	{
		public SerializeSimpleTable() {}

		public Type DataType
		{
			get
			{
				return typeof(SimpleTable);
			}
		}

		public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is SimpleTable) 
			{
				SimpleTable table = source as SimpleTable;
				object val;
				string s_val;

				if (table.Columns.Count > 0)
				{
					bool first = true;
					foreach (SimpleColumn column in table.Columns) 
					{
						if (first) first = false;
						else output.Append(",");

						output.Append("Column|");
						output.Append( ReadableSerialize(column.Name) );
					}
					output.Append(";");

					if (table.Rows.Count > 0)
					{
						foreach (SimpleRow row in table.Rows) 
						{
							first = true;
							foreach (SimpleColumn column in table.Columns) 
							{
								if (first) first = false;
								else output.Append(",");

								val = row[column];

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
							}
							output.Append(";");
						}
					}
				}
			}

			return output.ToString();
		}

		public object Reconstitute(string source)
		{
			SimpleTable st = new SimpleTable();
			
			string[] rows = source.Split(';');
			string[] items, typeset;
			string s_val;
			object val;
			Type type;
			SimpleRow sr = null;
			int colIndex = 0, rowIndex = 0;

			//------------------------
			// 'B' = Base64
			// 'H' = Human Readable
			//------------------------
			string mode = "B";

			foreach (string row in rows) 
			{
				if (row.Trim() != string.Empty) 
				{
					items = row.Split(',');
				
					if (rowIndex > 0) 
					{
						sr = st.Rows.Add();
					}

					colIndex = 0;
					foreach (string item in items)
					{
						val = null;

						s_val = item;

						typeset = s_val.Split('|');
						if (rowIndex == 0) 
						{
							st.Columns.Add( ReadableDeserialize( typeset[1] ) );
						}
						else 
						{
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

							sr[colIndex] = val;
						}
						colIndex++;
					}

					rowIndex++;
				}
			}

			return st;
		}
	}
}
