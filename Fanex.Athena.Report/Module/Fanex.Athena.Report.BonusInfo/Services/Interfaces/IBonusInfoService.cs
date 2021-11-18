using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.BonusInfo.Services
{
    public interface IBonusInfoService
    {
        Task<BonusInfoSummaryDTO> GetSummaryData(BonusInfoSummaryMetaModel metaModel);

        Task<BonusInfoDetailDTO> GetDetailsData(BonusInfoDetailMetaModel metaModel);

        Task<IEnumerable<MultipleSelectOption>> GetBonusTypes();
    }
}