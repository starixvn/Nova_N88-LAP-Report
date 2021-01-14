using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Data;
using System.Collections.Generic;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class ExportWinLossMetaModel : WinLossMetaModel, ISortOptionMetaModel
    {
        [SpParam(Ignored = true)]
        public string SortType { get; set; }

        [SpParam(Ignored = true)]
        public string SortBy { get; set; }

        [SpParam(Ignored = true)]
        public IEnumerable<string> OrderCols { get; set; }
    }
}