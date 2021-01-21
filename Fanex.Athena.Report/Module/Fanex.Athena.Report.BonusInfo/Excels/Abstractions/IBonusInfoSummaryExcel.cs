using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.BonusInfo.Excels
{
    public interface IBonusInfoSummaryExcel
    {
        IWorkbook Export(BonusInfoSummaryDTO data, BonusInfoSummaryMetaModel metaModel);
    }
}