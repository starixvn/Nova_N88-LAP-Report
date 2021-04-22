using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class DepositAmountExcel
    {
        private const string DetailTemplateSheetName = "Details";
        private const string DetailSheetName = "DetailsData";
        private const int DetailSheetColCount = 7;
        private const int DetailSheetTemplateTitle = 0;

        private const int DetailSheetTemplateHeader = 1;
        private const int DetailSheetTemplateContent = 2;

        private const int DetailSheetRowIdCol = 0;
        private const int DetailSheetUsernameCol = 1;
        private const int DetailSheetCurrencyCol = 2;
        private const int DetailSheetRegisteredDateCol = 3;
        private const int DetailSheetSignupUrlCol = 4;
        private const int DetailSheetAffCodeCol = 5;
        private const int DetailSheetFirstDepositAmountCol = 6;
        private const int DetailSheetNoOfDepositsCol = 7;
        private const int DetailSheetTotalDepositAmountCol = 8;

        protected override void BuildDetailSheet(IWorkbook workBook, DepositAmountExportMetaModel metaModel, IEnumerable<DepositAmountDetailDTO> detailData)
        {
            var sheet = _builder.CloneSheet(workBook, DetailTemplateSheetName, DetailSheetName);
            var templateSheet = workBook.GetSheet(DetailTemplateSheetName);

            AssignDetailTitle(sheet, templateSheet, metaModel);
            AssignDetailHeader(sheet, templateSheet);
            AssignDetailData(sheet, templateSheet, detailData);

            _builder.RemoveSheet(workBook, DetailTemplateSheetName);
        }

        private void AssignDetailTitle(ISheet sheet, ISheet templateSheet, DepositAmountExportMetaModel metaModel)
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
            row.Cells[DetailSheetSignupUrlCol].SetCellValue(ViewResources.SignupUrl);
            row.Cells[DetailSheetAffCodeCol].SetCellValue(ViewResources.AffCode);
            row.Cells[DetailSheetFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
            row.Cells[DetailSheetNoOfDepositsCol].SetCellValue(ViewResources.NumberOfDeposits);
            row.Cells[DetailSheetTotalDepositAmountCol].SetCellValue(ViewResources.TotalDepositAmount);
        }

        private void AssignDetailData(ISheet sheet, ISheet templateSheet, IEnumerable<DepositAmountDetailDTO> detailData)
        {
            var lastRowIndex = DetailSheetTemplateHeader;
            foreach (var item in detailData)
            {
                AssignDetailRowData(sheet, templateSheet, lastRowIndex++, item);
            }
        }

        private void AssignDetailRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, DepositAmountDetailDTO rowData)
        {
            var row = _builder.CopyRow(sheet, templateSheet, DetailSheetTemplateContent, lastRowIndex + 1);

            row.Cells[DetailSheetRowIdCol].SetCellValue(rowData.RowId);
            row.Cells[DetailSheetUsernameCol].SetCellValue(rowData.Username);
            row.Cells[DetailSheetCurrencyCol].SetCellValue(rowData.Currency);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(rowData.RegisteredDateDisplay);
            row.Cells[DetailSheetSignupUrlCol].SetCellValue(rowData.SignupUrl);
            row.Cells[DetailSheetAffCodeCol].SetCellValue(rowData.AffCode);
            ExcelHelper.SetValueCellDecimal(row.Cells[DetailSheetFirstDepositAmountCol], rowData.FirstDepositAmount);
            ExcelHelper.SetValueCellDecimal(row.Cells[DetailSheetNoOfDepositsCol], rowData.NoOfDeposits);
            ExcelHelper.SetValueCellDecimal(row.Cells[DetailSheetTotalDepositAmountCol], rowData.TotalDepositAmount);
        }
    }
}