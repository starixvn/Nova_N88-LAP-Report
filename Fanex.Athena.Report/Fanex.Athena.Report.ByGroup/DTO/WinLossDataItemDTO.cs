namespace Fanex.Athena.Report.ByGroup.DTO
{
    public class WinLossDataItemDTO
    {
        public string Group { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Turnover { get; set; }
        public decimal BasedTurnover { get; set; }
        public decimal BasedTurnoverNoDraw { get; set; }
        public decimal GGR { get; set; }
        public decimal BasedGGR { get; set; }
        public int BetCount { get; set; }
        public decimal AvgBetAmount { get; set; }
        public decimal BasedAvgBetAmount { get; set; }
        public int UniquePlayers { get; set; }
        public double Margin { get; set; }
    }
}