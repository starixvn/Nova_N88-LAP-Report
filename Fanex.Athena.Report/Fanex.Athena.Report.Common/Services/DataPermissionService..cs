using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.NVC.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fanex.Athena.Report.Common.Services
{
    public class DataPermissionService : IDataPermissionService
    {
        private readonly INVCPermissionService _permissionService;

        public DataPermissionService(INVCPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public void ClearDataByPermission<T>(T data)
        {
            var properties = GetPropertiesByDataAttribute<T>();

            foreach (var property in properties)
            {
                var (info, attribute) = property;

                if (!string.IsNullOrEmpty(attribute.Permission) && !_permissionService.HasPermission(attribute.Permission))
                {
                    info.SetValue(data, null);
                }
            }
        }

        private static IEnumerable<(PropertyInfo, DataPermissionAttribute)> GetPropertiesByDataAttribute<T>()
        {
            var properties = typeof(T).GetProperties().Where(_ => Attribute.IsDefined(_, typeof(DataPermissionAttribute)));
            return properties.Select(_ =>
            {
                var attribute = Attribute.GetCustomAttribute(_, typeof(DataPermissionAttribute)) as DataPermissionAttribute;
                return (_, attribute);
            });
        }
    }
}