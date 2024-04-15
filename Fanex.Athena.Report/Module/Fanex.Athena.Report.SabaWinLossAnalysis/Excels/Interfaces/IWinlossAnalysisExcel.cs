using Fanex.Athena.Report.SabaWinLossAnalysis.Excels.Options;
using Fanex.Athena.Report.SabaWinLossAnalysis.Models;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.Excels.Interfaces
{
    public interface IWinlossAnalysisExcel
    {
        byte[] Export(string templatePath, WinlossAnalysisResultModel data, WinlossAnalysisExcelOption option);
    }
}