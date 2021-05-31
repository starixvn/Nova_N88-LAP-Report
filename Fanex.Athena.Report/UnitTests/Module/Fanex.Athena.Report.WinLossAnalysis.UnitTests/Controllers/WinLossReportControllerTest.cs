using AutoFixture;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.WinLossAnalysis.Controllers;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.WinLossAnalysis.UnitTests.Controllers
{
    public class WinLossReportControllerTest
    {
        private readonly WinLossReportController _target;
        private readonly IFixture _fixture = new Fixture();

        public WinLossReportControllerTest()
        {
            _target = new WinLossReportController();
        }

        [Fact]
        public async Task GetData_WhenCall_ReturnResultCorrectly()
        {
            //Arrange
            var winLossService = new Mock<IWinLossService>();
            var metaModel = _fixture.Create<WinLossMetaModel>();
            var expectedResult = new ResultModel<WinLossRecordViewModel>();
            winLossService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _target.GetData(winLossService.Object, metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult.Value as ResultModel<WinLossRecordViewModel>);
        }

        [Fact]
        public async Task ExportData_WhenCall_ReturnFileResultCorrectly()
        {
            //Arrange
            var winLossService = new Mock<IWinLossService>();
            var metaModel = _fixture.Create<ExportWinLossMetaModel>();
            var expectedResult = new ResultModel<WinLossRecordViewModel>
            {
                DataList = new List<WinLossRecordViewModel>()
            };

            winLossService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult));

            var mockExcel = new Mock<IWinLossExcel>();
            var workBook = new Mock<IWorkbook>();

            MemoryStream expectedStream = null;
            workBook.Setup(_ => _.Write(It.IsAny<Stream>())).Callback<Stream>(stream => expectedStream = stream as MemoryStream);

            mockExcel.Setup(_ => _.Export(expectedResult, metaModel))
                .Returns(workBook.Object);

            //Act
            var result = await _target.ExportData(mockExcel.Object, winLossService.Object, metaModel);

            //Assert
            var fileContentResult = result as FileContentResult;
            Assert.Equal("application/octet-stream", result.ContentType);
            Assert.Equal(expectedStream?.ToArray(), fileContentResult.FileContents);

            expectedStream?.Dispose();
        }
    }
}