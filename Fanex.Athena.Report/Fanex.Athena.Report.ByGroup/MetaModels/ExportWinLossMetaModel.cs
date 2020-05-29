using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.Report.ByGroup.MetaModels
{
    public class ExportWinLossMetaModel : WinLossMetaModel, ISortOptionMetaModel
    {
        [SpParam(Ignored = true)]
        public string SortType { get; set; }

        [SpParam(Ignored = true)]
        public string SortBy { get; set; }
    }
}