using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Helpers;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public class PerformanceHighlightExcel : IPerformanceHighlightExcel
    {
        private const string TemplatePath = "/ExcelTemplate/Marketing/PerformanceHighlight.xls";

        private const string TemplateSheetName = "Template";
        private const string SheetName = "Data";

        private const int RowTitleTemplate = 0;
        private const int RowHeaderTemplate = 1;
        private const int RowContentTemplate = 2;
        private const int TotalColumn = 3;
        private const int ColumnName = 0;
        private const int ColumnRange = 1;
        private const int ColumnAllTime = 2;

        private readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public PerformanceHighlightExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(IEnumerable<PerformanceHighlightExportDTO> data, PerformanceHighlightExportMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);
            var sheet = _builder.CloneSheet(workBook, templateSheet.SheetName, SheetName);

            AssignTitle(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet, metaModel);
            AssignData(sheet, templateSheet, data);

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, PerformanceHighlightExportMetaModel metaModel)
        {
            var title = $"{ViewResources.Marketing} - {ViewResources.BusinessPerformanceHighlight}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: TotalColumn, value: title);
            _builder.ClearTemplate(sheet, RowTitleTemplate + 1);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet, PerformanceHighlightExportMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate);
            var rangeHeaer = $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}";

            row.Cells[ColumnName].SetCellValue(string.Empty);
            row.Cells[ColumnRange].SetCellValue(rangeHeaer);
            row.Cells[ColumnAllTime].SetCellValue(ViewResources.AllTime);
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<PerformanceHighlightExportDTO> data)
        {
            var currentRowIndex = RowContentTemplate;
            foreach (var rowData in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, rowData);
            }
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int currentRowIndex, PerformanceHighlightExportDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, RowContentTemplate, currentRowIndex);
            row.Cells[ColumnName].SetCellValue(rowData.Name);
            SetValueDecimalCell(row.Cells[ColumnRange], rowData.DateRange, rowData.DataType);
            SetValueDecimalCell(row.Cells[ColumnAllTime], rowData.AllRange, rowData.DataType);
        }

        private static void SetValueDecimalCell(ICell cell, decimal? value, byte? dataType)
        {
            var numberFormat = NumberHepler.Format(value);
            if (!string.IsNullOrEmpty(numberFormat) && dataType == DbColumnDataType.Percent)
            {
                numberFormat += "%";
            }

            cell.SetCellValue(numberFormat);
        }
    }
}