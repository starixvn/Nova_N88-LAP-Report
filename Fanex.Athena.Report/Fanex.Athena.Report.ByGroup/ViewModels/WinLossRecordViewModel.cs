using System.Collections.Generic;

namespace Fanex.Athena.Report.ByGroup.ViewModels
{
    public class WinLossRecordViewModel
    {
        public string Group { get; set; }
        public string Parent { get; set; }
        public IEnumerable<WinLossRecordViewModel> DrillDownData { get; set; }
        public decimal Turnover { get; set; }
        public decimal Ggr { get; set; }
        public int BetCount { get; set; }
        public decimal AvgBetAmount { get; set; }
        public int UniquePlayers { get; set; }
        public double Margin { get; set; }
    }
}