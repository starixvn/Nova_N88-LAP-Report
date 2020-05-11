using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ByGroup.Services
{
    public interface IWinLossService
    {
        Task<ResultModel<WinLossRecordViewModel>> GetData(WinLossMetaModel metaModel);
    }
}