using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Data;
using System;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class WinLossMetaModel: ITimezoneMetaModel
    {
        private const int GroupByCurrency = 4;

        [SpParam(SqlParamName = "FilterBy")]
        public byte DateType { get; set; }

        public byte GroupBy { get; set; }

        public int Timezone { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "LicenseeIds")]
        public string BrandIds { get; set; }

        public byte UserNameFilterId { get; set; }
        public string UserName { get; set; }
        public bool IsIncludedTestData { get; set; }

        [SpParam(SqlParamName = "CurrencyIds")]
        public string CurrencyIdList { get; set; }

        public bool IsNoDrawTicket { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGroupByCurreny => GroupBy == GroupByCurrency;

        [SpParam(Ignored = true)]
        public bool DrilldownInOriginalCurrency { get; set; }
    }
}