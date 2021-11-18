using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class NewlyRegisterUsersExcel
    {
        private const string SummaryTemplateSheetName = "Summary";
        private const string SummarySheetName = "SummaryData";
        private const int SummarySheetColCount = 6;

        private const int SummarySheetTemplateSpace = 3;
        private const int SummarySheetTemplateTitle = 0;

        private const int SummarySheetTemplateTotalHeader = 1;
        private const int SummarySheetTemplateTotalContent = 2;
        private const int SummarySheetTotalNameCol = 0;
        private const int SummarySheetTotalReportDateCol = 1;
        private const int SummarySheetTotalCompareDateCol = 2;
        private const int SummarySheetTotalChangePercentCol = 3;

        private const int SummarySheetTemplateBonusHeader = 4;
        private const int SummarySheetTemplateBonusContent = 5;
        private const int SummarySheetBonusCodeCol = 0;
        private const int SummarySheetBonusNameCol = 1;
        private const int SummarySheetBonusNRUCol = 2;
        private const int SummarySheetBonusDepositCol = 3;
        private const int SummarySheetBonusFirstDepositAmountCol = 4;
        private const int SummarySheetBonusTotalDepositAmountCol = 5;

        private const int SummarySheetTemplateSingupHeader = 6;
        private const int SummarySheetTemplateSingupContent = 7;
        private const int SummarySheetSingupNameCol = 0;
        private const int SummarySheetSingupNRUCol = 1;
        private const int SummarySheetSingupDepositCol = 2;
        private const int SummarySheetSingupFirstDepositAmountCol = 3;
        private const int SummarySheetSingupTotalDepositAmountCol = 4;

        private const int SummarySheetTemplateSingupTotalContent = 8;

        private int SummaryCurrentRowIndex = -1; 

        protected override void BuildSummarySheet(IWorkbook workBook, NewlyRegisterUsersExportMetaModel metaModel, NewlyRegisterUsersSummaryDTO summaryData)
        {
            var sheet = _builder.CloneSheet(workBook, SummaryTemplateSheetName, SummarySheetName);
            var templateSheet = workBook.GetSheet(SummaryTemplateSheetName);

            AssignSummarySheetTitle(sheet, templateSheet, metaModel);

            AssignSummarySheetTotalHeader(sheet, templateSheet, metaModel);
            AssignSummarySheetTotalData(sheet, templateSheet, summaryData.TotalData, metaModel);

            CreateSpaceRow(sheet, templateSheet);

            AssignSummarySheetBonusHeader(sheet, templateSheet);
            AssignSummarySheetBonusData(sheet, templateSheet, summaryData.CampaignData);

            CreateSpaceRow(sheet, templateSheet);

            AssignSummarySheetSignupHeader(sheet, templateSheet);
            AssignSummarySheetSignupData(sheet, templateSheet, summaryData.SignupData);
            AssignSummarySheetSignupTotalData(sheet, templateSheet, SummaryCurrentRowIndex, summaryData.TotalSignupData);

            _builder.RemoveSheet(workBook, SummaryTemplateSheetName);
        }

        private void CreateSpaceRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateSpace, destRowIndex: ++SummaryCurrentRowIndex);
        }

        private void AssignSummarySheetTitle(ISheet sheet, ISheet templateSheet, NewlyRegisterUsersExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTitle, destRowIndex: ++SummaryCurrentRowIndex, columnCount: SummarySheetColCount, value: title);
            _builder.ClearTemplate(sheet, SummaryCurrentRowIndex + 1);
        }

        private void AssignSummarySheetTotalHeader(ISheet sheet, ISheet templateSheet, NewlyRegisterUsersExportMetaModel metaModel)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalHeader, destRowIndex: ++SummaryCurrentRowIndex);
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

        private void AssignSummarySheetTotalData(ISheet sheet, ISheet templateSheet, IEnumerable<TotalNewlyRegisterUsersDTO> totalData, NewlyRegisterUsersExportMetaModel metaModel)
        {
            var data = totalData.FirstOrDefault();
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalContent, destRowIndex: ++SummaryCurrentRowIndex);

            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.NewlyRegisteredUsersAcronym);
            row.Cells[SummarySheetTotalReportDateCol].SetCellValue(data.NewlyRegisterUsers);

            if (metaModel.AllowedCompare)
            {
                row.Cells[SummarySheetTotalCompareDateCol].SetCellValue(data.CompareNewlyRegisterUsers);
                row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(data.ChangeRate);
            }
            else
            {
                MergeCellContent(sheet, row, SummarySheetTotalReportDateCol, SummarySheetTotalChangePercentCol);
            }
        }

        private void AssignSummarySheetBonusHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateBonusHeader, destRowIndex: ++SummaryCurrentRowIndex);

            row.Cells[SummarySheetBonusCodeCol].SetCellValue(ViewResources.BonusCode);
            row.Cells[SummarySheetBonusNameCol].SetCellValue(ViewResources.BonusName);
            row.Cells[SummarySheetBonusNRUCol].SetCellValue(ViewResources.NewlyRegisteredUsersAcronym);
            row.Cells[SummarySheetBonusDepositCol].SetCellValue(ViewResources.Deposit);
            row.Cells[SummarySheetBonusFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
            row.Cells[SummarySheetBonusTotalDepositAmountCol].SetCellValue(ViewResources.TotalDepositAmount);
        }

        private void AssignSummarySheetBonusData(ISheet sheet, ISheet templateSheet, IEnumerable<CampaignNewlyRegisterUsersDTO> campaignData)
        {
            foreach (var item in campaignData)
            {
                AssignSummaryBonusRowData(sheet, templateSheet, SummaryCurrentRowIndex++, item);
            }
        }

        private void AssignSummaryBonusRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, CampaignNewlyRegisterUsersDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateBonusContent, lastRowIndex + 1);
            row.Cells[SummarySheetBonusCodeCol].SetCellValue(string.IsNullOrEmpty(rowData.BonusCode) ? ViewResources.None : rowData.BonusCode);
            row.Cells[SummarySheetBonusNameCol].SetCellValue(rowData.BonusName);
            row.Cells[SummarySheetBonusNRUCol].SetCellValue(rowData.NewlyRegisterUsers);
            row.Cells[SummarySheetBonusDepositCol].SetCellValue(rowData.DepositUsers);
            row.Cells[SummarySheetBonusFirstDepositAmountCol].SetCellValue(rowData.FirstDepositAmount);
            row.Cells[SummarySheetBonusTotalDepositAmountCol].SetCellValue(rowData.TotalDepositAmount);
        }

        private void AssignSummarySheetSignupHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateSingupHeader, destRowIndex: ++SummaryCurrentRowIndex);
            row.Cells[SummarySheetSingupNameCol].SetCellValue(ViewResources.SignupUrl);
            row.Cells[SummarySheetSingupNRUCol].SetCellValue(ViewResources.NewlyRegisteredUsersAcronym);
            row.Cells[SummarySheetSingupDepositCol].SetCellValue(ViewResources.Deposit);
            row.Cells[SummarySheetSingupFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
            row.Cells[SummarySheetSingupTotalDepositAmountCol].SetCellValue(ViewResources.TotalDepositAmount);
        }

        private void AssignSummarySheetSignupData(ISheet sheet, ISheet templateSheet, IEnumerable<SignupUrlNewlyRegisterUsersDTO> signupData)
        {
            foreach (var item in signupData)
            {
                if(item.Childs != null && item.Childs.Any())
                {
                    AssignSummarySheetSignupData(sheet, templateSheet, item.Childs);
                }
                else
                {
                    AssignSummarySignupRowData(sheet, templateSheet, SummaryCurrentRowIndex++, item);
                }
            }
        }

        private void AssignSummarySignupRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, SignupUrlNewlyRegisterUsersDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateSingupContent, lastRowIndex + 1);
            row.Cells[SummarySheetSingupNameCol].SetCellValue(string.IsNullOrEmpty(rowData.SignupUrl) ? ViewResources.None : rowData.SignupUrl);
            row.Cells[SummarySheetSingupNRUCol].SetCellValue(rowData.NewlyRegisterUsers);
            row.Cells[SummarySheetSingupDepositCol].SetCellValue(rowData.DepositUsers);
            row.Cells[SummarySheetSingupFirstDepositAmountCol].SetCellValue(rowData.FirstDepositAmount);
            row.Cells[SummarySheetSingupTotalDepositAmountCol].SetCellValue(rowData.TotalDepositAmount);
        }

        private void AssignSummarySheetSignupTotalData(ISheet sheet, ISheet templateSheet, int lastRowIndex, TotalSignupUrlNewlyRegisterUsersDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateSingupTotalContent, lastRowIndex + 1);

            row.Cells[SummarySheetSingupNameCol].SetCellValue(ViewResources.Total);
            row.Cells[SummarySheetSingupNRUCol].SetCellValue(rowData.TotalNewlyRegisterUsers);
            row.Cells[SummarySheetSingupDepositCol].SetCellValue(rowData.TotalDepositUsers);
            row.Cells[SummarySheetSingupFirstDepositAmountCol].SetCellValue(rowData.TotalFirstDepositAmount);
            row.Cells[SummarySheetSingupTotalDepositAmountCol].SetCellValue(rowData.TotalDepositAmount);
        }
    }
}