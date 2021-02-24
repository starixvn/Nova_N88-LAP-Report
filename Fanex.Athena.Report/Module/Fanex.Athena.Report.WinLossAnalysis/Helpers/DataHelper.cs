using Fanex.Athena.Report.Common.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.WinLossAnalysis.Helpers
{
    public static class DataHelper
    {
        private static readonly IReadOnlyDictionary<int, string> Timezones = new Dictionary<int, string>
        {
            { -12, "GMT-12" },
            { -11, "GMT-11" },
            { -10, "GMT-10" },
            { -9, "GMT-09" },
            { -8, "GMT-08" },
            { -7, "GMT-07" },
            { -6, "GMT-06" },
            { -5, "GMT-05" },
            { -4, "GMT-04" },
            { -3, "GMT-03" },
            { -2, "GMT-02" },
            { -1, "GMT-01" },
            { 0, "GMT+00" },
            { 1, "GMT+01" },
            { 2, "GMT+02" },
            { 3, "GMT+03" },
            { 4, "GMT+04" },
            { 5, "GMT+05" },
            { 6, "GMT+06" },
            { 7, "GMT+07" },
            { 8, "GMT+08" },
            { 9, "GMT+09" },
            { 10, "GMT+10" },
            { 11, "GMT+11" },
            { 12, "GMT+12" },
            { 13, "GMT+13" },
            { 14, "GMT+14" }
        };

        public static readonly Lazy<IEnumerable<SingleSelectOption>> TimezoneOptions = new Lazy<IEnumerable<SingleSelectOption>>(() => Timezones.Select(
            _ => new SingleSelectOption
            {
                Value = _.Key,
                Label = _.Value
            }));

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