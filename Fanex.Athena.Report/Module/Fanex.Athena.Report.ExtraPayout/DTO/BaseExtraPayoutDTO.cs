using System;

namespace Fanex.Athena.Report.ExtraPayout.DTO
{
    public class BaseExtraPayoutDTO
    {
        public DateTime Date { get; set; }

        public string DateString { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal Amount { get; set; }

        public decimal SharingAmount { get; set; }

        public decimal BasedTurnover { get; set; }

        public decimal BasedAmount { get; set; }

        public decimal BasedSharingAmount { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }
    }
}