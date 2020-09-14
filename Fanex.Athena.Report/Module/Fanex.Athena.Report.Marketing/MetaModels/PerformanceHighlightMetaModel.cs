using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightMetaModel : BaseMarketingReportMetaModel
    {
        public DateTime CompareFromDate { get; set; }
        public DateTime CompareToDate { get; set; }
    }
}