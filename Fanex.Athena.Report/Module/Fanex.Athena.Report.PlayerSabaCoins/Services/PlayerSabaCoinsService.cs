using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.Common.Model.ResultModel;
using Fanex.Athena.Report.PlayerSabaCoins.DTO;
using Fanex.Athena.Report.PlayerSabaCoins.MetaModels;
using Fanex.Athena.Report.PlayerSabaCoins.Repositories.Interfaces;
using Fanex.Athena.Report.PlayerSabaCoins.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.PlayerSabaCoins.Services
{
    public class PlayerSabaCoinsService : IPlayerSabaCoinsService
    {
        private readonly IPlayerSabaCoinsRepository _repository;

        public PlayerSabaCoinsService(IPlayerSabaCoinsRepository repository)
        {
            _repository = repository;
        }

        public async Task<SortableReportResultModel<PlayerSabaCoinsDTO>> GetDataAsync(PlayerSabaCoinsMetaModel metaModel)
        {
            var (mainData, extraInfoData) = await GetDataFromDBAsync(metaModel).ConfigureAwait(false);
            var dataList = JoinData(mainData, extraInfoData).ToList();
            var sortedDataList = PaginateAndSortData(new SortableList<PlayerSabaCoinsDTO>(dataList), metaModel);

            return new SortableReportResultModel<PlayerSabaCoinsDTO>
            {
                DataList = sortedDataList,
                TotalData = SumTotalData(dataList),
                TotalRows = dataList.Count,
            };
        }

        private async Task<(IEnumerable<PlayerSabaCoinsDTO>, IEnumerable<PlayerSabaCoinsDTO>)> GetDataFromDBAsync(PlayerSabaCoinsMetaModel metaModel)
        {
            var getMainData = _repository.GetMainData(metaModel);
            var getExtraInfoData = _repository.GetExtraInfoData(metaModel);
            await Task.WhenAll(getMainData, getExtraInfoData).ConfigureAwait(false);

            return (await getMainData, await getExtraInfoData);
        }

        private static IEnumerable<PlayerSabaCoinsDTO> JoinData(IEnumerable<PlayerSabaCoinsDTO> mainData, IEnumerable<PlayerSabaCoinsDTO> extraInfo)
        {
            if (mainData != null && mainData.Any())
            {
                if (extraInfo != null)
                {
                    return mainData.LeftOuterJoin(extraInfo, m => m.CustId, e => e.CustId,
                        (m, e) => new PlayerSabaCoinsDTO
                        {
                            CustId = m.CustId,
                            Username = m.Username,
                            ExternalId = m.ExternalId,
                            OpeningBalance = m.OpeningBalance,
                            Turnover = m.Turnover,
                            Outstanding = m.Outstanding,
                            WinLoss = m.WinLoss,
                            ClosingBalance = m.ClosingBalance,
                            RewardPromotionAmt = e?.RewardPromotionAmt ?? 0,
                            ExchangeAmt = e?.ExchangeAmt ?? 0
                        }
                    );
                }

                return mainData;
            }

            return new List<PlayerSabaCoinsDTO>();
        }

        private static PlayerSabaCoinsDTO SumTotalData(List<PlayerSabaCoinsDTO> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new PlayerSabaCoinsDTO();
            }

            return new PlayerSabaCoinsDTO
            {
                OpeningBalance = dataList.Sum(_ => _.OpeningBalance),
                Turnover = dataList.Sum(_ => _.Turnover),
                Outstanding = dataList.Sum(_ => _.Outstanding),
                WinLoss = dataList.Sum(_ => _.WinLoss),
                ClosingBalance = dataList.Sum(_ => _.ClosingBalance),
                RewardPromotionAmt = dataList.Sum(_ => _.RewardPromotionAmt),
                ExchangeAmt = dataList.Sum(_ => _.ExchangeAmt)
            };
        }

        private static SortableList<PlayerSabaCoinsDTO> PaginateAndSortData(SortableList<PlayerSabaCoinsDTO> dataList, PlayerSabaCoinsMetaModel metaModel)
        {
            var paginateDataList = metaModel.PageIndex == 0 && metaModel.PageSize == 0
                ? dataList : PaginateData(dataList, metaModel.PageIndex, metaModel.PageSize);

            return SortData(paginateDataList, metaModel.SortBy, metaModel.SortOrder);
        }

        private static SortableList<PlayerSabaCoinsDTO> SortData(SortableList<PlayerSabaCoinsDTO> dataList, string sortBy, string sortOrder = "asc")
        {
            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
            {
                var property = typeof(PlayerSabaCoinsDTO).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    dataList.Sort(property.Name, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase), nameof(PlayerSabaCoinsDTO.Index), true);
                }
            }

            return dataList;
        }

        private static SortableList<PlayerSabaCoinsDTO> PaginateData(SortableList<PlayerSabaCoinsDTO> dataList, int pageIndex = 1, int pageSize = 50)
        {
            return new SortableList<PlayerSabaCoinsDTO>(dataList.Skip(pageSize * (pageIndex - 1)).Take(pageSize));
        }
    }
}