namespace Fanex.Athena.Report.Marketing.ViewModels
{
    public class PerformanceHighlightViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalDepositUsers { get; set; }

        public int TotalActiveUsers { get; set; }

        public int DepositUsers { get; set; }
        public double GrowthRateDepositUsers { get; set; }

        public int ActiveUsers { get; set; }
        public double GrowthRateActiveUsers { get; set; }

        public int NewlyRegisterUsers { get; set; }
        public double GrowthRateNewlyRegisterUsers { get; set; }

        public int NewlyActiveRegisterUsers { get; set; }
        public double GrowthRateNewlyActiveRegisterUsers { get; set; }

        public decimal DepositAmount { get; set; }
        public double GrowthRateDepositAmount { get; set; }

        public decimal WithdrawalAmount { get; set; }
        public double GrowthRateWithdrawalAmount { get; set; }

        public decimal BonusAmountClaimeds { get; set; }
        public double GrowthRateBonusAmountClaimeds { get; set; }

        public double ChurnRate { get; set; }
        public double DifferentChurnRate { get; set; }

        public double RetentionRate { get; set; }
        public double DifferentRetentionRate { get; set; }
    }
}