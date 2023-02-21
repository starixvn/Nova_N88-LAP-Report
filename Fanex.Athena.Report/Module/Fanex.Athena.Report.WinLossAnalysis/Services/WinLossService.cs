using Fanex.Athena.Report.Common.Helpers;
using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Repositories;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Fanex.NVC.Authentication;
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
        private readonly IDataPermissionService _dataPermissionService;

        public WinLossService(IWinLossRepository winLossRepository,
            ICurrencyService currencyService,
            IDataMapper dataMapper,
            IDataPermissionService dataPermissionService)
        {
            _winLossRepository = winLossRepository;
            _currencyService = currencyService;
            _dataMapper = dataMapper;
            _dataPermissionService = dataPermissionService;
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

            return ClearData(result);
        }

        private ResultModel<WinLossRecordViewModel> ClearData(ResultModel<WinLossRecordViewModel> data)
        {
            data.DataList.ToList().ForEach(_ =>
            {
                _dataPermissionService.ClearDataByPermission(_.DrillDownData);
                _dataPermissionService.ClearDataByPermission(_);
            });
            _dataPermissionService.ClearDataByPermission(data.TotalData);

            return data;
        }

        public async Task<IEnumerable<WinLossDataDetailItemDTO>> GetDetailData(WinLossDetailMetaModel metaModel)
        {
            var dataItems = await _winLossRepository.GetDetailData(metaModel);
            if (dataItems.Any())
            {
                var currencies = await _currencyService.GetCurrencyDictionaryAsync();
                foreach (var item in dataItems)
                {
                    item.CurrencyCode = _dataMapper.ToCurrencyCode(currencies, item.CurrencyId);
                }
            }

            return dataItems;
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
                Margin = NumberHepler.Margin(dataTotal.TotalBasedGGRForMargin, dataTotal.TotalBasedNetTurnoverForMargin),
                ClosingBalance = dataTotal.TotalBasedClosingBalance,
                NumberOfDeposit = dataTotal.TotalNumberOfDeposit,
                DepositAmount = dataTotal.TotalBasedDepositAmount,
                NumberOfWithdrawal = dataTotal.TotalNumberOfWithdrawal,
                WithdrawalAmount = dataTotal.TotalBasedWithdrawalAmount,
                BonusAmount = dataTotal.TotalBasedBonusAmount,
                ProgressiveBet = dataTotal.TotalBasedProgressiveBet,
                ProgressiveWin = dataTotal.TotalBasedProgressiveWin,
                Commission = dataTotal.TotalBasedCommission
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

        private static readonly Func<bool, bool, Func<string, IEnumerable<WinLossDataItemDTO>, WinLossRecordViewModel>> Mapper =
            (drilldownInOriginalCurrency, isGroupByPlayer) => (parentKey, childData) =>
        {
            var (turnover, netTurnover, netTurnoverForMargin, ggr, ggrForMargin, playerWinloss, betCount, uniquePlayers,
                numberOfDeposit, depositAmount, numberOfWithdrawal, withdrawalAmount, bonusAmount, closingBalance, progressiveBet, progressiveWin, commission) = SumChildData(childData);

            var firstChild = childData?.FirstOrDefault();
            var parent = new WinLossRecordViewModel
            {
                Key = parentKey,
                Group = isGroupByPlayer ? firstChild?.Group : parentKey,
                CustId = firstChild?.CustId,
                SabaId = firstChild?.SabaId,
                SabaSystemId = firstChild?.SabaSystemId,
                Turnover = turnover,
                NetTurnover = netTurnover,
                Ggr = ggr,
                PlayerWinloss = playerWinloss,
                BetCount = betCount,
                UniquePlayers = uniquePlayers,
                AvgBetAmount = NumberHepler.Divide(netTurnover, betCount),
                Margin = NumberHepler.Margin(ggrForMargin, netTurnoverForMargin),
                NumberOfDeposit = numberOfDeposit,
                DepositAmount = depositAmount,
                NumberOfWithdrawal = numberOfWithdrawal,
                WithdrawalAmount = withdrawalAmount,
                BonusAmount = bonusAmount,
                ClosingBalance = closingBalance,
                ProgressiveBet = progressiveBet,
                ProgressiveWin = progressiveWin,
                Commission = commission,
                DrillDownData = SelectDrillDownData(parentKey, childData, drilldownInOriginalCurrency, isGroupByPlayer)
            };

            return parent;
        };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Howard")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1541:Methods and properties should not be too complex", Justification = "Howard")]
        private static IEnumerable<WinLossRecordViewModel> SelectDrillDownData(string parentKey, IEnumerable<WinLossDataItemDTO> childData, bool drilldownInOriginalCurrency, bool isGroupByPlayer)
        {
            return childData.Select(_ => new WinLossRecordViewModel
            {
                Key = $"{parentKey}#{_.CurrencyCode}",
                Parent = parentKey,
                ParentName = isGroupByPlayer ? _.Group : parentKey,
                Group = _.CurrencyCode,
                CustId = _.CustId,
                SabaId = _.SabaId,
                SabaSystemId = _.SabaSystemId,
                Turnover = drilldownInOriginalCurrency ? _.Turnover : _.BasedTurnover,
                NetTurnover = drilldownInOriginalCurrency ? _.NetTurnover : _.BasedNetTurnover,
                Ggr = drilldownInOriginalCurrency ? _.GGR : _.BasedGGR,
                PlayerWinloss = drilldownInOriginalCurrency ? _.PlayerWinloss : _.BasedPlayerWinloss,
                ProgressiveBet = drilldownInOriginalCurrency ? _.ProgressiveBet : _.BasedProgressiveBet,
                ProgressiveWin = drilldownInOriginalCurrency ? _.ProgressiveWin : _.BasedProgressiveWin,
                Commission = drilldownInOriginalCurrency ? _.Commission : _.BasedCommission,
                BetCount = _.BetCount,
                AvgBetAmount = drilldownInOriginalCurrency ? _.AvgBetAmount : _.BasedAvgBetAmount,
                UniquePlayers = _.UniquePlayers,
                Margin = NumberHepler.Margin(_.BasedGGRForMargin, _.BasedNetTurnoverForMargin),
                NumberOfDeposit = _.NumberOfDeposit,
                DepositAmount = drilldownInOriginalCurrency ? _.DepositAmount : _.BasedDepositAmount,
                NumberOfWithdrawal = _.NumberOfWithdrawal,
                ClosingBalance = drilldownInOriginalCurrency ? _.ClosingBalance : _.BasedClosingBalance,
                WithdrawalAmount = drilldownInOriginalCurrency ? _.WithdrawalAmount : _.BasedWithdrawalAmount,
                BonusAmount = drilldownInOriginalCurrency ? _.BonusAmount : _.BasedBonusAmount
            });
        }

        private static (decimal, decimal, decimal, decimal, decimal, decimal, int, int,
            int, decimal, int, decimal, decimal, decimal, decimal, decimal, decimal) SumChildData(IEnumerable<WinLossDataItemDTO> childData)
        {
            var turnover = 0.0m;
            var netTurnover = 0.0m;
            var netTurnoverForMargin = 0.0m;
            var ggr = 0.0m;
            var ggrForMargin = 0.0m;
            var playerWinloss = 0.0m;
            var betCount = 0;
            var uniquePlayers = 0;
            var numberOfDeposit = 0;
            var depositAmount = 0.0m;
            var numberOfWithdrawal = 0;
            var withdrawalAmount = 0.0m;
            var bonusAmount = 0.0m;
            var closingBalance = 0.0m;
            var progressiveBet = 0.0m;
            var progressiveWin = 0.0m;
            var commission = 0.0m;

            foreach (var childItem in childData)
            {
                turnover += childItem.BasedTurnover;
                netTurnover += childItem.BasedNetTurnover;
                netTurnoverForMargin += childItem.BasedNetTurnoverForMargin;
                ggr += childItem.BasedGGR;
                ggrForMargin += childItem.BasedGGRForMargin;
                playerWinloss += childItem.BasedPlayerWinloss;
                betCount += childItem.BetCount;
                uniquePlayers += childItem.UniquePlayers;
                numberOfDeposit += childItem.NumberOfDeposit;
                depositAmount += childItem.BasedDepositAmount;
                numberOfWithdrawal += childItem.NumberOfWithdrawal;
                withdrawalAmount += childItem.BasedWithdrawalAmount;
                bonusAmount += childItem.BasedBonusAmount;
                closingBalance += childItem.BasedClosingBalance;
                progressiveBet += childItem.BasedProgressiveBet;
                progressiveWin += childItem.BasedProgressiveWin;
                commission += childItem.BasedCommission;
            }

            return (turnover, netTurnover, netTurnoverForMargin, ggr, ggrForMargin, playerWinloss, betCount, uniquePlayers,
                numberOfDeposit, depositAmount, numberOfWithdrawal, withdrawalAmount, bonusAmount, closingBalance, progressiveBet, progressiveWin, commission);
        }
    }
}