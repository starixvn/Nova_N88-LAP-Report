using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.BonusInfo.Excels
{
    public class BonusInfoSummaryExcel : IBonusInfoSummaryExcel
    {
        private const string TemplatePath = "/ExcelTemplate/BonusInfo/BonusInfo_Summary.xls";
        private const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = "MM/dd/yyyy";
        protected const string DateCellFormat = "dd-MMM-yyyy";

        private const int ColumnBonusName = 0;
        private const int ColumnBonusCode = 1;
        private const int ColumnBonusType = 2;
        private const int ColumnStartDate = 3;
        private const int ColumnEndDate = 4;
        private const int ColumnBonusStatus = 5;
        private const int ColumnTotalClaimants = 10;
        private const int ColumnTotalClaimedAmount = 11;
        private const int ColumnTotalDeposit = 12;
        private const int ColumnTotalWithdrawal = 13;

        private const int ColumnStatusRunning = 6;
        private const int ColumnStatusCancel = 7;
        private const int ColumnStatusFullfill = 8;
        private const int ColumnStatusOtherPlatform = 9;

        private const int ColumnTemplateCount = 14;

        private const int RowTitleTemplate = 0;
        private const int RowHeaderTemplate = 1;
        private const int RowContentTemplate = 3;
        private const int RowTotalDataTemplate = 5;
        private const int RowNoInfoTemplate = 6;

        private int ColumnClaimStatus = 6;

        private readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        private static readonly Dictionary<string, int> ClaimStatusMap =
           new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
       {
                {"1", ColumnStatusRunning },
                {"2", ColumnStatusCancel },
                {"4", ColumnStatusFullfill },
                {"5", ColumnStatusOtherPlatform }
       };

        public BonusInfoSummaryExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(BonusInfoSummaryDTO data, BonusInfoSummaryMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);
            var sheet = _builder.CloneSheet(workBook, templateSheet.SheetName, SheetName);

            var claimStatusSelected = metaModel.ClaimStatus.Split(',');
            var claimStatusNotExported = BonusInfoDropdownOptions.ClaimStatusList
                .Where(_ => !claimStatusSelected.Contains(_.Key.ToString()))
                .Select(_ => _.Key.ToString());

            ColumnClaimStatus = ClaimStatusMap[claimStatusSelected[0]];

            AssignTitle(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet);

            if (data.SortedData.TotalRows == 0 || !data.SortedData.DataList.Any())
            {
                AssignNoInfoRow(sheet, templateSheet);
            }
            else
            {
                AssignData(sheet, templateSheet, data.SortedData.DataList);
                AssignTotalData(sheet, templateSheet, data.SortedData.TotalData);
            }

            _builder.MergeCells(sheet, firstRow: RowHeaderTemplate, lastRow: RowHeaderTemplate, firstCol: ColumnClaimStatus, lastCol: ColumnStatusOtherPlatform);

            foreach (var statusId in claimStatusNotExported)
            {
                _builder.HideColumn(sheet, ClaimStatusMap[statusId]);
            }

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        private void AssignNoInfoRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, BonusInfoSummaryMetaModel metaModel)
        {
            Sites.SiteNameList.TryGetValue(metaModel.BrandId, out string brandName);

            var title = $"{ViewResources.BonusInfo}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}, {brandName}," +
                $" {string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant())}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: ColumnTemplateCount, value: title);

            sheet.CreateFreezePane(0, RowContentTemplate);
            _builder.ClearTemplate(sheet, RowContentTemplate);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate);
            row.Cells[ColumnBonusName].SetCellValue(ViewResources.BonusName);
            row.Cells[ColumnBonusCode].SetCellValue(ViewResources.BonusCode);
            row.Cells[ColumnBonusType].SetCellValue(ViewResources.BonusType);
            row.Cells[ColumnStartDate].SetCellValue(ViewResources.StartDate);
            row.Cells[ColumnEndDate].SetCellValue(ViewResources.EndDate);
            row.Cells[ColumnBonusStatus].SetCellValue(ViewResources.BonusStatus);
            row.Cells[ColumnClaimStatus].SetCellValue(ViewResources.ClaimStatus);
            row.Cells[ColumnTotalClaimants].SetCellValue(ViewResources.TotalClaimants);
            row.Cells[ColumnTotalClaimedAmount].SetCellValue(ViewResources.TotalClaimedAmount);
            row.Cells[ColumnTotalDeposit].SetCellValue(ViewResources.TotalDeposit);
            row.Cells[ColumnTotalWithdrawal].SetCellValue(ViewResources.TotalWithdrawal);

            // row 2
            var row2 = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate + 1, destRowIndex: RowHeaderTemplate + 1);
            row2.Cells[ColumnStatusRunning].SetCellValue(ViewResources.Running);
            row2.Cells[ColumnStatusCancel].SetCellValue(ViewResources.Canceled);
            row2.Cells[ColumnStatusFullfill].SetCellValue(ViewResources.Fullfill);
            row2.Cells[ColumnStatusOtherPlatform].SetCellValue(ViewResources.OtherPlatform);
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<BonusInfoSummaryItemDTO> data)
        {
            var currentRowIndex = 0;
            foreach (var item in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, item);
            }
        }

        private void AssignTotalData(ISheet sheet, ISheet templateSheet, BonusInfoSummaryItemDTO dataTotal)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowTotalDataTemplate, destRowIndex: sheet.LastRowNum + 1);
            row.Cells[ColumnBonusName].SetCellValue(ViewResources.Total);

            row.Cells[ColumnStatusRunning].SetCellValue(dataTotal.Running);
            row.Cells[ColumnStatusCancel].SetCellValue(dataTotal.Canceled);
            row.Cells[ColumnStatusFullfill].SetCellValue(dataTotal.Fulfill);
            row.Cells[ColumnStatusOtherPlatform].SetCellValue(dataTotal.OtherPlatform);
            row.Cells[ColumnTotalClaimants].SetCellValue((double)dataTotal.TotalClaimants);
            row.Cells[ColumnTotalClaimedAmount].SetCellValue((double)dataTotal.TotalClaimedAmount);
            row.Cells[ColumnTotalDeposit].SetCellValue((double)dataTotal.TotalDeposit);
            row.Cells[ColumnTotalWithdrawal].SetCellValue((double)dataTotal.TotalWithdrawal);

            _builder.MergeCells(sheet, firstRow: sheet.LastRowNum, lastRow: sheet.LastRowNum, firstCol: ColumnBonusName, lastCol: ColumnBonusStatus);
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, BonusInfoSummaryItemDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, RowContentTemplate, lastRowIndex + RowContentTemplate);
            row.Cells[ColumnBonusName].SetCellValue(rowData.BonusName);
            row.Cells[ColumnBonusCode].SetCellValue(rowData.BonusCode);
            row.Cells[ColumnBonusType].SetCellValue(rowData.BonusTypeText);
            row.Cells[ColumnStartDate].SetCellValue(rowData.StartDate.ToString(DateCellFormat));
            row.Cells[ColumnEndDate].SetCellValue(rowData.EndDate.ToString(DateCellFormat));
            row.Cells[ColumnBonusStatus].SetCellValue(rowData.BonusStatusText);
            row.Cells[ColumnStatusRunning].SetCellValue(rowData.Running);
            row.Cells[ColumnStatusCancel].SetCellValue(rowData.Canceled);
            row.Cells[ColumnStatusFullfill].SetCellValue(rowData.Fulfill);
            row.Cells[ColumnStatusOtherPlatform].SetCellValue(rowData.OtherPlatform);
            row.Cells[ColumnTotalClaimants].SetCellValue((double)rowData.TotalClaimants);
            row.Cells[ColumnTotalClaimedAmount].SetCellValue((double)rowData.TotalClaimedAmount);
            row.Cells[ColumnTotalDeposit].SetCellValue((double)rowData.TotalDeposit);
            row.Cells[ColumnTotalWithdrawal].SetCellValue((double)rowData.TotalWithdrawal);
        }
    }
}