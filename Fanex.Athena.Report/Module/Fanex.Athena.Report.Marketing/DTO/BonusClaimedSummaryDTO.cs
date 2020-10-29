using Fanex.Athena.Report.Common.Attributes;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.DTO
{
    public class TotalBonusClaimedDTO
    {
        public decimal BonusAmountClaimeds { get; set; }
        public decimal CompareBonusAmountClaimeds { get; set; }

        [SqlColumn(Ignored = true)]
        public double ChangeRate { get; set; }
    }

    public class DetailBonusClaimedDTO
    {
        public string BonusCode { get; set; }

        [SqlColumn(ColumnName = "BonusClaimeds")]
        public int NumberOfBonusClaimed { get; set; }

        public decimal BonusAmount { get; set; }
    }

    public class TotalDetailBonusClaimedDTO
    {
        public int TotalBonusClaimeds { get; set; }
        public decimal TotalBonusAmount { get; set; }
    }

    public class BonusClaimedSummaryDTO
    {
        public BonusClaimedSummaryDTO()
        {
            TotalDetailData = new TotalDetailBonusClaimedDTO();
        }

        public IEnumerable<TotalBonusClaimedDTO> TotalData { get; set; }
        public IEnumerable<DetailBonusClaimedDTO> DetailData { get; set; }
        public TotalDetailBonusClaimedDTO TotalDetailData { get; set; }
    }
}