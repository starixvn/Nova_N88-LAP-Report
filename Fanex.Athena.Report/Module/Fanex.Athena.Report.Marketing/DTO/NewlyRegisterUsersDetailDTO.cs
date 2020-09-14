using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Marketing.Consts;
using Newtonsoft.Json;
using System;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class NewlyRegisterUsersDetailDTO
    {
        public int RowId { get; set; }

        public string BonusCode { get; set; }

        public string Username { get; set; }

        [SqlColumn(ColumnName = "RegTime")]
        [JsonIgnore]
        public DateTime RegisteredDate { get; set; }

        [SqlColumn(Ignored = true)]
        public string RegisteredDateDisplay => RegisteredDate.ToString(DateTimeFormat.MonthName);

        public string SignupUrl { get; set; }
        public bool IsDeposit { get; set; }
        public decimal FirstDepositAmount { get; set; }
    }
}