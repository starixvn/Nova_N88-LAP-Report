using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Data;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class BaseReportMetaModel : ITimezoneMetaModel
    {
        private const int DefaultTimezone = 8;

        [SpParam(SqlParamName = "LicenseeId")]
        public int BrandId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Timezone { get; set; } = DefaultTimezone;
    }
}