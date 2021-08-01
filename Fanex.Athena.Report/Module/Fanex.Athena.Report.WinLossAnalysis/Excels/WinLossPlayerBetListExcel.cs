using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.WinLossAnalysis.Helpers;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public class WinLossPlayerBetListExcel : IWinLossPlayerBetListExcel
    {
        protected const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";

        protected const int RowTitleTemplate = 0;
        protected const int RowHeaderTemplate = 1;
        private const int RowContentTemplate = 3;
        private const int RowTotalDataTemplate = 4;
        private const int RowNoInfoTemplate = 5;

        protected const int ColumnRowNumber = 0;
        protected const int ColumnRefNo = 1;
        protected const int ColumnTransTime = 2;
        protected const int ColumnStake = 3;
        protected const int ColumnPlayerWinLoss = 4;
        protected const int ColumnProgressiveWin = 5;
        protected const int ColumnProgressiveBet = 6;
        protected const int ColumnGGR = 7;
        protected const int ColumnStatus = 8;

        private const int ColumnTemplateCount = 9;

        private const int MaxRows = 400000;

        private const string TemplatePath = "/ExcelTemplate/WinLossAnalysis/WinLossPlayerBetList.xlsx";
        private const string TitleFormat = "{0} {1} → {2} - {3} - {4}{5} - {6}, {7}";

        protected readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;
        private readonly IWinLossPlayerService _betlistService;

        public WinLossPlayerBetListExcel(IExcelBuilder builder, IFileProvider fileProvider, IWinLossPlayerService betlistService)
        {
            _builder = builder;
            _fileProvider = fileProvider;
            _betlistService = betlistService;
        }

        private static int GetNumberOfSheets(int numberOfItems, int pageSize) => Math.Min(
            (int)Math.Ceiling(numberOfItems * 1.0 / pageSize),
            (int)Math.Ceiling(MaxRows * 1.0 / pageSize));

        public async Task<IWorkbook> Export(ExportWinLossPlayerBetListMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);

            if (metaModel.TotalRows <= 0)
            {
                var sheet = _builder.CloneSheet(workBook, TemplateSheetName, SheetName);
                AssignTitle(sheet, templateSheet, metaModel);
                AssignHeader(sheet, templateSheet);
                AssignNoInfoRow(sheet, templateSheet);
            }
            else
            {
                await AssignMultiSheet(workBook, templateSheet, metaModel.TotalRows, metaModel);
            }

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        private async Task AssignMultiSheet(IWorkbook workBook, ISheet templateSheet, int totalRows, ExportWinLossPlayerBetListMetaModel metaModel)
        {
            var numberOfSheets = GetNumberOfSheets(totalRows, metaModel.PageSize);

            for (int sheetIndex = 1; sheetIndex <= numberOfSheets; sheetIndex++)
            {
                metaModel.PageIndex = sheetIndex;
                var sheetData = await _betlistService.GetPlayerBetList(metaModel);
                if (!sheetData.DataList.Any())
                {
                    return;
                }

                var sheet = _builder.CloneSheet(workBook, TemplateSheetName, $"{SheetName}-{sheetIndex}");

                AssignTitle(sheet, templateSheet, metaModel);
                AssignHeader(sheet, templateSheet);
                AssignData(sheet, templateSheet, sheetData.DataList);
                AssignTotalData(sheet, templateSheet, sheetData.TotalData);
            }
        }

        private void AssignNoInfoRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, ExportWinLossPlayerBetListMetaModel metaModel)
        {
            var title = string.Format(TitleFormat, ViewResources.PlayerBetList, metaModel.FromDate.ToShortDateString(), metaModel.ToDate.ToShortDateString(), metaModel.ProductName, metaModel.Username,
                string.IsNullOrWhiteSpace(metaModel.SabaSystemId) ? string.Empty : $" ({metaModel.SabaSystemId})",
                string.Format(ViewResources.Viewin, (metaModel.DrilldownInOriginalCurrency ? metaModel.OriginalCurrency : metaModel.BaseCurrency).ToUpperInvariant()),
                DataHelper.GetTimezoneName(metaModel.Timezone));

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: ColumnTemplateCount, value: title);

            _builder.ClearTemplate(sheet, RowContentTemplate);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate);

            row.Cells[ColumnRowNumber].SetCellValue("#");
            row.Cells[ColumnRefNo].SetCellValue(ViewResources.RefNo);
            row.Cells[ColumnTransTime].SetCellValue(ViewResources.TransTime);
            row.Cells[ColumnStake].SetCellValue(ViewResources.Stake);
            row.Cells[ColumnPlayerWinLoss].SetCellValue(ViewResources.PlayerWinloss);
            row.Cells[ColumnProgressiveWin].SetCellValue(ViewResources.ProgressiveWin);
            row.Cells[ColumnProgressiveBet].SetCellValue(ViewResources.ProgressiveBet);
            row.Cells[ColumnGGR].SetCellValue(ViewResources.GGR);
            row.Cells[ColumnStatus].SetCellValue(ViewResources.Status);
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<WinLossPlayerBetListViewModel> data)
        {
            var currentRowIndex = 0;
            foreach (var item in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, item);
            }
        }

        private void AssignTotalData(ISheet sheet, ISheet templateSheet, WinLossPlayerBetListViewModel dataTotal)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowTotalDataTemplate, destRowIndex: sheet.LastRowNum + 1);
            row.Cells[ColumnStake].SetCellValue(ViewResources.GrandTotal);
            row.Cells[ColumnPlayerWinLoss].SetCellValue(dataTotal.PlayerWinLoss);
            row.Cells[ColumnProgressiveWin].SetCellValue(dataTotal.ProgressiveWin);
            row.Cells[ColumnProgressiveBet].SetCellValue(dataTotal.ProgressiveBet);
            row.Cells[ColumnGGR].SetCellValue(dataTotal.GGR);
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, WinLossPlayerBetListViewModel rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, RowContentTemplate, RowContentTemplate + lastRowIndex);
            row.Cells[ColumnRowNumber].SetCellValue(rowData.RowNumber);
            row.Cells[ColumnRefNo].SetCellValue(rowData.RefNo);
            row.Cells[ColumnTransTime].SetCellValue(rowData.TransTime);
            row.Cells[ColumnStake].SetCellValue(rowData.Stake);
            row.Cells[ColumnPlayerWinLoss].SetCellValue(rowData.PlayerWinLoss);
            row.Cells[ColumnProgressiveWin].SetCellValue(rowData.ProgressiveWin);
            row.Cells[ColumnProgressiveBet].SetCellValue(rowData.ProgressiveBet);
            row.Cells[ColumnGGR].SetCellValue(rowData.GGR);
            row.Cells[ColumnStatus].SetCellValue(rowData.TicketStatus);
        }
    }
}