namespace Fanex.Athena.Report.ExtraPayout.DTO
{
    public class ExtraPayoutDetailDTO : BaseExtraPayoutDTO
    {
        public string Refno { get; set; }

        public int PromotionType { get; set; }

        public string PromotionName { get; set; }
    }
}