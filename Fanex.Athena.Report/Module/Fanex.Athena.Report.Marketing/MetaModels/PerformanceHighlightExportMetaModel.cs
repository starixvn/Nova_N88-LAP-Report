using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightExportMetaModel : BaseMarketingReportMetaModel
    {
        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }
    }
}