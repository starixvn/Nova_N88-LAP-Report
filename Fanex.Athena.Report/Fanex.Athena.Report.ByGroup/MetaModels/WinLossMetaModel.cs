using Fanex.Data;
using System;

namespace Fanex.Athena.Report.ByGroup.MetaModels
{
    public class WinLossMetaModel
    {
        private const int GroupByCurrency = 4;

        public int LicenseeId { get; set; }

        [SpParam(SqlParamName = "FilterBy")]
        public byte DateType { get; set; }

        public byte GroupBy { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }
        public string UserName { get; set; }

        [SpParam(SqlParamName = "CurrencyIds")]
        public string CurrencyIdList { get; set; }

        public bool IsNoDrawTicket { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGroupByCurreny => GroupBy == GroupByCurrency;
    }
}