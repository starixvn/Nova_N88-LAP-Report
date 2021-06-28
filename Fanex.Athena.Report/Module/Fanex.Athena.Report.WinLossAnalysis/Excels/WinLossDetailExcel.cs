using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.Helpers;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public class WinLossDetailExcel : IWinLossDetailExcel
    {
        protected const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = "MM/dd/yyyy";

        protected const int RowTitleTemplate = 0;
        protected const int RowHeaderTemplate = 1;

        protected const int ColumnDate = 0;
        protected const int ColumnMonth = 1;
        protected const int ColumnProduct = 2;
        protected const int ColumnBrand = 3;
        protected const int ColumnUsername = 4;
        protected const int ColumnCurrency = 5;
        protected const int ColumnSabaId = 6;
        protected const int ColumnSabaSystemId = 7;

        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 4;
        private const int ColumnTemplateCount = 20;

        private const string TemplatePath = "/ExcelTemplate/WinLossAnalysis/WinLossDetailReport.xlsx";

        private static readonly IEnumerable<int> DefaultColumns = new List<int> { ColumnDate, ColumnMonth, ColumnProduct, ColumnBrand,
            ColumnUsername, ColumnCurrency, ColumnSabaId, ColumnSabaSystemId };

        protected int[] OrderColumns { get; set; }

        protected readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public WinLossDetailExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(IEnumerable<WinLossDataDetailItemDTO> data, WinLossDetailMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);
            var sheet = _builder.CloneSheet(workBook, TemplateSheetName, SheetName);

            OrderColumns = GetOrderColumns(metaModel);

            AssignTitle(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet, metaModel.OrderCols);

            if (!data.Any())
            {
                AssignNoInfoRow(sheet, templateSheet);
            }
            else
            {
                AssignData(sheet, templateSheet, data, metaModel.OrderCols);
                HideColumns(sheet, metaModel.OrderCols);
                HideColumnBasedGroupBy(sheet, metaModel.GroupBy);
            }

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        private void HideColumns(ISheet sheet, IEnumerable<string> orderCols)
        {
            var numberOfHideCols = ExcelConsts.WinLossDetailInfoColumns.Count - orderCols.Count();
            if (numberOfHideCols > 0)
            {
                var hideRange = Enumerable.Range(OrderColumns.Count(), numberOfHideCols);
                _builder.HideColumn(sheet, hideRange.ToArray());
            }
        }

        private void HideColumnBasedGroupBy(ISheet sheet, byte groupBy)
        {
            var hiddenColumn = ColumnMonth;
            if (groupBy == GroupByConsts.Month)
            {
                hiddenColumn = ColumnDate;
            }
            _builder.HideColumn(sheet, hiddenColumn);
        }

        private void AssignNoInfoRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, WinLossDetailMetaModel metaModel)
        {
            var currency = metaModel.DrilldownInOriginalCurrency ? ViewResources.KeepOriginalCurrency : string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant());
            var title = $"{ViewResources.WinLossAnalysis}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}," +
                $" {currency}," +
                $" {DataHelper.GetTimezoneName(metaModel.Timezone)}";

            _builder.SetRowValueBeginCell(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate,
                 columnCount: ColumnTemplateCount, GetCellTitleIndex(metaModel), value: title);

            _builder.ClearTemplate(sheet, RowContentTemplate);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet, IEnumerable<string> orderCols)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate, orderCellContent: OrderColumns);

            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnMonth].SetCellValue(ViewResources.Month);
            row.Cells[ColumnProduct].SetCellValue(ViewResources.Product);
            row.Cells[ColumnBrand].SetCellValue(ViewResources.Brand);
            row.Cells[ColumnUsername].SetCellValue(ViewResources.Username);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnSabaId].SetCellValue(ViewResources.SabaId);
            row.Cells[ColumnSabaSystemId].SetCellValue(ViewResources.SabaSystemId);

            var colIndex = DefaultColumns.Count();
            foreach (var col in orderCols)
            {
                row.Cells[colIndex++].SetCellValue(ExcelConsts.WinLossDetailInfoColumns[col].headerName);
            }
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<WinLossDataDetailItemDTO> data, IEnumerable<string> orderCols)
        {
            var currentRowIndex = 0;
            foreach (var item in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, item, orderCols);
            }
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, WinLossDataDetailItemDTO rowData, IEnumerable<string> orderCols)
        {
            var row = _builder.CopyRow(sheet, templateSheet, RowContentTemplate, RowContentTemplate + lastRowIndex, OrderColumns);
            row.Cells[ColumnDate].SetCellValue(rowData.DateMonth);
            row.Cells[ColumnMonth].SetCellValue(rowData.DateMonth);
            row.Cells[ColumnProduct].SetCellValue(rowData.ProductName);
            row.Cells[ColumnBrand].SetCellValue(rowData.SiteName);
            row.Cells[ColumnUsername].SetCellValue(rowData.UserName);
            row.Cells[ColumnCurrency].SetCellValue(rowData.CurrencyCode);
            row.Cells[ColumnSabaId].SetCellValue(rowData.SabaId);
            row.Cells[ColumnSabaSystemId].SetCellValue(rowData.SabaSystemId);

            var colIndex = DefaultColumns.Count();
            foreach (var col in orderCols)
            {
                var value = rowData.Value(ExcelConsts.WinLossDetailInfoColumns[col].propertyName) ?? string.Empty;
                row.Cells[colIndex++].SetCellValue(value);
            }
        }

        private static int[] GetOrderColumns(WinLossDetailMetaModel metaModel)
        {
            var orderCell = new List<int>(DefaultColumns);
            orderCell.AddRange(metaModel.OrderCols.Select(orderCol => ExcelConsts.WinLossDetailInfoColumns[orderCol].colTemplate));
            return orderCell.ToArray();
        }

        private static int GetCellTitleIndex(WinLossDetailMetaModel metaModel)
            => metaModel.GroupBy == GroupByConsts.Month ? ColumnMonth : ColumnDate;
    }
}