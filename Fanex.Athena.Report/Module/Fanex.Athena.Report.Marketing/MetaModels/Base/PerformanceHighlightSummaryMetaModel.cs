using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Data;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class PerformanceHighlightSummaryMetaModel : BaseMarketingReportMetaModel
    {
        public DateTime CompareFromDate { get; set; }
        public DateTime CompareToDate { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

#pragma warning disable S1067 // Expressions should not be too complex

        public override bool IsValid => base.IsValid
             && !string.IsNullOrWhiteSpace(BaseCurrency)
            && CompareFromDate <= CompareToDate
            && FromDate > CompareToDate
            && CompareToDate.Subtract(CompareFromDate).TotalDays <= FilterConsts.MaxRange;

#pragma warning restore S1067 // Expressions should not be too complex
    }
}