namespace Fanex.Athena.Report.ExtraPayout.DTO
{
    public class ExtraPayoutDTO : BaseExtraPayoutDTO
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public int PromotionType { get; set; }

        public string PromotionName { get; set; }
    }
}