using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public interface IBonusClaimedRepository
    {
        Task<BonusClaimedSummaryDTO> GetSummaryData(BonusClaimedSummaryMetaModel metaModel);

        Task<(IEnumerable<BonusClaimedDetailDTO> rows, int totalRows)> GetDetailsData(BonusClaimedDetailMetaModel metaModel);
    }
}