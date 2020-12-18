using Fanex.Athena.Report.Common.Attributes;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class TotalDepositAmountSummaryDTO
    {
        public decimal DepositAmount { get; set; }
        public decimal CompareDepositAmount { get; set; }

        [SqlColumn(Ignored = true)]
        public double ChangeRate { get; set; }
    }

    public class SignupUrlDepositAmountDTO : ISignupDTO<SignupUrlDepositAmountDTO>
    {
        public string SignUpSource { get; set; }
        public string SignupUrl { get; set; }

        public decimal FirstDepositAmount { get; set; }
        public decimal DepositAmount { get; set; }

        [SqlColumn(Ignored = true)]
        public bool NoGrouping { get; set; }

        [SqlColumn(Ignored = true)]
        public IEnumerable<SignupUrlDepositAmountDTO> Childs { get; set; }
    }

    public class TotalSignupUrlDepositAmountDTO
    {
        public decimal TotalFirstDepositAmount { get; set; }

        public decimal TotalDepositAmount { get; set; }
    }

    public class DepositAmountSummaryDTO
    {
        public DepositAmountSummaryDTO()
        {
            TotalSignupData = new TotalSignupUrlDepositAmountDTO();
        }

        public IEnumerable<TotalDepositAmountSummaryDTO> TotalData { get; set; }
        public IEnumerable<SignupUrlDepositAmountDTO> SignupData { get; set; }
        public TotalSignupUrlDepositAmountDTO TotalSignupData { get; set; }
    }
}