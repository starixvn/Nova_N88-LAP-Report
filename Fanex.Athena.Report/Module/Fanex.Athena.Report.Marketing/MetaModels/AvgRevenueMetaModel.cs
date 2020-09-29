using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class AvgRevenueMetaModel : BaseMarketingReportMetaModel
    {
        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public override bool IsValid => base.IsValid && !string.IsNullOrEmpty(ProductIds);
    }
}