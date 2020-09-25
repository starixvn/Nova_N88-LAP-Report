using AutoFixture;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using Moq;
using NPOI.SS.UserModel;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public partial class NewlyRegisterUsersExcelTest
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
            _ = _target.Export(_fixture.Create<NewlyRegisterUsersSummaryDTO>(),
                _fixture.Create<ResultModel<NewlyRegisterUsersDetailDTO>>(),
                _fixture.Create<NewlyRegisterUsersExportMetaModel>());

            //Assert
            _mockExcelBuilder.Verify(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData"), Times.Once);
            _mockWorkbook.Verify(_ => _.GetSheet("Summary"), Times.Once);
        }

        [Fact]
        public void Export_AssignTitleSummarySheet_CorrectTitle()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<NewlyRegisterUsersExportMetaModel>();
            var mockRow = new Mock<IRow>();
            var sheet = Mock.Of<ISheet>();
            var templateSheet = Mock.Of<ISheet>();

            SetupSummarySheet(sheet, templateSheet);

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(15).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            var expectedTitle = $"{ViewResources.NewlyRegisteredUsersAcronym} - {ViewResources.NewlyRegisteredUsers}, " +
                $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";

            //Act
            _ = _target.Export(_fixture.Create<NewlyRegisterUsersSummaryDTO>(),
                _fixture.Create<ResultModel<NewlyRegisterUsersDetailDTO>>(),
                metaModel);

            //Assert
            _mockExcelBuilder.Verify(_ => _.SetRowValue(sheet, templateSheet, 0, 0, 4, expectedTitle), Times.Once);
        }

        private void SetupSummarySheet(ISheet sheet, ISheet templateSheet)
        {
            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Summary", "SummaryData")).Returns(sheet);
            _mockWorkbook.Setup(_ => _.GetSheet("Summary")).Returns(templateSheet);
        }
    }
}