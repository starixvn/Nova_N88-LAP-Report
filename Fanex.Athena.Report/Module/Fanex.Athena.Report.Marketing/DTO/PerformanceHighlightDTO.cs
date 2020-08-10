namespace Fanex.Athena.Report.Marketing.DTO
{
    public class PerformanceHighlightDTO
    {
        public int TotalUsers { get; set; }
        public int CompareTotalUsers { get; set; }

        public int PaidUsers { get; set; }
        public int ComparePaidUsers { get; set; }

        public int NewlyRegisterUsers { get; set; }
        public int CompareNewlyRegisterUsers { get; set; }

        public int ActiveUsers { get; set; }
        public int CompareActiveUsers { get; set; }
    }
}