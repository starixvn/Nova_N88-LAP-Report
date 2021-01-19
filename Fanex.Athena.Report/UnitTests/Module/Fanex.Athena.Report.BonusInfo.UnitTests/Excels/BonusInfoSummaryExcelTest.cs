using AutoFixture;
using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.Excels;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.BonusInfo.UnitTests.Excels.TestHelpers;
using Microsoft.Extensions.FileProviders;
using Moq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.BonusInfo.UnitTests.Excels
{
    public class BonusInfoSummaryExcelTest
    {
        private readonly IFixture _fixture = new Fixture();
        private const string TemplatePath = "/ExcelTemplate/BonusInfo/BonusInfo_Summary.xls";
        private const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = "MM/dd/yyyy";
        protected const string DateCellFormat = "dd-MMM-yyyy";

        private const int ColumnBonusName = 0;
        private const int ColumnBonusCode = 1;
        private const int ColumnBonusType = 2;
        private const int ColumnStartDate = 3;
        private const int ColumnEndDate = 4;
        private const int ColumnBonusStatus = 5;
        private const int ColumnTotalClaimants = 10;
        private const int ColumnTotalClaimedAmount = 11;
        private const int ColumnTotalDeposit = 12;
        private const int ColumnTotalWithdrawal = 13;

        private const int ColumnStatusRunning = 6;
        private const int ColumnStatusCancel = 7;
        private const int ColumnStatusFullfill = 8;
        private const int ColumnStatusOtherPlatform = 9;

        private const int ColumnTemplateCount = 14;

        private const int RowTitleTemplate = 0;
        private const int RowHeaderTemplate = 1;
        private const int RowContentTemplate = 3;
        private const int RowTotalDataTemplate = 5;
        private const int RowNoInfoTemplate = 6;

        private Mock<IExcelBuilder> _excelBuilder;
        private Mock<IFileProvider> _fileProvider;
        private Mock<ISheet> _sheet;
        private Mock<ISheet> _templateSheet;

        [Fact]
        public void AssignTitle_WhenCall_AssignCorrectValueForTitleRows()
        {
            // Arrange
            var excel = CreateExcel();
            CreateRows();

            var data = new BonusInfoSummaryDTO();
            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            metaModel.ClaimStatus = "1";
            Sites.SiteNameList.TryGetValue(metaModel.BrandId, out string brandName);

            var reportTitle = $"{ViewResources.BonusInfo}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}, {brandName}," +
                $" {string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant())}";

            // Action
            excel.Export(data, metaModel);

            // Assert
            _excelBuilder.Verify(ex => ex.SetRowValue(_sheet.Object, _templateSheet.Object,
                RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle), Times.Once);
        }

        [Fact]
        public void AssignNoInfoRow_WhenCall_AssignCorrectValueForContent()
        {
            // Arrange
            var excel = CreateExcel();
            CreateRows();

            var data = new BonusInfoSummaryDTO();
            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            metaModel.ClaimStatus = "1";

            // Action
            excel.Export(data, metaModel);

            // Assert
            _excelBuilder.Verify(ex => ex.SetRowValue(_sheet.Object, _templateSheet.Object,
                RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo), Times.Once);
        }

        [Fact]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var excel = CreateExcel();
            var rows = CreateRows();

            var sortedData = new SortableResultModel<BonusInfoSummaryItemDTO>();
            sortedData.DataList = new List<BonusInfoSummaryItemDTO> { _fixture.Create<BonusInfoSummaryItemDTO>() };
            sortedData.TotalData = _fixture.Create<BonusInfoSummaryItemDTO>();
            sortedData.TotalRows = 1;

            var data = new BonusInfoSummaryDTO() { SortedData = sortedData };

            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            metaModel.ClaimStatus = "1";

            // Action
            excel.Export(data, metaModel);
            var bodyRow = rows[2];

            // Assert
            AssertRowData(bodyRow, data.SortedData.DataList);
        }

        [Fact]
        public void AssignTotalData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var excel = CreateExcel();
            var rows = CreateRows();

            var sortedData = new SortableResultModel<BonusInfoSummaryItemDTO>();
            sortedData.DataList = new List<BonusInfoSummaryItemDTO> { _fixture.Create<BonusInfoSummaryItemDTO>() };
            sortedData.TotalData = _fixture.Create<BonusInfoSummaryItemDTO>();
            sortedData.TotalRows = 1;

            var data = new BonusInfoSummaryDTO() { SortedData = sortedData };

            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            metaModel.ClaimStatus = "1";

            // Action
            excel.Export(data, metaModel);
            var totalRow = rows[3];

            // Assert
            AssertTotalCellData(totalRow, data.SortedData.TotalData);
        }

        private IList<Mock<IRow>> CreateRows()
        {
            var rowList = CreateRowList();
            var headerRow = rowList[0];
            var headerRow2 = rowList[1];
            var bodyRow = rowList[2];
            var totalRow = rowList[3];

            _excelBuilder.Setup(ex => ex.CopyRow(_sheet.Object, _templateSheet.Object, RowHeaderTemplate, RowHeaderTemplate))
                .Returns(headerRow.Object);
            _excelBuilder.Setup(ex => ex.CopyRow(_sheet.Object, _templateSheet.Object, RowHeaderTemplate + 1, RowHeaderTemplate + 1))
                .Returns(headerRow2.Object);
            _excelBuilder.Setup(ex => ex.CopyRow(_sheet.Object, _templateSheet.Object, It.Is<int>(x => x == RowContentTemplate || x == RowNoInfoTemplate), RowContentTemplate))
                .Returns(bodyRow.Object);
            _excelBuilder.Setup(ex => ex.CopyRow(_sheet.Object, _templateSheet.Object, RowTotalDataTemplate, RowContentTemplate + 1))
                .Returns(totalRow.Object);

            return rowList;
        }

        private IList<Mock<IRow>> CreateRowList()
        {
            var headerRow = ExcelTestHelper.MockGetRow(_sheet, ColumnTemplateCount, RowHeaderTemplate);
            var headerRow2 = ExcelTestHelper.MockGetRow(_sheet, ColumnTemplateCount, RowHeaderTemplate + 1);
            var bodyRow = ExcelTestHelper.MockGetRow(_sheet, ColumnTemplateCount, RowContentTemplate);
            var totalRow = ExcelTestHelper.MockGetRow(_sheet, ColumnTemplateCount, RowTotalDataTemplate);

            return new List<Mock<IRow>> { headerRow, headerRow2, bodyRow, totalRow };
        }

        private BonusInfoSummaryExcel CreateExcel()
        {
            _excelBuilder = new Mock<IExcelBuilder>();
            var stringFilePath = _fixture.Create<string>();
            _fileProvider = new Mock<IFileProvider>();
            _fileProvider.Setup(f => f.GetFileInfo(TemplatePath).PhysicalPath).Returns(stringFilePath);

            var workbook = new Mock<IWorkbook>();
            _excelBuilder.Setup(eb => eb.LoadTemplate(stringFilePath)).Returns(workbook.Object);

            _sheet = new Mock<ISheet>();
            _sheet.Setup(s => s.LastRowNum).Returns(RowContentTemplate);
            _excelBuilder.Setup(eb => eb.CloneSheet(workbook.Object, null, SheetName)).Returns(_sheet.Object);

            _templateSheet = new Mock<ISheet>();
            workbook.Setup(w => w.GetSheet(It.Is<string>(x => x == TemplateSheetName))).Returns(_templateSheet.Object);

            return new BonusInfoSummaryExcel(_excelBuilder.Object, _fileProvider.Object);
        }

        private void AssertRowData(Mock<IRow> row, IEnumerable<BonusInfoSummaryItemDTO> data)
        {
            AssertCommonCellData(row, data.FirstOrDefault());
        }

        private void AssertTotalCellData(Mock<IRow> totalRow, BonusInfoSummaryItemDTO totalData)
        {
            Mock.Get(totalRow.Object.Cells[ColumnBonusName]).Verify(c => c.SetCellValue(ViewResources.Total), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnStatusRunning]).Verify(c => c.SetCellValue(totalData.Running), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnStatusCancel]).Verify(c => c.SetCellValue(totalData.Canceled), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnStatusFullfill]).Verify(c => c.SetCellValue(totalData.Fulfill), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnStatusOtherPlatform]).Verify(c => c.SetCellValue(totalData.OtherPlatform), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnTotalClaimants]).Verify(c => c.SetCellValue(Convert.ToDouble(totalData.TotalClaimants)), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnTotalClaimedAmount]).Verify(c => c.SetCellValue(Convert.ToDouble(totalData.TotalClaimedAmount)), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnTotalDeposit]).Verify(c => c.SetCellValue(Convert.ToDouble(totalData.TotalDeposit)), Times.Once);
            Mock.Get(totalRow.Object.Cells[ColumnTotalWithdrawal]).Verify(c => c.SetCellValue(Convert.ToDouble(totalData.TotalWithdrawal)), Times.Once);
        }

        private void AssertCommonCellData(Mock<IRow> row, BonusInfoSummaryItemDTO item)
        {
            Mock.Get(row.Object.Cells[ColumnBonusName]).Verify(c => c.SetCellValue(item.BonusName), Times.Once);
            Mock.Get(row.Object.Cells[ColumnBonusCode]).Verify(c => c.SetCellValue(item.BonusCode), Times.Once);
            Mock.Get(row.Object.Cells[ColumnBonusType]).Verify(c => c.SetCellValue(item.BonusTypeText), Times.Once);
            Mock.Get(row.Object.Cells[ColumnStartDate]).Verify(c => c.SetCellValue(item.StartDate.ToString(DateCellFormat)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnEndDate]).Verify(c => c.SetCellValue(item.EndDate.ToString(DateCellFormat)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnBonusStatus]).Verify(c => c.SetCellValue(item.BonusStatusText), Times.Once);
            Mock.Get(row.Object.Cells[ColumnStatusRunning]).Verify(c => c.SetCellValue(item.Running), Times.Once);
            Mock.Get(row.Object.Cells[ColumnStatusFullfill]).Verify(c => c.SetCellValue(item.Fulfill), Times.Once);
            Mock.Get(row.Object.Cells[ColumnStatusOtherPlatform]).Verify(c => c.SetCellValue(item.OtherPlatform), Times.Once);
            Mock.Get(row.Object.Cells[ColumnStatusCancel]).Verify(c => c.SetCellValue(item.Canceled), Times.Once);
            Mock.Get(row.Object.Cells[ColumnTotalClaimants]).Verify(c => c.SetCellValue(Convert.ToDouble(item.TotalClaimants)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnTotalClaimedAmount]).Verify(c => c.SetCellValue(Convert.ToDouble(item.TotalClaimedAmount)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnTotalDeposit]).Verify(c => c.SetCellValue(Convert.ToDouble(item.TotalDeposit)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnTotalWithdrawal]).Verify(c => c.SetCellValue(Convert.ToDouble(item.TotalWithdrawal)), Times.Once);
        }
    }
}