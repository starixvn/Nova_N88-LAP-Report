using AutoFixture;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Microsoft.Extensions.FileProviders;
using Moq;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Excels
{
    public partial class NewlyRegisterUsersExcelTest
    {
        private readonly NewlyRegisterUsersExcel _target;
        private readonly Mock<IExcelBuilder> _mockExcelBuilder;
        private readonly Mock<IFileProvider> _mockFileProvider;
        private readonly Mock<IWorkbook> _mockWorkbook;
        private readonly Mock<IFileInfo> _mockFileInfo;
        private readonly IFixture _fixture = new Fixture();

        public NewlyRegisterUsersExcelTest()
        {
            _mockExcelBuilder = new Mock<IExcelBuilder>();
            _mockFileProvider = new Mock<IFileProvider>();
            _mockWorkbook = new Mock<IWorkbook>();
            _mockFileInfo = new Mock<IFileInfo>();

            _target = new NewlyRegisterUsersExcel(_mockExcelBuilder.Object, _mockFileProvider.Object);
        }

        [Fact]
        public void Export_Template_CorrectTemplate()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            var mockFileInfo = new Mock<IFileInfo>();
            var mockRow = new Mock<IRow>();

            _mockExcelBuilder.Setup(_ => _.CloneSheet(It.IsAny<IWorkbook>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Mock.Of<ISheet>());
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());

            mockRow.Setup(_ => _.Cells).Returns(MockListCell(15).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>())).Returns(mockRow.Object);

            mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/Marketing/NewlyRegisterUsers.xls")).Returns(mockFileInfo.Object);

            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            //Act
            var actualWorkBoook = _target.Export(_fixture.Create<NewlyRegisterUsersSummaryDTO>(),
                _fixture.Create<ResultModel<NewlyRegisterUsersDetailDTO>>(),
                _fixture.Create<NewlyRegisterUsersExportMetaModel>());

            //Assert
            Assert.Equal(_mockWorkbook.Object, actualWorkBoook);
        }

        private void SetupTemplate()
        {
            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);
        }

        private IEnumerable<ICell> MockListCell(int count)
        {
            var mockListCell = new List<ICell>();
            for (int i = 0; i < count; i++)
            {
                mockListCell.Add(Mock.Of<ICell>());
            }
            return mockListCell;
        }
    }
}