using Fanex.Athena.Report.ByGroup.DTO;
using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.Repositories;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ByGroup.Services
{
    public class WinLossService : IWinLossService
    {
        private readonly IWinLossRepository _winLossRepository;

        public WinLossService(IWinLossRepository winLossRepository)
        {
            _winLossRepository = winLossRepository;
        }

        public async Task<ResultModel<WinLossRecordViewModel>> GetData(WinLossMetaModel metaModel)
        {
            var (dataItems, dataTotal, totalRows) = await _winLossRepository.GetData(metaModel);

            if (totalRows == 0 || !dataItems.Any())
            {
                return ResultModel<WinLossRecordViewModel>.Default;
            }

            var result = new ResultModel<WinLossRecordViewModel>
            {
                TotalRows = totalRows,
                DataList = dataItems.GroupBy(
                            _ => _.Group,
                            _ => _,
                            Mapper),
                TotalData = new WinLossRecordViewModel
                {
                    Turnover = dataTotal.TotalBasedTurnover,
                    Ggr = dataTotal.TotalBasedGGR,
                    BetCount = dataTotal.TotalBetCount,
                    AvgBetAmount = dataTotal.TotalBasedAvgBetAmount,
                    UniquePlayers = dataTotal.TotalUniquePlayers,
                    Margin = dataTotal.TotalMargin
                }
            };

            return result;
        }

        private static Func<string, IEnumerable<WinLossDataItemDTO>, WinLossRecordViewModel> Mapper = (key, childData) =>
        {
            var (turnoverNoDraw, turnover, ggr, betCount, uniquePlayers) = SumChildData(childData);
            var model = new WinLossRecordViewModel
            {
                Group = key,
                Turnover = turnover,
                Ggr = ggr,
                BetCount = betCount,
                UniquePlayers = uniquePlayers,
                AvgBetAmount = turnover / betCount,
                Margin = turnoverNoDraw == 0.0m ? 0.00 : (double)(ggr / turnoverNoDraw),
                DrillDownData = childData.Select(_ => new WinLossRecordViewModel
                {
                    HasParent = true,
                    Group = _.Code,
                    Turnover = _.Turnover,
                    Ggr = _.GGR,
                    BetCount = _.BetCount,
                    AvgBetAmount = _.AvgBetAmount,
                    UniquePlayers = _.UniquePlayers,
                    Margin = _.Margin
                })
            };

            return model;
        };

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