using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public interface IPerformanceHighlightRepository
    {
        Task<PerformanceHighlightDTO> GetData(PerformanceHighlightMetaModel metaModel);

        Task<PerformanceHighlightExportDTO> ExportData(PerformanceHighlightExportAllMetaModel metaModel);
    }
}