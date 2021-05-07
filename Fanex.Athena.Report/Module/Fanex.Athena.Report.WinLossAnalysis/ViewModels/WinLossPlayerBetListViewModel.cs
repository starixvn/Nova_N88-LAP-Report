namespace Fanex.Athena.Report.WinLossAnalysis.ViewModels
{
    public class WinLossPlayerBetListViewModel
    {
        public int RowNumber { get; set; }
        public string RefNo { get; set; }
        public string TransTime { get; set; }
        public decimal Stake { get; set; }
        public decimal PlayerWinLoss { get; set; }
        public decimal GGR { get; set; }
        public string TicketStatus { get; set; }
    }
}