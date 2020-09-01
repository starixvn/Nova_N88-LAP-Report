using Fanex.Data;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class NewlyRegisterUsersExportMetaModel : BaseMarketingReportMetaModel
    {
        public DateTime CompareFromDate { get; set; }
        public DateTime CompareToDate { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public byte SortbyId { get; set; }
        public bool IsSortAsc { get; set; }
    }
}