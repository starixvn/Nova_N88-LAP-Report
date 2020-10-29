using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class WithdrawalExcel
    {
        private const string SummaryTemplateSheetName = "Summary";
        private const string SummarySheetName = "SummaryData";
        private const int SummarySheetColCount = 4;

        private const int SummarySheetTemplateTitle = 0;
        private const int SummarySheetTemplateHeader = 1;
        private const int SummarySheetTemplateAmountRow = 2;
        private const int SummarySheetTemplateUsersRow = 3;

        private const int SummarySheetNameCol = 0;
        private const int SummarySheetReportDateCol = 1;
        private const int SummarySheetCompareDateCol = 2;
        private const int SummarySheetChangePercentCol = 3;

        protected override void BuildSummarySheet(IWorkbook workBook, WithdrawalExportMetaModel metaModel, WithdrawalSummaryDTO summaryData)
        {
            var sheet = _builder.CloneSheet(workBook, SummaryTemplateSheetName, SummarySheetName);
            var templateSheet = workBook.GetSheet(SummaryTemplateSheetName);

            AssignSummarySheetTitle(sheet, templateSheet, metaModel);
            AssignSummarySheetHeader(sheet, templateSheet, metaModel);
            AssignSummarySheetData(sheet, templateSheet, summaryData, metaModel);

            _builder.RemoveSheet(workBook, SummaryTemplateSheetName);
        }

        private void AssignSummarySheetTitle(ISheet sheet, ISheet templateSheet, WithdrawalExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTitle, destRowIndex: SummarySheetTemplateTitle, columnCount: SummarySheetColCount, value: title);
            _builder.ClearTemplate(sheet, SummarySheetTemplateTitle + 1);
        }

        private void AssignSummarySheetHeader(ISheet sheet, ISheet templateSheet, WithdrawalExportMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateHeader, destRowIndex: SummarySheetTemplateHeader);
            row.Cells[SummarySheetNameCol].SetCellValue(string.Empty);
            row.Cells[SummarySheetReportDateCol].SetCellValue(ViewResources.ReportDate);
            row.Cells[SummarySheetCompareDateCol].SetCellValue(ViewResources.CompareDate);
            row.Cells[SummarySheetChangePercentCol].SetCellValue(ViewResources.ChangePercent);

            if (metaModel.AllowedCompare)
            {
                row.Cells[SummarySheetCompareDateCol].SetCellValue(ViewResources.CompareDate);
                row.Cells[SummarySheetChangePercentCol].SetCellValue(ViewResources.ChangePercent);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetReportDateCol, SummarySheetChangePercentCol);
            }
        }

        private void AssignSummarySheetData(ISheet sheet, ISheet templateSheet, WithdrawalSummaryDTO data, WithdrawalExportMetaModel metaModel)
        {
            AssignSummarySheetAmountData(sheet, templateSheet, data, metaModel.AllowedCompare);
            AssignSummarySheetUsersData(sheet, templateSheet, data, metaModel.AllowedCompare);
        }

        private void AssignSummarySheetAmountData(ISheet sheet, ISheet templateSheet, WithdrawalSummaryDTO data, bool allowedCompare)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateAmountRow, destRowIndex: SummarySheetTemplateAmountRow);

            row.Cells[SummarySheetNameCol].SetCellValue(ViewResources.TotalWithdrawalAmount);
            row.Cells[SummarySheetReportDateCol].SetCellValue((double)data.WithdrawalAmount);

            if (allowedCompare)
            {
                row.Cells[SummarySheetCompareDateCol].SetCellValue((double)data.CompareWithdrawalAmount);
                row.Cells[SummarySheetChangePercentCol].SetCellValue(data.WithdrawalAmountChangeRate);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetReportDateCol, SummarySheetChangePercentCol);
            }
        }

        private void AssignSummarySheetUsersData(ISheet sheet, ISheet templateSheet, WithdrawalSummaryDTO data, bool allowedCompare)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateUsersRow, destRowIndex: SummarySheetTemplateUsersRow);

            row.Cells[SummarySheetNameCol].SetCellValue(ViewResources.TotalWithdrawalUsers);
            row.Cells[SummarySheetReportDateCol].SetCellValue(data.WithdrawalUsers);

            if (allowedCompare)
            {
                row.Cells[SummarySheetCompareDateCol].SetCellValue(data.CompareWithdrawalUsers);
                row.Cells[SummarySheetChangePercentCol].SetCellValue(data.WithdrawalUsersChangeRate);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetReportDateCol, SummarySheetChangePercentCol);
            }
        }
    }
}