using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Athena.Report.Common.Model.MetaModels.Interfaces;
using Fanex.Core.Utils;
using Fanex.Data;
using System;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class WinLossPlayerDetailMetaModel : ITimezoneMetaModel, IValidationMetaModel, IDataRangeMetaModel
    {
        [SpParam(SqlParamName = "FilterBy")]
        public byte DateType { get; set; }

        public int Timezone { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }

        public int CustId { get; set; }

        public bool IsNoDrawTicket { get; set; }

        public bool IsIncludedFreeSpin { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public bool DrilldownInOriginalCurrency { get; set; }

        [SpParam(Ignored = true)]
        public string UsernameParent { get; set; }

        [SpParam(Ignored = true)]
        public string OriginalCurrency { get; set; }

        [SpParam(SqlParamName = "ReportSettingTypeId")]
        public int DataView { get; set; }

        [SpParam(Ignored = true)]
        public string DataViewName { get; set; }

#pragma warning disable S1067

        [SpParam(Ignored = true)]
        public bool IsValid => !string.IsNullOrWhiteSpace(ProductIds) && !string.IsNullOrWhiteSpace(BaseCurrency) &&
            FromDate <= ToDate;

#pragma warning restore S1067
    }
}