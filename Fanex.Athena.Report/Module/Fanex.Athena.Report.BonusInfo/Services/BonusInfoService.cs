using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.BonusInfo.Repositories;
using Fanex.Athena.Report.Common.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.BonusInfo.Services
{
    public class BonusInfoService : IBonusInfoService
    {
        private readonly IBonusInfoRepository _repository;

        public BonusInfoService(IBonusInfoRepository repository)
        {
            _repository = repository;
        }

        public async Task<BonusInfoSummaryDTO> GetSummaryData(BonusInfoSummaryMetaModel metaModel)
        {
            var data = await _repository.GetSummaryData(metaModel);

            var sortAbleList = data.SortedData.DataList as ISortableList<BonusInfoSummaryItemDTO>;
            sortAbleList?.Sort(metaModel.SortBy, metaModel.SortType);

            data.SortedData.DataList = sortAbleList;

            return data;
        }

        public async Task<BonusInfoDetailDTO> GetDetailsData(BonusInfoDetailMetaModel metaModel)
        {
            var data = await _repository.GetDetailsData(metaModel);
            return data;
        }
    }
}