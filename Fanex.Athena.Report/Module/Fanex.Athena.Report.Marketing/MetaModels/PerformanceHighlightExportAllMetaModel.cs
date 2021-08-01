using Fanex.Data;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightExportAllMetaModel : BaseMarketingReportMetaModel
    {
        public DateTime? FromLastXDays { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public string CurrencyNameList { get; set; }

        public override bool IsValid => base.IsValid
            && !string.IsNullOrWhiteSpace(BaseCurrency);
    }
}