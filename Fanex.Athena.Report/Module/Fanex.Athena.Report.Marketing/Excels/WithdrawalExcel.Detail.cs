using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class WithdrawalExcel
    {
        private const string DetailTemplateSheetName = "Details";
        private const string DetailSheetName = "DetailsData";
        private const int DetailSheetColCount = 6;
        private const int DetailSheetTemplateTitle = 0;

        private const int DetailSheetTemplateHeader = 1;
        private const int DetailSheetTemplateContent = 2;

        private const int DetailSheetRowIdCol = 0;
        private const int DetailSheetUsernameCol = 1;
        private const int DetailSheetCurrencyCol = 2;
        private const int DetailSheetRegisteredDateCol = 3;
        private const int DetailSheetNumberOfWithdrawalsCol = 4;
        private const int DetailSheetWithdrawalAmountCol = 5;

        protected override void BuildDetailSheet(IWorkbook workBook, WithdrawalExportMetaModel metaModel, IEnumerable<WithdrawalDetailDTO> detailData)
        {
            var sheet = _builder.CloneSheet(workBook, DetailTemplateSheetName, DetailSheetName);
            var templateSheet = workBook.GetSheet(DetailTemplateSheetName);

            AssignDetailTitle(sheet, templateSheet, metaModel);
            AssignDetailHeader(sheet, templateSheet);
            AssignDetailData(sheet, templateSheet, detailData);

            _builder.RemoveSheet(workBook, DetailTemplateSheetName);
        }

        private void AssignDetailTitle(ISheet sheet, ISheet templateSheet, WithdrawalExportMetaModel metaModel)
        {
            var title = BuildTitle(metaModel);
            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateTitle, destRowIndex: DetailSheetTemplateTitle, columnCount: DetailSheetColCount, value: title);

            _builder.ClearTemplate(sheet, DetailSheetTemplateTitle + 1);
        }

        private void AssignDetailHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateHeader, destRowIndex: DetailSheetTemplateHeader);

            row.Cells[DetailSheetRowIdCol].SetCellValue("#");
            row.Cells[DetailSheetUsernameCol].SetCellValue(ViewResources.Username);
            row.Cells[DetailSheetCurrencyCol].SetCellValue(ViewResources.Currency);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(ViewResources.RegisteredDate);
            row.Cells[DetailSheetNumberOfWithdrawalsCol].SetCellValue(ViewResources.NumberOfWithdrawals);
            row.Cells[DetailSheetWithdrawalAmountCol].SetCellValue(ViewResources.TotalWithdrawalAmount);
        }

        private void AssignDetailData(ISheet sheet, ISheet templateSheet, IEnumerable<WithdrawalDetailDTO> detailData)
        {
            var lastRowIndex = DetailSheetTemplateHeader;
            foreach (var item in detailData)
            {
                AssignDetailRowData(sheet, templateSheet, lastRowIndex++, item);
            }
        }

        private void AssignDetailRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, WithdrawalDetailDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, DetailSheetTemplateContent, lastRowIndex + 1);

            row.Cells[DetailSheetRowIdCol].SetCellValue(rowData.RowId);
            row.Cells[DetailSheetUsernameCol].SetCellValue(rowData.Username);
            row.Cells[DetailSheetCurrencyCol].SetCellValue(rowData.Currency);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(rowData.RegisteredDateDisplay);
            row.Cells[DetailSheetNumberOfWithdrawalsCol].SetCellValue(rowData.NumberOfWithdrawals);
            row.Cells[DetailSheetWithdrawalAmountCol].SetCellValue((double)rowData.TotalWithdrawalAmount);
        }
    }
}