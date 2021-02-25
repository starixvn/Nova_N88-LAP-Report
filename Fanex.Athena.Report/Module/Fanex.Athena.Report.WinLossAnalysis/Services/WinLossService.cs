using Fanex.Athena.Report.Common.Helpers;
using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Repositories;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Services
{
    public class WinLossService : IWinLossService
    {
        private readonly IWinLossRepository _winLossRepository;
        private readonly IDataMapper _dataMapper;
        private readonly ICurrencyService _currencyService;

        public WinLossService(IWinLossRepository winLossRepository,
            ICurrencyService currencyService,
            IDataMapper dataMapper)
        {
            _winLossRepository = winLossRepository;
            _currencyService = currencyService;
            _dataMapper = dataMapper;
        }

        public async Task<ResultModel<WinLossRecordViewModel>> GetData(WinLossMetaModel metaModel)
        {
            var (dataItems, dataTotal, totalRows) = await _winLossRepository.GetData(metaModel);

            if (totalRows == 0 || !dataItems.Any())
            {
                return SortableResultModel<WinLossRecordViewModel>.Default;
            }

            var result = new ResultModel<WinLossRecordViewModel>
            {
                TotalRows = totalRows,
                DataList = await ToDataItemViewModels(dataItems, metaModel),
                TotalData = ToTotalDataViewModel(dataTotal)
            };

            return result;
        }

        private async Task<IEnumerable<WinLossRecordViewModel>> ToDataItemViewModels(IEnumerable<WinLossDataItemDTO> dataItems, WinLossMetaModel metaModel)
        {
            var currencies = await _currencyService.GetCurrencyDictionaryAsync();
            var drilldownInOriginalCurrency = metaModel.DrilldownInOriginalCurrency;

            return dataItems.GroupBy(
                       data => Grouping(data, metaModel, currencies),
                       _ =>
                       {
                           _.CurrencyCode = _dataMapper.ToCurrencyCode(currencies, _.CurrencyId);
                           return _;
                       },
                       Mapper(drilldownInOriginalCurrency, metaModel.IsGroupByPlayer)).ToList();
        }

        private WinLossRecordViewModel ToTotalDataViewModel(WinLossDataTotalDTO dataTotal)
        {
            return new WinLossRecordViewModel
            {
                Turnover = dataTotal.TotalBasedTurnover,
                NetTurnover = dataTotal.TotalBasedNetTurnover,
                Ggr = dataTotal.TotalBasedGGR,
                PlayerWinloss = dataTotal.TotalBasedPlayerWinloss,
                BetCount = dataTotal.TotalBetCount,
                AvgBetAmount = dataTotal.TotalBasedAvgBetAmount,
                UniquePlayers = dataTotal.TotalUniquePlayers,
                Margin = NumberHepler.Margin(dataTotal.TotalBasedGGR, dataTotal.TotalBasedNetTurnover),
                NumberOfDeposit = dataTotal.TotalNumberOfDeposit,
                DepositAmount = dataTotal.TotalBasedDepositAmount,
                NumberOfWithdrawal = dataTotal.TotalNumberOfWithdrawal,
                WithdrawalAmount = dataTotal.TotalBasedWithdrawalAmount
            };
        }

        private string Grouping(WinLossDataItemDTO data, WinLossMetaModel metaModel, IDictionary<int, Currency> currencies)
        {
            if (metaModel.IsGroupByCurreny)
            {
                return _dataMapper.ToCurrencyCode(currencies, Int32.Parse(data.Group));
            }

            if (metaModel.IsGroupByPlayer)
            {
                return $"{data.Group}_{data.CustId}";
            }

            return data.Group;
        }

        private static Func<bool, bool, Func<string, IEnumerable<WinLossDataItemDTO>, WinLossRecordViewModel>> Mapper = (drilldownInOriginalCurrency, isGroupByPlayer) => (parentKey, childData) =>
        {
            var (turnover, netTurnover, ggr, playerWinloss, betCount, uniquePlayers,
                numberOfDeposit, depositAmount, numberOfWithdrawal, withdrawalAmount) = SumChildData(childData);

            var firstChild = childData?.FirstOrDefault();
            var parent = new WinLossRecordViewModel
            {
                Key = parentKey,
                Group = isGroupByPlayer ? firstChild?.Group : parentKey,
                SabaId = firstChild?.SabaId,
                SabaSystemId = firstChild?.SabaSystemId,
                Turnover = turnover,
                NetTurnover = netTurnover,
                Ggr = ggr,
                PlayerWinloss = playerWinloss,
                BetCount = betCount,
                UniquePlayers = uniquePlayers,
                AvgBetAmount = NumberHepler.Divide(netTurnover, betCount),
                Margin = NumberHepler.Margin(ggr, netTurnover),
                NumberOfDeposit = numberOfDeposit,
                DepositAmount = depositAmount,
                NumberOfWithdrawal = numberOfWithdrawal,
                WithdrawalAmount = withdrawalAmount,
                DrillDownData = SelectDrillDownData(parentKey, childData, drilldownInOriginalCurrency, isGroupByPlayer)
            };

            return parent;
        };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Howard")]
        private static IEnumerable<WinLossRecordViewModel> SelectDrillDownData(string parentKey, IEnumerable<WinLossDataItemDTO> childData, bool drilldownInOriginalCurrency, bool isGroupByPlayer)
        {
            return childData.Select(_ => new WinLossRecordViewModel
            {
                Key = $"{parentKey}#{_.CurrencyCode}",
                Parent = parentKey,
                ParentName = isGroupByPlayer ? _.Group : parentKey,
                Group = _.CurrencyCode,
                SabaId = _.SabaId,
                SabaSystemId = _.SabaSystemId,
                Turnover = drilldownInOriginalCurrency ? _.Turnover : _.BasedTurnover,
                NetTurnover = drilldownInOriginalCurrency ? _.NetTurnover : _.BasedNetTurnover,
                Ggr = drilldownInOriginalCurrency ? _.GGR : _.BasedGGR,
                PlayerWinloss = drilldownInOriginalCurrency ? _.PlayerWinloss : _.BasedPlayerWinloss,
                BetCount = _.BetCount,
                AvgBetAmount = drilldownInOriginalCurrency ? _.AvgBetAmount : _.BasedAvgBetAmount,
                UniquePlayers = _.UniquePlayers,
                Margin = NumberHepler.Margin(_.BasedGGR, _.BasedNetTurnover),
                NumberOfDeposit = _.NumberOfDeposit,
                DepositAmount = drilldownInOriginalCurrency ? _.DepositAmount : _.BasedDepositAmount,
                NumberOfWithdrawal = _.NumberOfWithdrawal,
                WithdrawalAmount = drilldownInOriginalCurrency ? _.WithdrawalAmount : _.BasedWithdrawalAmount
            });
        }

        private static (decimal, decimal, decimal, decimal, int, int,
            int, decimal, int, decimal) SumChildData(IEnumerable<WinLossDataItemDTO> childData)
        {
            var turnover = 0.0m;
            var netTurnover = 0.0m;
            var ggr = 0.0m;
            var playerWinloss = 0.0m;
            var betCount = 0;
            var uniquePlayers = 0;
            var numberOfDeposit = 0;
            var depositAmount = 0.0m;
            var numberOfWithdrawal = 0;
            var withdrawalAmount = 0.0m;

            foreach (var childItem in childData)
            {
                turnover += childItem.BasedTurnover;
                netTurnover += childItem.BasedNetTurnover;
                ggr += childItem.BasedGGR;
                playerWinloss += childItem.BasedPlayerWinloss;
                betCount += childItem.BetCount;
                uniquePlayers += childItem.UniquePlayers;
                numberOfDeposit += childItem.NumberOfDeposit;
                depositAmount += childItem.BasedDepositAmount;
                numberOfWithdrawal += childItem.NumberOfWithdrawal;
                withdrawalAmount += childItem.BasedWithdrawalAmount;
            }

            return (turnover, netTurnover, ggr, playerWinloss, betCount, uniquePlayers,
                numberOfDeposit, depositAmount, numberOfWithdrawal, withdrawalAmount);
        }
    }
}