using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.ByGroup.Excels
{
    public class WinLossExcel : IWinLossExcel
    {
        private const string TemplateSheetName = "Template";
        private const string SheetName = "Data";
        private const string DateFormat = "MM/dd/yyyy";

        private const int RowTitleTemplate = 0;
        private const int RowHeaderTemplate = 1;
        private const int RowParentContentTemplate = 3;
        private const int RowChildContentTemplate = 4;
        private const int RowTotalDataTemplate = 6;
        private const int RowNoInfoTemplate = 7;

        private const int ColumnTemplateCount = 8;

        private const int ColumnGroup = 0;
        private const int ColumnCurrency = 1;
        private const int ColumnTurnover = 2;
        private const int ColumnGGR = 3;
        private const int ColumnBetCount = 4;
        private const int ColumnAvgBetAmount = 5;
        private const int ColumnUniquePlayers = 6;
        private const int ColumnMargin = 7;

        private readonly IExcelBuilder _builder;

        public WinLossExcel(IExcelBuilder builder)
        {
            _builder = builder;
        }

        public IWorkbook Export(string template, ResultModel<WinLossRecordViewModel> data, WinLossMetaModel metaModel)
        {
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);
            var sheet = _builder.CloneSheet(workBook, templateSheet.SheetName, SheetName);

            AssignTitle(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet);

            if (data.TotalRows == 0 || !data.DataList.Any())
            {
                AssignNoInfoRow(sheet, templateSheet);
            }
            else
            {
                AssignData(sheet, templateSheet, data.DataList);
                AssignTotalData(sheet, templateSheet, data.TotalData);
            }

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        private void AssignNoInfoRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowParentContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, WinLossMetaModel metaModel)
        {
            var title = $"{ViewResources.WinLoss}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}," +
                $" {ViewResources.DisplayCurreny}: {metaModel.BaseCurrency.ToUpperInvariant()}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: ColumnTemplateCount, value: title);

            sheet.CreateFreezePane(0, RowParentContentTemplate);
            _builder.ClearTemplate(sheet, RowParentContentTemplate);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate);
            row.Cells[ColumnGroup].SetCellValue("Group");
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnover].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnGGR].SetCellValue(ViewResources.GGR);
            row.Cells[ColumnBetCount].SetCellValue(ViewResources.BetCount);
            row.Cells[ColumnAvgBetAmount].SetCellValue(ViewResources.AvgBetAmount);
            row.Cells[ColumnUniquePlayers].SetCellValue(ViewResources.UniquePlayers);
            row.Cells[ColumnMargin].SetCellValue(ViewResources.Margin);
        }

        private void AssignTotalData(ISheet sheet, ISheet templateSheet, WinLossRecordViewModel dataTotal)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowTotalDataTemplate, destRowIndex: sheet.LastRowNum + 1);
            row.Cells[ColumnGroup].SetCellValue(ViewResources.Total);
            row.Cells[ColumnTurnover].SetCellValue((double)dataTotal.Turnover);
            row.Cells[ColumnGGR].SetCellValue((double)dataTotal.Ggr);
            row.Cells[ColumnBetCount].SetCellValue(dataTotal.BetCount);
            row.Cells[ColumnAvgBetAmount].SetCellValue((double)dataTotal.AvgBetAmount);
            row.Cells[ColumnUniquePlayers].SetCellValue(dataTotal.UniquePlayers);
            row.Cells[ColumnMargin].SetCellValue(dataTotal.Margin);
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<WinLossRecordViewModel> data)
        {
            var currentRowIndex = 0;
            foreach (var groupData in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, groupData);
                foreach (var childData in groupData.DrillDownData)
                {
                    AssignRowData(sheet, templateSheet, currentRowIndex++, childData);
                }
            }
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, WinLossRecordViewModel rowData)
        {
            var isParent = string.IsNullOrEmpty(rowData.Parent);

            var row = CreateRow(sheet, templateSheet, lastRowIndex, isParent);

            row.Cells[ColumnGroup].SetCellValue(isParent ? rowData.Group : rowData.Parent);
            row.Cells[ColumnCurrency].SetCellValue(isParent ? ViewResources.All : rowData.Group);
            row.Cells[ColumnTurnover].SetCellValue((double)rowData.Turnover);
            row.Cells[ColumnGGR].SetCellValue((double)rowData.Ggr);
            row.Cells[ColumnBetCount].SetCellValue(rowData.BetCount);
            row.Cells[ColumnAvgBetAmount].SetCellValue((double)rowData.AvgBetAmount);
            row.Cells[ColumnUniquePlayers].SetCellValue(rowData.UniquePlayers);
            row.Cells[ColumnMargin].SetCellValue(rowData.Margin);
        }

        private IRow CreateRow(ISheet sheet, ISheet templateSheet, int lastRowIndex, bool isParent)
        {
            var rowContentTemplate = isParent ? RowParentContentTemplate : RowChildContentTemplate;
            return _builder.CopyRow(sheet, templateSheet, rowContentTemplate, RowParentContentTemplate + lastRowIndex);
        }
    }
}