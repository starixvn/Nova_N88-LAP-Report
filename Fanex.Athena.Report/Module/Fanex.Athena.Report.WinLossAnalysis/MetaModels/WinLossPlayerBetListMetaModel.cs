using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Core.Utils;
using Fanex.Data;
using System;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class WinLossPlayerBetListMetaModel : ITimezoneMetaModel, IValidationMetaModel
    {
        private const int DefaultPageSize = 50;

        public WinLossPlayerBetListMetaModel()
        {
            PageIndex = 1;
            PageSize = DefaultPageSize;
        }

        [SpParam(SqlParamName = "FilterBy")]
        public byte DateType { get; set; }

        public int Timezone { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public bool IsNoDrawTicket { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public bool DrilldownInOriginalCurrency { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public byte SortbyId { get; set; }
        public bool IsSortAsc { get; set; }

#pragma warning disable S1067

        [SpParam(Ignored = true)]
        public bool IsValid => ProductId > 0 && !string.IsNullOrWhiteSpace(BaseCurrency) &&
            FromDate <= ToDate;
#pragma warning restore S1067
    }
}