using System.Threading.Tasks;
using Fanex.Athena.Report.AbnormalBet.DTO;
using Fanex.Athena.Report.AbnormalBet.MetaModels;
using Fanex.Athena.Report.Common.Model.ViewModel;

namespace Fanex.Athena.Report.AbnormalBet.Repositories.Interfaces
{
    public interface IAbnormalBetRepository
    {
        Task<SortableResultModel<AbnormalBetSummary>> GetSummaryData(
            AbnormalBetMetaModel metaModel);

        Task<SortableResultModel<AbnormalBetIpSummary>> GetIpSummaryData(
            AbnormalBetMetaModel metaModel);

        Task<SortableResultModel<AbnormalBetDetail>> GetDetailData(
            AbnormalBetMetaModel metaModel);
    }
}