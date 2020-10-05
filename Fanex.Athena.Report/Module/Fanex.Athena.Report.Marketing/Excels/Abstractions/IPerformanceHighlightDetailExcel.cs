using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.Marketing.Excels
{
#pragma warning disable S2436 // Types and methods should not have too many generic parameters

    public interface IPerformanceHighlightDetailExcel<in TMetaModel, in TSummaryData, in TDetailData>
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
    {
        IWorkbook Export(TSummaryData summaryData, TDetailData detailData, TMetaModel metaModel);
    }
}