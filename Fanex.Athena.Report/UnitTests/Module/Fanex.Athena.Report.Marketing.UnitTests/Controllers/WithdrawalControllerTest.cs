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
    public class WithdrawalControllerTest
    {
        private readonly Mock<IWithdrawalService> _mockService;
        private readonly WithdrawalController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public WithdrawalControllerTest()
        {
            _mockService = new Mock<IWithdrawalService>();
            _targetController = new WithdrawalController(_mockService.Object);
        }

        [Fact]
        public async Task ExportData_WhenCall_ReturnDataCorCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<WithdrawalExportMetaModel>();
            var fakeDetailsData = _fixture.Create<ResultModel<WithdrawalDetailDTO>>();
            var fakeSummaryData = _fixture.Create<WithdrawalSummaryDTO>();

            var mockExcel = new Mock<IWithdrawalExcel>();
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