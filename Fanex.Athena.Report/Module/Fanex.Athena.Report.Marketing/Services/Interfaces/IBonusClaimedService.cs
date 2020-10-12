using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public interface IBonusClaimedService
    {
        Task<BonusClaimedSummaryDTO> GetSummaryData(BonusClaimedSummaryMetaModel metaModel);

        Task<ResultModel<BonusClaimedDetailDTO>> GetDetailsData(BonusClaimedDetailMetaModel metaModel);

        Task<(BonusClaimedSummaryDTO, ResultModel<BonusClaimedDetailDTO>)> GetExportData(BonusClaimedExportMetaModel metaModel);
    }
}