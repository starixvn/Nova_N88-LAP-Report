using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ExtraPayout.Services.Interfaces
{
    public interface IExtraPayoutService
    {
        Task<SortableResultModel<ExtraPayoutDTO>> GetDataAsync(ExtraPayoutMetaModel metaModel);

        Task<SortableResultModel<ExtraPayoutDetailDTO>> GetDetailDataAsync(ExtraPayoutDetailMetaModel metaModel);
    }
}