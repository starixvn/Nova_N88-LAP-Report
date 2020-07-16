using Fanex.Athena.Report.Resources;
using System.Collections.Generic;

namespace Fanex.Athena.Report.ByGroup.Helpers
{
    public static class DataHelper
    {
        private static readonly IDictionary<int, string> Timezones = new Dictionary<int, string>
        {
            { 8, ViewResources.GMT8 },
            { -4, ViewResources.GMTNegative4 },
            { 0, ViewResources.GMT0 },
        };

        public static string GetTimezoneName(int timezone)
        {
            if (Timezones.ContainsKey(timezone))
            {
                return Timezones[timezone];
            }

            return string.Empty;
        }
    }
}