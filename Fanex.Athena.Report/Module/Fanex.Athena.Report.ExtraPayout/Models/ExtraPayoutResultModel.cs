using Fanex.Athena.Report.ExtraPayout.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.Report.ExtraPayout.Models
{
    public class ExtraPayoutResultModel
    {
        public List<ExtraPayoutDTO> Data { get; set; }

        public List<ExtraPayoutDTO> TotalCurrency { get; set; }

        public ExtraPayoutDTO TotalSummary { get; set; }

        public int TotalRows { get; set; }
    }
}