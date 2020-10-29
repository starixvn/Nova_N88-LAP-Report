namespace Fanex.Athena.Report.Marketing.DTO
{
    public class PerformanceHighlightExportDTO
    {
        public string Name { get; set; }
        public string NameVN { get; set; }
        public decimal? DateRange { get; set; }
        public decimal? MonthToDate { get; set; }
        public decimal? AllRange { get; set; }
        public byte? DataType { get; set; }
    }
}