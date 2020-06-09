using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.ByGroup.Excels
{
    public class WinLossExcel : BaseWinLossExcel, IWinLossExcel
    {
        private const string _templatePath = "/ExcelTemplate/WinLossReports/WinLossReport.xlsx";
        private const int _rowParentContentTemplate = 3;
        private const int _rowChildContentTemplate = 4;
        private const int _rowTotalDataTemplate = 6;
        private const int _rowNoInfoTemplate = 7;

        private const int _columnTemplateCount = 8;

        private const int ColumnTurnover = 2;
        private const int ColumnGGR = 3;
        private const int ColumnBetCount = 4;
        private const int ColumnAvgBetAmount = 5;
        private const int ColumnUniquePlayers = 6;
        private const int ColumnMargin = 7;

        protected override int RowParentContentTemplate => _rowParentContentTemplate;

        protected override int RowChildContentTemplate => _rowChildContentTemplate;

        protected override int RowNoInfoTemplate => _rowNoInfoTemplate;

        protected override int ColumnTemplateCount => _columnTemplateCount;

        protected override int RowTotalDataTemplate => _rowTotalDataTemplate;

        protected override string TemplatePath => _templatePath;

        public WinLossExcel(IExcelBuilder builder, IFileProvider fileProvider)
            : base(builder, fileProvider)
        { }

        protected override void AssignRowData(IRow row, WinLossRecordViewModel rowData)
        {
            row.Cells[ColumnTurnover].SetCellValue((double)rowData.Turnover);
            row.Cells[ColumnGGR].SetCellValue((double)rowData.Ggr);
            row.Cells[ColumnBetCount].SetCellValue(rowData.BetCount);
            row.Cells[ColumnAvgBetAmount].SetCellValue((double)rowData.AvgBetAmount);
            row.Cells[ColumnUniquePlayers].SetCellValue(rowData.UniquePlayers);
            row.Cells[ColumnMargin].SetCellValue(rowData.Margin);
        }

        protected override void AssignHeader(IRow row)
        {
            row.Cells[ColumnGroup].SetCellValue(ViewResources.Group);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnover].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnGGR].SetCellValue(ViewResources.GGR);
            row.Cells[ColumnBetCount].SetCellValue(ViewResources.BetCount);
            row.Cells[ColumnAvgBetAmount].SetCellValue(ViewResources.AvgBetAmount);
            row.Cells[ColumnUniquePlayers].SetCellValue(ViewResources.UniquePlayers);
            row.Cells[ColumnMargin].SetCellValue(ViewResources.Margin);
        }

        protected override void AssignTotalData(IRow row, WinLossRecordViewModel dataTotal)
        {
            row.Cells[ColumnGroup].SetCellValue(ViewResources.Total);
            row.Cells[ColumnTurnover].SetCellValue((double)dataTotal.Turnover);
            row.Cells[ColumnGGR].SetCellValue((double)dataTotal.Ggr);
            row.Cells[ColumnBetCount].SetCellValue(dataTotal.BetCount);
            row.Cells[ColumnAvgBetAmount].SetCellValue((double)dataTotal.AvgBetAmount);
            row.Cells[ColumnUniquePlayers].SetCellValue(dataTotal.UniquePlayers);
            row.Cells[ColumnMargin].SetCellValue(dataTotal.Margin);
        }
    }
}