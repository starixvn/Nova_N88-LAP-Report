using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Services.Interfaces;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Common.Services
{
    public class DataMapper : IDataMapper
    {
        public string ToCurrencyCode(IDictionary<int, Currency> currencies, int currencyId)
        {
            return currencies.ContainsKey(currencyId) ? currencies[currencyId].Name : currencyId.ToString();
        }
    }
}