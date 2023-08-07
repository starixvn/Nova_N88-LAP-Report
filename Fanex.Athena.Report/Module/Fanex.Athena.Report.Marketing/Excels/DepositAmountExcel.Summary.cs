using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class DepositAmountExcel
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

        private const int SummarySheetTemplateSignupHeader = 4;
        private const int SummarySheetTemplateSignupContent = 5;
        private const int SummarySheetTemplateSignupTotalContent = 7;
        private const int SummarySheetSignupNameCol = 0;
        private const int SummarySheetSignupFirstDepositAmountCol = 1;
        private const int SummarySheetSignupTotalDepositAmountCol = 2;
        private const int SummarySheetSignupNoOfPlayersCol = 3;

        protected override void BuildSummarySheet(IWorkbook workBook, DepositAmountExportMetaModel metaModel, DepositAmountSummaryDTO summaryData)
        {
            var sheet = _builder.CloneSheet(workBook, SummaryTemplateSheetName, SummarySheetName);
            var templateSheet = workBook.GetSheet(SummaryTemplateSheetName);

            AssignSummarySheetTitle(sheet, templateSheet, metaModel);
            AssignSummarySheetTotalHeader(sheet, templateSheet, metaModel);
            AssignSummarySheetTotalData(sheet, templateSheet, summaryData.TotalData, metaModel);

            var lastRowIndex = SummarySheetTemplateSignupHeader;
            AssignSummarySheetSignupHeader(sheet, templateSheet);
            AssignSummarySheetSignupData(sheet, templateSheet, summaryData.SignupData, ref lastRowIndex);
            AssignSummarySignupTotalRowData(sheet, templateSheet, lastRowIndex, summaryData.TotalSignupData);

            _builder.RemoveSheet(workBook, SummaryTemplateSheetName);
        }

        private void AssignSummarySheetTitle(ISheet sheet, ISheet templateSheet, DepositAmountExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTitle, destRowIndex: SummarySheetTemplateTitle, columnCount: SummarySheetColCount, value: title);
            _builder.ClearTemplate(sheet, SummarySheetTemplateTitle + 1);
        }

        private void AssignSummarySheetTotalHeader(ISheet sheet, ISheet templateSheet, DepositAmountExportMetaModel metaModel)
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

        private void AssignSummarySheetTotalData(ISheet sheet, ISheet templateSheet, IEnumerable<TotalDepositAmountSummaryDTO> totalData, DepositAmountExportMetaModel metaModel)
        {
            var data = totalData.FirstOrDefault() ?? new TotalDepositAmountSummaryDTO();

            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalContent, destRowIndex: SummarySheetTemplateTotalContent);

            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.DepositAmount);
            row.Cells[SummarySheetTotalReportDateCol].SetCellValue((double)data.DepositAmount);

            if (metaModel.AllowedCompare)
            {
                row.Cells[SummarySheetTotalCompareDateCol].SetCellValue((double)data.CompareDepositAmount);
                row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(data.ChangeRate);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetTotalReportDateCol, SummarySheetTotalChangePercentCol);
            }
        }

        private void AssignSummarySheetSignupHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateSignupHeader, destRowIndex: SummarySheetTemplateSignupHeader);

            row.Cells[SummarySheetSignupNameCol].SetCellValue(ViewResources.SignupUrl);
            row.Cells[SummarySheetSignupFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
            row.Cells[SummarySheetSignupTotalDepositAmountCol].SetCellValue(ViewResources.TotalDepositAmount);
            row.Cells[SummarySheetSignupNoOfPlayersCol].SetCellValue(ViewResources.NumberOfPlayers);
        }

        private void AssignSummarySheetSignupData(ISheet sheet, ISheet templateSheet, IEnumerable<SignupUrlDepositAmountDTO> signupData, ref int lastRowIndex)
        {
            foreach (var item in signupData)
            {
                if (item.Childs != null && item.Childs.Any())
                {
                    AssignSummarySheetSignupData(sheet, templateSheet, item.Childs, ref lastRowIndex);
                }
                else
                {
                    AssignSummarySignupRowData(sheet, templateSheet, lastRowIndex++, item);
                }
            }
        }

        private void AssignSummarySignupRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, SignupUrlDepositAmountDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateSignupContent, lastRowIndex + 1);
            row.Cells[SummarySheetSignupNameCol].SetCellValue(rowData.SignupUrl);
            row.Cells[SummarySheetSignupFirstDepositAmountCol].SetCellValue(rowData.FirstDepositAmount);
            row.Cells[SummarySheetSignupTotalDepositAmountCol].SetCellValue(rowData.TotalDepositAmount);
            row.Cells[SummarySheetSignupNoOfPlayersCol].SetCellValue(rowData.NoOfPlayers);
        }

        private void AssignSummarySignupTotalRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, TotalSignupUrlDepositAmountDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateSignupTotalContent, lastRowIndex + 1);

            row.Cells[SummarySheetSignupNameCol].SetCellValue(ViewResources.Total);
            row.Cells[SummarySheetSignupFirstDepositAmountCol].SetCellValue(rowData.TotalFirstDepositAmount);
            row.Cells[SummarySheetSignupTotalDepositAmountCol].SetCellValue(rowData.TotalDepositAmount);
            row.Cells[SummarySheetSignupNoOfPlayersCol].SetCellValue(rowData.TotalNoOfPlayers);
        }
    }
}