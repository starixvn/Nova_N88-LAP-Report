using Fanex.Athena.Report.Common.Model.MetaModels.Interfaces;
using Fanex.NVC.Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.Core.Helpers
{
    public static class AuthHelper
    {
        public static IDictionary<int, string> GetCurrencyList(IEnumerable<Brand> brands, IBrandIdMetaModel brandIdMetaModel, IBrandIdListMetaModel brandIdListMetaModel)
        {
            int[] brandIds;
            if (brandIdMetaModel != null)
            {
                brandIds = new[] { brandIdMetaModel.BrandId };
            }
            else if (brandIdListMetaModel != null)
            {
                brandIds = brandIdListMetaModel.BrandIds
                  .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(x => int.Parse(x)).ToArray();
            }
            else
            {
                brandIds = null;
            }

            var result = GetCurrencyList(brands, brandIds);
            return result;
        }

        public static bool IsValidDataRangeLimitation(IDataRangeMetaModel dataRange, int DataRangeLimitation)
            => (DataRangeLimitation <= 0) || (dataRange.ToDate - dataRange.FromDate).Days <= DataRangeLimitation;

        private static IDictionary<int, string> GetCurrencyList(IEnumerable<Brand> brands, int[] brandIds)
             => brands
                .Where(_ => brandIds == null || brandIds.Contains(_.Id))
                .SelectMany(_ => _.TpCurrencyList)
                .GroupBy(_ => _.Key)
                .ToDictionary(x => x.Key, y => y.First().Value);
    }
}