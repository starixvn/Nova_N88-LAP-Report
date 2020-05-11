using Fanex.Data;
using System;

namespace Fanex.Athena.Report.ByGroup.MetaModels
{
    public class WinLossMetaModel
    {
        public int LicenseeId { get; set; }

        [SpParam(SqlParamName = "FilterBy")]
        public byte DateType { get; set; }

        public byte GroupBy { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }
        public string UserName { get; set; }
        public string CurrencyIds { get; set; }

        public bool IsIncludedTestData { get; set; }

        public bool IsNoDrawTicket { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }
    }
}