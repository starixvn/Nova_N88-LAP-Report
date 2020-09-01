using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class UserActivitiesMetaModel : BaseMarketingReportMetaModel
    {
        [SpParam(SqlParamName = "ViewBy")]
        public byte ViewMode { get; set; }
    }
}