using System.Threading.Tasks;
using Fanex.Athena.Report.SabaWinLossAnalysis.DTOs;
using Fanex.Athena.Report.SabaWinLossAnalysis.MetaModels;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.Repositories.Interfaces
{
    public interface IWinlossAnalysisRepository
    {
        Task<WinlossAnalysisAggregateDTO> GetDataByAgencyCriteria(WinlossAnalysisMetaModel metaModel);

        Task<WinlossAnalysisAggregateDTO> GetDataByPlayerCriteria(WinlossAnalysisMetaModel metaModel);
    }
}