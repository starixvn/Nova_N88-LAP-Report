using Fanex.Data;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class NewlyRegisterUsersDetailMetaModel : BaseMarketingReportMetaModel
    {
        private const int DefaultPageSize = 500;
        private const int MaxPageSize = 2000;

        public NewlyRegisterUsersDetailMetaModel()
        {
            PageIndex = 1;
            PageSize = DefaultPageSize;
        }

        public byte SortbyId { get; set; }
        public bool IsSortAsc { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

#pragma warning disable S1067, S109 // Expressions should not be too complex

        public override bool IsValid => base.IsValid
            && SortbyId >= 1 && SortbyId <= 6
           && PageIndex >= 1 && PageSize <= MaxPageSize;

#pragma warning restore S1067, S109 // Expressions should not be too complex
    }
}