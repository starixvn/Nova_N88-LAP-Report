using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.Consts;
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
        private const int DetailSheetColCount = 7;
        private const int DetailSheetTemplateTitle = 0;

        private const int DetailSheetTemplateHeader = 1;
        private const int DetailSheetTemplateContent = 2;
        private const int DetailSheetRowIdCol = 0;
        private const int DetailSheetCampaignNameCol = 1;
        private const int DetailSheetUsernameCol = 2;
        private const int DetailSheetRegisteredDateCol = 3;
        private const int DetailSheetSignupUrlCol = 4;
        private const int DetailSheetDepositCol = 5;
        private const int DetailSheetFirstDepositAmountCol = 6;

        private int DetailCurrentRowIndex = -1;

        private void BuildDetailSheet(IWorkbook workBook, NewlyRegisterUsersExportMetaModel metaModel, ResultModel<NewlyRegisterUsersDetailDTO> detailData)
        {
            var sheet = _builder.CloneSheet(workBook, DetailTemplateSheetName, DetailSheetName);
            var templateSheet = workBook.GetSheet(DetailTemplateSheetName);

            AssignDetailTitle(sheet, templateSheet, metaModel);
            AssignDetailHeader(sheet, templateSheet);
            AssignDetailData(sheet, templateSheet, detailData.DataList);

            _builder.RemoveSheet(workBook, DetailTemplateSheetName);
        }

        private void AssignDetailTitle(ISheet sheet, ISheet templateSheet, NewlyRegisterUsersExportMetaModel metaModel)
        {
            var title = $"{ViewResources.NewlyRegisteredUsersAcronym} - {ViewResources.NewlyRegisteredUsers}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateTitle, destRowIndex: ++DetailCurrentRowIndex, columnCount: DetailSheetColCount, value: title);

            _builder.ClearTemplate(sheet, DetailCurrentRowIndex + 1);
        }

        private void AssignDetailHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: DetailSheetTemplateHeader, destRowIndex: ++DetailCurrentRowIndex);

            row.Cells[DetailSheetRowIdCol].SetCellValue("#");
            row.Cells[DetailSheetCampaignNameCol].SetCellValue(ViewResources.Campaign);
            row.Cells[DetailSheetUsernameCol].SetCellValue(ViewResources.Username);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(ViewResources.RegisteredDate);
            row.Cells[DetailSheetSignupUrlCol].SetCellValue(ViewResources.SignupUrl);
            row.Cells[DetailSheetDepositCol].SetCellValue(ViewResources.Deposit);
            row.Cells[DetailSheetFirstDepositAmountCol].SetCellValue(ViewResources.FirstDepositAmount);
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
            row.Cells[DetailSheetCampaignNameCol].SetCellValue(rowData.CampaignName);
            row.Cells[DetailSheetUsernameCol].SetCellValue(rowData.Username);
            row.Cells[DetailSheetRegisteredDateCol].SetCellValue(rowData.RegisteredDateDisplay);
            row.Cells[DetailSheetSignupUrlCol].SetCellValue(rowData.SignupUrl);
            row.Cells[DetailSheetDepositCol].SetCellValue(rowData.IsDeposit ? ViewResources.Yes : ViewResources.No);
            row.Cells[DetailSheetFirstDepositAmountCol].SetCellValue((double)rowData.FirstDepositAmount);
        }
    }
}