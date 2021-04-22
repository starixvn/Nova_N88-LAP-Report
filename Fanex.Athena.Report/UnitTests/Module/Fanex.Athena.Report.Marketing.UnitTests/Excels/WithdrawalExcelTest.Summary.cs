using AutoFixture;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Moq;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public partial class WithdrawalExcelTest
    {
        [Fact]
        public void Export_TemplateSummarySheet_CorrectTemplate()
        {
            //Arrange
            SetupTemplate();

            var mockRow = new Mock<IRow>();

            _mockExcelBuilder.Setup(_ => _.CloneSheet(It.IsAny<IWorkbook>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Mock.Of<ISheet>());
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(15).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            //Act
            _ = _target.Export(_fixture.Create<WithdrawalSummaryDTO>(),
                _fixture.CreateMany<WithdrawalDetailDTO>(),
                _fixture.Create<WithdrawalExportMetaModel>());

            //Assert
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Summary"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleSummarySheet_CorrectTitle()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<WithdrawalExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(15).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            var expectedTitle = $"{ViewResources.WithdrawalAmount}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(_fixture.Create<WithdrawalSummaryDTO>(),
                _fixture.CreateMany<WithdrawalDetailDTO>(),
                metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 4, expectedTitle), Times.Once);
        }


        [Fact]
        public void Export_AssingHeaderNameSummarySheet_CorrectHeaderName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<WithdrawalExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            var mockCellName = new Mock<ICell>();
            var mockCellReportDate = new Mock<ICell>();
            var mockCellCompareDate = new Mock<ICell>();
            var mockCellChangeRate = new Mock<ICell>();
            var listCell = new List<ICell>
            {
                mockCellName.Object, mockCellReportDate.Object, mockCellCompareDate.Object, mockCellChangeRate.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(6).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 1)).Returns(mockRowHeader.Object);

            //Act
            _ = _target.Export(_fixture.Create<WithdrawalSummaryDTO>(),
                Enumerable.Empty<WithdrawalDetailDTO>(),
                metaModel);

            //Assert
            mockCellName.Verify(_ => _.SetCellValue(string.Empty), Times.Once);
            mockCellReportDate.Verify(_ => _.SetCellValue(ViewResources.ReportDate), Times.Once);
            mockCellCompareDate.Verify(_ => _.SetCellValue(ViewResources.CompareDate), Times.Once);
            mockCellChangeRate.Verify(_ => _.SetCellValue(ViewResources.ChangePercent), Times.Once);
        }

        private void SetupSummarySheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Summary")).Returns(templateSheet);
        }
    }
}