using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Fanex.Athena.Report.Common.Repositories
{
    public class BaseRepository
    {
        protected virtual string TotalRowsParamName => "TotalRows";

        protected BaseRepository()
        {
        }

        protected IEnumerable<T> GetDataItems<T>(IDataReader reader)
        {
            var dataItems = new List<T>();
            var properties = GetProperties<T>();

            while (reader.Read())
            {
                var dataItem = ReadDataOfType<T>(reader, properties);
                dataItems.Add(dataItem);
            }
            return dataItems;
        }

        protected T GetDataTotal<T>(IDataReader reader)
        {
            if (reader.NextResult() && reader.Read())
            {
                return ReadDataOfType<T>(reader, GetProperties<T>());
            }
            return default(T);
        }

        protected virtual int GetTotalRows(IObjectDb db)
        {
            return Convert.ToInt32(db.GetParameterValue(TotalRowsParamName));
        }

        protected T ReadDataOfType<T>(IDataReader reader, IEnumerable<(PropertyInfo, SqlColReaderAttribute)> properties)
        {
            T model = Activator.CreateInstance<T>();
            foreach (var property in properties)
            {
                var (info, attribute) = property;

                var colName = info.Name;

                if (attribute != null)
                {
                    if (attribute.Ignored)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(attribute.ColName))
                    {
                        colName = attribute.ColName;
                    }
                }

                var value = GetValueColumn<T>(reader, info.PropertyType, colName);
                model.SetPropertyValue(info, value);
            }
            return model;
        }

        protected virtual object GetValueColumn<T>(IDataReader reader, Type type, string colName)
        {
            return reader.GetVal(type, colName);
        }

        private static IEnumerable<(PropertyInfo, SqlColReaderAttribute)> GetProperties<T>()
        {
            var properties = typeof(T).GetProperties();
            return properties.Select(_ =>
            {
                var attribute = Attribute.GetCustomAttribute(_, typeof(SqlColReaderAttribute)) as SqlColReaderAttribute;
                return (_, attribute);
            });
        }
    }
}