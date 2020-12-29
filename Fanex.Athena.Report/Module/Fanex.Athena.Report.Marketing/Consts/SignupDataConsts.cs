using System;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Consts
{
    public static class SignupDataConsts
    {
        public static readonly IReadOnlyDictionary<string, bool> NoGroupingSignupSources = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        {
            { "None", true },
            { "AFF", true },
            { "Bong88/Viva88", true }
        };

        public static readonly IReadOnlyDictionary<string, int> SignupSourceOrderWeights = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Nova88.net", 1000 },
            { "Goal123.com", 1000 },
            { "Bwing.com", 1000 },
            { "AFF", 900 },
            { "Bong88/Viva88", 800 },
            { "Landing Pages", 700 },
            { "None", 600 }
        };
    }
}