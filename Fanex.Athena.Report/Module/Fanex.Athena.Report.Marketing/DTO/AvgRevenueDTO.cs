using Fanex.Athena.Report.Common.Attributes;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class AvgRevenueDTO
    {
        public int ProductId { get; set; }
        public decimal Turnover { get; set; }

        [SqlColumn(ColumnName = "PaidUser_Turnover")]
        public decimal PaidUserTurnover { get; set; }
    }
}