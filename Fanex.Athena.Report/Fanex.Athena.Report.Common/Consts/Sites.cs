using System.Collections.Generic;

namespace Fanex.Athena.Report.Common.Consts
{
    public static class Sites
    {
        public const int N88SiteId = 402;
        public const int N8GSiteId = 436;
        public const int N8BSiteId = 437;

        public static readonly IReadOnlyDictionary<int, string> SiteNameList = new Dictionary<int, string>
        {
            { N88SiteId, "N88" },
            { N8GSiteId, "N8G" },
            { N8BSiteId, "N8B" }
        };

        public static readonly IReadOnlyDictionary<int, string> LaunchDate = new Dictionary<int, string>
        {
            { N8GSiteId, "09/12/2020" },
            { N8BSiteId, "08/14/2020" }
        };
    }
}