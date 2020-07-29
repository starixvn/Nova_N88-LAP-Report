using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightMetaModel : BaseReportMetaModel
    {
        public DateTime CompareFromDate { get; set; }
        public DateTime CompareToDate { get; set; }
    }
}