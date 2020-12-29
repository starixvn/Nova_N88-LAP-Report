using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public interface IDepositAmountExcel : IPerformanceHighlightDetailExcel<DepositAmountExportMetaModel,
        DepositAmountSummaryDTO,
        IEnumerable<DepositAmountDetailDTO>>
    {
    }
}