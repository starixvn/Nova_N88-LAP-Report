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
            { "betCount", ("BetCount", ViewResources.BetCount, 8, true) },
            { "avgBetAmount", ("AvgBetAmount", ViewResources.AvgBetAmount, 9, true) },
            { "uniquePlayers", ("UniquePlayers", ViewResources.UniquePlayers, 10, true) },
            { "margin", ("Margin", ViewResources.Margin, 11, true) },
            { "numberOfDeposit", ("NumberOfDeposit", ViewResources.NoOfDeposit, 12, true) },
            { "depositAmount", ("DepositAmount", ViewResources.TotalDepositAmount, 13, true) },
            { "numberOfWithdrawal", ("NumberOfWithdrawal", ViewResources.NoOfWithdrawal, 14, true) },
            { "withdrawalAmount", ("WithdrawalAmount", ViewResources.TotalWithdrawalAmount, 15, true) },
       };

        public static readonly IReadOnlyDictionary<
           string,
           (string propertyName, string headerName, int colTemplate, bool hasTotal)> WinLossDetailInfoColumns = new Dictionary<string, (string, string, int, bool)>(StringComparer.OrdinalIgnoreCase)
       {
            { "turnover", ("Turnover", ViewResources.Turnover, 8, true) },
            { "netTurnover", ("NetTurnover", ViewResources.NetTurnover, 9, true) },
            { "ggr", ("GGR", ViewResources.GGR, 10, true) },
            { "playerWinloss", ("PlayerWinloss", ViewResources.PlayerWinloss, 11, true) },
            { "betCount", ("BetCount", ViewResources.BetCount, 12, true) },
            { "avgBetAmount", ("AvgBetAmount", ViewResources.AvgBetAmount, 13, true) },
            { "uniquePlayers", ("UniquePlayers", ViewResources.UniquePlayers, 14, true) },
            { "margin", ("Margin", ViewResources.Margin, 15, true) },
            { "numberOfDeposit", ("NumberOfDeposit", ViewResources.NoOfDeposit, 16, true) },
            { "depositAmount", ("DepositAmount", ViewResources.TotalDepositAmount, 17, true) },
            { "numberOfWithdrawal", ("NumberOfWithdrawal", ViewResources.NoOfWithdrawal, 18, true) },
            { "withdrawalAmount", ("WithdrawalAmount", ViewResources.TotalWithdrawalAmount, 19, true) },
       };
    }
}