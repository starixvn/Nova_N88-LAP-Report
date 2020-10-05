using Fanex.Athena.Report.Common.Attributes;
using Fanex.Athena.Report.Marketing.Consts;
using Newtonsoft.Json;
using System;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class BonusClaimedDetailDTO
    {
        public int RowId { get; set; }
        public string BonusCode { get; set; }
        public string Username { get; set; }

        [SqlColumn(ColumnName = "RegTime")]
        [JsonIgnore]
        public DateTime RegisteredDate { get; set; }

        [SqlColumn(Ignored = true)]
        public string RegisteredDateDisplay => RegisteredDate.ToString(DateTimeFormat.MonthName);

        public decimal DepositAmount { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public decimal BonusAmount { get; set; }
    }
}