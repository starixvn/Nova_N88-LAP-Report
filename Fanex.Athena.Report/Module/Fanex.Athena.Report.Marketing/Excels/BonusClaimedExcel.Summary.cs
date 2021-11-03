using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class BonusClaimedExcel
    {
        private const string SummaryTemplateSheetName = "Summary";
        private const string SummarySheetName = "SummaryData";
        private const int SummarySheetColCount = 5;
        private const int SummarySheetTemplateSpace = 8;

        private const int SummarySheetTemplateTitle = 0;

        private const int SummarySheetTemplateTotalHeader = 1;
        private const int SummarySheetTemplateTotalContent = 2;
        private const int SummarySheetTemplateTotalRow = 3;

        private const int SummarySheetTotalNameCol = 0;
        private const int SummarySheetTotalReportDateCol = 1;
        private const int SummarySheetTotalCompareDateCol = 2;
        private const int SummarySheetTotalChangePercentCol = 3;

        private const int SummarySheetTemplateCampaignHeader = 4;
        private const int SummarySheetTemplateCampaignContent = 5;
        private const int SummarySheetTemplateCampaignTotalContent = 7;
        private const int SummarySheetBonusCodeCol = 0;
        private const int SummarySheetCurrencyCol = 1;
        private const int SummarySheetBonusNameCol = 2;
        private const int SummarySheetNoOfBonusClaimedCol = 3;
        private const int SummarySheetBonusAmountCol = 4;

        private int SummaryCurrentRowIndex = -1;

        protected override void BuildSummarySheet(IWorkbook workBook, BonusClaimedExportMetaModel metaModel, BonusClaimedSummaryDTO summaryData)
        {
            var sheet = _builder.CloneSheet(workBook, SummaryTemplateSheetName, SummarySheetName);
            var templateSheet = workBook.GetSheet(SummaryTemplateSheetName);

            AssignSummarySheetTitle(sheet, templateSheet, metaModel);

            AssignSummarySheetTotalHeader(sheet, templateSheet, metaModel);
            AssignSummarySheetTotalData(sheet, templateSheet, summaryData.TotalData, metaModel.AllowedCompare);
            AssignSummaryTotalRowData(sheet, templateSheet, summaryData.TotalTotalData, metaModel.AllowedCompare);

            CreateSpaceRow(sheet, templateSheet);

            AssignSummarySheetCampaignHeader(sheet, templateSheet);
            AssignSummarySheetCampaignData(sheet, templateSheet, summaryData.DetailData);
            AssignSummaryCampaignTotalRowData(sheet, templateSheet, summaryData.TotalDetailData);

            _builder.RemoveSheet(workBook, SummaryTemplateSheetName);
        }

        private void AssignSummarySheetTitle(ISheet sheet, ISheet templateSheet, BonusClaimedExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTitle, destRowIndex: ++SummaryCurrentRowIndex, columnCount: SummarySheetColCount, value: title);
            _builder.ClearTemplate(sheet, SummarySheetTemplateTitle + 1);
        }

        private void CreateSpaceRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateSpace, destRowIndex: ++SummaryCurrentRowIndex);
        }

        private void AssignSummarySheetTotalHeader(ISheet sheet, ISheet templateSheet, BonusClaimedExportMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalHeader, destRowIndex: ++SummaryCurrentRowIndex);
            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.TotalBonusAmountClaimed);
            row.Cells[SummarySheetTotalReportDateCol].SetCellValue(ViewResources.ReportDate);

            if (metaModel.AllowedCompare)
            {
                row.Cells[SummarySheetTotalCompareDateCol].SetCellValue(ViewResources.CompareDate);
                row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(ViewResources.ChangePercent);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetTotalReportDateCol, SummarySheetTotalChangePercentCol);
            }
        }

        private void AssignSummarySheetTotalData(ISheet sheet, ISheet templateSheet, IEnumerable<TotalBonusClaimedDTO> totalData, bool allowedCompare)
        {
            foreach (var item in totalData)
            {
                var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalContent, destRowIndex: ++SummaryCurrentRowIndex);

                row.Cells[SummarySheetTotalNameCol].SetCellValue(item.CurrencyCode);
                row.Cells[SummarySheetTotalReportDateCol].SetCellValue(item.BonusAmountClaimeds);

                if (allowedCompare)
                {
                    row.Cells[SummarySheetTotalCompareDateCol].SetCellValue(item.CompareBonusAmountClaimeds);
                    row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(item.ChangeRate);
                }
                else
                {
                    MergeCellContent(sheet, row, SummarySheetTotalReportDateCol, SummarySheetTotalChangePercentCol);
                }
            }
        }

        private void AssignSummarySheetCampaignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateCampaignHeader, destRowIndex: ++SummaryCurrentRowIndex);

            row.Cells[SummarySheetBonusCodeCol].SetCellValue(ViewResources.BonusCode);
            row.Cells[SummarySheetCurrencyCol].SetCellValue(ViewResources.Currency);
            row.Cells[SummarySheetBonusNameCol].SetCellValue(ViewResources.BonusName);
            row.Cells[SummarySheetNoOfBonusClaimedCol].SetCellValue(ViewResources.NoOfUserClaimingBonus);
            row.Cells[SummarySheetBonusAmountCol].SetCellValue(ViewResources.TotalBonusAmountClaimed);
        }

        private void AssignSummarySheetCampaignData(ISheet sheet, ISheet templateSheet, IEnumerable<DetailBonusClaimedDTO> data)
        {
            foreach (var item in data)
            {
                var isParent = item.Childs.IsPresent();
                if (isParent)
                {
                    AssignSummaryCampaignRowData(sheet, templateSheet, ++SummaryCurrentRowIndex, item, true);
                    AssignSummarySheetCampaignData(sheet, templateSheet, item.Childs);
                }
                else
                {
                    AssignSummaryCampaignRowData(sheet, templateSheet, ++SummaryCurrentRowIndex, item);
                }
            }
        }

        private void AssignSummaryCampaignRowData(ISheet sheet, ISheet templateSheet, int rowIndex, DetailBonusClaimedDTO rowData, bool isParent = false)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateCampaignContent, rowIndex);
            row.Cells[SummarySheetBonusCodeCol].SetCellValue(string.IsNullOrEmpty(rowData.BonusCode) ? ViewResources.None : rowData.BonusCode);
            row.Cells[SummarySheetCurrencyCol].SetCellValue(isParent ? ViewResources.All : rowData.CurrencyCode);
            row.Cells[SummarySheetBonusNameCol].SetCellValue(rowData.BonusName);
            row.Cells[SummarySheetNoOfBonusClaimedCol].SetCellValue(rowData.NumberOfBonusUser);
            row.Cells[SummarySheetBonusAmountCol].SetCellValue(rowData.BonusAmount);
        }

        private void AssignSummaryCampaignTotalRowData(ISheet sheet, ISheet templateSheet, TotalDetailBonusClaimedDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateCampaignTotalContent, ++SummaryCurrentRowIndex);

            row.Cells[SummarySheetBonusCodeCol].SetCellValue(ViewResources.Total);
            row.Cells[SummarySheetNoOfBonusClaimedCol].SetCellValue(rowData.TotalBonusUsers);
            row.Cells[SummarySheetBonusAmountCol].SetCellValue((double)rowData.TotalBonusAmount);
        }

        private void AssignSummaryTotalRowData(ISheet sheet, ISheet templateSheet, TotalBonusClaimedDTO rowData, bool allowedCompare)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalRow, destRowIndex: ++SummaryCurrentRowIndex);

            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.Total);
            row.Cells[SummarySheetTotalReportDateCol].SetCellValue(rowData.BonusAmountClaimeds);

            if (allowedCompare)
            {
                row.Cells[SummarySheetTotalCompareDateCol].SetCellValue(rowData.CompareBonusAmountClaimeds);
                row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(rowData.ChangeRate);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetTotalReportDateCol, SummarySheetTotalChangePercentCol);
            }
        }
    }
}