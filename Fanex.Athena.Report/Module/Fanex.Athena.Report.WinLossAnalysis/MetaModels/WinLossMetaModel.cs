using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Core.Utils;
using Fanex.Data;
using Fanex.NVC.Authentication.Infrastructure;
using System;
using System.Linq;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class WinLossMetaModel : ITimezoneMetaModel, IValidationMetaModel, IBrandIdListMetaModel
    {
        private const int GroupByCurrency = 4;
        private const int GroupByPlayer = 5;

        private const int DefaultPageSize = 500;
        private const int MaxPageSize = 2000;
        private static readonly int[] ListTimezoneValid = new[] { 8, -4, 0 };

        public WinLossMetaModel()
        {
            PageIndex = 1;
            PageSize = DefaultPageSize;
            UserNameFilterId = 1;
        }

        [SpParam(SqlParamName = "FilterBy")]
        public byte DateType { get; set; }

        public byte GroupBy { get; set; }

        public int Timezone { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "LicenseeIds")]
        public string BrandIds { get; set; }

        public byte UserNameFilterId { get; set; }
        public string UserName { get; set; }
        public bool IsIncludedTestData { get; set; }

        [SpParam(SqlParamName = "CurrencyIds")]
        public string CurrencyIdList { get; set; }

        public bool IsNoDrawTicket { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGroupByCurreny => GroupBy == GroupByCurrency;

        [SpParam(Ignored = true)]
        public bool IsGroupByPlayer => GroupBy == GroupByPlayer;

        [SpParam(Ignored = true)]
        public bool DrilldownInOriginalCurrency { get; set; }

        [SpParam(Ignored = true)]
#pragma warning disable S1067, S109, S1541 // Expressions should not be too complex, Magic number
        public bool IsValid => !string.IsNullOrWhiteSpace(ProductIds) && !string.IsNullOrWhiteSpace(BrandIds) && !string.IsNullOrWhiteSpace(BaseCurrency) &&
            GroupBy >= 1 && GroupBy <= 5 &&
            DateType >= 1 && DateType <= 2 &&
            UserNameFilterId >= 1 && UserNameFilterId <= 3 &&
            FromDate <= ToDate &&
            PageSize <= MaxPageSize &&
            ListTimezoneValid.Any(_ => _ == Timezone);
#pragma warning restore S1067, S109,S1541 // Expressions should not be too complex, Magic number
    }
}