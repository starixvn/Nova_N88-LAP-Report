using System;
using Fanex.Athena.Report.SabaWinLossAnalysis.Enums;
using Fanex.Core.Utils;
using Fanex.NVC.Authentication.Infrastructure;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.MetaModels
{
    public class WinlossAnalysisMetaModel : IAgentMetaModel, ICurrencyIdListMetaModel, IDataTestableMetaModel, IValidationMetaModel, IBrandIdListMetaModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MovebetDay { get; set; }

        public GroupByMode GroupMode { get; set; }

        public BreakdownMode BreakdownMode { get; set; }

        public string ProductCodes { get; set; }

        public string SportIds { get; set; }

        public string BetTypeIds { get; set; }

        public string PlatformIds { get; set; }

        public string StatusIds { get; set; }

        public string LiveStatusIds { get; set; }

        public string BaseCurrency { get; set; }

        public string BaseCurrencyName { get; set; }

        public bool IsVipOnly { get; set; }

        public string CustomerClass { get; set; }

        public string FullCustomerClass { get; set; }

        public string OddsGroup { get; set; }

        public string SearchUserName { get; set; }

        public int SearchMode { get; set; }

        public bool IsExcludedTestData { get; set; }

        public int CashOutMode { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string AgentListXml { get; set; }

        public string CurrencyIdList { get; set; }

        public string CurrencyCodes { get; set; }

        public WinlossAnalysisExportMode ExportExcelMode { get; set; }

        public int SortBy { get; set; }

        public int SortOrder { get; set; }

        public bool IsShowInOrginalCurrency { get; set; }

        public int SpecialPlatform { get; set; }

        public int SpecialPlatformType { get; set; }

        public bool IsValid => true;

        public string BrandIds { get; set; }
    }
}