using Fanex.Data;

namespace Fanex.Athena.Report.BonusInfo.MetaModels
{
    public class BonusInfoDetailMetaModel : BaseBonusInfoReportMetaModel
    {
        private const int DefaultPageSize = 100;
        private const int MaxPageSize = 1000;

        public BonusInfoDetailMetaModel()
        {
            PageIndex = 1;
            PageSize = DefaultPageSize;
        }

        public string BonusCode { get; set; }

        public byte SortbyId { get; set; }

        public bool IsSortAsc { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public int GroupBy { get; set; }

        [SpParam(Ignored = true)]
        public override bool IsValid => PageSize <= MaxPageSize && base.IsValid;
    }
}