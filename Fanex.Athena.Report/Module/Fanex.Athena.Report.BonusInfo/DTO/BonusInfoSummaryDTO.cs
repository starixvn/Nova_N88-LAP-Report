using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Common.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.BonusInfo.DTO
{
    public class BonusInfoSummaryItemDTO : ClaimStatusDTO
    {
        public string BonusName { get; set; }

        public string BonusCode { get; set; }

        public int BonusTypeId { get; set; }
        public string BonusTypeName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int BonusStatus { get; set; }

        public decimal TotalClaimants { get; set; }

        public decimal TotalClaimedAmount { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalWithdrawal { get; set; }

        public decimal GGR { get; set; }

        public DateTime Date { get; set; }

        [SqlColumn(Ignored = true)]
        public string StartDateString => StartDate == DateTime.MinValue ? string.Empty : StartDate.ToString(DateTimeFormat.MonthAbbreviationsDate);

        [SqlColumn(Ignored = true)]
        public string EndDateString => EndDate == DateTime.MinValue ? string.Empty : EndDate.ToString(DateTimeFormat.MonthAbbreviationsDate);

        [SqlColumn(Ignored = true)]
        public string DateString => Date.ToString(DateTimeFormat.MonthAbbreviationsDate);

        [SqlColumn(Ignored = true)]
        public string BonusStatusText => BonusInfoDropdownOptions.BonusStatusList.FirstOrDefault(_ => _.Key == BonusStatus)?.Value;
    }

    public class BonusInfoSummaryTotalDTO : ClaimStatusDTO
    {
        public decimal TotalClaimants { get; set; }

        public decimal TotalClaimedAmount { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalWithdrawal { get; set; }

        public decimal GGR { get; set; }
    }

    public class BonusInfoSummaryDTO
    {
        public BonusInfoSummaryDTO()
        {
            SortedData = new SortableResultModel<BonusInfoSummaryItemDTO>();
            BonusCodeList = new List<string>();
        }

        public SortableResultModel<BonusInfoSummaryItemDTO> SortedData { get; set; }

        public IEnumerable<string> BonusCodeList { get; set; }
    }
}