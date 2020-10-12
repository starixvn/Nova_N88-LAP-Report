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
        public void Export_TemplateDetailSheet_CorrectTemplate()
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
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Details"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleDetailSheet_CorrectTitle()
        {
            //Arrange
            var metaModel = _fixture.Create<WithdrawalExportMetaModel>();

            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupTemplate();
            SetupDetailSheet(sheet, templateSheet);

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
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 5, expectedTitle), Times.Once);
        }

        [Fact]
        public void Export_AssingHeaderNameDetailSheet_CorrectHeaderName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<WithdrawalExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowHeader = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockCellRowId = new Mock<ICell>();
            var mockCellUsername = new Mock<ICell>();
            var mockCellRegisteredDate = new Mock<ICell>();
            var mockCellNumberOfWithdrawals = new Mock<ICell>();
            var mockCellWithdrawalAmount = new Mock<ICell>();
            var listCell = new List<ICell>
            {
                mockCellRowId.Object, mockCellUsername.Object, mockCellRegisteredDate.Object, mockCellNumberOfWithdrawals.Object, mockCellWithdrawalAmount.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(5).ToList());
            mockRowHeader.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 1, 1)).Returns(mockRowHeader.Object);

            //Act
            _ = _target.Export(_fixture.Create<WithdrawalSummaryDTO>(),
                _fixture.CreateMany<WithdrawalDetailDTO>(),
                metaModel);

            //Assert
            mockCellRowId.Verify(_ => _.SetCellValue("#"), Times.Once);
            mockCellUsername.Verify(_ => _.SetCellValue(ViewResources.Username), Times.Once);
            mockCellRegisteredDate.Verify(_ => _.SetCellValue(ViewResources.RegisteredDate), Times.Once);
            mockCellNumberOfWithdrawals.Verify(_ => _.SetCellValue(ViewResources.NumberOfWithdrawals), Times.Once);
            mockCellWithdrawalAmount.Verify(_ => _.SetCellValue(ViewResources.TotalWithdrawalAmount), Times.Once);
        }

        [Fact]
        public void Export_AssingDataDetailSheet_CorrectData()
        {
            //Arrange
            SetupTemplate();

            var fakeDetailData = _fixture.CreateMany<WithdrawalDetailDTO>(1);

            var metaModel = _fixture.Create<WithdrawalExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockCellRowId = new Mock<ICell>();
            var mockCellUsername = new Mock<ICell>();
            var mockCellRegisteredDate = new Mock<ICell>();
            var mockCellNumberOfWithdrawals = new Mock<ICell>();
            var mockCellWithdrawalAmount = new Mock<ICell>();
            var listCell = new List<ICell>
            {
                mockCellRowId.Object, mockCellUsername.Object, mockCellRegisteredDate.Object, mockCellNumberOfWithdrawals.Object, mockCellWithdrawalAmount.Object
            };

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(5).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(listCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowDataItem.Object);

            //Act
            _ = _target.Export(_fixture.Create<WithdrawalSummaryDTO>(),
                fakeDetailData,
                metaModel);

            //Assert
            var fakeRowData = fakeDetailData.FirstOrDefault();
            mockCellRowId.Verify(_ => _.SetCellValue(fakeRowData.RowId), Times.Once);
            mockCellUsername.Verify(_ => _.SetCellValue(fakeRowData.Username), Times.Once);
            mockCellRegisteredDate.Verify(_ => _.SetCellValue(fakeRowData.RegisteredDateDisplay), Times.Once);
            mockCellNumberOfWithdrawals.Verify(_ => _.SetCellValue(fakeRowData.NumberOfWithdrawals), Times.Once);
            mockCellWithdrawalAmount.Verify(_ => _.SetCellValue((double)fakeRowData.WithdrawalAmount), Times.Once);
        }

        private void SetupDetailSheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Details")).Returns(templateSheet);
        }
    }
}