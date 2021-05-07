using Fanex.Data;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class ExportWinLossPlayerBetListMetaModel : WinLossPlayerBetListMetaModel
    {
        private const int MaxRowExport = 50000;

        public ExportWinLossPlayerBetListMetaModel()
        {
            PageIndex = 1;
            PageSize = MaxRowExport;
        }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [SpParam(Ignored = true)]
        public string SabaSystemId { get; set; }
    }
}