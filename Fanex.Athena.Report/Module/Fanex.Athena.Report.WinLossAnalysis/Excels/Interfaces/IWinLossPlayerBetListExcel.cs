using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public interface IWinLossPlayerBetListExcel
    {
        IWorkbook Export(ResultModel<WinLossPlayerBetListViewModel> data, ExportWinLossPlayerBetListMetaModel metaModel);
    }
}