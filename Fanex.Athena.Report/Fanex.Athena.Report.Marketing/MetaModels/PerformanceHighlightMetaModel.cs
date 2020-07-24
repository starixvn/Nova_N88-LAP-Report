using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightMetaModel
    {
        public int LicenseeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public DateTime CompareFromDate { get; set; }
        public DateTime CompareToDate { get; set; }
    }
}