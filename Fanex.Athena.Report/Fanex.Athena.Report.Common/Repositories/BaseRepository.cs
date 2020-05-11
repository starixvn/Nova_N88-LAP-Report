using Fanex.Athena.Report.Common.Extensions;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;
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
            var properties = typeof(T).GetProperties();
            while (reader.Read())
            {
                var dataItem = ReadDataItem<T>(reader, properties);
                dataItems.Add(dataItem);
            }
            return dataItems;
        }

        protected T GetDataTotal<T>(IDataReader reader)
        {
            if (reader.NextResult() && reader.Read())
            {
                return ReadDataTotal<T>(reader, typeof(T).GetProperties());
            }
            return default;
        }

        protected virtual int GetTotalRows(IObjectDb db)
        {
            return Convert.ToInt32(db.GetParameterValue(TotalRowsParamName));
        }

        protected virtual T ReadDataItem<T>(IDataReader reader, PropertyInfo[] properties)
        {
            return ReadDataOfType<T>(reader, properties);
        }

        protected virtual T ReadDataTotal<T>(IDataReader reader, PropertyInfo[] properties)
        {
            return ReadDataOfType<T>(reader, properties);
        }

        protected static T ReadDataOfType<T>(IDataReader reader, PropertyInfo[] properties)
        {
            T model = Activator.CreateInstance<T>();
            foreach (var property in properties)
            {
                var value = reader.GetVal(property.PropertyType, property.Name);
                model.SetPropertyValue(property, value);
            }
            return model;
        }
    }
}