using AutoFixture;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.Controllers;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NPOI.SS.UserModel;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Controllers
{
    public class NewlyRegisterUsersControllerTest
    {
        private readonly Mock<INewlyRegisterUsersService> _mockService;
        private readonly NewlyRegisterUsersController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public NewlyRegisterUsersControllerTest()
        {
            _mockService = new Mock<INewlyRegisterUsersService>();
            _targetController = new NewlyRegisterUsersController(_mockService.Object);
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersSummaryMetaModel>();
            var expectedResult = new NewlyRegisterUsersSummaryDTO();

            _mockService.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetSummaryData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult.Value as NewlyRegisterUsersSummaryDTO);
        }

        [Fact]
        public async Task GetDetailsData_WhenCall_ReturnDataCorCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersDetailMetaModel>();
            var expectedResult = _fixture.Create<ResultModel<NewlyRegisterUsersDetailDTO>>();

            _mockService.Setup(_ => _.GetDetailsData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetDetailsData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult.Value as ResultModel<NewlyRegisterUsersDetailDTO>);
        }

        [Fact]
        public async Task ExportData_WhenCall_ReturnDataCorCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersExportMetaModel>();
            var fakeDetailsData = _fixture.Create<ResultModel<NewlyRegisterUsersDetailDTO>>();
            var fakeSummaryData = new NewlyRegisterUsersSummaryDTO();

            var mockExcel = new Mock<INewlyRegisterUsersExcel>();
            var workBook = new Mock<IWorkbook>();
            MemoryStream expectedStream = null;

            _mockService.Setup(_ => _.GetExportData(metaModel)).Returns(Task.FromResult((fakeSummaryData, fakeDetailsData)));
            workBook.Setup(_ => _.Write(It.IsAny<Stream>())).Callback<Stream>(stream => expectedStream = stream as MemoryStream);
            mockExcel.Setup(_ => _.Export(fakeSummaryData, fakeDetailsData.DataList, metaModel))
                .Returns(workBook.Object);

            //Act
            var actualResult = await _targetController.ExportData(mockExcel.Object, metaModel);

            //Assert
            var fileContentResult = actualResult as FileContentResult;
            Assert.Equal("application/octet-stream", actualResult.ContentType);
            Assert.Equal(expectedStream?.ToArray(), fileContentResult.FileContents);

            expectedStream?.Dispose();
        }
    }
}