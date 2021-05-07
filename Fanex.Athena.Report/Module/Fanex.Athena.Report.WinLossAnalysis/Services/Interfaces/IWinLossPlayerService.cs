using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Services
{
    public interface IWinLossPlayerService
    {
        Task<IEnumerable<WinLossRecordViewModel>> GetPlayerDetail(WinLossPlayerDetailMetaModel metaModel);

        Task<ResultModel<WinLossPlayerBetListViewModel>> GetPlayerBetList(WinLossPlayerBetListMetaModel metaModel);
    }
}