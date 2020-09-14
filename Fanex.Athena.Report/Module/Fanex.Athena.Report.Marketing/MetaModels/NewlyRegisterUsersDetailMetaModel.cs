using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class NewlyRegisterUsersDetailMetaModel : BaseMarketingReportMetaModel
    {
        public byte SortbyId { get; set; }
        public bool IsSortAsc { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }
    }
}