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

    public class SignupUrlNewlyRegisterUsersDTO : ISignupDTO<SignupUrlNewlyRegisterUsersDTO>
    {
        public string SignUpSource { get; set; }
        public string SignupUrl { get; set; }

        public int NewlyRegisterUsers { get; set; }

        [SqlColumn(ColumnName = "Deposit")]
        public int DepositUsers { get; set; }

        public decimal FirstDepositAmount { get; set; }

        [SqlColumn(Ignored = true)]
        public bool NoGrouping { get; set; }

        [SqlColumn(Ignored = true)]
        public IEnumerable<SignupUrlNewlyRegisterUsersDTO> Childs { get; set; }
    }

    public class TotalSignupUrlNewlyRegisterUsersDTO
    {
        public int TotalNewlyRegisterUsers { get; set; }

        public int TotalDepositUsers { get; set; }

        public decimal TotalFirstDepositAmount { get; set; }
    }

    public class NewlyRegisterUsersSummaryDTO
    {
        public NewlyRegisterUsersSummaryDTO()
        {
            TotalSignupData = new TotalSignupUrlNewlyRegisterUsersDTO();
        }

        public IEnumerable<TotalNewlyRegisterUsersDTO> TotalData { get; set; }
        public IEnumerable<CampaignNewlyRegisterUsersDTO> CampaignData { get; set; }
        public IEnumerable<SignupUrlNewlyRegisterUsersDTO> SignupData { get; set; }
        public TotalSignupUrlNewlyRegisterUsersDTO TotalSignupData { get; set; }
    }
}