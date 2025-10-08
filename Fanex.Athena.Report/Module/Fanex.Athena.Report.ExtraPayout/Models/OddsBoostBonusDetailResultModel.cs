using System.Collections.Generic;
using Fanex.Athena.Report.ExtraPayout.DTO;

namespace Fanex.Athena.Report.ExtraPayout.Models
{
    public class OddsBoostBonusDetailResultModel
    {
        public List<OddsBoostBonusDetail> Data { get; set; }

        public OddsBoostBonusDetail TotalSummary { get; set; }
    }
}
