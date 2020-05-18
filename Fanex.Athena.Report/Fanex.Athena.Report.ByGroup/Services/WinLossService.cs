using Fanex.Athena.Report.ByGroup.DTO;
using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.Repositories;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ByGroup.Services
{
    public class WinLossService : IWinLossService
    {
        private readonly IWinLossRepository _winLossRepository;
        private readonly IDataMapper _dataMapper;
        private readonly ICurrencyService _currencyService;

        public WinLossService(IWinLossRepository winLossRepository, ICurrencyService currencyService, IDataMapper dataMapper)
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
                DataList = await ToDataItemViewModels(dataItems, metaModel.IsGroupByCurreny),
                TotalData = ToTotalDataViewModel(dataTotal)
            };

            return result;
        }

        private async Task<IEnumerable<WinLossRecordViewModel>> ToDataItemViewModels(IEnumerable<WinLossDataItemDTO> dataItems, bool isGroupByCurreny)
        {
            var currencies = await _currencyService.GetCurrencyDictionaryAsync();
            return dataItems.GroupBy(
                       _ => isGroupByCurreny ? _dataMapper.ToCurrencyCode(currencies, Int32.Parse(_.Group)) : _.Group,
                       _ =>
                       {
                           _.CurrencyCode = _dataMapper.ToCurrencyCode(currencies, _.CurrencyId);
                           return _;
                       },
                       Mapper);
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
                Margin = dataTotal.TotalMargin
            };
        }

        private static Func<string, IEnumerable<WinLossDataItemDTO>, WinLossRecordViewModel> Mapper = (parentKey, childData) =>
        {
            var (turnoverNoDraw, turnover, ggr, betCount, uniquePlayers) = SumChildData(childData);
            var parent = new WinLossRecordViewModel
            {
                Group = parentKey,
                Turnover = turnover,
                Ggr = ggr,
                BetCount = betCount,
                UniquePlayers = uniquePlayers,
                AvgBetAmount = turnover / betCount,
                Margin = turnoverNoDraw == 0.0m ? 0.00 : (double)(ggr / turnoverNoDraw) * 100.00,
                DrillDownData = SelectDrillDownData(parentKey, childData)
            };

            return parent;
        };

        private static IEnumerable<WinLossRecordViewModel> SelectDrillDownData(string parentKey, IEnumerable<WinLossDataItemDTO> childData)
        {
            return childData.Select(_ => new WinLossRecordViewModel
            {
                Parent = parentKey,
                Group = _.CurrencyCode,
                Turnover = _.Turnover,
                Ggr = _.GGR,
                BetCount = _.BetCount,
                AvgBetAmount = _.AvgBetAmount,
                UniquePlayers = _.UniquePlayers,
                Margin = _.Margin
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