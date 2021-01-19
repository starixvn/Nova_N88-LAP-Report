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

        public int BonusType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int BonusStatus { get; set; }

        public decimal TotalClaimants { get; set; }

        public decimal TotalClaimedAmount { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalWithdrawal { get; set; }

        [SqlColumn(Ignored = true)]
        public string StartDateString { get { return StartDate.ToString("dd-MMM-yyyy"); } }

        [SqlColumn(Ignored = true)]
        public string EndDateString { get { return EndDate.ToString("dd-MMM-yyyy"); } }

        [SqlColumn(Ignored = true)]
        public string BonusStatusText
        {
            get { return BonusInfoDropdownOptions.BonusStatusList.Where(_ => _.Key == BonusStatus).Select(_ => _.Value).FirstOrDefault(); }
        }

        [SqlColumn(Ignored = true)]
        public string BonusTypeText
        {
            get { return BonusInfoDropdownOptions.BonusTypeList.Where(_ => _.Key == BonusType).Select(_ => _.Value).FirstOrDefault(); }
        }
    }

    public class BonusInfoSummaryTotalDTO : ClaimStatusDTO
    {
        public decimal TotalClaimants { get; set; }

        public decimal TotalClaimedAmount { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalWithdrawal { get; set; }
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