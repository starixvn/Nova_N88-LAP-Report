using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.BonusInfo.Services
{
    public interface IBonusInfoService
    {
        Task<BonusInfoSummaryDTO> GetSummaryData(BonusInfoSummaryMetaModel metaModel);

        Task<BonusInfoDetailDTO> GetDetailsData(BonusInfoDetailMetaModel metaModel);
    }
}