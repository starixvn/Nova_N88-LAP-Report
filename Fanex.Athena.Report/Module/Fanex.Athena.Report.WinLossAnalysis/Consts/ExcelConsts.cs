using Fanex.Athena.Report.Resources;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.Report.WinLossAnalysis.Consts
{
    public static class ExcelConsts
    {
        public const string OctetStreamContentType = "application/octet-stream";

        public static readonly IReadOnlyDictionary<
           string,
           (string propertyName, string headerName, int colTemplate, bool hasTotal)> WinLossInfoColumns = new Dictionary<string, (string, string, int, bool)>(StringComparer.OrdinalIgnoreCase)
       {
            { "sabaId", ("SabaId", ViewResources.SabaId, 2, false) },
            { "sabaSystemId", ("SabaSystemId", ViewResources.SabaSystemId, 3, false) },
            { "turnover", ("Turnover", ViewResources.Turnover, 4, true) },
            { "netTurnover", ("NetTurnover", ViewResources.NetTurnover, 5, true) },
            { "ggr", ("Ggr", ViewResources.GGR, 6, true) },
            { "playerWinloss", ("PlayerWinloss", ViewResources.PlayerWinloss, 7, true) },
            { "progressiveWin", ("ProgressiveWin", ViewResources.ProgressiveWin, 8, true) },
            { "progressiveBet", ("ProgressiveBet", ViewResources.ProgressiveBet, 9, true) },
            { "commission", ("Commission", ViewResources.Commission, 10, true) },
            { "betCount", ("BetCount", ViewResources.BetCount, 11, true) },
            { "avgBetAmount", ("AvgBetAmount", ViewResources.AvgBetAmount, 12, true) },
            { "uniquePlayers", ("UniquePlayers", ViewResources.UniquePlayers, 13, true) },
            { "margin", ("Margin", ViewResources.Margin, 14, true) },
            { "closingBalance", ("ClosingBalance", ViewResources.ClosingBalance, 15, true) },
            { "numberOfDeposit", ("NumberOfDeposit", ViewResources.NoOfDeposit, 16, true) },
            { "depositAmount", ("DepositAmount", ViewResources.TotalDepositAmount, 17, true) },
            { "numberOfWithdrawal", ("NumberOfWithdrawal", ViewResources.NoOfWithdrawal, 18, true) },
            { "withdrawalAmount", ("WithdrawalAmount", ViewResources.TotalWithdrawalAmount, 19, true) },
            { "bonusAmount", ("BonusAmount", ViewResources.TotalBonus, 20, true) },
       };

        public static readonly IReadOnlyDictionary<
           string,
           (string propertyName, string headerName, int colTemplate, bool hasTotal)> WinLossDetailInfoColumns = new Dictionary<string, (string, string, int, bool)>(StringComparer.OrdinalIgnoreCase)
       {
            { "turnover", ("Turnover", ViewResources.Turnover, 8, true) },
            { "netTurnover", ("NetTurnover", ViewResources.NetTurnover, 9, true) },
            { "ggr", ("GGR", ViewResources.GGR, 10, true) },
            { "playerWinloss", ("PlayerWinloss", ViewResources.PlayerWinloss, 11, true) },
            { "progressiveWin", ("ProgressiveWin", ViewResources.ProgressiveWin, 12, true) },
            { "progressiveBet", ("ProgressiveBet", ViewResources.ProgressiveBet, 13, true) },
            { "commission", ("Commission", ViewResources.Commission, 14, true) },
            { "betCount", ("BetCount", ViewResources.BetCount, 15, true) },
            { "avgBetAmount", ("AvgBetAmount", ViewResources.AvgBetAmount, 16, true) },
            { "uniquePlayers", ("UniquePlayers", ViewResources.UniquePlayers, 17, true) },
            { "margin", ("Margin", ViewResources.Margin, 18, true) }
       };
    }
}