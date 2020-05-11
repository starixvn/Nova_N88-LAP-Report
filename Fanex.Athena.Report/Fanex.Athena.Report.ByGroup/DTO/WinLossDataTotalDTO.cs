namespace Fanex.Athena.Report.ByGroup.DTO
{
    public class WinLossDataTotalDTO
    {
        public decimal TotalBasedTurnover { get; set; }
        public decimal TotalBasedGGR { get; set; }
        public decimal TotalBasedAvgBetAmount { get; set; }
        public int TotalBetCount { get; set; }
        public int TotalUniquePlayers { get; set; }
        public double TotalMargin { get; set; }
    }
}