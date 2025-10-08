using Fanex.Athena.Report.Common.Model.ResultModel;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.MetaModels;

namespace Fanex.Athena.Report.ExtraPayout.Excels.Interfaces
{
    public interface IOddsBoostBonusDetailExcel
    {
        byte[] Export(string template, SortableReportResultModel<OddsBoostBonusDetail> data, ExtraPayoutMetaModel metaModel);
    }
}
