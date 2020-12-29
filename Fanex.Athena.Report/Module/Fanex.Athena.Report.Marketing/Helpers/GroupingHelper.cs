using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.Marketing.Helpers
{
    public static class GroupingHelper
    {
        public static IEnumerable<T> GroupingSignupData<T>(IEnumerable<T> sources, Action<T, IEnumerable<T>> childHandler)
         where T : ISignupDTO<T>, new()
        {
            return sources.GroupBy(_ => _.SignUpSource,
            _ => _,
            (signupSource, childs) =>
            {
                var noGrouping = SignupDataConsts.NoGroupingSignupSources.ContainsKey(signupSource);

                var result = new T
                {
                    SignUpSource = signupSource,
                    SignupUrl = signupSource,
                    NoGrouping = noGrouping,
                    Childs = noGrouping ? Enumerable.Empty<T>() : childs.OrderBy(_ => _.SignupUrl)
                };

                if (childHandler != null)
                {
                    childHandler(result, childs);
                }

                return result;
            })
                .OrderByDescending(_ => SignupDataConsts.SignupSourceOrderWeights.ContainsKey(_.SignUpSource) ? SignupDataConsts.SignupSourceOrderWeights[_.SignUpSource] : default(int));
        }
    }
}