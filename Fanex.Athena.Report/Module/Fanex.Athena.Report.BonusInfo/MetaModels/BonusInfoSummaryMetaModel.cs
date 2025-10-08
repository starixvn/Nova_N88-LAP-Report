using Fanex.Athena.Report.Common.Model.MetaModels.Interfaces;
using Fanex.Data;

namespace Fanex.Athena.Report.BonusInfo.MetaModels
{
    public class BonusInfoSummaryMetaModel : BaseBonusInfoReportMetaModel, IBonusTypeMetaModel
    {
        public int GroupBy { get; set; }

        public string BonusName { get; set; }

        [SpParam(SqlParamName = "BonusTypeIds")]
        public string BonusType { get; set; }

        [SpParam(SqlParamName = "BonusStatusIds")]
        public string BonusStatus { get; set; }

        [SpParam(Ignored = true)]
        public string SortType { get; set; }

        [SpParam(Ignored = true)]
        public string SortBy { get; set; }
    }
}