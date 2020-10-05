using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Marketing.Consts;
using Newtonsoft.Json;
using System;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class WithdrawalDetailDTO
    {
        public int RowId { get; set; }

        public string Username { get; set; }

        [SqlColumn(ColumnName = "RegTime")]
        [JsonIgnore]
        public DateTime RegisteredDate { get; set; }

        [SqlColumn(Ignored = true)]
        public string RegisteredDateDisplay => RegisteredDate.ToString(DateTimeFormat.MonthName);

        [SqlColumn(ColumnName = "BetCount")]
        public int NumberOfWithdrawals { get; set; }

        public decimal WithdrawalAmount { get; set; }
    }
}