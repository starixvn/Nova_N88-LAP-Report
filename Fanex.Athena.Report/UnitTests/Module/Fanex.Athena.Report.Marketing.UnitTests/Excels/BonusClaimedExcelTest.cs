using AutoFixture;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.UnitTestUtils.Excels;
using Moq;
using NPOI.SS.UserModel;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public partial class BonusClaimedExcelTest : ExcelTest<BonusClaimedExcel>
    {
        [Fact]
        public void Export_Template_CorrectTemplate()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            var mockRow = new Mock<IRow>();

            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(10).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/Marketing/BonusClaimed.xls")).Returns(_mockFileInfo.Object);

            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            //Act
            var actualWorkBoook = _target.Export(FakeSummaryData(),
            _fixture.CreateMany<BonusClaimedDetailDTO>(),
            _fixture.Create<BonusClaimedExportMetaModel>());

            //Assert
            Assert.Equal(_mockWorkbook.Object, actualWorkBoook);
        }
    }
}