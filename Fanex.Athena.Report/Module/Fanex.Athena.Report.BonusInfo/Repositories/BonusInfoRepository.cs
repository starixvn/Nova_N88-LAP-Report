using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.BonusInfo.Repositories
{
    public class BonusInfoRepository : BaseRepository, IBonusInfoRepository
    {
        protected override string TotalRowsParamName => "NRows";

        public async Task<BonusInfoSummaryDTO> GetSummaryData(BonusInfoSummaryMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.BonusInfoSummary))
            {
                IEnumerable<BonusInfoSummaryItemDTO> dataList;
                BonusInfoSummaryTotalDTO totalDetail;

                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    dataList = GetDataItems<BonusInfoSummaryItemDTO>(reader);
                    totalDetail = GetDataTotal<BonusInfoSummaryTotalDTO>(reader);
                }

                if (!dataList.Any())
                {
                    return new BonusInfoSummaryDTO { SortedData = SortableResultModel<BonusInfoSummaryItemDTO>.Default };
                }

                return new BonusInfoSummaryDTO
                {
                    SortedData = new SortableResultModel<BonusInfoSummaryItemDTO>
                    {
                        DataList = dataList,
                        TotalData = ToTotalDataViewModel(totalDetail),
                        TotalRows = dataList.Count()
                    },

                    BonusCodeList = dataList.OrderBy(_ => _.BonusCode).Select(_ => _.BonusCode).Distinct()
                };
            }
        }

        private BonusInfoSummaryItemDTO ToTotalDataViewModel(BonusInfoSummaryTotalDTO dataTotal)
        {
            return new BonusInfoSummaryItemDTO
            {
                Running = dataTotal.Running,
                Canceled = dataTotal.Canceled,
                Fulfill = dataTotal.Fulfill,
                OtherPlatform = dataTotal.OtherPlatform,
                TotalClaimants = dataTotal.TotalClaimants,
                TotalClaimedAmount = dataTotal.TotalClaimedAmount,
                TotalDeposit = dataTotal.TotalDeposit,
                TotalWithdrawal = dataTotal.TotalWithdrawal,
                GGR = dataTotal.GGR
            };
        }

        public async Task<BonusInfoDetailDTO> GetDetailsData(BonusInfoDetailMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.BonusInfoDetail))
            {
                IEnumerable<BonusInfoDetailItemDTO> dataList;

                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    dataList = GetDataItems<BonusInfoDetailItemDTO>(reader);
                }

                return new BonusInfoDetailDTO
                {
                    DataList = dataList,
                    TotalRows = GetTotalRows(db)
                };
            }
        }
    }
}