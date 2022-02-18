using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.BonusInfo.Excels
{
    public class BonusInfoDetailExcel : IBonusInfoDetailExcel
    {
        private const string TemplatePath = "/ExcelTemplate/BonusInfo/BonusInfo_Detail.xls";
        private const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = DateTimeFormat.USDate;
        protected const string DateCellFormat = DateTimeFormat.MonthAbbreviationsDate;

        private const int ColumnUserName = 0;
        private const int ColumnBonusCode = 1;
        private const int ColumnClaimDate = 2;
        private const int ColumnBonusAmountClaimed = 3;
        private const int ColumnTransactionType = 4;
        private const int ColumnTotalDeposit = 5;
        private const int ColumnTotalWithdrawal = 6;
        private const int ColumnStatus = 7;
        private const int ColumnCompletion = 8;

        private const int ColumnTemplateCount = 9;

        private const int RowTitleTemplate = 0;
        private const int RowHeaderTemplate = 1;
        private const int RowContentTemplate = 2;
        private const int RowNoInfoTemplate = 7;

        private readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public BonusInfoDetailExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(BonusInfoDetailDTO data, BonusInfoDetailMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
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
            }

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        private void AssignNoInfoRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, BonusInfoDetailMetaModel metaModel)
        {
            Sites.SiteNameList.TryGetValue(metaModel.BrandId, out string brandName);
            string title = metaModel.GroupBy == GroupBy.BonusCode ? $"{ViewResources.BonusInfo}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}, {brandName}," +
                    $" {string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant())}, {metaModel.BonusCode}" : $"{ViewResources.BonusInfo}, {brandName}," +
                    $" {string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant())}, {metaModel.BonusCode}, {metaModel.FromDate.ToString(DateFormat)}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: ColumnTemplateCount, value: title);

            sheet.CreateFreezePane(0, RowContentTemplate);
            _builder.ClearTemplate(sheet, RowContentTemplate);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate);

            row.Cells[ColumnUserName].SetCellValue(ViewResources.Username);
            row.Cells[ColumnBonusCode].SetCellValue(ViewResources.BonusCode);
            row.Cells[ColumnClaimDate].SetCellValue(ViewResources.ClaimDate);
            row.Cells[ColumnBonusAmountClaimed].SetCellValue(ViewResources.BonusAmountClaimed);
            row.Cells[ColumnTransactionType].SetCellValue(ViewResources.TransactionType);
            row.Cells[ColumnTotalDeposit].SetCellValue(ViewResources.TotalDeposit);
            row.Cells[ColumnTotalWithdrawal].SetCellValue(ViewResources.TotalWithdrawal);
            row.Cells[ColumnStatus].SetCellValue(ViewResources.Status);
            row.Cells[ColumnCompletion].SetCellValue(ViewResources.Completion + "%");
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<BonusInfoDetailItemDTO> data)
        {
            var currentRowIndex = 0;
            foreach (var item in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, item);
            }
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, BonusInfoDetailItemDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, RowContentTemplate, lastRowIndex + RowContentTemplate);
            row.Cells[ColumnUserName].SetCellValue(rowData.UserName);
            row.Cells[ColumnBonusCode].SetCellValue(rowData.BonusCode);
            row.Cells[ColumnClaimDate].SetCellValue(rowData.ClaimedDate.ToString(DateCellFormat));
            row.Cells[ColumnBonusAmountClaimed].SetCellValue((double)rowData.ClaimedAmount);
            row.Cells[ColumnTransactionType].SetCellValue(rowData.TransactionType);
            row.Cells[ColumnTotalDeposit].SetCellValue((double)rowData.TotalDeposit);
            row.Cells[ColumnTotalWithdrawal].SetCellValue((double)rowData.TotalWithdrawal);
            row.Cells[ColumnStatus].SetCellValue(rowData.StatusText);
            row.Cells[ColumnCompletion].SetCellValue(rowData.PercentagedCompletion);
        }
    }
}