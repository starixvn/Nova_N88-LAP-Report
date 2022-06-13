using Fanex.Athena.Report.Common.Helpers;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Repositories;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Services
{
    public class WinLossPlayerService : IWinLossPlayerService
    {
        private const string DateTimeFormat = "MM/dd/yyyy HH:mm:ss tt";

        private readonly IWinLossPlayerRepository _winLossRepository;

        public WinLossPlayerService(IWinLossPlayerRepository winLossRepository)
        {
            _winLossRepository = winLossRepository;
        }

        public async Task<IEnumerable<WinLossRecordViewModel>> GetPlayerDetail(WinLossPlayerDetailMetaModel metaModel)
        {
            var dataItems = await _winLossRepository.GetPlayerDetail(metaModel);
            return ToPlayerDetailViewModel(dataItems, metaModel.DrilldownInOriginalCurrency, metaModel.UsernameParent, metaModel.OriginalCurrency);
        }

        public async Task<ResultModel<WinLossPlayerBetListViewModel>> GetPlayerBetList(WinLossPlayerBetListMetaModel metaModel)
        {
            var (dataItems, dataTotal, totalRows) = await _winLossRepository.GetPlayerBetList(metaModel);
            if (totalRows == 0 || !dataItems.Any())
            {
                return SortableResultModel<WinLossPlayerBetListViewModel>.Default;
            }

            var result = new ResultModel<WinLossPlayerBetListViewModel>
            {
                TotalRows = totalRows,
                DataList = ToPlayerBetListViewModel(dataItems, metaModel.DrilldownInOriginalCurrency),
                TotalData = new WinLossPlayerBetListViewModel
                {
                    PlayerWinLoss = metaModel.DrilldownInOriginalCurrency ? dataTotal.TotalPlayerWinLoss : dataTotal.TotalBasedPlayerWinLoss,
                    GGR = metaModel.DrilldownInOriginalCurrency ? dataTotal.TotalGGR : dataTotal.TotalBasedGGR,
                    ProgressiveBet = metaModel.DrilldownInOriginalCurrency ? dataTotal.TotalProgressiveBet : dataTotal.TotalBasedProgressiveBet,
                    ProgressiveWin = metaModel.DrilldownInOriginalCurrency ? dataTotal.TotalProgressiveWin : dataTotal.TotalBasedProgressiveWin
                }
            };

            return result;
        }

        private IEnumerable<WinLossPlayerBetListViewModel> ToPlayerBetListViewModel(IEnumerable<WinLossPlayerBetListDTO> dataItems, bool drilldownInOriginalCurrency)
            => dataItems.Select(_ => new WinLossPlayerBetListViewModel
            {
                RowNumber = _.RowNumber,
                RefNo = _.RefNo,
                TransTime = _.TransTime.ToString(DateTimeFormat),
                Stake = drilldownInOriginalCurrency ? _.Stake : _.BasedStake,
                PlayerWinLoss = drilldownInOriginalCurrency ? _.PlayerWinLoss : _.BasedPlayerWinLoss,
                GGR = drilldownInOriginalCurrency ? _.GGR : _.BasedGGR,
                TicketStatus = _.TicketStatus,
                ProgressiveBet = drilldownInOriginalCurrency ? _.ProgressiveBet : _.BasedProgressiveBet,
                ProgressiveWin = drilldownInOriginalCurrency ? _.ProgressiveWin : _.BasedProgressiveWin
            });

        private IEnumerable<WinLossRecordViewModel> ToPlayerDetailViewModel(IEnumerable<WinLossPlayerDetailDTO> dataItems, bool drilldownInOriginalCurrency,
            string usernameParent, string originalCurrency)
            => dataItems.Select(_ => new WinLossRecordViewModel
            {
                Key = $"{_.CustId}_{_.ProductId}",
                Group = _.ProductName,
                CustId = _.CustId,
                ProductId = _.ProductId,
                SabaId = _.SabaId,
                SabaSystemId = _.SabaSystemId,
                ParentName = usernameParent,
                OriginalCurrency = originalCurrency,
                Turnover = drilldownInOriginalCurrency ? _.Turnover : _.BasedTurnover,
                NetTurnover = drilldownInOriginalCurrency ? _.NetTurnover : _.BasedNetTurnover,
                Ggr = drilldownInOriginalCurrency ? _.GGR : _.BasedGGR,
                PlayerWinloss = drilldownInOriginalCurrency ? _.PlayerWinloss : _.BasedPlayerWinloss,
                ProgressiveWin = drilldownInOriginalCurrency ? _.ProgressiveWin : _.BasedProgressiveWin,
                ProgressiveBet = drilldownInOriginalCurrency ? _.ProgressiveBet : _.BasedProgressiveBet,
                Commission = drilldownInOriginalCurrency ? _.Commission : _.BasedCommission,
                BetCount = _.BetCount,
                AvgBetAmount = drilldownInOriginalCurrency ? _.AvgBetAmount : _.BasedAvgBetAmount,
                UniquePlayers = _.UniquePlayers,
                Margin = NumberHepler.Margin(_.BasedGGRForMargin, _.BasedNetTurnoverForMargin)
            });
    }
}