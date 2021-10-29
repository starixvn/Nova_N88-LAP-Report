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
    public partial class BonusClaimedExcelTest
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
                _fixture.CreateMany<BonusClaimedDetailDTO>(),
                _fixture.Create<BonusClaimedExportMetaModel>());

            //Assert
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Summary"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleSummarySheet_CorrectTitle()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<BonusClaimedExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            var expectedTitle = $"{ViewResources.BonusAmountClaimed}, " +
                          $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                          $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(FakeSummaryData(),
                _fixture.CreateMany<BonusClaimedDetailDTO>(),
                metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 5, expectedTitle), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Export_AssingTotalHeaderNameSummarySheet_CorrectHeaderName(bool allowedCompare)
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<BonusClaimedExportMetaModel>();
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
               _fixture.CreateMany<BonusClaimedDetailDTO>(),
               metaModel);

            //Assert
            mockCellName.Verify(_ => _.SetCellValue(ViewResources.TotalBonusAmountClaimed), Times.Once);
            mockCellReportDate.Verify(_ => _.SetCellValue(ViewResources.ReportDate), Times.Once);
            if(metaModel.AllowedCompare)
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

        [Fact]
        public void Export_AssingCampaignHeaderNameSummarySheet_CorrectHeaderName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<BonusClaimedExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            var mockCellBonusCode = new Mock<ICell>(); 
            var mockCellCurrency = new Mock<ICell>();
            var mockCellBonusName = new Mock<ICell>();
            var mockCellNumberOfBonusClaimed = new Mock<ICell>();
            var mockCellTotalBonusAmountClaimed = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellBonusCode.Object, mockCellCurrency.Object,  mockCellBonusName.Object, mockCellNumberOfBonusClaimed.Object, mockCellTotalBonusAmountClaimed.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 4, 4)).Returns(mockRowHeader.Object);

            //Act
            _ = _target.Export(FakeSummaryData(),
               _fixture.CreateMany<BonusClaimedDetailDTO>(),
               metaModel);

            //Assert
            mockCellBonusCode.Verify(_ => _.SetCellValue(ViewResources.BonusCode), Times.Once);
            mockCellCurrency.Verify(_ => _.SetCellValue(ViewResources.Currency), Times.Once);
            mockCellBonusName.Verify(_ => _.SetCellValue(ViewResources.BonusName), Times.Once);
            mockCellNumberOfBonusClaimed.Verify(_ => _.SetCellValue(ViewResources.NoOfUserClaimingBonus), Times.Once);
            mockCellTotalBonusAmountClaimed.Verify(_ => _.SetCellValue(ViewResources.TotalBonusAmountClaimed), Times.Once);
        }

        private void SetupSummarySheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Summary")).Returns(templateSheet);
        }
    }
}