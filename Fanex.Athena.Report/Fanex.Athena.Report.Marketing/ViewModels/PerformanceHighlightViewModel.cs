namespace Fanex.Athena.Report.Marketing.ViewModels
{
    public class PerformanceHighlightViewModel
    {
        public int TotalUsers { get; set; }
        public double GrowthRateTotalUsers { get; set; }

        public int PaidUsers { get; set; }
        public double GrowthRatePaidUsers { get; set; }

        public int NewlyRegisterUsers { get; set; }
        public double GrowthRateNewlyRegisterUsers { get; set; }

        public int ActiveUsers { get; set; }
        public double GrowthRateActiveUsers { get; set; }

        public double ChurnRate { get; set; }
        public double DifferentChurnRate { get; set; }

        public double RetentionRate { get; set; }
        public double DifferentRetentionRate { get; set; }
    }
}