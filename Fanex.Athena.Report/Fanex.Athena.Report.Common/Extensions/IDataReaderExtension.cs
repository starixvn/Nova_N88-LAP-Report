using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;

namespace Fanex.Athena.Report.Common.Extensions
{
    public static class IDataReaderExtension
    {
        private static readonly IDictionary<Type, Func<IDataReader, string, object>> GetValByTypeFuncs = new Dictionary<Type, Func<IDataReader, string, object>>
        {
            { typeof(string), (dataReader, columnName) => GetVal<string>(dataReader, columnName) },
            { typeof(decimal), (dataReader, columnName) => GetVal<decimal>(dataReader, columnName) },
            { typeof(int), (dataReader, columnName) => GetVal<int>(dataReader, columnName) },
            { typeof(double), (dataReader, columnName) => GetVal<double>(dataReader, columnName) },
            { typeof(DateTime), (dataReader, columnName) => GetVal<DateTime>(dataReader, columnName) }
        };

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
            if (GetValByTypeFuncs.ContainsKey(type))
            {
                return GetValByTypeFuncs[type].Invoke(dataReader, columnName);
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