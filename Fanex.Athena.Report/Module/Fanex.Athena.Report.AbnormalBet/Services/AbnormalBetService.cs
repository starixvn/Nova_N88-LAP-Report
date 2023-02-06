using System.Threading.Tasks;
using Fanex.Athena.Report.AbnormalBet.Consts;
using Fanex.Athena.Report.AbnormalBet.MetaModels;
using Fanex.Athena.Report.AbnormalBet.Repositories.Interfaces;
using Fanex.Athena.Report.AbnormalBet.Services.Interfaces;

namespace Fanex.Athena.Report.AbnormalBet.Services
{
    public class AbnormalBetService : IAbnormalBetService
    {
        private readonly IAbnormalBetRepository _repository;

        public AbnormalBetService(IAbnormalBetRepository repository)
        {
            _repository = repository;
        }

        public async Task<dynamic> GetDataAsync(AbnormalBetMetaModel metaModel)
        {
            if (metaModel.ViewIn == (int)AbnormalBetViewInLevel.Summary)
            {
                return await _repository.GetSummaryData(metaModel).ConfigureAwait(false);
            }
            else if (metaModel.ViewIn == (int)AbnormalBetViewInLevel.IpSummary)
            {
                return await _repository.GetIpSummaryData(metaModel).ConfigureAwait(false);
            }
            else
            {
                return await _repository.GetDetailData(metaModel).ConfigureAwait(false);
            }
        }
    }
}