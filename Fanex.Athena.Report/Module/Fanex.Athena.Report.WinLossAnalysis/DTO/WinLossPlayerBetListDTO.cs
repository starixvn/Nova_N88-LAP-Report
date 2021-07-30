using Fanex.Athena.Report.Common.Attributes;
using Newtonsoft.Json;
using System;

namespace Fanex.Athena.Report.WinLossAnalysis.DTO
{
    public class WinLossPlayerBetListDTO
    {
        public int RowNumber { get; set; }
        public string RefNo { get; set; }
        public DateTime TransTime { get; set; }
        public decimal Stake { get; set; }
        public decimal BasedStake { get; set; }
        public decimal PlayerWinLoss { get; set; }
        public decimal BasedPlayerWinLoss { get; set; }
        public decimal GGR { get; set; }
        public decimal BasedGGR { get; set; }
        public string TicketStatus { get; set; }
        public decimal ProgressiveBet { get; set; }
        public decimal ProgressiveWin { get; set; }
        public decimal BasedProgressiveBet { get; set; }
        public decimal BasedProgressiveWin { get; set; }
    }
}