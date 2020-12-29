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
    public partial class NewlyRegisterUsersExcelTest
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
            _ = _target.Export(FakeSummaryDTO(),
                   _fixture.CreateMany<NewlyRegisterUsersDetailDTO>(),
                   _fixture.Create<NewlyRegisterUsersExportMetaModel>());

            //Assert
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Details"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleDetailSheet_CorrectTitle()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersExportMetaModel>();

            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupTemplate();
            SetupDetailSheet(sheet, templateSheet);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(15).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            var expectedTitle = $"{ViewResources.NewlyRegisteredUsers}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(FakeSummaryDTO(),
                _fixture.CreateMany<NewlyRegisterUsersDetailDTO>(),
                metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 7, expectedTitle), Times.Once);
        }

        [Theory]
        [InlineData(true, 12321)]
        [InlineData(false, 0)]
        public void Export_AssignDataDetailSheet_FirstDepositLogic_CorrectData(bool isDeposit, decimal firstDepositAmount)
        {
            //Arrange
            SetupTemplate();

            var fakeRowData = new NewlyRegisterUsersDetailDTO
            {
                IsDeposit = isDeposit,
                FirstDepositAmount = firstDepositAmount
            };

            var fakeDetailData = new List<NewlyRegisterUsersDetailDTO>
                {
                    fakeRowData
                };

            var metaModel = _fixture.Create<NewlyRegisterUsersExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockListCell = MockListCell(7).ToList();
            var mockDepositCell = new Mock<ICell>();
            var mockDepositAmountCell = new Mock<ICell>();
            mockListCell[5] = mockDepositCell.Object;
            mockListCell[6] = mockDepositAmountCell.Object;

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(mockListCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowDataItem.Object);

            //Act
            _ = _target.Export(FakeSummaryDTO(),
                fakeDetailData,
                metaModel);

            //Assert
            mockDepositCell.Verify(_ => _.SetCellValue(isDeposit ? ViewResources.Yes : ViewResources.No), Times.Once);
            if (isDeposit)
            {
                mockDepositAmountCell.Verify(_ => _.SetCellValue((double)fakeRowData.FirstDepositAmount), Times.Once);
            }
            else
            {
                mockDepositAmountCell.Verify(_ => _.SetCellValue(0), Times.Once);
            }
        }

        [Theory]
        [InlineData(null, "SignupUrl")]
        [InlineData("BonusCode", null)]
        public void Export_AssingDataDetailSheet_BonusOrSignupIsNull_CorrectData(string bonusCode, string signupUrl)
        {
            //Arrange
            SetupTemplate();

            var fakeRowData = new NewlyRegisterUsersDetailDTO
            {
                BonusCode = bonusCode,
                SignupUrl = signupUrl
            };

            var fakeDetailData = new List<NewlyRegisterUsersDetailDTO>
                {
                    fakeRowData
                };

            var metaModel = _fixture.Create<NewlyRegisterUsersExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var mockRowDataItem = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupDetailSheet(sheet, templateSheet);

            var mockListCell = MockListCell(7).ToList();
            var mockBonusCodeCell = new Mock<ICell>();
            var mockSignupUrlCell = new Mock<ICell>();
            mockListCell[1] = mockBonusCodeCell.Object;
            mockListCell[4] = mockSignupUrlCell.Object;

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            mockRowDataItem.Setup(_ => _.Cells).Returns(mockListCell);
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);
            _mockExcelBuilder.Setup(_ => _.CopyRow(sheet, templateSheet, 2, 2)).Returns(mockRowDataItem.Object);

            //Act
            _ = _target.Export(FakeSummaryDTO(),
                fakeDetailData,
                metaModel);

            //Assert
            mockBonusCodeCell.Verify(_ => _.SetCellValue(string.IsNullOrEmpty(fakeRowData.BonusCode) ? ViewResources.None : fakeRowData.BonusCode), Times.Once);
            mockSignupUrlCell.Verify(_ => _.SetCellValue(string.IsNullOrEmpty(fakeRowData.SignupUrl) ? ViewResources.None : fakeRowData.SignupUrl), Times.Once);
        }

        private void SetupDetailSheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Details", "DetailsData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Details")).Returns(templateSheet);
        }
    }
}