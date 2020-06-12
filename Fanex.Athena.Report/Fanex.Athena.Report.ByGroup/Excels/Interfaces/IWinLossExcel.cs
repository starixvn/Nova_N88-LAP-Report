using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.ByGroup.Excels
{
    public interface IWinLossExcel
    {
        IWorkbook Export(ResultModel<WinLossRecordViewModel> data, WinLossMetaModel metaModel);
    }
}