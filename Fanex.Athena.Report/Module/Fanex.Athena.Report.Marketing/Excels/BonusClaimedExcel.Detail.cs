using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class BonusClaimedExcel
    {
        private const string DetailTemplateSheetName = "Details";
        private const string DetailSheetName = "DetailsData";
        private const int DetailSheetColCount = 7;
        private const int DetailSheetTemplateTitle = 0;

        private const int DetailSheetTemplateHeader = 1;
        private const int DetailSheetTemplateContent = 2;

        private const int DetailSheetRowIdCol = 0;
        private const int DetailSheetBonusCol = 1;
        private const int DetailSheetUsernameCol = 2;
        private const int DetailSheetRegisteredDateCol = 3;
        private const int DetailSheetDepositAmountCol = 4;
        private const int DetailSheetWithdrawalAmountCol = 5;
        private const int DetailSheetBonusAmountClaimedCol = 6;

        protected override void BuildDetailSheet(IWorkbook workBook, BonusClaimedExportMetaModel metaModel, IEnumerable<BonusClaimedDetailDTO> detailData)
        {
            var sheet = _builder.CloneSheet(workBook, DetailTemplateSheetName, DetailSheetName);
            var templateSheet = workBook.GetSheet(DetailTemplateSheetName);

            AssignDetailTitle(sheet, templateSheet, metaModel);
            AssignDetailHeader(sheet, templateSheet);
            AssignDetailData(sheet, templateSheet, detailData);

            _builder.RemoveSheet(workBook, DetailTemplateSheetName);
        }

        private void AssignDetailTitle(ISheet sheet, ISheet templateSheet, BonusClaimedExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateTitle, destRowIndex: DetailSheetTemplateTitle, columnCount: DetailSheetColCount, value: title);

            _builder.ClearTemplate(sheet, DetailSheetTemplateTitle + 1);
        }

        private void AssignDetailHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateHeader, destRowIndex: DetailSheetTemplateHeader);

            row.Cells[DetailSheetRowIdCol].SetCellValue("#");
            row.Cells[DetailSheetBonusCol].SetCellValue(ViewResources.Bonus);
            row.Cells[DetailSheetUsernameCol].SetCellValue(ViewResources.Username);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(ViewResources.RegisteredDate);
            row.Cells[DetailSheetDepositAmountCol].SetCellValue(ViewResources.TotalDepositAmount);
            row.Cells[DetailSheetWithdrawalAmountCol].SetCellValue(ViewResources.TotalWithdrawalAmount);
            row.Cells[DetailSheetBonusAmountClaimedCol].SetCellValue(ViewResources.TotalBonusAmountClaimed);
        }

        private void AssignDetailData(ISheet sheet, ISheet templateSheet, IEnumerable<BonusClaimedDetailDTO> detailData)
        {
            var lastRowIndex = DetailSheetTemplateHeader;
            foreach (var item in detailData)
            {
                AssignDetailRowData(sheet, templateSheet, lastRowIndex++, item);
            }
        }

        private void AssignDetailRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, BonusClaimedDetailDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, DetailSheetTemplateContent, lastRowIndex + 1);

            row.Cells[DetailSheetRowIdCol].SetCellValue(rowData.RowId);
            row.Cells[DetailSheetBonusCol].SetCellValue(rowData.BonusCode);
            row.Cells[DetailSheetUsernameCol].SetCellValue(rowData.Username);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(rowData.RegisteredDateDisplay);
            SetCellValue(row.Cells[DetailSheetDepositAmountCol], rowData.DepositAmount);
            SetCellValue(row.Cells[DetailSheetWithdrawalAmountCol], rowData.WithdrawalAmount);
            SetCellValue(row.Cells[DetailSheetBonusAmountClaimedCol], rowData.BonusAmount);
        }
    }
}