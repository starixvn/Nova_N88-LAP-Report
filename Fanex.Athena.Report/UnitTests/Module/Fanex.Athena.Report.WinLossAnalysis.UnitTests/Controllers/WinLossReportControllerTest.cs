using AutoFixture;
using Fanex.Athena.Report.WinLossAnalysis.Controllers;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.Common.Model.ViewModel;
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
        private readonly Mock<IWinLossService> _winLossService;
        private readonly IFixture _fixture = new Fixture();

        public WinLossReportControllerTest()
        {
            _winLossService = new Mock<IWinLossService>();
            _target = new WinLossReportController(_winLossService.Object);
        }

        [Fact]
        public async Task GetData_WhenCall_ReturnResultCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<WinLossMetaModel>();
            var expectedResult = new SortableResultModel<WinLossRecordViewModel>();
            _winLossService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _target.GetData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("group", "desc", "Group", false)]
        [InlineData("avgBetAMOUNT", "Asc", "AvgBetAmount", true)]
        public async Task ExportData_WhenCall_ShouldSortDataList(string sortBy, string sortType, string expectedSortBy, bool expectedSortType)
        {
            //Arrange
            var metaModel = _fixture.Create<ExportWinLossMetaModel>();
            metaModel.SortBy = sortBy;
            metaModel.SortType = sortType;

            var mockDataList = new Mock<ISortableList<WinLossRecordViewModel>>();
            var expectedResult = new Mock<SortableResultModel<WinLossRecordViewModel>>();

            expectedResult.Setup(_ => _.DataList).Returns(mockDataList.Object);
            mockDataList.Setup(_ => _.GetEnumerator()).Returns(new List<WinLossRecordViewModel>() { new WinLossRecordViewModel() }.GetEnumerator());
            _winLossService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult.Object));

            var mockExcelCreator = new Mock<IWinLossExcelCreator>();
            var mockExcel = new Mock<IWinLossExcel>();

            mockExcelCreator.Setup(_ => _.CreateWinLossExcel(It.IsAny<byte>()))
                .Returns(mockExcel.Object);

            mockExcel.Setup(_ => _.Export(It.IsAny<SortableResultModel<WinLossRecordViewModel>>(), metaModel))
                .Returns(Mock.Of<IWorkbook>());

            //Act
            await _target.ExportData(mockExcelCreator.Object, metaModel);

            //Assert
            mockDataList.Verify(_ => _.Sort(expectedSortBy, expectedSortType), Times.Once);
        }

        [Fact]
        public async Task ExportData_WhenCall_ReturnFileResultCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<ExportWinLossMetaModel>();
            var expectedResult = new SortableResultModel<WinLossRecordViewModel>
            {
                DataList = new List<WinLossRecordViewModel>()
            };

            _winLossService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult));

            var mockExcelCreator = new Mock<IWinLossExcelCreator>();
            var mockExcel = new Mock<IWinLossExcel>();
            var workBook = new Mock<IWorkbook>();

            MemoryStream expectedStream = null;
            workBook.Setup(_ => _.Write(It.IsAny<Stream>())).Callback<Stream>(stream => expectedStream = stream as MemoryStream);

            mockExcelCreator.Setup(_ => _.CreateWinLossExcel(metaModel.GroupBy))
                .Returns(mockExcel.Object);

            mockExcel.Setup(_ => _.Export(expectedResult, metaModel))
                .Returns(workBook.Object);

            //Act
            var result = await _target.ExportData(mockExcelCreator.Object, metaModel);

            //Assert
            var fileContentResult = result as FileContentResult;
            Assert.Equal("application/octet-stream", result.ContentType);
            Assert.Equal(expectedStream?.ToArray(), fileContentResult.FileContents);

            expectedStream?.Dispose();
        }
    }
}