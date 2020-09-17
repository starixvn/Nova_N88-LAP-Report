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

        public async Task<SortableResultModel<WinLossRecordViewModel>> GetData(WinLossMetaModel metaModel)
        {
            var (dataItems, dataTotal, totalRows) = await _winLossRepository.GetData(metaModel);

            if (totalRows == 0 || !dataItems.Any())
            {
                return SortableResultModel<WinLossRecordViewModel>.Default;
            }

            var result = new SortableResultModel<WinLossRecordViewModel>
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
                       Mapper(drilldownInOriginalCurrency, metaModel.IsGroupByPlayer));
        }

        private WinLossRecordViewModel ToTotalDataViewModel(WinLossDataTotalDTO dataTotal)
        {
            return new WinLossRecordViewModel
            {
                Turnover = dataTotal.TotalBasedTurnover,
                Ggr = dataTotal.TotalBasedGGR,
                BetCount = dataTotal.TotalBetCount,
                AvgBetAmount = dataTotal.TotalBasedAvgBetAmount,
                UniquePlayers = dataTotal.TotalUniquePlayers,
                Margin = CalculatorHepler.Margin(dataTotal.TotalBasedGGR, dataTotal.TotalBasedTurnoverNoDraw)
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
                return $"{data.Group}_{data.SabaId}";
            }

            return data.Group;
        }

        private static Func<bool, bool, Func<string, IEnumerable<WinLossDataItemDTO>, WinLossRecordViewModel>> Mapper = (drilldownInOriginalCurrency, isGroupByPlayer) => (parentKey, childData) =>
        {
            var (turnoverNoDraw, turnover, ggr, betCount, uniquePlayers) = SumChildData(childData);
            var firstChild = childData?.FirstOrDefault();
            var parent = new WinLossRecordViewModel
            {
                Key = parentKey,
                Group = isGroupByPlayer ? firstChild.Group : parentKey,
                SabaId = firstChild?.SabaId,
                SabaSystemId = firstChild?.SabaSystemId,
                Turnover = turnover,
                Ggr = ggr,
                BetCount = betCount,
                UniquePlayers = uniquePlayers,
                AvgBetAmount = turnover / betCount,
                Margin = CalculatorHepler.Margin(ggr, turnoverNoDraw),
                DrillDownData = SelectDrillDownData(parentKey, childData, drilldownInOriginalCurrency, isGroupByPlayer)
            };

            return parent;
        };

        private static IEnumerable<WinLossRecordViewModel> SelectDrillDownData(string parentKey, IEnumerable<WinLossDataItemDTO> childData, bool drilldownInOriginalCurrency, bool isGroupByPlayer)
        {
            return childData.Select(_ => new WinLossRecordViewModel
            {
                Parent = parentKey,
                ParentName = isGroupByPlayer ? _.Group : parentKey,
                Group = _.CurrencyCode,
                SabaId = _.SabaId,
                SabaSystemId = _.SabaSystemId,
                Turnover = drilldownInOriginalCurrency ? _.Turnover : _.BasedTurnover,
                Ggr = drilldownInOriginalCurrency ? _.GGR : _.BasedGGR,
                BetCount = _.BetCount,
                AvgBetAmount = drilldownInOriginalCurrency ? _.AvgBetAmount : _.BasedAvgBetAmount,
                UniquePlayers = _.UniquePlayers,
                Margin = CalculatorHepler.Margin(_.BasedGGR, _.BasedTurnoverNoDraw)
            });
        }

        private static (decimal, decimal, decimal, int, int) SumChildData(IEnumerable<WinLossDataItemDTO> childData)
        {
            var turnoverNoDraw = 0.0m;
            var turnover = 0.0m;
            var ggr = 0.0m;
            var betCount = 0;
            var uniquePlayers = 0;

            foreach (var childItem in childData)
            {
                turnover += childItem.BasedTurnover;
                ggr += childItem.BasedGGR;
                betCount += childItem.BetCount;
                uniquePlayers += childItem.UniquePlayers;
                turnoverNoDraw += childItem.BasedTurnoverNoDraw;
            }

            return (turnoverNoDraw, turnover, ggr, betCount, uniquePlayers);
        }
    }
}