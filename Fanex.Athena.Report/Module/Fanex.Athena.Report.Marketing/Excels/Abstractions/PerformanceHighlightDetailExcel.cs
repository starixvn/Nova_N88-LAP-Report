using Fanex.Athena.Report.Common.Excel;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.Marketing.Excels
{
#pragma warning disable S2436 // Types and methods should not have too many generic parameters

    public abstract class PerformanceHighlightDetailExcel<TMetaModel, TSummaryData, TDetailData> : IPerformanceHighlightDetailExcel<TMetaModel, TSummaryData, TDetailData>
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
       where TMetaModel : class
       where TSummaryData : class
       where TDetailData : class
    {
        protected readonly IExcelBuilder _builder;
        protected readonly IFileProvider _fileProvider;

        protected PerformanceHighlightDetailExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(TSummaryData summaryData, TDetailData detailData, TMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);

            BuildSummarySheet(workBook, metaModel, summaryData);
            BuildDetailSheet(workBook, metaModel, detailData);

            return workBook;
        }

        protected abstract string TemplatePath { get; }

        protected abstract void BuildSummarySheet(IWorkbook workBook, TMetaModel metaModel, TSummaryData summaryData);

        protected abstract void BuildDetailSheet(IWorkbook workBook, TMetaModel metaModel, TDetailData detailData);

        protected abstract string BuildTitle(TMetaModel metaModel);

        protected void MergeCellContent(ISheet sheet, IRow row, int contentCol, int lastCol)
        {
            if (contentCol >= lastCol)
            {
                return;
            }

            ClearContent(row, contentCol + 1, lastCol);
            _builder.MergeCells(sheet, row.RowNum, lastRow: row.RowNum, firstCol: contentCol, lastCol: lastCol);
        }

        private static void ClearContent(IRow row, int firstCol, int lastCol)
        {
            for (int colIndex = firstCol; colIndex < lastCol; colIndex++)
            {
                row.Cells[colIndex].SetCellValue(string.Empty);
            }
        }
    }
}