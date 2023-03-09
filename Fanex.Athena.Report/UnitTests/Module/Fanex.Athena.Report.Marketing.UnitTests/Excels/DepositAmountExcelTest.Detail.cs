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
    public partial class DepositAmountExcelTest
    {
        [Fact]
        public void Export_TemplateDetailSheet_CorrectTemplate()
        {
            //Arrange
            SetupTemplate();

            var mockRow = new Mock<IRow>();
            _mockExcelBuilder.Setup(_ => _.CloneSheet(It.IsAny<IWorkbook>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Mock.Of<ISheet>());
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            //Act
            _ = _target.Export(FakeSummaryData(),
                _fixture.CreateMany<DepositAmountDetailDTO>(),
                _fixture.Create<DepositAmountExportMetaModel>());

            //Assert
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Details"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleDetailSheet_CorrectTitle()
        {
            //Arrange
            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();

            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupTemplate();
            SetupDetailSheet(sheet, templateSheet);

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
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 7, expectedTitle), Times.Once);
        }

        [Fact]
        public void Export_AssingHeaderNameDetailSheet_CorrectHeaderName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockCellRowId = new Mock<ICell>();
            var mockCellUsername = new Mock<ICell>();
            var mockCellCurrency = new Mock<ICell>();
            var mockCellRegisteredDate = new Mock<ICell>();
            var mockCellSignupUrl = new Mock<ICell>();
            var mockCellAffCode = new Mock<ICell>();
            var mockCellFirstDepositAmount = new Mock<ICell>();
            var mockCellNoOfDeposits = new Mock<ICell>();
            var mockCellTotalDepositAmount = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellRowId.Object, mockCellUsername.Object, mockCellCurrency.Object, mockCellRegisteredDate.Object, mockCellSignupUrl.Object,
                mockCellAffCode.Object, mockCellFirstDepositAmount.Object, mockCellNoOfDeposits.Object, mockCellTotalDepositAmount.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(9).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 1)).Returns(mockRowHeader.Object);

            //Act
            _ = _target.Export(FakeSummaryData(),
                  _fixture.CreateMany<DepositAmountDetailDTO>(),
                metaModel);

            //Assert
            mockCellRowId.Verify(_ => _.SetCellValue("#"), Times.Once);
            mockCellUsername.Verify(_ => _.SetCellValue(ViewResources.Username), Times.Once);
            mockCellCurrency.Verify(_ => _.SetCellValue(ViewResources.Currency), Times.Once);
            mockCellRegisteredDate.Verify(_ => _.SetCellValue(ViewResources.RegisteredDate), Times.Once);
            mockCellSignupUrl.Verify(_ => _.SetCellValue(ViewResources.SignupUrl), Times.Once);
            mockCellAffCode.Verify(_ => _.SetCellValue(ViewResources.AffCode), Times.Once);
            mockCellFirstDepositAmount.Verify(_ => _.SetCellValue(ViewResources.FirstDepositAmount), Times.Once);
            mockCellNoOfDeposits.Verify(_ => _.SetCellValue(ViewResources.NumberOfDeposits), Times.Once);
            mockCellTotalDepositAmount.Verify(_ => _.SetCellValue(ViewResources.TotalDepositAmount), Times.Once);
        }

        [Theory]
        [InlineData(1230.123, 412.6)]
        [InlineData(0, 0)]
        [InlineData(1230.123, 123.45)]
        public void Export_AssingDataDetailSheet_CorrectData(double depositAmount, double firstDepositAmount)
        {
            //Arrange
            SetupTemplate();

            var fakeDetailData = _fixture.CreateMany<DepositAmountDetailDTO>(1);
            var fakeRowData = fakeDetailData.First();
            fakeRowData.TotalDepositAmount = (decimal)depositAmount;
            fakeRowData.FirstDepositAmount = (decimal)firstDepositAmount;
            var metaModel = _fixture.Create<DepositAmountExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockCellRowId = new Mock<ICell>();
            var mockCellUsername = new Mock<ICell>();
            var mockCellCurrency = new Mock<ICell>();
            var mockCellRegisteredDate = new Mock<ICell>();
            var mockCellSignupUrl = new Mock<ICell>();
            var mockCellAffCode = new Mock<ICell>();
            var mockCellFirstDepositAmount = new Mock<ICell>();
            var mockCellNoOfDeposits = new Mock<ICell>();
            var mockCellTotalDepositAmount = new Mock<ICell>();

            var listCell = new List<ICell>
            {
                mockCellRowId.Object, mockCellUsername.Object, mockCellCurrency.Object, mockCellRegisteredDate.Object, mockCellSignupUrl.Object,
                mockCellAffCode.Object, mockCellFirstDepositAmount.Object, mockCellNoOfDeposits.Object, mockCellTotalDepositAmount.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(9).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowDataItem.Object);

            //Act
            _ = _target.Export(FakeSummaryData(),
                fakeDetailData,
                metaModel);

            //Assert

            mockCellRowId.Verify(_ => _.SetCellValue(fakeRowData.RowId), Times.Once);
            mockCellUsername.Verify(_ => _.SetCellValue(fakeRowData.Username), Times.Once);
            mockCellCurrency.Verify(_ => _.SetCellValue(fakeRowData.Currency), Times.Once);
            mockCellRegisteredDate.Verify(_ => _.SetCellValue(fakeRowData.RegisteredDateDisplay), Times.Once);
            mockCellSignupUrl.Verify(_ => _.SetCellValue(fakeRowData.SignupUrl), Times.Once);
            mockCellAffCode.Verify(_ => _.SetCellValue(fakeRowData.AffCode), Times.Once);
            AssertDecimalCell(mockCellFirstDepositAmount, fakeRowData.FirstDepositAmount, Times.Once);
            AssertDecimalCell(mockCellNoOfDeposits, fakeRowData.NoOfDeposits, Times.Once);
            AssertDecimalCell(mockCellTotalDepositAmount, fakeRowData.TotalDepositAmount, Times.Once);
        }

        protected void AssertDecimalCell(Mock<ICell> mockCell, decimal value, Func<Times> times)
        {
            mockCell.Verify(_ => _.SetCellValue((double)value), times);
        }

        private void SetupDetailSheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Details")).Returns(templateSheet);
        }

        private DepositAmountSummaryDTO FakeSummaryData()
        {
            var summaryDTO = _fixture.Build<DepositAmountSummaryDTO>()
                .With(_ => _.TotalData, _fixture.CreateMany<TotalDepositAmountSummaryDTO>())
                .With(_ => _.TotalSignupData, _fixture.Create<TotalSignupUrlDepositAmountDTO>())
                .With(_ => _.SignupData, Enumerable.Empty<SignupUrlDepositAmountDTO>())
                .Create();
            return summaryDTO;
        }
    }
}