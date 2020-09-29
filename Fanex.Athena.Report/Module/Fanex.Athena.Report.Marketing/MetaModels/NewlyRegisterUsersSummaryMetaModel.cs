using Fanex.Data;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class NewlyRegisterUsersSummaryMetaModel : BaseMarketingReportMetaModel
    {
        public DateTime CompareFromDate { get; set; }
        public DateTime CompareToDate { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public override bool IsValid => base.IsValid &&
            CompareFromDate <= CompareToDate &&
            FromDate > CompareToDate;
    }
}