namespace Fanex.Athena.Report.ByGroup.DTO
{
    public class WinLossRecordDTO
    {
        public string GroupName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Turnover { get; set; }
        public decimal TurnoverNoDraw { get; set; }
        public decimal BasedTurnover { get; set; }
        public decimal WinLoss { get; set; }
        public decimal BasedWinLoss { get; set; }
        public int BetCount { get; set; }
        public decimal AvgBetAmount { get; set; }
        public decimal BasedAvgBetAmount { get; set; }
        public int UniquePlayers { get; set; }
        public int Margin { get; set; }
    }
}