using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.Helpers;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public class WinLossExcel : IWinLossExcel
    {
        protected const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = "MM/dd/yyyy";
        private const string ClosingBalanceSupportFromDate = "03/05/2021";

        protected const int RowTitleTemplate = 0;
        protected const int RowNoteTemplate = 1;
        protected const int RowHeaderTemplate = 2;

        protected const int ColumnGroup = 0;
        protected const int ColumnCurrency = 1;

        private const int RowParentContentTemplate = 4;
        private const int RowChildContentTemplate = 5;
        private const int RowTotalDataTemplate = 6;
        private const int RowNoInfoTemplate = 7;
        private const int ColumnTemplateCount = 16;

        private const string TemplatePath = "/ExcelTemplate/WinLossAnalysis/WinLossReport.xlsx";

        private static readonly IEnumerable<int> DefaultColumns = new List<int> { ColumnGroup, ColumnCurrency };

        protected int[] OrderColumns { get; set; }

        protected readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public WinLossExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(ResultModel<WinLossRecordViewModel> data, ExportWinLossMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);
            var sheet = _builder.CloneSheet(workBook, TemplateSheetName, SheetName);

            OrderColumns = GetOrderColumns(metaModel);

            AssignTitle(sheet, templateSheet, metaModel);
            AssignNote(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet, metaModel.OrderCols);

            if (data.TotalRows == 0 || !data.DataList.Any())
            {
                AssignNoInfoRow(sheet, templateSheet);
            }
            else
            {
                AssignData(sheet, templateSheet, data.DataList, metaModel.OrderCols);
                AssignTotalData(sheet, templateSheet, data.TotalData, metaModel.OrderCols);
                HideColumns(sheet, metaModel.OrderCols);
            }

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        private void HideColumns(ISheet sheet, IEnumerable<string> orderCols)
        {
            var numberOfHideCols = ExcelConsts.WinLossInfoColumns.Count - orderCols.Count();
            if (numberOfHideCols > 0)
            {
                var hideRange = Enumerable.Range(OrderColumns.Count(), numberOfHideCols);
                _builder.HideColumn(sheet, hideRange.ToArray());
            }
        }

        private void AssignNoInfoRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowParentContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, ExportWinLossMetaModel metaModel)
        {
            var title = $"{ViewResources.WinLossAnalysis}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}," +
                $" {string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant())}," +
                $" {DataHelper.GetTimezoneName(metaModel.Timezone)}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: ColumnTemplateCount, value: title);

            _builder.ClearTemplate(sheet, RowParentContentTemplate);
        }

        private void AssignNote(ISheet sheet, ISheet templateSheet, ExportWinLossMetaModel metaModel)
        {
            if(metaModel.IsGroupByPlayer)
            {
                var note = ViewResources.Note + ": " + string.Format(ViewResources.NoteSupportClosingBalance, ClosingBalanceSupportFromDate);
                _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowNoteTemplate, destRowIndex: RowNoteTemplate, columnCount: ColumnTemplateCount, value: note);
            }
            else
            {
                _builder.HideRow(sheet, RowNoteTemplate);
            }
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet, IEnumerable<string> orderCols)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate, orderCellContent: OrderColumns);

            row.Cells[ColumnGroup].SetCellValue(ViewResources.Group);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);

            var colIndex = DefaultColumns.Count();
            foreach (var col in orderCols)
            {
                row.Cells[colIndex++].SetCellValue(ExcelConsts.WinLossInfoColumns[col].headerName);
            }
        }

        private void AssignTotalData(ISheet sheet, ISheet templateSheet, WinLossRecordViewModel dataTotal, IEnumerable<string> orderCols)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowTotalDataTemplate, destRowIndex: sheet.LastRowNum + 1, orderCellContent: OrderColumns);
            row.Cells[ColumnGroup].SetCellValue(ViewResources.Total);

            var colIndex = DefaultColumns.Count();
            foreach (var col in orderCols)
            {
                var infoCol = ExcelConsts.WinLossInfoColumns[col];
                if (infoCol.hasTotal)
                {
                    var value = dataTotal.Value(ExcelConsts.WinLossInfoColumns[col].propertyName) ?? string.Empty;
                    row.Cells[colIndex].SetCellValue(value);
                }
                colIndex++;
            }
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<WinLossRecordViewModel> data, IEnumerable<string> orderCols)
        {
            var currentRowIndex = 0;
            foreach (var groupData in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, groupData, orderCols);
                foreach (var childData in groupData.DrillDownData)
                {
                    AssignRowData(sheet, templateSheet, currentRowIndex++, childData, orderCols);
                }
            }
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, WinLossRecordViewModel rowData, IEnumerable<string> orderCols)
        {
            var isParent = string.IsNullOrEmpty(rowData.Parent);
            var row = CreateRow(sheet, templateSheet, lastRowIndex, isParent);
            row.Cells[ColumnGroup].SetCellValue(isParent ? rowData.Group : rowData.ParentName);
            row.Cells[ColumnCurrency].SetCellValue(isParent ? ViewResources.All : rowData.Group);

            var colIndex = DefaultColumns.Count();
            foreach (var col in orderCols)
            {
                var value = rowData.Value(ExcelConsts.WinLossInfoColumns[col].propertyName) ?? string.Empty;
                row.Cells[colIndex++].SetCellValue(value);
            }
        }

        private IRow CreateRow(ISheet sheet, ISheet templateSheet, int lastRowIndex, bool isParent)
        {
            var rowContentTemplate = isParent ? RowParentContentTemplate : RowChildContentTemplate;
            return _builder.CopyRow(sheet, templateSheet, rowContentTemplate, RowParentContentTemplate + lastRowIndex, OrderColumns);
        }

        private static int[] GetOrderColumns(ExportWinLossMetaModel metaModel)
        {
            var orderCell = new List<int>(DefaultColumns);
            orderCell.AddRange(metaModel.OrderCols.Select(orderCol => ExcelConsts.WinLossInfoColumns[orderCol].colTemplate));
            return orderCell.ToArray();
        }
    }
}