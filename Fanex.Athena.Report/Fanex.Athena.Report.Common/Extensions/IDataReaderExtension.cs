using System;
using System.Data;
using System.Data.SqlTypes;

namespace Fanex.Athena.Report.Common.Extensions
{
    public static class IDataReaderExtension
    {
        public static T GetVal<T>(this IDataReader dataReader, string columnName, T defauleValue = default(T))
        {
            var dbValue = dataReader[columnName];
            if (dbValue == DBNull.Value)
            {
                return defauleValue;
            }

            return Cast<T>(dbValue, defauleValue);
        }

        public static object GetVal(this IDataReader dataReader, Type type, string columnName)
        {
            if (type == typeof(string))
            {
                return GetVal<string>(dataReader, columnName);
            }
            if (type == typeof(decimal))
            {
                return GetVal<decimal>(dataReader, columnName);
            }
            if (type == typeof(int))
            {
                return GetVal<int>(dataReader, columnName);
            }
            if (type == typeof(double))
            {
                return GetVal<double>(dataReader, columnName);
            }

            return null;
        }

        private static T Cast<T>(object dbValue, T defauleValue = default(T))
        {
            if (dbValue is T result)
            {
                return result;
            }

            if (dbValue is INullable temp && temp.IsNull)
            {
                return defauleValue;
            }

            return (T)Convert.ChangeType(dbValue, typeof(T));
        }
    }
}