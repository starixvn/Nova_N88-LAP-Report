namespace Fanex.Athena.Report.Marketing.DTO
{
    public class PerformanceHighlightDTO
    {
        public int TotalUsers { get; set; }
        public int UptoTotalUsers { get; set; }
        public int CompareUptoTotalUsers { get; set; }

        public int TotalDepositUsers { get; set; }
        public int TotalActiveUsers { get; set; }

        public int DepositUsers { get; set; }
        public int CompareDepositUsers { get; set; }

        public int NewlyRegisterUsers { get; set; }
        public int CompareNewlyRegisterUsers { get; set; }

        public int NewlyActiveRegisterUsers { get; set; }
        public int CompareNewlyActiveRegisterUsers { get; set; }

        public int ActiveUsers { get; set; }
        public int CompareActiveUsers { get; set; }

        public decimal DepositAmount { get; set; }
        public decimal CompareDepositAmount { get; set; }

        public decimal WithdrawalAmount { get; set; }
        public decimal CompareWithdrawalAmount { get; set; }

        public decimal BonusAmountClaimeds { get; set; }
        public decimal CompareBonusAmountClaimeds { get; set; }
    }
}