namespace Fanex.Athena.Report.Marketing.ViewModels
{
    public class AvgRevenueViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal AllUsersRevenue { get; set; }
        public decimal PaidUsersRevenue { get; set; }
    }
}