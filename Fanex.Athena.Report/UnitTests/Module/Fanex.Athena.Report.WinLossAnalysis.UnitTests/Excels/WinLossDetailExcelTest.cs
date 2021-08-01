using AutoFixture;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.UnitTestUtils.Excels;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.Helpers;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Moq;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.WinLossAnalysis.UnitTests.Excels
{
    public class WinLossDetailExcelTest : ExcelTest<WinLossDetailExcel>
    {
        [Fact]
        public void Export_Template_CorrectTemplate()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            SetupRowsWithOrderCell(new Mock<IRow>(), 10);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());
            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/WinLossAnalysis/WinLossDetailReport.xlsx")).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            var metaModel = _fixture.Create<WinLossDetailMetaModel>();
            metaModel.OrderCols = Enumerable.Empty<string>();

            //Act
            var actualWorkBoook = _target.Export(Enumerable.Empty<WinLossDataDetailItemDTO>(), metaModel);

            //Assert
            Assert.Equal(_mockWorkbook.Object, actualWorkBoook);
        }

        [Fact]
        public void Export_AssignTitle_CorrectTitle()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<WinLossDetailMetaModel>();
            metaModel.OrderCols = Enumerable.Empty<string>();

            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();
            SetupSheet(sheet, templateSheet);
            SetupRowsWithOrderCell(new Mock<IRow>(), 10);

            var currency = metaModel.DrilldownInOriginalCurrency ? ViewResources.KeepOriginalCurrency : string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant());
            var expectedTitle = $"{ViewResources.WinLossAnalysis}, {metaModel.FromDate.ToString("MM/dd/yyyy")} - {metaModel.ToDate.ToString("MM/dd/yyyy")}," +
                $" {currency}," +
                $" {DataHelper.GetTimezoneName(metaModel.Timezone)}";

            //Act
            _ = _target.Export(Enumerable.Empty<WinLossDataDetailItemDTO>(), metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValueBeginCell(sheet, templateSheet, 0, 0, 20, metaModel.GroupBy == 2 ? 1 : 0, expectedTitle), Times.Once);
            _mockExcelBuilder.Verify(_ => _.ClearTemplate(sheet, 3), Times.Once);
        }

        [Fact]
        public void Export_AssignHeader_CorrectHeadersName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<WinLossDetailMetaModel>();
            metaModel.OrderCols = new[] { "turnover", "withdrawalAmount" };

            var headerTextList = new List<string>
            {
                ViewResources.Date,
                ViewResources.Month,
                ViewResources.Product,
                ViewResources.Brand,
                ViewResources.Username,
                ViewResources.Currency,
                ViewResources.SabaId,
                ViewResources.SabaSystemId,
                ViewResources.Turnover,
                ViewResources.TotalWithdrawalAmount
            };

            var sheet = new Mock<ISheet>();
            var templateSheet = new Mock<ISheet>();

            SetupSheet(sheet.Object, templateSheet.Object);
            SetupRowsWithOrderCell(new Mock<IRow>(), 10);

            var mockHeaderRow = ExcelTestHelper.MockGetRow(sheet, 10, 1);
            _mockExcelBuilder.Setup(ex => ex.CopyRow(sheet.Object, templateSheet.Object, 1, 1, It.IsAny<int[]>()))
                    .Returns(mockHeaderRow.Object);

            //Act
            _ = _target.Export(Enumerable.Empty<WinLossDataDetailItemDTO>(), metaModel);

            //Assert
            ExcelTestHelper.AssertSetTextRowHeader(mockHeaderRow.Object, headerTextList);
        }

        [Fact]
        public void Export_AssingRowData_CorrectData()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<WinLossDetailMetaModel>();
            metaModel.OrderCols = new[] { "turnover", "withdrawalAmount" };

            var sheet = new Mock<ISheet>();
            var templateSheet = new Mock<ISheet>();

            SetupSheet(sheet.Object, templateSheet.Object);
            SetupRowsWithOrderCell(new Mock<IRow>(), 10);

            var mockDataRow = ExcelTestHelper.MockGetRow(sheet, 10, 1);
            _mockExcelBuilder.Setup(ex => ex.CopyRow(sheet.Object, templateSheet.Object, 3, 3, It.IsAny<int[]>()))
                    .Returns(mockDataRow.Object);

            var fakeRowData = _fixture.CreateMany<WinLossDataDetailItemDTO>(1);

            //Act
            _ = _target.Export(fakeRowData, metaModel);

            //Assert
            AssertRowData(mockDataRow, fakeRowData.First());
        }

        [Theory]
        [InlineData(8, 14)]
        [InlineData(10, 12, "turnover", "withdrawalAmount")]
        public void Export_HideColumns_Correctly(int startHideRange, int countHideRange, params string[] orderCols)
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<WinLossDetailMetaModel>();
            metaModel.OrderCols = orderCols ?? Enumerable.Empty<string>();

            var sheet = new Mock<ISheet>();
            var templateSheet = new Mock<ISheet>();

            SetupSheet(sheet.Object, templateSheet.Object);
            SetupRowsWithOrderCell(new Mock<IRow>(), 10);

            var hideRange = Enumerable.Range(startHideRange, countHideRange);

            //Act
            _ = _target.Export(_fixture.CreateMany<WinLossDataDetailItemDTO>(1), metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.HideColumn(sheet.Object,
                It.Is<int[]>(param => Enumerable.SequenceEqual(param, hideRange))
                ), Times.Once);
        }

        private void AssertRowData(Mock<IRow> mockRowData, WinLossDataDetailItemDTO fakeRowData)
        {
            var cells = mockRowData.Object.Cells;
            Mock.Get(cells[0]).Verify(c => c.SetCellValue(fakeRowData.DateMonth), Times.Once);
            Mock.Get(cells[1]).Verify(c => c.SetCellValue(fakeRowData.DateMonth), Times.Once);
            Mock.Get(cells[2]).Verify(c => c.SetCellValue(fakeRowData.ProductName), Times.Once);
            Mock.Get(cells[3]).Verify(c => c.SetCellValue(fakeRowData.SiteName), Times.Once);
            Mock.Get(cells[4]).Verify(c => c.SetCellValue(fakeRowData.UserName), Times.Once);
            Mock.Get(cells[5]).Verify(c => c.SetCellValue(fakeRowData.CurrencyCode), Times.Once);
            Mock.Get(cells[6]).Verify(c => c.SetCellValue(fakeRowData.SabaId), Times.Once);
            Mock.Get(cells[7]).Verify(c => c.SetCellValue(fakeRowData.SabaSystemId), Times.Once);
            Mock.Get(cells[8]).Verify(c => c.SetCellValue((double)fakeRowData.Turnover), Times.Once);
            Mock.Get(cells[9]).Verify(c => c.SetCellValue((double)fakeRowData.WithdrawalAmount), Times.Once);
        }

        private void SetupSheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Template", "Data")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Template")).Returns(templateSheet);
        }
    }
}