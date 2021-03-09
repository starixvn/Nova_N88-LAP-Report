using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Repositories
{
    public interface IWinLossRepository
    {
        Task<(IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO, int)> GetData(WinLossMetaModel metaModel);
        Task<IEnumerable<WinLossDataDetailItemDTO>> GetDetailData(WinLossDetailMetaModel metaModel);
    }
}