using Fanex.Athena.Report.Common.Attributes;

namespace Fanex.Athena.Report.WinLossAnalysis.DTO
{
    public class WinLossDataItemDTO
    {
        public string Group { get; set; }
        public int CurrencyId { get; set; }

        [SqlColumn(Ignored = true)]
        public string CurrencyCode { get; set; }

        public int? CustId { get; set; }
        public string SabaId { get; set; }
        public string SabaSystemId { get; set; }
        public decimal Turnover { get; set; }
        public decimal NetTurnover { get; set; }
        public decimal BasedTurnover { get; set; }
        public decimal BasedNetTurnover { get; set; }
        public decimal BasedNetTurnoverForMargin { get; set; }
        public decimal GGR { get; set; }
        public decimal BasedGGR { get; set; }
        public decimal BasedGGRForMargin { get; set; }
        public decimal PlayerWinloss { get; set; }
        public decimal BasedPlayerWinloss { get; set; }
        public int BetCount { get; set; }
        public decimal AvgBetAmount { get; set; }
        public decimal BasedAvgBetAmount { get; set; }
        public int UniquePlayers { get; set; }
        public int NumberOfDeposit { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal BasedDepositAmount { get; set; }
        public int NumberOfWithdrawal { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public decimal BasedWithdrawalAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal BasedBonusAmount { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal BasedClosingBalance { get; set; }
        public decimal ProgressiveWin { get; set; }
        public decimal ProgressiveBet { get; set; }
        public decimal BasedProgressiveWin { get; set; }
        public decimal BasedProgressiveBet { get; set; }
    }
}