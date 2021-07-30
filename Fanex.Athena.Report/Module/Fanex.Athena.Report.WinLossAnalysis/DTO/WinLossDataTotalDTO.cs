namespace Fanex.Athena.Report.WinLossAnalysis.DTO
{
    public class WinLossDataTotalDTO
    {
        public decimal TotalBasedTurnover { get; set; }
        public decimal TotalBasedNetTurnover { get; set; }
        public decimal TotalBasedNetTurnoverForMargin { get; set; }
        public decimal TotalBasedGGR { get; set; }
        public decimal TotalBasedGGRForMargin { get; set; }
        public decimal TotalBasedPlayerWinloss { get; set; }
        public decimal TotalBasedTurnoverNoDraw { get; set; }
        public decimal TotalBasedAvgBetAmount { get; set; }
        public int TotalBetCount { get; set; }
        public int TotalUniquePlayers { get; set; }
        public int TotalNumberOfDeposit { get; set; }
        public decimal TotalBasedDepositAmount { get; set; }
        public int TotalNumberOfWithdrawal { get; set; }
        public decimal TotalBasedWithdrawalAmount { get; set; }
        public decimal TotalBasedClosingBalance { get; set; }
        public decimal TotalBasedProgressiveWin { get; set; }
        public decimal TotalBasedProgressiveBet { get; set; }
    }
}