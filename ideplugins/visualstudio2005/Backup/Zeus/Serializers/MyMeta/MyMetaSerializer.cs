using System;
using MyMeta;
using Zeus;
using System.Text;
using System.Reflection;

namespace TypeSerializer.MyMeta
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class MyMetaSerializer : ITypeSerializer
	{
		protected const string RPL_PIPE = "{~P~}";
		protected const string RPL_SEMI = "{~S~}";
		protected const string RPL_COMMA = "{~C~}";

		public MyMetaSerializer() {}

		virtual public Type DataType
		{
			get { return typeof(dbRoot); }
		}

		virtual public Type GetTypeFromName(string typeName)
		{
			Assembly a = Assembly.GetAssembly(typeof(dbRoot));
			return a.GetType(typeName);
		}

		virtual public string Dissolve(object source)
		{
			StringBuilder output = new StringBuilder();
			if (source is dbRoot) 
			{
				dbRoot myMeta = source as dbRoot;
				output.Append( ReadableSerialize(myMeta.ConnectionString) );
				output.Append( ";" );
				output.Append( ReadableSerialize(myMeta.DbTargetMappingFileName) );
				output.Append( ";" );
				output.Append( ReadableSerialize(myMeta.DomainOverride.ToString()) );
				output.Append( ";" );
				output.Append( ReadableSerialize(myMeta.DriverString) );
				output.Append( ";" );
				output.Append( ReadableSerialize(myMeta.Language) );
				output.Append( ";" );
				output.Append( ReadableSerialize(myMeta.LanguageMappingFileName) );
				output.Append( ";" );
				output.Append( ReadableSerialize(myMeta.UserMetaDataFileName) );
			}

			return output.ToString();
		}

		virtual public object Reconstitute(string source)
		{
			string[] items = source.Split(';');
			
			dbRoot myMeta = new dbRoot();

			if (items.Length >= 7) 
			{
				// DriverString, ConnectionString
				myMeta.Connect( 
						ReadableDeserialize(items[3]),
						ReadableDeserialize(items[0]) 
					);

				// DbTargetMappingFileName
				myMeta.DbTargetMappingFileName = ReadableDeserialize(items[1]);
			
				// DomainOverride
				myMeta.DomainOverride = Convert.ToBoolean(items[2]);
						
				// Language
				myMeta.Language = ReadableDeserialize(items[4]);
			
				// LanguageMappingFileName
				myMeta.LanguageMappingFileName = ReadableDeserialize(items[5]);
			
				// UserMetaDataFileName
				myMeta.UserMetaDataFileName = ReadableDeserialize(items[6]);
			}

			return myMeta;
		}
		
		protected string ReadableSerialize(string str) 
		{
			return str.Replace("|", RPL_PIPE).Replace(";", RPL_SEMI).Replace(",", RPL_COMMA);
		}

		protected string ReadableDeserialize(string str) 
		{
			return str.Replace(RPL_PIPE, "|").Replace(RPL_SEMI, ";").Replace(RPL_COMMA, ",");
		}
	}
}
