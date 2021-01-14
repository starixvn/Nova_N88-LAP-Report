using AutoFixture;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.UnitTestUtils.Excels;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.Helpers;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Moq;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.WinLossAnalysis.UnitTests.Excels
{
    public class WinLossExcelTest : ExcelTest<WinLossExcel>
    {
        [Fact]
        public void Export_Template_CorrectTemplate()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            var mockRow = new Mock<IRow>();

            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(7).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>())).Returns(mockRow.Object);

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/WinLossReports/WinLossReport.xlsx")).Returns(_mockFileInfo.Object);

            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            var metaModel = _fixture.Create<ExportWinLossMetaModel>();
            metaModel.OrderCols = Enumerable.Empty<string>();

            //Act
            var actualWorkBoook = _target.Export(
                new ResultModel<WinLossRecordViewModel> { DataList = new List<WinLossRecordViewModel>() },
                metaModel);

            //Assert
            Assert.Equal(_mockWorkbook.Object, actualWorkBoook);
        }

        [Fact]
        public void Export_AssignTitleSummarySheet_CorrectTitle()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<ExportWinLossMetaModel>();
            metaModel.OrderCols = Enumerable.Empty<string>();
            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSheet(sheet, templateSheet);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(7).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>())).Returns(mockRow.Object);

            var expectedTitle = $"{ViewResources.WinLossAnalysis}, {metaModel.FromDate.ToString("MM/dd/yyyy")} - {metaModel.ToDate.ToString("MM/dd/yyyy")}," +
                $" {string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant())}," +
                $" {DataHelper.GetTimezoneName(metaModel.Timezone)}";

            //Act
            _ = _target.Export(new ResultModel<WinLossRecordViewModel> { DataList = new List<WinLossRecordViewModel>() },
                metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 15, expectedTitle), Times.Once);
            _mockExcelBuilder.Verify(_ => _.ClearTemplate(sheet, 3), Times.Once);
        }

        [Fact]
        public void Export_AssingParentData_CorrectData()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<ExportWinLossMetaModel>();
            metaModel.OrderCols = new List<string> { "turnover" };

            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSheet(sheet, templateSheet);

            var mockCellGroup = new Mock<ICell>();
            var mockCellCurrency = new Mock<ICell>();
            var mockCellTunrover = new Mock<ICell>();
            var listCell = new List<ICell>
            {
                mockCellGroup.Object, mockCellCurrency.Object, mockCellTunrover.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(3).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 3, 3, It.IsAny<int[]>())).Returns(mockRowDataItem.Object);

            var fakeRowData = new WinLossRecordViewModel
            {
                Group = _fixture.Create<string>(),
                Turnover = _fixture.Create<decimal>(),
                DrillDownData = Enumerable.Empty<WinLossRecordViewModel>()
            };

            var fakeRawData = new ResultModel<WinLossRecordViewModel>
            {
                DataList = new List<WinLossRecordViewModel>
                {
                   fakeRowData
                },
                TotalData = new WinLossRecordViewModel(),
                TotalRows = 1
            };

            //Act
            _ = _target.Export(fakeRawData, metaModel);

            //Assert
            mockCellGroup.Verify(_ => _.SetCellValue(fakeRowData.Group), Times.Once);
            mockCellCurrency.Verify(_ => _.SetCellValue("All"), Times.Once);
            mockCellTunrover.Verify(_ => _.SetCellValue((double)fakeRowData.Turnover), Times.Once);
        }

        private void SetupSheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Template", "Data")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Template")).Returns(templateSheet);
        }
    }
}