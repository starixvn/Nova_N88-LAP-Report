using Fanex.Core.Utils;
using Fanex.NVC.Authentication.Infrastructure;
using System;

namespace Fanex.Athena.Report.ExtraPayout.MetaModels
{
    public class BaseWinLossMetaModel : IAgentMetaModel, IBrandIdMetaModel, IDataTestableMetaModel, ICurrencyIdListMetaModel
    {
        public int BrandId { get; set; }

        public int DrawFilter { get; set; }

        public byte ReportBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public string AgentListXml { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public bool IsExcludedTestData { get; set; }

        public string CurrencyIdList { get; set; }
    }
}