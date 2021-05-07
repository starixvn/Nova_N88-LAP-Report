using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.WinLossAnalysis.Helpers;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public class WinLossPlayerBetListExcel : IWinLossPlayerBetListExcel
    {
        protected const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = "MM/dd/yyyy";

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
        protected const int ColumnGGR = 5;
        protected const int ColumnStatus = 6;

        private const int ColumnTemplateCount = 7;

        private const string TemplatePath = "/ExcelTemplate/WinLossAnalysis/WinLossPlayerBetList.xlsx";

        protected readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public WinLossPlayerBetListExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(ResultModel<WinLossPlayerBetListViewModel> data, ExportWinLossPlayerBetListMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);
            var sheet = _builder.CloneSheet(workBook, TemplateSheetName, SheetName);

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
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, ExportWinLossPlayerBetListMetaModel metaModel)
        {
            const string titlePattern = "{0} {1} - {2} - {3} ({4}), {5}, {6}";
            var title = string.Format(titlePattern,
                ViewResources.PlayerBetList,
                metaModel.FromDate.ToString(DateFormat),
                metaModel.ToDate.ToString(DateFormat),
                metaModel.Username, metaModel.SabaSystemId,
                string.Format(ViewResources.Viewin, metaModel.DrilldownInOriginalCurrency ? ViewResources.OriginalCurrency.ToLowerInvariant() : metaModel.BaseCurrency.ToUpperInvariant()),
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
            row.Cells[ColumnGGR].SetCellValue(rowData.GGR);
            row.Cells[ColumnStatus].SetCellValue(rowData.TicketStatus);
        }
    }
}