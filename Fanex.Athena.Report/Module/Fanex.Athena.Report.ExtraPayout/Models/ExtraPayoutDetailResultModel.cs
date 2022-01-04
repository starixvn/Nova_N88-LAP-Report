using Fanex.Athena.Report.ExtraPayout.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.Report.ExtraPayout.Models
{
    public class ExtraPayoutDetailResultModel
    {
        public List<ExtraPayoutDetailDTO> Data { get; set; }

        public ExtraPayoutDetailDTO TotalSummary { get; set; }

        public int TotalRows { get; set; }
    }
}