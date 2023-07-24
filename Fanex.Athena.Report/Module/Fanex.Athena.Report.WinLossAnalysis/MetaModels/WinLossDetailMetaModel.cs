using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Athena.Report.Common.Model.MetaModels.Interfaces;
using Fanex.Core.Utils;
using Fanex.Data;
using Fanex.NVC.Authentication.Infrastructure;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class WinLossDetailMetaModel : ITimezoneMetaModel, IValidationMetaModel, IBrandIdListMetaModel, ICurrencyIdListMetaModel, IDataRangeMetaModel, IPlayerListMetaModel
    {
        [SpParam(SqlParamName = "FilterBy")]
        public byte DateType { get; set; }

        public byte GroupBy { get; set; }

        public int Timezone { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }

        [SpParam(SqlParamName = "LicenseeIds")]
        public string BrandIds { get; set; }

        public byte UserNameFilterId { get; set; } = 1;

        public string UserName { get; set; }

        public bool IsIncludedTestData { get; set; }

        public bool IsIncludedFreeSpin { get; set; }

        public int DataLimitBy { get; set; }

        public int IsDataLimitDesc { get; set; }

        public int DataLimitValue { get; set; }

        [SpParam(SqlParamName = "CurrencyIds")]
        public string CurrencyIdList { get; set; }

        public bool IsNoDrawTicket { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        [SpParam(SqlParamName = "IsDetailInOriginalCurr")]
        public bool DrilldownInOriginalCurrency { get; set; }

        [SpParam(Ignored = true)]
        public IEnumerable<string> OrderCols { get; set; }

#pragma warning disable S1067, S109

        [SpParam(Ignored = true)]
        public bool IsValid => !string.IsNullOrWhiteSpace(ProductIds) && !string.IsNullOrWhiteSpace(BrandIds) && !string.IsNullOrWhiteSpace(BaseCurrency) &&
            GroupBy >= 1 && GroupBy <= 2 &&
            DateType >= 1 && DateType <= 2 &&
            UserNameFilterId >= 1 && UserNameFilterId <= 3 &&
            FromDate <= ToDate;

#pragma warning restore S1067, S109
    }
}