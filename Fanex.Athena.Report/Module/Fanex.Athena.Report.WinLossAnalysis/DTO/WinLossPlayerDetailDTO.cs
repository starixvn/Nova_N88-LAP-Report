namespace Fanex.Athena.Report.WinLossAnalysis.DTO
{
    public class WinLossPlayerDetailDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CustId { get; set; }
        public string SabaId { get; set; }
        public string SabaSystemId { get; set; }
        public decimal Turnover { get; set; }
        public decimal NetTurnover { get; set; }
        public decimal BasedTurnover { get; set; }
        public decimal BasedNetTurnover { get; set; }
        public decimal BasedNetTurnoverForMargin { get; set; }
        public decimal GGR { get; set; }
        public decimal BasedGGR { get; set; }
        public decimal Commission { get; set; }
        public decimal BasedCommission { get; set; }
        public decimal BasedGGRForMargin { get; set; }
        public decimal PlayerWinloss { get; set; }
        public decimal ProgressiveBet { get; set; }
        public decimal ProgressiveWin { get; set; }
        public decimal BasedPlayerWinloss { get; set; }
        public decimal BasedProgressiveBet { get; set; }
        public decimal BasedProgressiveWin { get; set; }
        public int BetCount { get; set; }
        public decimal AvgBetAmount { get; set; }
        public decimal BasedAvgBetAmount { get; set; }
        public int UniquePlayers { get; set; }
    }
}