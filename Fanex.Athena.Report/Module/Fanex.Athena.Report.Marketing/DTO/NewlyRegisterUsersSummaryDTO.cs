using Fanex.Athena.Report.Common.Attributes;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class TotalNewlyRegisterUsersDTO
    {
        public int NewlyRegisterUsers { get; set; }
        public int CompareNewlyRegisterUsers { get; set; }

        [SqlColumn(Ignored = true)]
        public double ChangeRate { get; set; }
    }

    public class CampaignNewlyRegisterUsersDTO
    {
        public string BonusCode { get; set; }

        public int NewlyRegisterUsers { get; set; }

        [SqlColumn(ColumnName = "Deposit")]
        public int DepositUsers { get; set; }

        public decimal FirstDepositAmount { get; set; }
    }

    public class SignupUrlNewlyRegisterUsersDTO
    {
        public string SignupUrl { get; set; }

        public int NewlyRegisterUsers { get; set; }

        [SqlColumn(ColumnName = "Deposit")]
        public int DepositUsers { get; set; }

        public decimal FirstDepositAmount { get; set; }
    }

    public class NewlyRegisterUsersSummaryDTO
    {
        public IEnumerable<TotalNewlyRegisterUsersDTO> TotalData { get; set; }
        public IEnumerable<CampaignNewlyRegisterUsersDTO> CampaignData { get; set; }
        public IEnumerable<SignupUrlNewlyRegisterUsersDTO> SignupData { get; set; }
    }
}