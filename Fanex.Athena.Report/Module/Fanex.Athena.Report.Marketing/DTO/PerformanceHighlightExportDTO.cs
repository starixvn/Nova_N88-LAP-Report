using Fanex.Athena.Report.Common.Attributes;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class PerformanceHighlightCommonExportDTO
    {
        public string Name { get; set; }
        public string NameVN { get; set; }
        public decimal? DateRange { get; set; }
        public decimal? MonthToDate { get; set; }
        public decimal? AllRange { get; set; }
        public byte? DataType { get; set; }
    }

    public class PerformanceHighlightRegistrationExportDTO
    {
        public string SignupUrl { get; set; }
        public int SignupUsers { get; set; }

        [SqlColumn(ColumnName = "MTD_SignupUsers")]
        public int MonthToDateSignupUsers { get; set; }

        [SqlColumn(ColumnName = "Total_SignupUsers")]
        public int TotalSignupUsers { get; set; }

        public byte? DataType { get; set; }
    }

    public class PerformanceHighlightExportDTO
    {
        public IEnumerable<PerformanceHighlightCommonExportDTO> CommonData { get; set; }
        public IEnumerable<PerformanceHighlightRegistrationExportDTO> RegistrationData { get; set; }
    }
}