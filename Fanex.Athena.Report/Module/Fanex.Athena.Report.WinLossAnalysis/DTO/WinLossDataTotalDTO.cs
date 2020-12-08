namespace Fanex.Athena.Report.WinLossAnalysis.DTO
{
    public class WinLossDataTotalDTO
    {
        public decimal TotalBasedTurnover { get; set; }
        public decimal TotalBasedNetTurnover { get; set; }
        public decimal TotalBasedGGR { get; set; }
        public decimal TotalBasedTurnoverNoDraw { get; set; }
        public decimal TotalBasedAvgBetAmount { get; set; }
        public int TotalBetCount { get; set; }
        public int TotalUniquePlayers { get; set; }
        public double TotalMargin { get; set; }
    }
}