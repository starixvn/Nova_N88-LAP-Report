using Fanex.Athena.Report.Marketing.Consts;
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
        private const int SummarySheetColCount = 4;

        private const int SummarySheetTemplateSpace = 3;
        private const int SummarySheetTemplateTitle = 0;

        private const int SummarySheetTemplateTotalHeader = 1;
        private const int SummarySheetTemplateTotalContent = 2;
        private const int SummarySheetTotalNameCol = 0;
        private const int SummarySheetTotalReportDateCol = 1;
        private const int SummarySheetTotalCompareDateCol = 2;
        private const int SummarySheetTotalChangePercentCol = 3;

        private const int SummarySheetTemplateCampaignHeader = 4;
        private const int SummarySheetTemplateCampaignContent = 5;
        private const int SummarySheetCampaignNameCol = 0;
        private const int SummarySheetCampaignNRUCol = 1;
        private const int SummarySheetCampaignDepositCol = 2;
        private const int SummarySheetCampaignFirstDepositAmountCol = 3;

        private const int SummarySheetTemplateSingupHeader = 6;
        private const int SummarySheetTemplateSingupContent = 7;
        private const int SummarySheetSingupNameCol = 0;
        private const int SummarySheetSingupNRUCol = 1;
        private const int SummarySheetSingupDepositCol = 2;
        private const int SummarySheetSingupFirstDepositAmountCol = 3;

        private int SummaryCurrentRowIndex = -1;

        private void BuildSummarySheet(IWorkbook workBook, NewlyRegisterUsersExportMetaModel metaModel, NewlyRegisterUsersSummaryDTO summaryData)
        {
            var sheet = _builder.CloneSheet(workBook, SummaryTemplateSheetName, SummarySheetName);
            var templateSheet = workBook.GetSheet(SummaryTemplateSheetName);

            AssignSummarySheetTitle(sheet, templateSheet, metaModel);

            AssignSummarySheetTotalHeader(sheet, templateSheet);
            AssignSummarySheetTotalData(sheet, templateSheet, summaryData.TotalData);

            CreateSpaceRow(sheet, templateSheet);

            AssignSummarySheetCampaignHeader(sheet, templateSheet);
            AssignSummarySheetCampaignData(sheet, templateSheet, summaryData.CampaignData);

            CreateSpaceRow(sheet, templateSheet);

            AssignSummarySheetSignupHeader(sheet, templateSheet);
            AssignSummarySheetSignupData(sheet, templateSheet, summaryData.SignupData);

            _builder.RemoveSheet(workBook, SummaryTemplateSheetName);
        }

        private void CreateSpaceRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateSpace, destRowIndex: ++SummaryCurrentRowIndex);
        }

        private void AssignSummarySheetTitle(ISheet sheet, ISheet templateSheet, NewlyRegisterUsersExportMetaModel metaModel)
        {
            var title = $"{ViewResources.NewlyRegisteredUsersAcronym} - {ViewResources.NewlyRegisteredUsers}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTitle, destRowIndex: ++SummaryCurrentRowIndex, columnCount: SummarySheetColCount, value: title);
            _builder.ClearTemplate(sheet, SummaryCurrentRowIndex + 1);
        }

        private void AssignSummarySheetTotalHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalHeader, destRowIndex: ++SummaryCurrentRowIndex);
            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.Total);
            row.Cells[SummarySheetTotalReportDateCol].SetCellValue(ViewResources.ReportDate);
            row.Cells[SummarySheetTotalCompareDateCol].SetCellValue(ViewResources.CompareDate);
            row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(ViewResources.ChangePercent);
        }

        private void AssignSummarySheetTotalData(ISheet sheet, ISheet templateSheet, IEnumerable<TotalNewlyRegisterUsersDTO> totalData)
        {
            var data = totalData.FirstOrDefault();
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateTotalContent, destRowIndex: ++SummaryCurrentRowIndex);

            row.Cells[SummarySheetTotalNameCol].SetCellValue(ViewResources.NewlyRegisteredUsersAcronym);
            row.Cells[SummarySheetTotalReportDateCol].SetCellValue(data.NewlyRegisterUsers);
            row.Cells[SummarySheetTotalCompareDateCol].SetCellValue(data.CompareNewlyRegisterUsers);
            row.Cells[SummarySheetTotalChangePercentCol].SetCellValue(data.ChangeRate);
        }

        private void AssignSummarySheetCampaignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateCampaignHeader, destRowIndex: ++SummaryCurrentRowIndex);

            row.Cells[SummarySheetCampaignNameCol].SetCellValue(ViewResources.Bonus);
            row.Cells[SummarySheetCampaignNRUCol].SetCellValue(ViewResources.NewlyRegisteredUsersAcronym);
            row.Cells[SummarySheetCampaignDepositCol].SetCellValue(ViewResources.Deposit);
            row.Cells[SummarySheetCampaignFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
        }

        private void AssignSummarySheetCampaignData(ISheet sheet, ISheet templateSheet, IEnumerable<CampaignNewlyRegisterUsersDTO> campaignData)
        {
            foreach (var item in campaignData)
            {
                AssignSummaryCampaignRowData(sheet, templateSheet, SummaryCurrentRowIndex++, item);
            }
        }

        private void AssignSummaryCampaignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, CampaignNewlyRegisterUsersDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateCampaignContent, lastRowIndex + 1);
            row.Cells[SummarySheetCampaignNameCol].SetCellValue(string.IsNullOrEmpty(rowData.BonusCode) ? ViewResources.None : rowData.BonusCode);
            row.Cells[SummarySheetCampaignNRUCol].SetCellValue(rowData.NewlyRegisterUsers);
            row.Cells[SummarySheetCampaignDepositCol].SetCellValue(rowData.DepositUsers);
            row.Cells[SummarySheetCampaignFirstDepositAmountCol].SetCellValue((double)rowData.FirstDepositAmount);
        }

        private void AssignSummarySheetSignupHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: SummarySheetTemplateSingupHeader, destRowIndex: ++SummaryCurrentRowIndex);
            row.Cells[SummarySheetSingupNameCol].SetCellValue(ViewResources.SignupUrl);
            row.Cells[SummarySheetSingupNRUCol].SetCellValue(ViewResources.NewlyRegisteredUsersAcronym);
            row.Cells[SummarySheetSingupDepositCol].SetCellValue(ViewResources.Deposit);
            row.Cells[SummarySheetSingupFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
        }

        private void AssignSummarySheetSignupData(ISheet sheet, ISheet templateSheet, IEnumerable<SignupUrlNewlyRegisterUsersDTO> signupData)
        {
            foreach (var item in signupData)
            {
                AssignSummarySignupRowData(sheet, templateSheet, SummaryCurrentRowIndex++, item);
            }
        }

        private void AssignSummarySignupRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, SignupUrlNewlyRegisterUsersDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, SummarySheetTemplateSingupContent, lastRowIndex + 1);
            row.Cells[SummarySheetSingupNameCol].SetCellValue(string.IsNullOrEmpty(rowData.SignupUrl) ? ViewResources.None : rowData.SignupUrl);
            row.Cells[SummarySheetSingupNRUCol].SetCellValue(rowData.NewlyRegisterUsers);
            row.Cells[SummarySheetSingupDepositCol].SetCellValue(rowData.DepositUsers);
            row.Cells[SummarySheetSingupFirstDepositAmountCol].SetCellValue((double)rowData.FirstDepositAmount);
        }
    }
}