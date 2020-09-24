using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public interface IPerformanceHighlightExcel
    {
        IWorkbook Export(IEnumerable<PerformanceHighlightExportDTO> data, PerformanceHighlightExportMetaModel metaModel);
    }
}