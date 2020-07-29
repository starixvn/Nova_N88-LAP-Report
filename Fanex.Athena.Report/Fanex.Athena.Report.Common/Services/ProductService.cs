using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Common.Services
{
    public class ProductService : IProductService
    {
        private readonly IMemoryCache _cache;

        public ProductService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<IDictionary<int, TpProduct>> GetDictionaryTpProductAsync()
        {
            var tpProductDictionary = _cache.Get<IDictionary<int, TpProduct>>(CacheKeys.DictionaryTpProduct);
            if (tpProductDictionary != null)
            {
                return tpProductDictionary;
            }

            var tpProductList = await GetListTpProductFromDbAsync();
            tpProductDictionary = tpProductList.ToDictionary(_ => _.Id, _ => _);
            _cache.Set(CacheKeys.DictionaryTpProduct, tpProductDictionary, CacheExpireTimes.OneDay);

            return tpProductDictionary;
        }

        private static async Task<IEnumerable<TpProduct>> GetListTpProductFromDbAsync()
        {
            const string UspName = "N88_GetListGameType";
            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                var data = await db.QueryAsync<TpProduct>();
                return data;
            }
        }
    }
}