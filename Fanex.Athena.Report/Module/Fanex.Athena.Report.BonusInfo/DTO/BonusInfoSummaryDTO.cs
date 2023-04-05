using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Common.Consts;
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

        [DataPermission(Permission = PermissionKeys.SpecialReportData_TotalDepositAmount)]
        public decimal TotalDeposit { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_TotalWithdrawalAmount)]
        public decimal TotalWithdrawal { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_GGR)]
        public decimal GGR { get; set; }

        public DateTime Date { get; set; }

        [SqlColumn(Ignored = true)]
        public string StartDateString => StartDate == DateTime.MinValue ? string.Empty : StartDate.ToString(DateTimeFormat.MonthAbbreviationsDate);

        [SqlColumn(Ignored = true)]
        public string EndDateString => EndDate == DateTime.MinValue ? string.Empty : EndDate.ToString(DateTimeFormat.MonthAbbreviationsDate);

        [SqlColumn(Ignored = true)]
        public string DateString => Date.ToString(DateTimeFormat.MonthAbbreviationsDate);

        [SqlColumn(Ignored = true)]
        public string MonthString => Date.ToString(DateTimeFormat.MonthAbbreviations);

        [SqlColumn(Ignored = true)]
        public string BonusStatusText => BonusInfoDropdownOptions.BonusStatusList.FirstOrDefault(_ => _.Key == BonusStatus)?.Value;
    }

    public class BonusInfoSummaryTotalDTO : ClaimStatusDTO
    {
        public decimal TotalClaimants { get; set; }

        public decimal TotalClaimedAmount { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_TotalDepositAmount)]
        public decimal TotalDeposit { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_TotalWithdrawalAmount)]
        public decimal TotalWithdrawal { get; set; }

        [DataPermission(Permission = PermissionKeys.SpecialReportData_GGR)]
        public decimal GGR { get; set; }
    }

    public class BonusInfoSummaryDTO
    {
        public BonusInfoSummaryDTO()
        {
            SortedData = new SortableResultModel<BonusInfoSummaryItemDTO>();
            BonusCodeList = new List<string>();
            BonusCodeInfoDetailList = new List<BonusCodeInfoDetailDTO>();
        }

        public SortableResultModel<BonusInfoSummaryItemDTO> SortedData { get; set; }

        public IEnumerable<string> BonusCodeList { get; set; }

        public IEnumerable<BonusCodeInfoDetailDTO> BonusCodeInfoDetailList { get; set; }
    }

    public class BonusCodeInfoDetailDTO
    {
        public DateTime Date { get; set; }

        public string BonusCode { get; set; }
    }
}