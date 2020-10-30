using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public interface IPerformanceHighlightExcel
    {
        IWorkbook Export(PerformanceHighlightExportDTO data, PerformanceHighlightExportAllMetaModel metaModel);
    }
}