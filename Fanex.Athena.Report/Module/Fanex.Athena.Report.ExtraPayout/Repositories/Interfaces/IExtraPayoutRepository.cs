using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ExtraPayout.Repositories.Interfaces
{
    public interface IExtraPayoutRepository
    {
        Task<SortableResultModel<ExtraPayoutDTO>> GetExtraPayoutMainData(
            ExtraPayoutMetaModel metaModel);

        Task<SortableResultModel<ExtraPayoutDTO>> GetExtraPayoutMemberData(
            ExtraPayoutMetaModel metaModel);

        Task<SortableResultModel<ExtraPayoutDetailDTO>> GetExtraPayoutDetailData(
            ExtraPayoutDetailMetaModel metaModel);
    }
}