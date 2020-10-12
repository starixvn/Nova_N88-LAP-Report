using Fanex.Athena.Report.Common.Attributes;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class WithdrawalSummaryDTO
    {
        public decimal WithdrawalAmount { get; set; }
        public decimal CompareWithdrawalAmount { get; set; }
        public int WithdrawalUsers { get; set; }
        public int CompareWithdrawalUsers { get; set; }

        [SqlColumn(Ignored = true)]
        public double WithdrawalAmountChangeRate { get; set; }

        [SqlColumn(Ignored = true)]
        public double WithdrawalUsersChangeRate { get; set; }
    }
}