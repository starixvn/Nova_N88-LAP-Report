using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Repositories
{
    public interface IWinLossPlayerRepository
    {
        Task<IEnumerable<WinLossPlayerDetailDTO>> GetPlayerDetail(WinLossPlayerDetailMetaModel metaModel);

        Task<(IEnumerable<WinLossPlayerBetListDTO>, WinLossPlayerBetListTotalDTO, int)> GetPlayerBetList(WinLossPlayerBetListMetaModel metaModel);
    }
}