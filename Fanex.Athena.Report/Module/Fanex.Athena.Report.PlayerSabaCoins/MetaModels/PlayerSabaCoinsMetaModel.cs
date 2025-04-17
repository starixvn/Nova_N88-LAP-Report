using System;
using Fanex.Core.Utils;
using Fanex.NVC.Authentication.Infrastructure;

namespace Fanex.Athena.Report.PlayerSabaCoins.MetaModels
{
    public class PlayerSabaCoinsMetaModel : IAgentMetaModel, ICurrencyIdListMetaModel, IDataTestableMetaModel, IValidationMetaModel, IBrandIdMetaModel
    {
        private string _username;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CurrencyIdList { get; set; }

        public string AgentListXml { get; set; }

        public string SubsidiaryList { get; set; }

        public bool SubsidiarySearch { get; set; }

        public bool IsExcludedTestData { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string Username
        {
            get => string.IsNullOrEmpty(_username) ? string.Empty : _username?.Trim();
            set => _username = value;
        }

        public bool IsValid => true;

        public int BrandId { get; set; }
    }
}