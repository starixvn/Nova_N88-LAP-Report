using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Services
{
    public interface IWinLossService
    {
        Task<ResultModel<WinLossRecordViewModel>> GetData(WinLossMetaModel metaModel);
    }
}