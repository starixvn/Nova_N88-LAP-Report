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
    public partial class DepositAmountExcelTest
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
            _ = _target.Export(FakeSummaryData(),
               _fixture.CreateMany<DepositAmountDetailDTO>(),
               _fixture.Create<DepositAmountExportMetaModel>());

            //Assert
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Summary"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleSummarySheet_CorrectTitle()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            var expectedTitle = $"{ViewResources.DepositAmount}, " +
                          $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                          $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(FakeSummaryData(),
                  _fixture.CreateMany<DepositAmountDetailDTO>(),
                metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 4, expectedTitle), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Export_AssingTotalHeaderNameSummarySheet_CorrectHeaderName(bool allowedCompare)
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();
            metaModel.AllowedCompare = allowedCompare;

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

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 1)).Returns(mockRowHeader.Object);

            //Act
            _ = _target.Export(FakeSummaryData(),
                _fixture.CreateMany<DepositAmountDetailDTO>(),
                metaModel);

            //Assert
            mockCellName.Verify(_ => _.SetCellValue(ViewResources.Total), Times.Once);
            mockCellReportDate.Verify(_ => _.SetCellValue(ViewResources.ReportDate), Times.Once);
            if (metaModel.AllowedCompare)
            {
                mockCellCompareDate.Verify(_ => _.SetCellValue(ViewResources.CompareDate), Times.Once);
                mockCellChangeRate.Verify(_ => _.SetCellValue(ViewResources.ChangePercent), Times.Once);
            }
            else
            {
                mockCellCompareDate.Verify(_ => _.SetCellValue(ViewResources.CompareDate), Times.Never);
                mockCellChangeRate.Verify(_ => _.SetCellValue(ViewResources.ChangePercent), Times.Never);
            }
        }

        [Theory]
        [InlineData(1230.123, 412.6)]
        [InlineData(0, 0)]
        [InlineData(1230.123, 123.45)]
        public void Export_AssignSummarySheetTotalData_CorrectData(double depositAmount, double compareDepositAmount)
        {
            //Arrange
            SetupTemplate();

            var fakeSummaryData = FakeSummaryData();
            var fakeTotalData = fakeSummaryData.TotalData.First();
            fakeTotalData.DepositAmount = (decimal)depositAmount;
            fakeTotalData.CompareDepositAmount = (decimal)compareDepositAmount;
            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            var mockCellTotalName = new Mock<ICell>();
            var mockCellTotalDate = new Mock<ICell>();
            var mockCellTotalCompareDate = new Mock<ICell>();
            var mockCellTotalChangePercent = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellTotalName.Object, mockCellTotalDate.Object, mockCellTotalCompareDate.Object, mockCellTotalChangePercent.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(9).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowDataItem.Object);

            //Act
            _ = _target.Export(fakeSummaryData,
                  _fixture.CreateMany<DepositAmountDetailDTO>(),
                metaModel);

            //Assert

            mockCellTotalName.Verify(_ => _.SetCellValue(ViewResources.DepositAmount), Times.Once);
            AssertDecimalCell(mockCellTotalDate, fakeTotalData.DepositAmount, Times.Once);
            if (metaModel.AllowedCompare)
            {
                AssertDecimalCell(mockCellTotalCompareDate, fakeTotalData.CompareDepositAmount, Times.Once);
                mockCellTotalChangePercent.Verify(_ => _.SetCellValue(ViewResources.ChangePercent), Times.Once);
            }
            else
            {
                AssertDecimalCell(mockCellTotalCompareDate, fakeTotalData.CompareDepositAmount, Times.Never);
                mockCellTotalChangePercent.Verify(_ => _.SetCellValue(ViewResources.ChangePercent), Times.Never);
            }
        }

        [Fact]
        public void Export_AssignSummarySignupTotalRowData_CorrectData()
        {
            //Arrange
            SetupTemplate();

            var fakeSummaryData = FakeSummaryData();
            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            var mockCellSignupName = new Mock<ICell>();
            var mockCellSignupFirstDepositAmount = new Mock<ICell>();
            var mockCellSignupTotalDepositAmount = new Mock<ICell>();
            var mockCellSignupNoOfPlayers = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellSignupName.Object, mockCellSignupFirstDepositAmount.Object, mockCellSignupTotalDepositAmount.Object, mockCellSignupNoOfPlayers.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(9).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 7, It.IsAny<int>())).Returns(mockRowDataItem.Object);

            //Act
            _ = _target.Export(fakeSummaryData,
                  _fixture.CreateMany<DepositAmountDetailDTO>(),
                metaModel);

            //Assert

            mockCellSignupName.Verify(_ => _.SetCellValue(ViewResources.Total), Times.Once);
            mockCellSignupFirstDepositAmount.Verify(_ => _.SetCellValue((double)fakeSummaryData.TotalSignupData.TotalFirstDepositAmount), Times.Once);
            mockCellSignupTotalDepositAmount.Verify(_ => _.SetCellValue((double)fakeSummaryData.TotalSignupData.TotalDepositAmount), Times.Once);
            mockCellSignupNoOfPlayers.Verify(_ => _.SetCellValue(fakeSummaryData.TotalSignupData.TotalNoOfPlayers), Times.Once);
        }

        [Fact]
        public void Export_AssingCampaignHeaderNameSummarySheet_CorrectHeaderName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            var mockCellSignupUrl = new Mock<ICell>();
            var mockCellFirstDepositAmount = new Mock<ICell>();
            var mockCellTotalDepositAmount = new Mock<ICell>();
            var mockCellNumberOfPlayers = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellSignupUrl.Object, mockCellFirstDepositAmount.Object,  mockCellTotalDepositAmount.Object, mockCellNumberOfPlayers.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 4, 4)).Returns(mockRowHeader.Object);

            //Act
            _ = _target.Export(FakeSummaryData(),
                _fixture.CreateMany<DepositAmountDetailDTO>(),
                metaModel);

            //Assert
            mockCellSignupUrl.Verify(_ => _.SetCellValue(ViewResources.SignupUrl), Times.Once);
            mockCellFirstDepositAmount.Verify(_ => _.SetCellValue(ViewResources.FirstDepositAmount), Times.Once);
            mockCellTotalDepositAmount.Verify(_ => _.SetCellValue(ViewResources.TotalDepositAmount), Times.Once);
            mockCellNumberOfPlayers.Verify(_ => _.SetCellValue(ViewResources.NumberOfPlayers), Times.Once);
        }

        private void SetupSummarySheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Summary")).Returns(templateSheet);
        }
    }
}