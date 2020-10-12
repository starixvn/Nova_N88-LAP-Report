using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Core.Utils;
using Fanex.Data;
using Fanex.NVC.Authentication.Infrastructure;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class BaseMarketingReportMetaModel : ITimezoneMetaModel, IBrandIdMetaModel, IValidationMetaModel
    {
        private const int DefaultTimezone = 8;

        [SpParam(SqlParamName = "LicenseeId")]
        public int BrandId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Timezone { get; set; } = DefaultTimezone;

        [SpParam(Ignored = true)]
        public virtual bool IsValid => BrandId > 0 && FromDate <= ToDate;
    }
}