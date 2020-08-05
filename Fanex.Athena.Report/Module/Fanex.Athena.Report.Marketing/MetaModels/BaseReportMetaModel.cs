using Fanex.Athena.Report.Common.Model.MetaModels;
using System;

namespace Fanex.Athena.Report.Marketing.MetaModels
{
    public class BaseReportMetaModel : ILicenseeMetaModel, ITimezoneMetaModel
    {
        private const int DefaultTimezone = 8;

        public int LicenseeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Timezone { get; set; } = DefaultTimezone;
    }
}