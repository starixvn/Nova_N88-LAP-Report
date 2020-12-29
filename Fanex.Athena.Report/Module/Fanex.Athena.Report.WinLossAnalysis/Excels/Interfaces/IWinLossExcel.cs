using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public interface IWinLossExcel
    {
        IWorkbook Export(ResultModel<WinLossRecordViewModel> data, ExportWinLossMetaModel metaModel);
    }
}