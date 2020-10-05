using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightExportAllMetaModel : BaseMarketingReportMetaModel
    {
        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public override bool IsValid => base.IsValid
            && !string.IsNullOrWhiteSpace(BaseCurrency);
    }
}