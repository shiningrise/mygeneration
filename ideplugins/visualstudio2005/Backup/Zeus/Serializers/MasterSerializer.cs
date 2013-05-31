using System;
using System.Collections;
using Zeus.Serializers;
//using TypeSerializer.MyMeta;

namespace Zeus
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class MasterSerializer
	{
		static private Hashtable _serializers = null;

		static public Type GetTypeFromName(string typeName) 
		{
			Type type = Type.GetType(typeName);

			if (type == null) 
			{
				LoadCustomSerializers();
				foreach (ITypeSerializer cts in _serializers.Values) 
				{
					type = cts.GetTypeFromName(typeName);
					if (type != null) break;
				}
			}

			return type;
		}

		static public bool IsSimpleType(object data)
		{
			bool isSimple = false;

			if ((data is String) 
				|| (data is SByte) 
				|| (data is Int16)
				|| (data is Int32)
				|| (data is Int64)
				|| (data is Byte)
				|| (data is UInt16)
				|| (data is UInt32)
				|| (data is UInt64)
				|| (data is Decimal)
				|| (data is Char)
				|| (data is Guid)
				|| (data is Single)
				|| (data is Double)
				|| (data is Boolean)
				|| (data is DateTime)
				|| (data is TimeSpan)) 
			{
				isSimple = true;
			}

			return isSimple;
		}

		static public string Dissolve(object data)
		{
			string returnData = null;

			if (data is String) 
			{
				returnData = (string)data;
			}
			else if ((data is SByte) 
				|| (data is Int16)
				|| (data is Int32)
				|| (data is Int64)
				|| (data is Byte)
				|| (data is UInt16)
				|| (data is UInt32)
				|| (data is UInt64)
				|| (data is Decimal)
				|| (data is Char)
				|| (data is Guid)
				|| (data is Single)
				|| (data is Double)
				|| (data is Boolean))
			{
				returnData = data.ToString();
			}
			else if (data is DateTime) 
			{
				DateTime date = (DateTime)data;

				returnData = date.ToUniversalTime().ToString();
			}
			else if (data is TimeSpan) 
			{
				TimeSpan timespan = (TimeSpan)data;

				returnData = timespan.ToString();
			}
			else 
			{
				LoadCustomSerializers();
				ITypeSerializer cts =_serializers[data.GetType()] as ITypeSerializer;
				if (cts != null) 
				{
					returnData = cts.Dissolve(data);
				}
				else 
				{
					bool found = false;
					foreach (ITypeSerializer ts in _serializers.Values) 
					{
						if (ts.DataType.IsInstanceOfType(data)) 
						{
							returnData = ts.Dissolve(data);
							found = true;
						}
					}

					if (!found) 
					{
						throw new Exception("Unserializable type: " + data.GetType().FullName);
					}
				}
			}

			return returnData;
		}

		static public object Reconstitute(System.Type type, string data)
		{
			object returnData = null;

			if (type == typeof(String)) 
				returnData = (string)data;
			else if (type == typeof(Boolean)) 
				returnData = Convert.ToBoolean(data);

			else if (type == typeof(SByte)) 
				returnData = Convert.ToSByte(data);
			else if (type == typeof(Int16)) 
				returnData = Convert.ToInt16(data);
			else if (type == typeof(Int32)) 
				returnData = Convert.ToInt32(data);
			else if (type == typeof(Int64)) 
				returnData = Convert.ToInt64(data);
			else if (type == typeof(Byte))
				returnData = Convert.ToByte(data);

			else if (type == typeof(UInt16)) 
				returnData = Convert.ToUInt16(data);
			else if (type == typeof(UInt32)) 
				returnData = Convert.ToUInt32(data);
			else if (type == typeof(UInt64)) 
				returnData = Convert.ToUInt64(data);

			else if (type == typeof(Decimal)) 
				returnData = Convert.ToDecimal(data);
			else if (type == typeof(Guid)) 
				returnData = new Guid(data);
			else if (type == typeof(Single)) 
				returnData = Convert.ToSingle(data);
			else if (type == typeof(Double)) 
				returnData = Convert.ToDouble(data);

			else if (type == typeof(DateTime)) 
				returnData = DateTime.Parse(data);
			else if (type == typeof(TimeSpan)) 
				returnData = TimeSpan.Parse(data);

			else 
			{
				LoadCustomSerializers();
				ITypeSerializer cts =_serializers[type] as ITypeSerializer;
				if (cts != null) 
				{
					returnData = cts.Reconstitute(data);
				}
				else 
				{
					bool found = false;
					foreach (ITypeSerializer ts in _serializers.Values) 
					{
						if (ts.DataType.IsClass)
						{
							if (type.IsSubclassOf(ts.DataType)) 
							{
								returnData = ts.Reconstitute(data);
								found = true;
							}
						}
						else if (ts.DataType.IsInterface) 
						{
							Type[] ifaces = type.GetInterfaces();
							foreach (Type iface in ifaces) 
							{
								if (iface == ts.DataType) 
								{
									returnData = ts.Reconstitute(data);
									found = true;
									break;
								}
							}
						}
						if (found) break;
					}

					if (!found) 
					{
						throw new Exception("Unserializable type: " + data.GetType().FullName);
					}
				}
			}

			return returnData;
		}

		static private void LoadCustomSerializers()
		{
			if (_serializers == null) 
			{
				ITypeSerializer szr;

				_serializers = new Hashtable();

				szr = new SerializeArrayList();
				_serializers[szr.DataType] = szr;

				szr = new SerializeHashtable();
				_serializers[szr.DataType] = szr;

				szr = new SerializeSimpleTable();
				_serializers[szr.DataType] = szr;

				ArrayList listcustom = ZeusFactory.Serializers;
				foreach (ITypeSerializer tmpSzr in listcustom) 
				{
					_serializers[tmpSzr.DataType] = tmpSzr;
				}
			}
		}

	}
}
