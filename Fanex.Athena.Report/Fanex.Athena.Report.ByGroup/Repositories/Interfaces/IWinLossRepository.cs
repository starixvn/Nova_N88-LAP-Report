using Fanex.Athena.Report.ByGroup.DTO;
using Fanex.Athena.Report.ByGroup.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ByGroup.Repositories
{
    public interface IWinLossRepository
    {
        Task<(IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO, int)> GetData(WinLossMetaModel metaModel);
    }
}