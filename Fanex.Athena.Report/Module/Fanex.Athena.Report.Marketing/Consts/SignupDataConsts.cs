using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Consts
{
    public static class SignupDataConsts
    {
        public static readonly IReadOnlyDictionary<string, bool> NoGroupingSignupSources = new Dictionary<string, bool>
        {
            { "None", true },
            { "AFF", true },
            { "Credit Site", true }
        };
    }
}