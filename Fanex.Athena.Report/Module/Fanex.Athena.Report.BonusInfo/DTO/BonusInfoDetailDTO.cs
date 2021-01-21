using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.BonusInfo.DTO
{
    public class BonusInfoDetailItemDTO
    {
        public string UserName { get; set; }

        public string BonusCode { get; set; }

        public DateTime ClaimedDate { get; set; }

        public decimal ClaimedAmount { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalWithdrawal { get; set; }

        public int Status { get; set; }

        public double PercentagedCompletion { get; set; }

        [SqlColumn(Ignored = true)]
        public string ClaimDateString => ClaimedDate.ToString("dd-MMM-yyyy");

        [SqlColumn(Ignored = true)]
        public string StatusText => BonusInfoDropdownOptions.ClaimStatusList.FirstOrDefault(_ => _.Key == Status)?.Value;
    }

    public class BonusInfoDetailDTO
    {
        public BonusInfoDetailDTO()
        {
            DataList = new List<BonusInfoDetailItemDTO>();
        }

        public IEnumerable<BonusInfoDetailItemDTO> DataList { get; set; }

        public int TotalRows { get; set; }
    }
}