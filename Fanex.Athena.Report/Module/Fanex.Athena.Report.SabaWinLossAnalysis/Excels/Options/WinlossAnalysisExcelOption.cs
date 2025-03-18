using System;
using Fanex.Athena.Report.SabaWinLossAnalysis.Enums;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.Excels.Options
{
    public class WinlossAnalysisExcelOption
    {
        public string CurrencyList { get; set; }

        public string BaseCurrency { get; set; }

        public string ProductCodes { get; set; }

        public string SearchUsername { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public WinlossAnalysisExportMode ExportExcelMode { get; set; }

        public GroupByMode GroupByMode { get; set; }

        public BreakdownMode BreakdownMode { get; set; }
    }
}