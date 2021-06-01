using System.Collections.Generic;

namespace Fanex.Athena.Report.WinLossAnalysis.ViewModels
{
    public class WinLossRecordViewModel
    {
        public string Key { get; set; }
        public string Group { get; set; }
        public string Parent { get; set; }
        public string ParentName { get; set; }
        public string OriginalCurrency { get; set; }
        public IEnumerable<WinLossRecordViewModel> DrillDownData { get; set; }
        public int? CustId { get; set; }
        public int? ProductId { get; set; }
        public string SabaId { get; set; }
        public string SabaSystemId { get; set; }
        public decimal Turnover { get; set; }
        public decimal NetTurnover { get; set; }
        public decimal Ggr { get; set; }
        public decimal PlayerWinloss { get; set; }
        public int BetCount { get; set; }
        public decimal AvgBetAmount { get; set; }
        public int UniquePlayers { get; set; }
        public double Margin { get; set; }
        public int NumberOfDeposit { get; set; }
        public decimal DepositAmount { get; set; }
        public int NumberOfWithdrawal { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public decimal ClosingBalance { get; set; }
    }
}