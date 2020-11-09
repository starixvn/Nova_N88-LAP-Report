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
        private const int SummarySheetColCount = 4;

        private const int SummarySheetTemplateTitle = 0;

        private const int SummarySheetTemplateTotalHeader = 1;
        private const int SummarySheetTemplateTotalContent = 2;
        private const int SummarySheetTotalNameCol = 0;
        private const int SummarySheetTotalReportDateCol = 1;
        private const int SummarySheetTotalCompareDateCol = 2;
        private const int SummarySheetTotalChangePercentCol = 3;

        private const int SummarySheetTemplateCampaignHeader = 4;
        private const int SummarySheetTemplateCampaignContent = 5;
        private const int SummarySheetTemplateCampaignTotalContent = 7;
        private const int SummarySheetCampaignNameCol = 0;
        private const int SummarySheetNoOfBonusClaimedCol = 1;
        private const int SummarySheetBonusAmountCol = 3;

        protected override void BuildSummarySheet(IWorkbook workBook, BonusClaimedExportMetaModel metaModel, BonusClaimedSummaryDTO summaryData)
        {
            var sheet = _builder.CloneSheet(workBook, SummaryTemplateSheetName, SummarySheetName);
            var templateSheet = workBook.GetSheet(SummaryTemplateSheetName);

            AssignSummarySheetTitle(sheet, templateSheet, metaModel);
            AssignSummarySheetTotalHeader(sheet, templateSheet, metaModel);
            AssignSummarySheetTotalData(sheet, templateSheet, summaryData.TotalData, metaModel);

            AssignSummarySheetCampaignHeader(sheet, templateSheet);
            AssignSummarySheetCampaignData(sheet, templateSheet, summaryData.DetailData, summaryData.TotalDetailData);

            _builder.RemoveSheet(workBook, SummaryTemplateSheetName);
        }

        private void AssignSummarySheetTitle(ISheet sheet, ISheet templateSheet, BonusClaimedExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTitle, destRowIndex: SummarySheetTemplateTitle, columnCount: SummarySheetColCount, value: title);
            _builder.ClearTemplate(sheet, SummarySheetTemplateTitle + 1);
        }

        private void AssignSummarySheetTotalHeader(ISheet sheet, ISheet templateSheet, BonusClaimedExportMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalHeader, destRowIndex: SummarySheetTemplateTotalHeader);
            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.Total);
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

        private void AssignSummarySheetTotalData(ISheet sheet, ISheet templateSheet, IEnumerable<TotalBonusClaimedDTO> totalData, BonusClaimedExportMetaModel metaModel)
        {
            var data = totalData.FirstOrDefault();
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalContent, destRowIndex: SummarySheetTemplateTotalContent);

            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.BonusAmountClaimed);
            row.Cells[SummarySheetTotalReportDateCol].SetCellValue((double)data.BonusAmountClaimeds);

            if (metaModel.AllowedCompare)
            {
                row.Cells[SummarySheetTotalCompareDateCol].SetCellValue((double)data.CompareBonusAmountClaimeds);
                row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(data.ChangeRate);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetTotalReportDateCol, SummarySheetTotalChangePercentCol);
            }
        }

        private void AssignSummarySheetCampaignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateCampaignHeader, destRowIndex: SummarySheetTemplateCampaignHeader);

            row.Cells[SummarySheetCampaignNameCol].SetCellValue(ViewResources.Bonus);
            row.Cells[SummarySheetNoOfBonusClaimedCol].SetCellValue(ViewResources.NoOfBonusClaimed);
            row.Cells[SummarySheetBonusAmountCol].SetCellValue(ViewResources.TotalBonusAmountClaimed);
        }

        private void AssignSummarySheetCampaignData(ISheet sheet, ISheet templateSheet, IEnumerable<DetailBonusClaimedDTO> data, TotalDetailBonusClaimedDTO total)
        {
            var lastRowIndex = SummarySheetTemplateCampaignHeader;
            foreach (var item in data)
            {
                AssignSummaryCampaignRowData(sheet, templateSheet, lastRowIndex++, item);
            }
            AssignSummaryCampaignTotalRowData(sheet, templateSheet, lastRowIndex, total);
        }

        private void AssignSummaryCampaignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, DetailBonusClaimedDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateCampaignContent, lastRowIndex + 1);
            row.Cells[SummarySheetCampaignNameCol].SetCellValue(string.IsNullOrEmpty(rowData.BonusCode) ? ViewResources.None : rowData.BonusCode);
            row.Cells[SummarySheetNoOfBonusClaimedCol].SetCellValue(rowData.NumberOfBonusClaimed);
            row.Cells[SummarySheetBonusAmountCol].SetCellValue((double)rowData.BonusAmount);
        }

        private void AssignSummaryCampaignTotalRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, TotalDetailBonusClaimedDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateCampaignTotalContent, lastRowIndex + 1);

            row.Cells[SummarySheetCampaignNameCol].SetCellValue(ViewResources.Total);
            row.Cells[SummarySheetNoOfBonusClaimedCol].SetCellValue(rowData.TotalBonusClaimeds);
            row.Cells[SummarySheetBonusAmountCol].SetCellValue((double)rowData.TotalBonusAmount);
        }
    }
}