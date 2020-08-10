using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Services.Interfaces;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Common.Services
{
    public class DataMapper : IDataMapper
    {
        public string ToCurrencyCode(IDictionary<int, Currency> currencies, int id)
        {
            return currencies.ContainsKey(id) ? currencies[id].Name : id.ToString();
        }

        public string ToTpProductName(IDictionary<int, TpProduct> tpProducts, int id)
        {
            return tpProducts.ContainsKey(id) ? tpProducts[id].Name : id.ToString();
        }
    }
}