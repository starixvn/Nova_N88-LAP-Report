using AutoFixture;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.UnitTestUtils.Excels;
using Moq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public class PerformanceHighlightExcelTest : ExcelTest<PerformanceHighlightExcel>
    {
        [Fact]
        public void Export_TemplateSheet_CorrectTemplateSheet()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            var mockRow = new Mock<IRow>();

            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(4).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/Marketing/PerformanceHighlight.xls")).Returns(_mockFileInfo.Object);

            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            //Act
            _ = _target.Export(FakeRawData(), FakeMetaModel());

            //Assert
            _mockWorkbook.Verify(_ => _.GetSheet("Template"), Times.Once);
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Template", "DataEng"), Times.Once);
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Template", "DataVn"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitle_CorrectTitle()
        {
            //Arrange
            var metaModel = FakeMetaModel();

            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(4).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            var expectedTitle = $"{ViewResources.Marketing} - {ViewResources.BusinessPerformanceHighlight}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                (string.IsNullOrEmpty(metaModel.CurrencyNameList) ? ViewResources.AllCurrencies : metaModel.CurrencyNameList) + ", " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(FakeRawData(), metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 4, expectedTitle), Times.Exactly(2));
        }

        [Theory]
        [InlineData(402, "All time", "Toàn bộ dữ liệu")]
        [InlineData(436, "From 09/12/2020", "Từ 09/12/2020")]
        [InlineData(437, "From 08/14/2020", "Từ 08/14/2020")]
        public void Export_AssignHeader_CorrectHeader(int brandId, string launchDateHeaderExpected, string launchDateHeaderVNExpected)
        {
            //Arrange
            var metaModel = FakeMetaModel(brandId);

            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();

            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);

            var mockCellName = new Mock<ICell>();
            var mockCelllRange = new Mock<ICell>();
            var mockCellMonthToDate = new Mock<ICell>();
            var mockCellAllTime = new Mock<ICell>();

            var listCell = new List<ICell>
            {
               mockCellName.Object,
               mockCelllRange.Object,
               mockCellMonthToDate.Object,
               mockCellAllTime.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(4).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);

            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 1)).Returns(mockRowHeader.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 2)).Returns(mockRowHeader.Object);
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            //Act
            _ = _target.Export(FakeRawData(), metaModel);

            //Assert
            mockCellName.Verify(_ => _.SetCellValue(string.Empty), Times.Exactly(2));
            mockCelllRange.Verify(_ => _.SetCellValue($"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}"), Times.Exactly(4));
            mockCellMonthToDate.Verify(_ => _.SetCellValue("MTD"), Times.Exactly(2));
            mockCellMonthToDate.Verify(_ => _.SetCellValue("Từ đầu tháng"), Times.Exactly(2));
            mockCellAllTime.Verify(_ => _.SetCellValue(launchDateHeaderExpected), Times.Exactly(2));
            mockCellAllTime.Verify(_ => _.SetCellValue(launchDateHeaderVNExpected), Times.Exactly(2));
        }

        [Theory]
        [InlineData(9123213.23, 1235.1235, 21321.634)]
        [InlineData(null, null, null)]
        public void Export_AssignRowData_IntegerDataType_CorrectRowData(double? dateRange, double? monthToDate, double? allRange)
        {
            //Arrange
            var metaModel = FakeMetaModel();
            var fakeRowData = new PerformanceHighlightCommonExportDTO
            {
                Name = _fixture.Create<string>(),
                NameVN = _fixture.Create<string>(),
                DateRange = (decimal?)dateRange,
                MonthToDate = (decimal?)monthToDate,
                AllRange = (decimal?)allRange,
                DataType = DbColumnDataType.Integer
            };

            var mockRow = new Mock<IRow>();
            var mockRowData = new Mock<IRow>();

            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);

            var mockCellName = new Mock<ICell>();
            var mockCellRange = new Mock<ICell>();
            var mockMonthToDate = new Mock<ICell>();
            var mockCellAllTime = new Mock<ICell>();

            var listCell = new List<ICell>
            {
               mockCellName.Object,
               mockCellRange.Object,
               mockMonthToDate.Object,
               mockCellAllTime.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(4).ToList());
            mockRowData.Setup(_ => _.Cells).Returns(listCell);

            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowData.Object);

            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            var fakeData = FakeRawData();
            fakeData.CommonData = new List<PerformanceHighlightCommonExportDTO> { fakeRowData };

            //Act
            _ = _target.Export(fakeData, metaModel);

            //Assert
            AssertData(fakeRowData, mockCellName, mockCellRange, mockMonthToDate, mockCellAllTime);
        }

        [Theory]
        [InlineData(9123213.23, 1235.1235, 21321.634)]
        [InlineData(null, null, null)]
        public void Export_AssignRowData_MoneyDataType_CorrectRowData(double? dateRange, double? monthToDate, double? allRange)
        {
            //Arrange
            var metaModel = FakeMetaModel();
            var fakeRowData = new PerformanceHighlightCommonExportDTO
            {
                Name = _fixture.Create<string>(),
                NameVN = _fixture.Create<string>(),
                DateRange = (decimal?)dateRange,
                MonthToDate = (decimal?)monthToDate,
                AllRange = (decimal?)allRange,
                DataType = DbColumnDataType.Money
            };

            var mockRow = new Mock<IRow>();
            var mockRowData = new Mock<IRow>();

            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);

            var mockCellName = new Mock<ICell>();
            var mockCellRange = new Mock<ICell>();
            var mockMonthToDate = new Mock<ICell>();
            var mockCellAllTime = new Mock<ICell>();

            var listCell = new List<ICell>
            {
               mockCellName.Object,
               mockCellRange.Object,
               mockMonthToDate.Object,
               mockCellAllTime.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(4).ToList());
            mockRowData.Setup(_ => _.Cells).Returns(listCell);

            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 3, 2)).Returns(mockRowData.Object);

            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            var fakeData = FakeRawData();
            fakeData.CommonData = new List<PerformanceHighlightCommonExportDTO> { fakeRowData };

            //Act
            _ = _target.Export(fakeData, metaModel);

            //Assert
            AssertData(fakeRowData, mockCellName, mockCellRange, mockMonthToDate, mockCellAllTime);
        }

        [Theory]
        [InlineData(9123213.23, 1235.1235, 21321.634)]
        [InlineData(null, null, null)]
        public void Export_AssignRowData_PercentDataType_CorrectRowData(double? dateRange, double? monthToDate, double? allRange)
        {
            //Arrange
            var metaModel = FakeMetaModel();
            var fakeRowData = new PerformanceHighlightCommonExportDTO
            {
                Name = _fixture.Create<string>(),
                NameVN = _fixture.Create<string>(),
                DateRange = (decimal?)dateRange,
                MonthToDate = (decimal?)monthToDate,
                AllRange = (decimal?)allRange,
                DataType = DbColumnDataType.Percent
            };

            var mockRow = new Mock<IRow>();
            var mockRowData = new Mock<IRow>();

            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);

            var mockCellName = new Mock<ICell>();
            var mockCellRange = new Mock<ICell>();
            var mockMonthToDate = new Mock<ICell>();
            var mockCellAllTime = new Mock<ICell>();

            var listCell = new List<ICell>
            {
               mockCellName.Object,
               mockCellRange.Object,
               mockMonthToDate.Object,
               mockCellAllTime.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(4).ToList());
            mockRowData.Setup(_ => _.Cells).Returns(listCell);

            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 4, 2)).Returns(mockRowData.Object);

            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            var fakeData = FakeRawData();
            fakeData.CommonData = new List<PerformanceHighlightCommonExportDTO> { fakeRowData };

            //Act
            _ = _target.Export(fakeData, metaModel);

            //Assert
            AssertData(fakeRowData, mockCellName, mockCellRange, mockMonthToDate, mockCellAllTime);
        }

        private void AssertData(PerformanceHighlightCommonExportDTO fakeRowData, Mock<ICell> mockCellName, Mock<ICell> mockCellRange, Mock<ICell> mockMonthToDate, Mock<ICell> mockCellAllTime)
        {
            mockCellName.Verify(_ => _.SetCellValue(fakeRowData.Name), Times.Once);
            mockCellName.Verify(_ => _.SetCellValue(fakeRowData.NameVN), Times.Once);
            VerifyDecimalCell(mockCellRange, fakeRowData.DateRange, Times.Exactly(2));
            VerifyDecimalCell(mockMonthToDate, fakeRowData.MonthToDate, Times.Exactly(2));
            VerifyDecimalCell(mockCellAllTime, fakeRowData.AllRange, Times.Exactly(2));
        }

        private void VerifyDecimalCell(Mock<ICell> mockCell, decimal? value, Times times)
        {
            if (!value.HasValue)
            {
                mockCell.Verify(_ => _.SetCellValue(string.Empty), times);
            }
            else if (value == 0)
            {
                mockCell.Verify(_ => _.SetCellValue("-"), times);
            }
            else
            {
                mockCell.Verify(_ => _.SetCellValue((double)value), times);
            }
        }

        private PerformanceHighlightExportAllMetaModel FakeMetaModel(int brandId = 0)
        {
            var metaModel = _fixture.Create<PerformanceHighlightExportAllMetaModel>();
            metaModel.BrandId = brandId != 0 ? brandId : new[] { Sites.N88SiteId, Sites.N8BSiteId, Sites.N8GSiteId }[new Random().Next(0, 2)];
            return metaModel;
        }

        private PerformanceHighlightExportDTO FakeRawData()
        {
            return new PerformanceHighlightExportDTO
            {
                CommonData = Enumerable.Empty<PerformanceHighlightCommonExportDTO>(),
                RegistrationData = Enumerable.Empty<PerformanceHighlightRegistrationExportDTO>()
            };
        }
    }
}