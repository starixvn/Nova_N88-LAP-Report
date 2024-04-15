using System.Collections.Generic;
using Fanex.Athena.Report.SabaWinLossAnalysis.DTOs;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.Models
{
    public class WinlossAnalysisResultModel
    {
        public int TotalRow { get; set; }

        public IEnumerable<TreeViewItem<WinlossAnalysisViewModel, WinlossAnalysisViewModel>> Data { get; set; }

        public WinlossAnalysisViewModel SumTotal { get; set; }
    }
}