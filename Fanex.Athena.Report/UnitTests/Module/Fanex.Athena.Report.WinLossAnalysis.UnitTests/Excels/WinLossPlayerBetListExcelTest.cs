using AutoFixture;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.UnitTestUtils.Excels;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Moq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.WinLossAnalysis.UnitTests.Excels
{
    public class WinLossPlayerBetListExcelTest : ExcelTest<WinLossPlayerBetListExcel>
    {
        private Mock<IWinLossPlayerService> _mockBetListService;

        protected override void Create()
        {
            _mockBetListService = new Mock<IWinLossPlayerService>();
            _target = Activator.CreateInstance(typeof(WinLossPlayerBetListExcel), _mockExcelBuilder.Object, _mockFileProvider.Object, _mockBetListService.Object) as WinLossPlayerBetListExcel;
        }

        [Fact]
        public async Task Export_Template_CorrectTemplate()
        {
            //Arrange
            const string templatePath = "/templatePath/physicalpath";
            SetupRows(new Mock<IRow>(), 9);
            _mockWorkbook.Setup(_ => _.GetSheet(It.IsAny<string>())).Returns(Mock.Of<ISheet>());
            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(templatePath);
            _mockFileProvider.Setup(_ => _.GetFileInfo("/ExcelTemplate/WinLossAnalysis/WinLossPlayerBetList.xlsx")).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(templatePath)).Returns(_mockWorkbook.Object);

            var metaModel = _fixture.Create<ExportWinLossPlayerBetListMetaModel>();
            metaModel.TotalRows = 0;

            //Act
            var actualWorkBoook = await _target.Export(metaModel);

            //Assert
            Assert.Equal(_mockWorkbook.Object, actualWorkBoook);
        }

        [Fact]
        public void Export_AssignHeader_CorrectHeadersName()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<ExportWinLossPlayerBetListMetaModel>();
            metaModel.TotalRows = 0;

            var headerTextList = new List<string>
            {
                "#",
                ViewResources.RefNo,
                ViewResources.TransTime,
                ViewResources.Stake,
                ViewResources.PlayerWinloss,
                ViewResources.ProgressiveWin,
                ViewResources.ProgressiveBet,
                ViewResources.GGR,
                ViewResources.Status
            };

            var sheet = new Mock<ISheet>();
            var templateSheet = new Mock<ISheet>();

            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Template", "Data")).Returns(sheet.Object);
            _mockWorkbook.Setup(_ => _.GetSheet("Template")).Returns(templateSheet.Object);

            SetupRows(new Mock<IRow>(), 7);

            var mockHeaderRow = ExcelTestHelper.MockGetRow(sheet, 9, 1);
            _mockExcelBuilder.Setup(ex => ex.CopyRow(sheet.Object, templateSheet.Object, 1, 1))
                    .Returns(mockHeaderRow.Object);

            //Act
            _ = _target.Export(metaModel);

            //Assert
            ExcelTestHelper.AssertSetTextRowHeader(mockHeaderRow.Object, headerTextList);
        }

        [Fact]
        public async Task Export_AssingRowDataAndTotalData_CorrectData()
        {
            //Arrange
            SetupTemplate();

            var metaModel = _fixture.Create<ExportWinLossPlayerBetListMetaModel>();
            metaModel.TotalRows = 1;

            var sheet = new Mock<ISheet>();
            var templateSheet = new Mock<ISheet>();

            _mockExcelBuilder.Setup(_ => _.CloneSheet(_mockWorkbook.Object, "Template", "Data-1")).Returns(sheet.Object);
            _mockWorkbook.Setup(_ => _.GetSheet("Template")).Returns(templateSheet.Object);

            SetupRows(new Mock<IRow>(), 9);

            var mockDataRow = ExcelTestHelper.MockGetRow(sheet, 9, 3);
            _mockExcelBuilder.Setup(ex => ex.CopyRow(sheet.Object, templateSheet.Object, 3, 3))
                    .Returns(mockDataRow.Object);

            var mockDataTotal = ExcelTestHelper.MockGetRow(sheet, 9, 4);
            _mockExcelBuilder.Setup(ex => ex.CopyRow(sheet.Object, templateSheet.Object, 4, 4))
                    .Returns(mockDataTotal.Object);
            sheet.Setup(_ => _.LastRowNum).Returns(3);

            var fakeData = new ResultModel<WinLossPlayerBetListViewModel>
            {
                DataList = _fixture.CreateMany<WinLossPlayerBetListViewModel>(1),
                TotalData = _fixture.Create<WinLossPlayerBetListViewModel>(),
                TotalRows = 1
            };

            _mockBetListService.Setup(_ => _.GetPlayerBetList(metaModel)).Returns(Task.FromResult(fakeData));

            //Act
            _ = await _target.Export(metaModel);

            //Assert
            AssertRowData(mockDataRow, fakeData.DataList.First());
            AssertTotalData(mockDataTotal, fakeData.TotalData);
        }

        private void AssertRowData(Mock<IRow> mockRowData, WinLossPlayerBetListViewModel fakeRowData)
        {
            var cells = mockRowData.Object.Cells;
            Mock.Get(cells[0]).Verify(c => c.SetCellValue(fakeRowData.RowNumber), Times.Once);
            Mock.Get(cells[1]).Verify(c => c.SetCellValue(fakeRowData.RefNo), Times.Once);
            Mock.Get(cells[2]).Verify(c => c.SetCellValue(fakeRowData.TransTime), Times.Once);
            Mock.Get(cells[3]).Verify(c => c.SetCellValue((double)fakeRowData.Stake), Times.Once);
            Mock.Get(cells[4]).Verify(c => c.SetCellValue((double)fakeRowData.PlayerWinLoss), Times.Once);
            Mock.Get(cells[5]).Verify(c => c.SetCellValue((double)fakeRowData.ProgressiveWin), Times.Once);
            Mock.Get(cells[6]).Verify(c => c.SetCellValue((double)fakeRowData.ProgressiveBet), Times.Once);
            Mock.Get(cells[7]).Verify(c => c.SetCellValue((double)fakeRowData.GGR), Times.Once);
            Mock.Get(cells[8]).Verify(c => c.SetCellValue(fakeRowData.TicketStatus), Times.Once);
        }

        private void AssertTotalData(Mock<IRow> mockRowData, WinLossPlayerBetListViewModel fakeTotalData)
        {
            var cells = mockRowData.Object.Cells;
            Mock.Get(cells[3]).Verify(c => c.SetCellValue(ViewResources.GrandTotal), Times.Once);
            Mock.Get(cells[4]).Verify(c => c.SetCellValue((double)fakeTotalData.PlayerWinLoss), Times.Once);
            Mock.Get(cells[5]).Verify(c => c.SetCellValue((double)fakeTotalData.ProgressiveWin), Times.Once);
            Mock.Get(cells[6]).Verify(c => c.SetCellValue((double)fakeTotalData.ProgressiveBet), Times.Once);
            Mock.Get(cells[7]).Verify(c => c.SetCellValue((double)fakeTotalData.GGR), Times.Once);
        }
    }
}