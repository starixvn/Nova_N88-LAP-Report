using AutoFixture;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Helpers;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using Moq;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public class PerformanceHighlightExcelTest
    {
        private readonly PerformanceHighlightExcel _target;
        private readonly Mock<IExcelBuilder> _mockExcelBuilder;
        private readonly Mock<IFileProvider> _mockFileProvider;
        private readonly Mock<IFileInfo> _mockFileInfo;
        private readonly Mock<IWorkbook> _mockWorkbook;
        private readonly IFixture _fixture = new Fixture();

        public PerformanceHighlightExcelTest()
        {
            _mockExcelBuilder = new Mock<IExcelBuilder>();
            _mockFileProvider = new Mock<IFileProvider>();
            _mockFileInfo = new Mock<IFileInfo>();
            _mockWorkbook = new Mock<IWorkbook>();
            _target = new PerformanceHighlightExcel(_mockExcelBuilder.Object, _mockFileProvider.Object);
        }

        [Fact]
        public void Export_TemplateSheet_CorrectTemplateSheet()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            var mockRow = new Mock<IRow>();

            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(3).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/Marketing/PerformanceHighlight.xls")).Returns(_mockFileInfo.Object);

            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            //Act
            _ = _target.Export(_fixture.Create<IEnumerable<PerformanceHighlightExportDTO>>(), _fixture.Create<PerformanceHighlightExportAllMetaModel>());

            //Assert
            _mockWorkbook.Verify(_ => _.GetSheet("Template"), Times.Once);
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Template", "Data"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitle_CorrectTitle()
        {
            //Arrange
            var metaModel = _fixture.Create<PerformanceHighlightExportAllMetaModel>();

            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(3).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            var expectedTitle = $"{ViewResources.Marketing} - {ViewResources.BusinessPerformanceHighlight}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(_fixture.Create<IEnumerable<PerformanceHighlightExportDTO>>(), metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 3, expectedTitle), Times.Once);
        }

        [Fact]
        public void Export_AssignHeader_CorrectHeader()
        {
            //Arrange
            var metaModel = _fixture.Create<PerformanceHighlightExportAllMetaModel>();

            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();

            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);

            var mockCellName = new Mock<ICell>();
            var mockCelllRange = new Mock<ICell>();
            var mockCellAllTime = new Mock<ICell>();

            var listCell = new List<ICell>
            {
               mockCellName.Object,
               mockCelllRange.Object,
               mockCellAllTime.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(3).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);

            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 1)).Returns(mockRowHeader.Object);
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            //Act
            _ = _target.Export(_fixture.Create<IEnumerable<PerformanceHighlightExportDTO>>(), metaModel);

            //Assert
            mockCellName.Verify(_ => _.SetCellValue(string.Empty), Times.Once);
            mockCelllRange.Verify(_ => _.SetCellValue($"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}"), Times.Once);
            mockCellAllTime.Verify(_ => _.SetCellValue(ViewResources.AllTime), Times.Once);
        }

        [Theory]
        [InlineData(9123213.23, 1235.1235, (byte)1)]
        [InlineData(9123213.23, null, (byte)1)]
        [InlineData(8909, null, (byte)3)]
        [InlineData(123768, 2132, (byte)3)]
        public void Export_AssignRowData_CorrectRowData(double? dateRange, double? allRange, byte? dataType)
        {
            //Arrange
            var metaModel = _fixture.Create<PerformanceHighlightExportAllMetaModel>();
            var fakeRowData = new PerformanceHighlightExportDTO
            {
                Name = _fixture.Create<string>(),
                DateRange = (decimal?)dateRange,
                AllRange = (decimal?)allRange,
                DataType = dataType
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
            var mockCellAllTime = new Mock<ICell>();

            var listCell = new List<ICell>
            {
               mockCellName.Object,
               mockCellRange.Object,
               mockCellAllTime.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(3).ToList());
            mockRowData.Setup(_ => _.Cells).Returns(listCell);

            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowData.Object);
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, It.IsAny<string>(), It.IsAny<string>())).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(templateSheet);

            var cellRangeValue = NumberHepler.Format(fakeRowData.DateRange);
            var cellAllTimeValue = NumberHepler.Format(fakeRowData.AllRange);

            //Act
            _ = _target.Export(new List<PerformanceHighlightExportDTO> { fakeRowData }, metaModel);

            //Assert

            mockCellName.Verify(_ => _.SetCellValue(fakeRowData.Name), Times.Once);
            mockCellRange.Verify(_ => _.SetCellValue(!string.IsNullOrEmpty(cellRangeValue) && fakeRowData.DataType == 3 ? cellRangeValue + "%" : cellRangeValue), Times.Once);
            mockCellAllTime.Verify(_ => _.SetCellValue(!string.IsNullOrEmpty(cellAllTimeValue) && fakeRowData.DataType == 3 ? cellAllTimeValue + "%" : cellAllTimeValue), Times.Once);
        }

        private IEnumerable<ICell> MockListCell(int count)
        {
            var mockListCell = new List<ICell>();
            for (int i = 0; i < count; i++)
            {
                mockListCell.Add(Mock.Of<ICell>());
            }
            return mockListCell;
        }
    }
}