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
            { "progressiveWin", ("ProgressiveWin", ViewResources.ProgressiveWin, 7, true) },
            { "progressiveBet", ("ProgressiveBet", ViewResources.ProgressiveBet, 8, true) },
            { "playerWinloss", ("PlayerWinloss", ViewResources.PlayerWinloss, 9, true) },
            { "betCount", ("BetCount", ViewResources.BetCount, 10, true) },
            { "avgBetAmount", ("AvgBetAmount", ViewResources.AvgBetAmount, 11, true) },
            { "uniquePlayers", ("UniquePlayers", ViewResources.UniquePlayers, 12, true) },
            { "margin", ("Margin", ViewResources.Margin, 13, true) },
            { "closingBalance", ("ClosingBalance", ViewResources.ClosingBalance, 14, true) },
            { "numberOfDeposit", ("NumberOfDeposit", ViewResources.NoOfDeposit, 15, true) },
            { "depositAmount", ("DepositAmount", ViewResources.TotalDepositAmount, 16, true) },
            { "numberOfWithdrawal", ("NumberOfWithdrawal", ViewResources.NoOfWithdrawal, 17, true) },
            { "withdrawalAmount", ("WithdrawalAmount", ViewResources.TotalWithdrawalAmount, 18, true) },
            { "bonusAmount", ("BonusAmount", ViewResources.TotalBonus, 19, true) },
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
            { "betCount", ("BetCount", ViewResources.BetCount, 14, true) },
            { "avgBetAmount", ("AvgBetAmount", ViewResources.AvgBetAmount, 15, true) },
            { "uniquePlayers", ("UniquePlayers", ViewResources.UniquePlayers, 16, true) },
            { "margin", ("Margin", ViewResources.Margin, 17, true) }
       };
    }
}