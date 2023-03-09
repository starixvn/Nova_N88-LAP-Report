using AutoFixture;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.UnitTestUtils.Excels;
using Moq;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public partial class DepositAmountExcelTest : ExcelTest<DepositAmountExcel>
    {
        [Fact]
        public void Export_Template_CorrectTemplate()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            var mockRow = new Mock<IRow>();

            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(15).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/Marketing/DepositAmount.xls")).Returns(_mockFileInfo.Object);

            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            //Act
            var actualWorkBoook = _target.Export(FakeSummaryDTO(),
                _fixture.CreateMany<DepositAmountDetailDTO>(),
                _fixture.Create<DepositAmountExportMetaModel>());

            //Assert
            Assert.Equal(_mockWorkbook.Object, actualWorkBoook);
        }

        private DepositAmountSummaryDTO FakeSummaryDTO()
        {
            return new DepositAmountSummaryDTO
            {
                TotalData = _fixture.CreateMany<TotalDepositAmountSummaryDTO>(),
                TotalSignupData = _fixture.Create<TotalSignupUrlDepositAmountDTO>(),
                SignupData = new List<SignupUrlDepositAmountDTO>()
            };
        }
    }
}