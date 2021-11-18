using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.BonusInfo.Repositories
{
    public interface IBonusInfoRepository
    {
        Task<BonusInfoSummaryDTO> GetSummaryData(BonusInfoSummaryMetaModel metaModel);

        Task<BonusInfoDetailDTO> GetDetailsData(BonusInfoDetailMetaModel metaModel);

        Task<IEnumerable<BonusTypeDTO>> GetBonusTypes();
    }
}