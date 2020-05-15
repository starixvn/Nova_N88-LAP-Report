namespace Fanex.Athena.Report.Common.Services
{
    using Fanex.Athena.Report.Common.Model.DTO;
    using Fanex.Athena.Report.Common.Services.Interfaces;
    using Fanex.Data;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CurrencyService : ICurrencyService
    {
        private const int CacheTime24Hours = 24;

        private const string CurrencyListCacheKey = "REPORT_LAP_CACHE_CURRENCY_LIST";
        private const string CurrencyDictionaryCacheKey = "REPORT_LAP_CACHE_CURRENCY_DICTIONARY";

        private readonly IMemoryCache _cache;

        public CurrencyService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<IEnumerable<Currency>> GetCurrencyListAsync()
        {
            var currencyList = _cache.Get<IEnumerable<Currency>>(CurrencyListCacheKey);

            if (currencyList != null)
            {
                return currencyList;
            }

            currencyList = await GetCurrencyListFromDbAsync().ConfigureAwait(false);
            _cache.Set(CurrencyListCacheKey, currencyList, DateTimeOffset.Now.AddHours(CacheTime24Hours));

            return currencyList;
        }

        public async Task<IDictionary<int, Currency>> GetCurrencyDictionaryAsync()
        {
            var currencyDictionary = _cache.Get<IDictionary<int, Currency>>(CurrencyDictionaryCacheKey);
            if (currencyDictionary != null)
            {
                return currencyDictionary;
            }

            var currencyList = await GetCurrencyListAsync();
            currencyDictionary = currencyList.ToDictionary(_ => _.ID, _ => _);
            _cache.Set(CurrencyDictionaryCacheKey, currencyDictionary, DateTimeOffset.Now.AddHours(CacheTime24Hours));

            return currencyDictionary;
        }

        private static async Task<IEnumerable<Currency>> GetCurrencyListFromDbAsync()
        {
            const string UspName = "Lrf_CurrencySel";

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                return await db.QueryAsync<Currency>().ConfigureAwait(false);
            }
        }
    }
}