using Fanex.Athena.Report.Common.Model.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Common.Services.Interfaces
{
    public interface IDataMapper
    {
        string ToCurrencyCode(IDictionary<int, Currency> currencies, int id);

        string ToTpProductName(IDictionary<int, TpProduct> tpProducts, int id);
    }
}