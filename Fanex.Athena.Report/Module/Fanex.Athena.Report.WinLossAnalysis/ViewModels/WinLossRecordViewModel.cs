using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Common.Consts;
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

        [DataPermission(Permission = PermissionKeys.SpecialReportData_Turnover)]
        public decimal Turnover { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_ValidTurnover)]
        public decimal NetTurnover { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_GGR)]
        public decimal Ggr { get; set; }

        public decimal Commission { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_PlayerWinLoss)]
        public decimal PlayerWinloss { get; set; }

        public int BetCount { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_AvgBetAmount)]
        public decimal AvgBetAmount { get; set; }

        public int UniquePlayers { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_Margin)]
        public double Margin { get; set; }

        public int NumberOfDeposit { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_TotalDepositAmount)]
        public decimal DepositAmount { get; set; }

        public int NumberOfWithdrawal { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_TotalWithdrawalAmount)]
        public decimal WithdrawalAmount { get; set; }

        public decimal BonusAmount { get; set; }

        public decimal ClosingBalance { get; set; }

        public decimal ProgressiveWin { get; set; }

        public decimal ProgressiveBet { get; set; }
    }
}