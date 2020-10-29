using AutoFixture;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Moq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public partial class BonusClaimedExcelTest
    {
        [Fact]
        public void Export_TemplateDetailSheet_CorrectTemplate()
        {
            //Arrange
            SetupTemplate();

            var mockRow = new Mock<IRow>();
            _mockExcelBuilder.Setup(_ => _.CloneSheet(It.IsAny<IWorkbook>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Mock.Of<ISheet>());
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(7).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            //Act
            _ = _target.Export(_fixture.Create<BonusClaimedSummaryDTO>(),
                 _fixture.CreateMany<BonusClaimedDetailDTO>(),
                 _fixture.Create<BonusClaimedExportMetaModel>());

            //Assert
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Details"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleDetailSheet_CorrectTitle()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusClaimedExportMetaModel>();

            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupTemplate();
            SetupDetailSheet(sheet, templateSheet);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(7).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            var expectedTitle = $"{ViewResources.BonusAmountClaimed}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(_fixture.Create<BonusClaimedSummaryDTO>(),
                  _fixture.CreateMany<BonusClaimedDetailDTO>(),
                  metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 7, expectedTitle), Times.Once);
        }

        [Fact]
        public void Export_AssingHeaderNameDetailSheet_CorrectHeaderName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<BonusClaimedExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockCellRowId = new Mock<ICell>();
            var mockCellBonus = new Mock<ICell>();
            var mockCellUsername = new Mock<ICell>();
            var mockCellRegisteredDate = new Mock<ICell>();
            var mockCellDepositAmount = new Mock<ICell>();
            var mockCellWithdrawalAmount = new Mock<ICell>();
            var mockCellBonusAmountClaimed = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellRowId.Object, mockCellBonus.Object, mockCellUsername.Object,
                mockCellRegisteredDate.Object, mockCellDepositAmount.Object, mockCellWithdrawalAmount.Object, mockCellBonusAmountClaimed.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(7).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 1)).Returns(mockRowHeader.Object);

            //Act
            _ = _target.Export(_fixture.Create<BonusClaimedSummaryDTO>(),
                  _fixture.CreateMany<BonusClaimedDetailDTO>(),
                metaModel);

            //Assert
            mockCellRowId.Verify(_ => _.SetCellValue("#"), Times.Once);
            mockCellBonus.Verify(_ => _.SetCellValue(ViewResources.Bonus), Times.Once);
            mockCellUsername.Verify(_ => _.SetCellValue(ViewResources.Username), Times.Once);
            mockCellRegisteredDate.Verify(_ => _.SetCellValue(ViewResources.RegisteredDate), Times.Once);
            mockCellDepositAmount.Verify(_ => _.SetCellValue(ViewResources.TotalDepositAmount), Times.Once);
            mockCellWithdrawalAmount.Verify(_ => _.SetCellValue(ViewResources.TotalWithdrawalAmount), Times.Once);
            mockCellBonusAmountClaimed.Verify(_ => _.SetCellValue(ViewResources.TotalBonusAmountClaimed), Times.Once);
        }

        [Theory]
        [InlineData(1230.123, 412.6, 432)]
        [InlineData(0, 0, 0)]
        [InlineData(1230.123, 123.45, 0)]
        public void Export_AssingDataDetailSheet_CorrectData(double depositAmount, double withdrawalAmount, double bonusAmount)
        {
            //Arrange
            SetupTemplate();

            var fakeDetailData = _fixture.CreateMany<BonusClaimedDetailDTO>(1);
            var fakeRowData = fakeDetailData.First();
            fakeRowData.DepositAmount = (decimal)depositAmount;
            fakeRowData.WithdrawalAmount = (decimal)withdrawalAmount;
            fakeRowData.BonusAmount = (decimal)bonusAmount;

            var metaModel = _fixture.Create<BonusClaimedExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockCellRowId = new Mock<ICell>();
            var mockCellBonus = new Mock<ICell>();
            var mockCellUsername = new Mock<ICell>();
            var mockCellRegisteredDate = new Mock<ICell>();
            var mockCellDepositAmount = new Mock<ICell>();
            var mockCellWithdrawalAmount = new Mock<ICell>();
            var mockCellBonusAmountClaimed = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellRowId.Object, mockCellBonus.Object, mockCellUsername.Object,
                mockCellRegisteredDate.Object, mockCellDepositAmount.Object, mockCellWithdrawalAmount.Object, mockCellBonusAmountClaimed.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(7).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowDataItem.Object);

            //Act
            _ = _target.Export(_fixture.Create<BonusClaimedSummaryDTO>(),
                fakeDetailData,
                metaModel);

            //Assert

            mockCellRowId.Verify(_ => _.SetCellValue(fakeRowData.RowId), Times.Once);
            mockCellBonus.Verify(_ => _.SetCellValue(fakeRowData.BonusCode), Times.Once);
            mockCellUsername.Verify(_ => _.SetCellValue(fakeRowData.Username), Times.Once);
            mockCellRegisteredDate.Verify(_ => _.SetCellValue(fakeRowData.RegisteredDateDisplay), Times.Once);
            AssertDecimalCell(mockCellDepositAmount, fakeRowData.DepositAmount, Times.Once);
            AssertDecimalCell(mockCellWithdrawalAmount, fakeRowData.WithdrawalAmount, Times.Once);
            AssertDecimalCell(mockCellBonusAmountClaimed, fakeRowData.BonusAmount, Times.Once);
        }

        protected void AssertDecimalCell(Mock<ICell> mockCell, decimal value, Func<Times> times)
        {
            if (value == 0)
            {
                mockCell.Verify(_ => _.SetCellValue("-"), times);
            }
            else
            {
                mockCell.Verify(_ => _.SetCellValue((double)value), times);
            }
        }

        private void SetupDetailSheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Details")).Returns(templateSheet);
        }
    }
}