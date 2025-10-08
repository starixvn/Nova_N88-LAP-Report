using Fanex.Athena.Report.Common.Model.ResultModel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.ExtraPayout.Const;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.MetaModels;
using Fanex.Athena.Report.ExtraPayout.Repositories.Interfaces;
using Fanex.Athena.Report.ExtraPayout.Services.Interfaces;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ExtraPayout.Services
{
    public class ExtraPayoutService : IExtraPayoutService
    {
        private readonly IExtraPayoutRepository _repository;

        public ExtraPayoutService(IExtraPayoutRepository repository)
        {
            _repository = repository;
        }

        public async Task<SortableResultModel<ExtraPayoutDTO>> GetDataAsync(ExtraPayoutMetaModel metaModel)
        {
            if (metaModel.DrillDownLevel == (int)ReportLevel.Main)
            {
                return await _repository.GetExtraPayoutMainData(metaModel).ConfigureAwait(false);
            }

            return await _repository.GetExtraPayoutMemberData(metaModel).ConfigureAwait(false);
        }

        public async Task<SortableResultModel<ExtraPayoutDetailDTO>> GetDetailDataAsync(ExtraPayoutDetailMetaModel metaModel)
        {
            return await _repository.GetExtraPayoutDetailData(metaModel).ConfigureAwait(false);
        }

        public async Task<SortableReportResultModel<OddsBoostBonusDetail>> GetOddsBoostBonusDetailDataAsync(ExtraPayoutMetaModel metaModel)
        {
            return await _repository.GetOddsBoostBonusDetailData(metaModel).ConfigureAwait(false);
        }
    }
}