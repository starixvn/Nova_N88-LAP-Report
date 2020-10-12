using Fanex.Data;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightExportMetaModel : BaseMarketingReportMetaModel
    {
        public DateTime CompareFromDate { get; set; }
        public DateTime CompareToDate { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public byte SortbyId { get; set; }
        public bool IsSortAsc { get; set; }

#pragma warning disable S1067, S109 // Expressions should not be too complex

        public override bool IsValid => base.IsValid
           && !string.IsNullOrWhiteSpace(BaseCurrency)
           && CompareFromDate <= CompareToDate
           && FromDate > CompareToDate
           && SortbyId >= 1 && SortbyId <= 6;

#pragma warning restore S1067, S109 // Expressions should not be too complex
    }
}