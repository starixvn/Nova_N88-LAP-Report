using AutoFixture;
using Fanex.Athena.Report.BonusInfo.Controllers;
using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.Excels;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.BonusInfo.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NPOI.SS.UserModel;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.BonusInfo.UnitTests.Controllers
{
    public class BonusInfoControllerTest
    {
        private readonly Mock<IBonusInfoService> _mockService;
        private readonly BonusInfoController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public BonusInfoControllerTest()
        {
            _mockService = new Mock<IBonusInfoService>();
            _targetController = new BonusInfoController(_mockService.Object);
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            var expectedResult = new BonusInfoSummaryDTO();

            _mockService.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetSummaryData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult.Value as BonusInfoSummaryDTO);
        }

        [Fact]
        public async Task GetDetailsData_WhenCall_ReturnDataCorCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();
            var expectedResult = _fixture.Create<BonusInfoDetailDTO>();

            _mockService.Setup(_ => _.GetDetailsData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetDetailsData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult.Value as BonusInfoDetailDTO);
        }

        [Fact]
        public async Task ExportSummaryData_WhenCall_ReturnDataCorCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            var fakeSummaryData = new BonusInfoSummaryDTO();

            var mockExcel = new Mock<IBonusInfoSummaryExcel>();
            var workBook = new Mock<IWorkbook>();
            MemoryStream expectedStream = null;

            _mockService.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(fakeSummaryData));
            workBook.Setup(_ => _.Write(It.IsAny<Stream>())).Callback<Stream>(stream => expectedStream = stream as MemoryStream);
            mockExcel.Setup(_ => _.Export(fakeSummaryData, metaModel))
                .Returns(workBook.Object);

            //Act
            var fileContentResult = await _targetController.ExportSummaryData(mockExcel.Object, metaModel);

            //Assert
            Assert.Equal("application/octet-stream", fileContentResult.ContentType);
            Assert.Equal(expectedStream?.ToArray(), fileContentResult.FileContents);

            expectedStream?.Dispose();
        }

        [Fact]
        public async Task ExportDetailData_WhenCall_ReturnDataCorCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();
            var fakeDetailData = new BonusInfoDetailDTO();

            var mockExcel = new Mock<IBonusInfoDetailExcel>();
            var workBook = new Mock<IWorkbook>();
            MemoryStream expectedStream = null;

            _mockService.Setup(_ => _.GetDetailsData(metaModel)).Returns(Task.FromResult(fakeDetailData));
            workBook.Setup(_ => _.Write(It.IsAny<Stream>())).Callback<Stream>(stream => expectedStream = stream as MemoryStream);
            mockExcel.Setup(_ => _.Export(fakeDetailData, metaModel))
                .Returns(workBook.Object);

            //Act
            var fileContentResult = await _targetController.ExportDetailData(mockExcel.Object, metaModel);

            //Assert
            Assert.Equal("application/octet-stream", fileContentResult.ContentType);
            Assert.Equal(expectedStream?.ToArray(), fileContentResult.FileContents);

            expectedStream?.Dispose();
        }
    }
}