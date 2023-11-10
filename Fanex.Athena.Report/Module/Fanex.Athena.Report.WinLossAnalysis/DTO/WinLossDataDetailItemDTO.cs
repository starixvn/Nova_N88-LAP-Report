using Fanex.Athena.Report.Common.Attributes;
using System;

namespace Fanex.Athena.Report.WinLossAnalysis.DTO
{
    public class WinLossDataDetailItemDTO
    {
        [SqlColumn(ColumnName = "rptDate")]
        public DateTime DateMonth { get; set; }

        public string ProductName { get; set; }

        public string UserName { get; set; }

        [SqlColumn(ColumnName = "TpUserName")]
        public string SabaId { get; set; }

        [SqlColumn(ColumnName = "TpSystemUserName")]
        public string SabaSystemId { get; set; }

        public int CurrencyId { get; set; }

        public int TpCurrencyId { get; set; }

        [SqlColumn(Ignored = true)]
        public string CurrencyCode { get; set; }

        public string SiteName { get; set; }

        public decimal Turnover { get; set; }
        public decimal NetTurnover { get; set; }
        public decimal GGR { get; set; }
        public decimal PlayerWinloss { get; set; }
        public decimal ProgressiveBet { get; set; }
        public decimal Commission { get; set; }
        public decimal ProgressiveWin { get; set; }
        public int BetCount { get; set; }
        public decimal AvgBetAmount { get; set; }
        public int UniquePlayers { get; set; }
        public double Margin { get; set; }
        public int NumberOfDeposit { get; set; }
        public decimal DepositAmount { get; set; }
        public int NumberOfWithdrawal { get; set; }
        public decimal WithdrawalAmount { get; set; }
    }
}