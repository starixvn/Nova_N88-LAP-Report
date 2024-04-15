using System.Threading.Tasks;
using Fanex.Athena.Report.SabaWinLossAnalysis.DTOs;
using Fanex.Athena.Report.SabaWinLossAnalysis.MetaModels;
using Fanex.Athena.Report.SabaWinLossAnalysis.Models;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.Services.Interfaces
{
    public interface IWinlossAnalysisService
    {
        Task<WinlossAnalysisAggregateDTO> GetRawDataAsync(WinlossAnalysisMetaModel param);

        Task<WinlossAnalysisResultModel> GetDataAsync(WinlossAnalysisMetaModel param);
    }
}