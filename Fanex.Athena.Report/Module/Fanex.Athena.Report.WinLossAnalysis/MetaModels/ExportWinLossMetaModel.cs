using Fanex.Data;
using System.Collections.Generic;

namespace Fanex.Athena.Report.WinLossAnalysis.MetaModels
{
    public class ExportWinLossMetaModel : WinLossMetaModel
    {
        [SpParam(Ignored = true)]
        public IEnumerable<string> OrderCols { get; set; }
    }
}