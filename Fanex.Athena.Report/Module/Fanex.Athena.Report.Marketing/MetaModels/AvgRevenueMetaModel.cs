using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class AvgRevenueMetaModel : BaseReportMetaModel
    {
        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }
    }
}