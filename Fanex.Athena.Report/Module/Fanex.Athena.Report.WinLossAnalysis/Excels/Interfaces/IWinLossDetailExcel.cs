using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public interface IWinLossDetailExcel
    {
        IWorkbook Export(IEnumerable<WinLossDataDetailItemDTO> data, WinLossDetailMetaModel metaModel);
    }
}