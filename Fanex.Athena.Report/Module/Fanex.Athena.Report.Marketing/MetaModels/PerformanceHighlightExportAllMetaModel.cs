using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightExportAllMetaModel : BaseMarketingReportMetaModel
    {
        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public string CurrencyNameList { get; set; }

        public override bool IsValid => base.IsValid
            && !string.IsNullOrWhiteSpace(BaseCurrency);
    }
}