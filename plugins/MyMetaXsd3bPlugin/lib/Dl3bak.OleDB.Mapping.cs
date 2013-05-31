using System;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace Dl3bak.Data.OleDB
{
    /// <summary>
    /// Mapping support between oledb and dotnet-system
    /// 
    /// (c) 2007 by dl3bak@qsl.net
    /// </summary>
    public class Mapping
    {
        private static OleDbParameter systemTypeToOleDbType = new OleDbParameter();

        public const string MAPPING_DEFAULT_SYSTEM_TYPE = "System.String";

        public static string GetSystemTypeFromOleDBTypeID(OleDbType oleDbTypeID)
        {
            string rootDataType = null;
            systemTypeToOleDbType.OleDbType = oleDbTypeID;
            switch (systemTypeToOleDbType.OleDbType)
            {
                case OleDbType.Binary:
                case OleDbType.VarBinary:
                    return typeof(System.Byte[]).ToString();
                case OleDbType.DBDate:
                    return typeof(System.DateTime).ToString();
                case OleDbType.DBTime:
                    return typeof(System.TimeSpan).ToString();
                case OleDbType.Currency:
                    return typeof(System.Double).ToString();
                case OleDbType.TinyInt:
                    return typeof(System.Int16).ToString();
            }

            // else assume DbType-Name is identical to System-TypeName
            rootDataType = "System." + systemTypeToOleDbType.DbType.ToString();
            /// System.String

            if ((rootDataType == null)
                || (null == Type.GetType(rootDataType, false, true)) // does not exist
                || (rootDataType.ToLower().IndexOf("string") >= 0)) // AnsiString, AnsiStringFixedLength, StringFixedLength
                rootDataType = MAPPING_DEFAULT_SYSTEM_TYPE;

            return rootDataType;
        }

        public static string GetSystemTypeFromOleDBTypeID(int oleDbTypeID)
        {
            // string rootDataType = null;
            try
            {
                return GetSystemTypeFromOleDBTypeID((OleDbType)oleDbTypeID);
            }
            catch (Exception)
            {
            }
            return MAPPING_DEFAULT_SYSTEM_TYPE;
        }

        public static int GetOleDbTypeIDFromOleDbTypeName(string name)
        {
            try
            {
                return (int)Enum.Parse(typeof(OleDbType), name, true);
            }
            catch (Exception)
            {
                return GetOleDbTypeIDFromSystemTypeName(name);
            }
        }

        public static int GetOleDbTypeIDFromSystemTypeName(string name)
        {
            try
            {
                systemTypeToOleDbType.DbType = (DbType)Enum.Parse(typeof(DbType), name, true);
                return (int)systemTypeToOleDbType.OleDbType;
            }
            catch (Exception)
            {
            }
            return 0;
        }

        public static string GetOleDBTypeNameFromID(int oleDbTypeID)
        {
            try
            {
                OleDbType oleTyp = (OleDbType)oleDbTypeID;
                return oleTyp.ToString();

            }
            catch (Exception)
            {
            }
            return null;
        }

    }
}
