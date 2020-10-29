using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Excel;
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
        private const int RowContentIntegerTemplate = 2;
        private const int RowContentMoneyTemplate = 3;
        private const int RowContentPercentTemplate = 4;

        private const int TotalColumn = 4;
        private const int ColumnName = 0;
        private const int ColumnRange = 1;
        private const int ColumnMonthToDate = 2;
        private const int ColumnAllTime = 3;

        private static IReadOnlyDictionary<int, int> TemplateRows = new Dictionary<int, int>
        {
            { DbColumnDataType.Integer, RowContentIntegerTemplate },
            { DbColumnDataType.Money, RowContentMoneyTemplate },
            { DbColumnDataType.Percent, RowContentPercentTemplate }
        };

        private bool _isModeVN;
        private IWorkbook _workBook;

        private readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public PerformanceHighlightExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
            _isModeVN = false;
        }

        public IWorkbook Export(IEnumerable<PerformanceHighlightExportDTO> data, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            _workBook = _builder.LoadTemplate(template);
            var templateSheet = _workBook.GetSheet(TemplateSheetName);

            BuildSheetEng(templateSheet, data, metaModel);
            BuildSheetVn(templateSheet, data, metaModel);

            _builder.RemoveSheet(_workBook, TemplateSheetName);

            return _workBook;
        }

        private void BuildSheetEng(ISheet templateSheet, IEnumerable<PerformanceHighlightExportDTO> data, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var sheet = CloneSheet();

            AssignTitle(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet, metaModel);
            AssignData(sheet, templateSheet, data);
        }

        private void BuildSheetVn(ISheet templateSheet, IEnumerable<PerformanceHighlightExportDTO> data, PerformanceHighlightExportAllMetaModel metaModel)
        {
            SwitchToModeVN();

            var sheet = CloneSheet();

            AssignTitle(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet, metaModel);
            AssignData(sheet, templateSheet, data);

            _builder.RemoveSheet(_workBook, TemplateSheetName);
        }

        private ISheet CloneSheet()
        {
            var suffixSheetName = _isModeVN ? "Vn" : "Eng";
            var sheet = _builder.CloneSheet(_workBook, TemplateSheetName, SheetName + suffixSheetName);
            return sheet;
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var title = $"{ViewResources.Marketing} - {ViewResources.BusinessPerformanceHighlight}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: TotalColumn, value: title);
            _builder.ClearTemplate(sheet, RowTitleTemplate + 1);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate);

            var rangeHeaer = $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}";
            var launchDateHeader = metaModel.BrandId == Sites.N88SiteId ? Sites.LaunchDate[metaModel.BrandId] : string.Format(ViewResources.FromLaunchDate, Sites.LaunchDate[metaModel.BrandId]);

            row.Cells[ColumnName].SetCellValue(string.Empty);
            row.Cells[ColumnRange].SetCellValue(rangeHeaer);
            row.Cells[ColumnMonthToDate].SetCellValue(ViewResources.MonthToDate);
            row.Cells[ColumnAllTime].SetCellValue(launchDateHeader);
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<PerformanceHighlightExportDTO> data)
        {
            var currentRowIndex = RowContentIntegerTemplate;
            foreach (var rowData in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, rowData);
            }
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int currentRowIndex, PerformanceHighlightExportDTO rowData)
        {
            var rowTemplateIndex = GetRowTemplateIndex(rowData.DataType);
            var row = _builder.CopyRow(sheet, templateSheet, rowTemplateIndex, currentRowIndex);

            row.Cells[ColumnName].SetCellValue(_isModeVN ? rowData.NameVN : rowData.Name);
            ExcelHelper.SetValueCellDecimal(row.Cells[ColumnRange], rowData.DateRange);
            ExcelHelper.SetValueCellDecimal(row.Cells[ColumnMonthToDate], rowData.MonthToDate);
            ExcelHelper.SetValueCellDecimal(row.Cells[ColumnAllTime], rowData.AllRange);
        }

        private void SwitchToModeVN()
        {
            _isModeVN = true;
        }

        private static int GetRowTemplateIndex(byte? dataType)
        {
            return TemplateRows[dataType ?? RowContentIntegerTemplate];
        }
    }
}