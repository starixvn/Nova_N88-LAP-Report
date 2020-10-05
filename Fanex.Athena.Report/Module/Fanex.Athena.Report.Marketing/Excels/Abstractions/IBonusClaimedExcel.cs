using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public interface IBonusClaimedExcel : IPerformanceHighlightDetailExcel<BonusClaimedExportMetaModel,
        BonusClaimedSummaryDTO,
        IEnumerable<BonusClaimedDetailDTO>>
    {
    }
}