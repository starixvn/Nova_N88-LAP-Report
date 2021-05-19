using Fanex.Data;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class ExportWinLossPlayerBetListMetaModel : WinLossPlayerBetListMetaModel
    {
        private const int MaxPageSize = 50000;

        public ExportWinLossPlayerBetListMetaModel()
        {
            PageIndex = 1;
            PageSize = MaxPageSize;
        }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [SpParam(Ignored = true)]
        public string SabaSystemId { get; set; }

        [SpParam(Ignored = true)]
        public string OriginalCurrency { get; set; }

        [SpParam(Ignored = true)]
        public string ProductName { get; set; }

        [SpParam(Ignored = true)]
        public int TotalRows { get; set; }
    }
}