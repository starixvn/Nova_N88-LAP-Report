using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.BonusInfo.Excels
{
    public interface IBonusInfoDetailExcel
    {
        IWorkbook Export(BonusInfoDetailDTO data, BonusInfoDetailMetaModel metaModel);
    }
}