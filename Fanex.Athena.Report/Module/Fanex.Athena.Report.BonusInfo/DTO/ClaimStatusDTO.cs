using Fanex.Athena.Report.Common.Attributes;

namespace Fanex.Athena.Report.BonusInfo.DTO
{
    public class ClaimStatusDTO
    {
        public int Running { get; set; }

        public int Canceled { get; set; }

        public int Fulfill { get; set; }

        public int OtherPlatform { get; set; }
    }
}