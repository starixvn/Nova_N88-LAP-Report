using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Core.Utils;
using Fanex.Data;
using Fanex.NVC.Authentication.Infrastructure;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class BaseMarketingReportMetaModel : ITimezoneMetaModel, IBrandIdMetaModel, IValidationMetaModel
    {
        [SpParam(SqlParamName = "LicenseeId")]
        public int BrandId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "CurrencyIds")]
        public string CurrencyIdList { get; set; }

        public int Timezone { get; set; } = FilterConsts.DefaultTimezone;

        [SpParam(Ignored = true)]
        public virtual bool IsValid => BrandId > 0 && !string.IsNullOrWhiteSpace(CurrencyIdList) && FromDate <= ToDate &&
            ToDate.Subtract(FromDate).TotalDays <= FilterConsts.MaxRange;
    }
}