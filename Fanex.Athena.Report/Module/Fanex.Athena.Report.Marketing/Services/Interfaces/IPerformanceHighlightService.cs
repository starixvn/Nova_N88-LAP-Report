using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public interface IPerformanceHighlightService
    {
        Task<PerformanceHighlightViewModel> GetData(PerformanceHighlightMetaModel metaModel);

        Task<IEnumerable<PerformanceHighlightExportDTO>> ExportData(PerformanceHighlightExportMetaModel metaModel);
    }
}