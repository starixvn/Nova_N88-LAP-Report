using Fanex.Athena.Report.Common.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class TotalBonusClaimedDTO
    {
        [SqlColumn(ColumnName = "Currency")]
        public int CurrencyId { get; set; }

        public decimal BonusAmountClaimeds { get; set; }

        [JsonIgnore]
        public decimal BasedBonusAmountClaimeds { get; set; }

        public decimal CompareBonusAmountClaimeds { get; set; }

        [JsonIgnore]
        public decimal CompareBasedBonusAmountClaimeds { get; set; }

        [SqlColumn(Ignored = true)]
        public double ChangeRate { get; set; }

        [SqlColumn(Ignored = true)]
        public string CurrencyCode { get; set; }
    }

    public class DetailBonusClaimedDTO
    {
        public string BonusCode { get; set; }

        [SqlColumn(ColumnName = "Currency")]
        public int CurrencyId { get; set; }

        public string BonusName { get; set; }

        [SqlColumn(ColumnName = "BonusUsers")]
        public int NumberOfBonusUser { get; set; }

        public decimal BonusAmount { get; set; }
        public decimal BasedBonusAmount { get; set; }


        public IEnumerable<DetailBonusClaimedDTO> Childs { get; set; }


        [SqlColumn(Ignored = true)]
        public string Id => $"{BonusCode}_{CurrencyCode}";

        [SqlColumn(Ignored = true)]
        public string CurrencyCode { get; set; }
    }

    public class TotalDetailBonusClaimedDTO
    {
        public int TotalBonusUsers { get; set; }
        public decimal TotalBonusAmount { get; set; }
    }

    public class BonusClaimedSummaryDTO
    {
        public BonusClaimedSummaryDTO()
        {
            TotalDetailData = new TotalDetailBonusClaimedDTO();
            TotalTotalData = new TotalBonusClaimedDTO();
        }

        public IEnumerable<TotalBonusClaimedDTO> TotalData { get; set; }
        public TotalBonusClaimedDTO TotalTotalData { get; set; }

        public IEnumerable<DetailBonusClaimedDTO> DetailData { get; set; }
        public TotalDetailBonusClaimedDTO TotalDetailData { get; set; }
    }
}