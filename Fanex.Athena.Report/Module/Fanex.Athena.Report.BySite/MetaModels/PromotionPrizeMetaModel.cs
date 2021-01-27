namespace Fanex.Athena.Report.BySite.MetaModels
{
    using Fanex.Athena.Report.Common.Model.DataDomain;
    using Fanex.Core.Utils;
    using Fanex.NVC.Authentication.Infrastructure;
    using System;

    public class PromotionPrizeMetaModel : IAgentMetaModel, IDataTestableMetaModel, ICurrencyIdMetaModel, IValidationMetaModel, IBrandIdListMetaModel
    {
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string UserName { get; set; }

        public string AgentListXml { get; set; }

        public bool IsExcludedTestData { get; set; }

        public int AgentId { get; set; }

        public string DefaultBaseCurrency { get; } = SystemBaseCurrency.EUR;

        public bool IsValid => FromDate <= ToDate;

        public string BrandIds { get; set; }
    }
}