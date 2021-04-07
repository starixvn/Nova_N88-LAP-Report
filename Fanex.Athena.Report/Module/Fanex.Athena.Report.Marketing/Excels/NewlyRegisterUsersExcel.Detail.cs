using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class NewlyRegisterUsersExcel
    {
        private const string DetailTemplateSheetName = "Details";
        private const string DetailSheetName = "DetailsData";
        private const int DetailSheetColCount = 10;
        private const int DetailSheetTemplateTitle = 0;

        private const int DetailSheetTemplateHeader = 1;
        private const int DetailSheetTemplateContent = 2;
        private const int DetailSheetRowIdCol = 0;
        private const int DetailSheetBonusCodeCol = 1;
        private const int DetailSheetUsernameCol = 2;
        private const int DetailSheetCurrencyCol = 3;
        private const int DetailSheetRegisteredDateCol = 4;
        private const int DetailSheetSignupUrlCol = 5;
        private const int DetailSheetAffCodeCol = 6;
        private const int DetailSheetDepositCol = 7;
        private const int DetailSheetFirstDepositAmountCol = 8;
        private const int DetailSheetTotalDepositAmountCol = 9;

        private int DetailCurrentRowIndex = -1;

        protected override void BuildDetailSheet(IWorkbook workBook, NewlyRegisterUsersExportMetaModel metaModel, IEnumerable<NewlyRegisterUsersDetailDTO> detailData)
        {
            var sheet = _builder.CloneSheet(workBook, DetailTemplateSheetName, DetailSheetName);
            var templateSheet = workBook.GetSheet(DetailTemplateSheetName);

            AssignDetailTitle(sheet, templateSheet, metaModel);
            AssignDetailHeader(sheet, templateSheet);
            AssignDetailData(sheet, templateSheet, detailData);

            _builder.RemoveSheet(workBook, DetailTemplateSheetName);
        }

        private void AssignDetailTitle(ISheet sheet, ISheet templateSheet, NewlyRegisterUsersExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateTitle, destRowIndex: ++DetailCurrentRowIndex, columnCount: DetailSheetColCount, value: title);

            _builder.ClearTemplate(sheet, DetailCurrentRowIndex + 1);
        }

        private void AssignDetailHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateHeader, destRowIndex: ++DetailCurrentRowIndex);

            row.Cells[DetailSheetRowIdCol].SetCellValue("#");
            row.Cells[DetailSheetBonusCodeCol].SetCellValue(ViewResources.BonusCode);
            row.Cells[DetailSheetUsernameCol].SetCellValue(ViewResources.Username);
            row.Cells[DetailSheetCurrencyCol].SetCellValue(ViewResources.Currency);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(ViewResources.RegisteredDate);
            row.Cells[DetailSheetSignupUrlCol].SetCellValue(ViewResources.SignupUrl);
            row.Cells[DetailSheetAffCodeCol].SetCellValue(ViewResources.AffCode);
            row.Cells[DetailSheetDepositCol].SetCellValue(ViewResources.Deposit);
            row.Cells[DetailSheetFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
            row.Cells[DetailSheetTotalDepositAmountCol].SetCellValue(ViewResources.TotalDepositAmount);
        }

        private void AssignDetailData(ISheet sheet, ISheet templateSheet, IEnumerable<NewlyRegisterUsersDetailDTO> detailData)
        {
            foreach (var item in detailData)
            {
                AssignDetailRowData(sheet, templateSheet, DetailCurrentRowIndex++, item);
            }
        }

        private void AssignDetailRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, NewlyRegisterUsersDetailDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, DetailSheetTemplateContent, lastRowIndex + 1);

            row.Cells[DetailSheetRowIdCol].SetCellValue(rowData.RowId);
            row.Cells[DetailSheetBonusCodeCol].SetCellValue(string.IsNullOrEmpty(rowData.BonusCode) ? ViewResources.None : rowData.BonusCode);
            row.Cells[DetailSheetUsernameCol].SetCellValue(rowData.Username);
            row.Cells[DetailSheetCurrencyCol].SetCellValue(rowData.Currency);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(rowData.RegisteredDateDisplay);
            row.Cells[DetailSheetSignupUrlCol].SetCellValue(string.IsNullOrEmpty(rowData.SignupUrl) ? ViewResources.None : rowData.SignupUrl);
            row.Cells[DetailSheetAffCodeCol].SetCellValue(rowData.AffCode);
            row.Cells[DetailSheetDepositCol].SetCellValue(rowData.IsDeposit ? ViewResources.Yes : ViewResources.No);
            row.Cells[DetailSheetFirstDepositAmountCol].SetCellValue(rowData.FirstDepositAmount);
            row.Cells[DetailSheetTotalDepositAmountCol].SetCellValue(rowData.TotalDepositAmount);
        }
    }
}