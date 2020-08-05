using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class UserActivitiesMetaModel : BaseReportMetaModel
    {
        [SpParam(SqlParamName = "ViewBy")]
        public byte ViewMode { get; set; }
    }
}