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
        private const int RowRegistrationHeaderTemplate = 5;

        private const int TotalColumn = 3;
        private const int ColumnName = 0;
        private const int ColumnRange = 1;
        private const int ColumnMonthToDate = 2;

        private static IReadOnlyDictionary<int, int> TemplateRows = new Dictionary<int, int>
        {
            { DbColumnDataType.Integer, RowContentIntegerTemplate },
            { DbColumnDataType.Money, RowContentMoneyTemplate },
            { DbColumnDataType.Percent, RowContentPercentTemplate }
        };

        private int _currentRowIndex;
        private bool _isModeVN;
        private IWorkbook _workBook;

        private readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public PerformanceHighlightExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(PerformanceHighlightExportDTO data, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            _workBook = _builder.LoadTemplate(template);
            var templateSheet = _workBook.GetSheet(TemplateSheetName);

            ResetState();

            BuildSheetEng(templateSheet, data, metaModel);
            BuildSheetVn(templateSheet, data, metaModel);

            _builder.RemoveSheet(_workBook, TemplateSheetName);

            return _workBook;
        }

        private void BuildSheetEng(ISheet templateSheet, PerformanceHighlightExportDTO data, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var sheet = CloneSheet();

            AssignTitle(sheet, templateSheet, metaModel);

            AssignCommonHeader(sheet, templateSheet, metaModel);
            AssignCommonData(sheet, templateSheet, data.CommonData);

            AssignRegistrationHeader(sheet, templateSheet, metaModel);
            AssignRegistrationData(sheet, templateSheet, data.RegistrationData);
        }

        private void BuildSheetVn(ISheet templateSheet, PerformanceHighlightExportDTO data, PerformanceHighlightExportAllMetaModel metaModel)
        {
            SwitchToModeVN();

            _currentRowIndex = 0;

            var sheet = CloneSheet();

            AssignTitle(sheet, templateSheet, metaModel);

            AssignCommonHeader(sheet, templateSheet, metaModel);
            AssignCommonData(sheet, templateSheet, data.CommonData);

            AssignRegistrationHeader(sheet, templateSheet, metaModel);
            AssignRegistrationData(sheet, templateSheet, data.RegistrationData);
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
                (string.IsNullOrEmpty(metaModel.CurrencyNameList) ? ViewResources.AllCurrencies : metaModel.CurrencyNameList) + ", " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: _currentRowIndex++, columnCount: TotalColumn, value: title);
            _builder.ClearTemplate(sheet, RowTitleTemplate + 1);
        }

        private void AssignCommonHeader(ISheet sheet, ISheet templateSheet, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: _currentRowIndex++);

            var rangeHeaer = $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}";

            row.Cells[ColumnName].SetCellValue(string.Empty);
            row.Cells[ColumnRange].SetCellValue(rangeHeaer);
            row.Cells[ColumnMonthToDate].SetCellValue(_isModeVN ? ViewResources.MonthToDateVN : ViewResources.MonthToDate);
        }

        private void AssignRegistrationHeader(ISheet sheet, ISheet templateSheet, PerformanceHighlightExportAllMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowRegistrationHeaderTemplate, destRowIndex: _currentRowIndex++);

            var rangeHeaer = $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}";

            row.Cells[ColumnName].SetCellValue(_isModeVN ? ViewResources.RegistrationDetailsVN : ViewResources.RegistrationDetails);
            row.Cells[ColumnRange].SetCellValue(rangeHeaer);
            row.Cells[ColumnMonthToDate].SetCellValue(_isModeVN ? ViewResources.MonthToDateVN : ViewResources.MonthToDate);
        }

        private void AssignCommonData(ISheet sheet, ISheet templateSheet, IEnumerable<PerformanceHighlightCommonExportDTO> data)
        {
            foreach (var rowData in data)
            {
                AssignRowCommonData(sheet, templateSheet, _currentRowIndex++, rowData);
            }
        }

        private void AssignRegistrationData(ISheet sheet, ISheet templateSheet, IEnumerable<PerformanceHighlightRegistrationExportDTO> data)
        {
            foreach (var rowData in data)
            {
                AssignRegistrationData(sheet, templateSheet, _currentRowIndex++, rowData);
            }
        }

        private void AssignRegistrationData(ISheet sheet, ISheet templateSheet, int currentRowIndex, PerformanceHighlightRegistrationExportDTO rowData)
        {
            var rowTemplateIndex = GetRowTemplateIndex(rowData.DataType);
            var row = _builder.CopyRow(sheet, templateSheet, rowTemplateIndex, currentRowIndex);

            row.Cells[ColumnName].SetCellValue(_isModeVN ? rowData.SignupUrlVN : rowData.SignupUrl);
            ExcelHelper.SetValueCellDecimal(row.Cells[ColumnRange], rowData.SignupUsers);
            ExcelHelper.SetValueCellDecimal(row.Cells[ColumnMonthToDate], rowData.MonthToDateSignupUsers);
        }

        private void AssignRowCommonData(ISheet sheet, ISheet templateSheet, int currentRowIndex, PerformanceHighlightCommonExportDTO rowData)
        {
            var rowTemplateIndex = GetRowTemplateIndex(rowData.DataType);
            var row = _builder.CopyRow(sheet, templateSheet, rowTemplateIndex, currentRowIndex);

            row.Cells[ColumnName].SetCellValue(_isModeVN ? rowData.NameVN : rowData.Name);
            ExcelHelper.SetValueCellDecimal(row.Cells[ColumnRange], rowData.DateRange);
            ExcelHelper.SetValueCellDecimal(row.Cells[ColumnMonthToDate], rowData.MonthToDate);
        }

        private void ResetState()
        {
            _isModeVN = false;
            _currentRowIndex = 0;
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