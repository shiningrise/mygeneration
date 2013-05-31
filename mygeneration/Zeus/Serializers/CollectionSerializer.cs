using System;
using System.Text;
//using System;

namespace Zeus.Serializers
{
	/// <summary>
	/// Summary description for CollectionSerializer.
	/// </summary>
	public abstract class CollectionSerializer
	{
		protected const string RPL_PIPE = "{~P~}";
		protected const string RPL_SEMI = "{~S~}";
		protected const string RPL_COMMA = "{~C~}";

		virtual public Type GetTypeFromName(string typeName)
		{
			return Type.GetType(typeName);
		}

		protected string ReadableSerialize(string str) 
		{
			return str.Replace("|", RPL_PIPE).Replace(";", RPL_SEMI).Replace(",", RPL_COMMA);
		}

		protected string ReadableDeserialize(string str) 
		{
			return str.Replace(RPL_PIPE, "|").Replace(RPL_SEMI, ";").Replace(RPL_COMMA, ",");
		}

		protected string ConvertBytesToUTF8(string str) 
		{
			UTF8Encoding utf8 = new UTF8Encoding();
			
			byte[] bytes = Convert.FromBase64String(str);
			int charCount = utf8.GetCharCount(bytes);
			Char[] chars = new Char[charCount];
			utf8.GetChars(bytes, 0, bytes.Length, chars, 0);

			return new String(chars);
		}

		protected string ConvertUTF8ToBytes(string str) 
		{
			UTF8Encoding utf8 = new UTF8Encoding();

			int byteCount = utf8.GetByteCount(str);
			Byte[] bytes = new Byte[byteCount];
			utf8.GetBytes(str, 0, str.Length, bytes, 0);

			return Convert.ToBase64String(bytes);
		}
	}
}
