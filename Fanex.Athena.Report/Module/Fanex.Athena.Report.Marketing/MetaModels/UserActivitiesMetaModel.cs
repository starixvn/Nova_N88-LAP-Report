using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class UserActivitiesMetaModel : BaseMarketingReportMetaModel
    {
        [SpParam(SqlParamName = "ViewBy")]
        public byte ViewMode { get; set; }

        public override bool IsValid => base.IsValid &&
#pragma warning disable S109 // Magic numbers should not be used
           ViewMode > 0 && ViewMode < 3;
#pragma warning restore S109 // Magic numbers should not be used
    }
}