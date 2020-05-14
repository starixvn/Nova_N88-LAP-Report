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
                var dataItem = ReadDataOfType<T>(reader, properties);
                dataItems.Add(dataItem);
            }
            return dataItems;
        }

        protected T GetDataTotal<T>(IDataReader reader)
        {
            if (reader.NextResult() && reader.Read())
            {
                return ReadDataOfType<T>(reader, typeof(T).GetProperties());
            }
            return default(T);
        }

        protected virtual int GetTotalRows(IObjectDb db)
        {
            return Convert.ToInt32(db.GetParameterValue(TotalRowsParamName));
        }

        protected T ReadDataOfType<T>(IDataReader reader, PropertyInfo[] properties)
        {
            T model = Activator.CreateInstance<T>();
            foreach (var property in properties)
            {
                var value = GetValueColumn<T>(reader, property);
                model.SetPropertyValue(property, value);
            }
            return model;
        }

        protected virtual object GetValueColumn<T>(IDataReader reader, PropertyInfo property)
        {
            return reader.GetVal(property.PropertyType, property.Name);
        }
    }
}