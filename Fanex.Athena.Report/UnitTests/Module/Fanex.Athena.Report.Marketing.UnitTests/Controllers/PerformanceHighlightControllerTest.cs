using AutoFixture;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.Athena.Report.Marketing.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NPOI.SS.UserModel;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Controllers
{
    public class PerformanceHighlightControllerTest
    {
        private readonly Mock<IPerformanceHighlightService> _mockService;
        private readonly PerformanceHighlightController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public PerformanceHighlightControllerTest()
        {
            _mockService = new Mock<IPerformanceHighlightService>();
            _targetController = new PerformanceHighlightController(_mockService.Object);
        }

        [Fact]
        public async Task GetData_WhenCall_CallMetaModelCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<PerformanceHighlightMetaModel>();
            var expectedResult = _fixture.Create<PerformanceHighlightViewModel>();

            _mockService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult.Value as PerformanceHighlightViewModel);
        }

        [Fact]
        public async Task ExportData_WhenCall_ReturnDataCorCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<PerformanceHighlightExportAllMetaModel>();
            var fakeData = _fixture.Create<PerformanceHighlightExportDTO>();

            var mockExcel = new Mock<IPerformanceHighlightExcel>();
            var workBook = new Mock<IWorkbook>();
            MemoryStream expectedStream = null;

            _mockService.Setup(_ => _.ExportData(metaModel)).Returns(Task.FromResult(fakeData));
            workBook.Setup(_ => _.Write(It.IsAny<Stream>())).Callback<Stream>(stream => expectedStream = stream as MemoryStream);
            mockExcel.Setup(_ => _.Export(fakeData, metaModel))
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