using System;
using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Athena.Report.Common.Model.MetaModels.Interfaces;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Core.Utils;
using Fanex.Data;
using Fanex.NVC.Authentication.Infrastructure;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class WinLossMetaModel : ITimezoneMetaModel, IValidationMetaModel, IBrandIdListMetaModel, ICurrencyIdListMetaModel, IDataRangeMetaModel, IPlayerListMetaModel
    {
        private const int DefaultPageSize = 500;
        private const int MaxPageSize = 2000;

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

        public int DataLimitBy { get; set; }

        public int IsDataLimitDesc { get; set; }

        public int DataLimitValue { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "LicenseeIds")]
        public string BrandIds { get; set; }

        public byte UserNameFilterId { get; set; }

        public string UserName { get; set; }

        public bool IsIncludedTestData { get; set; }

        public bool IsIncludedFreeSpin { get; set; }

        [SpParam(SqlParamName = "CurrencyIds")]
        public string CurrencyIdList { get; set; }

        public bool IsNoDrawTicket { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGroupByCurreny => GroupBy == GroupByConsts.Currency;

        [SpParam(Ignored = true)]
        public bool IsGroupByPlayer => GroupBy == GroupByConsts.Player;

        [SpParam(Ignored = true)]
        public bool DrilldownInOriginalCurrency { get; set; }

        public byte SortbyId { get; set; }

        public bool IsSortAsc { get; set; }

        [SpParam(SqlParamName = "ReportSettingTypeId")]
        public int DataView { get; set; }

        [SpParam(Ignored = true)]
        public string DataViewName { get; set; }

#pragma warning disable S1067, S109, S1541

        [SpParam(Ignored = true)]
        public bool IsValid => !string.IsNullOrWhiteSpace(ProductIds) && !string.IsNullOrWhiteSpace(BrandIds) && !string.IsNullOrWhiteSpace(BaseCurrency) &&
            GroupBy >= 1 && GroupBy <= 6 &&
            DateType >= 1 && DateType <= 2 &&
            UserNameFilterId >= 1 && UserNameFilterId <= 3 &&
            FromDate <= ToDate &&
            PageSize <= MaxPageSize;

#pragma warning restore S1067, S109, S1541
    }
}