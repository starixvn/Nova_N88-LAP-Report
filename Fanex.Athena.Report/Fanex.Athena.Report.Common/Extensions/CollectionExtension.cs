using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.Common.Extensions
{
    public static class CollectionExtension
    {
        private const string Ascending = "asc";

        private static string GetPropertyName<T>(string propertyName)
        {
            return typeof(T).GetProperties()
                .FirstOrDefault(_ => _.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                ?.Name;
        }

        public static void Sort<T>(this ISortableList<T> sortableList, string sortBy, string sortType = Ascending)
        {
            if (!sortableList.Any())
            {
                return;
            }

            var propertyName = GetPropertyName<T>(sortBy);
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            var ascending = sortType.Equals(Ascending, StringComparison.InvariantCultureIgnoreCase);
            sortableList.Sort(propertyName, ascending);
        }

        public static bool IsPresent<T>(this IEnumerable<T> list) => list != null && list.Any();
    }
}