using AutoFixture;
using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.Excels;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.BonusInfo.UnitTests.Excels.TestHelpers;
using Microsoft.Extensions.FileProviders;
using Moq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.Report.BonusInfo.UnitTests.Excels
{
    public class BonusInfoDetailExcelTest
    {
        private readonly IFixture _fixture = new Fixture();
        private const string TemplatePath = "/ExcelTemplate/BonusInfo/BonusInfo_Detail.xls";
        private const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = "MM/dd/yyyy";
        protected const string DateCellFormat = "dd-MMM-yyyy";

        private const int ColumnUserName = 0;
        private const int ColumnBonusCode = 1;
        private const int ColumnClaimDate = 2;
        private const int ColumnBonusAmountClaimed = 3;
        private const int ColumnTotalDeposit = 4;
        private const int ColumnTotalWithdrawal = 5;
        private const int ColumnStatus = 6;
        private const int ColumnCompletion = 7;

        private const int ColumnTemplateCount = 8;

        private const int RowTitleTemplate = 0;
        private const int RowHeaderTemplate = 1;
        private const int RowContentTemplate = 2;
        private const int RowNoInfoTemplate = 7;

        private Mock<IExcelBuilder> _excelBuilder;
        private Mock<IFileProvider> _fileProvider;
        private Mock<ISheet> _sheet;
        private Mock<ISheet> _templateSheet;

        [Fact]
        public void AssignTitle_WhenCall_AssignCorrectValueForTitleRows()
        {
            // Arrange
            var excel = CreateExcel();
            CreateRows();

            var data = new BonusInfoDetailDTO();
            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();
            Sites.SiteNameList.TryGetValue(metaModel.BrandId, out string brandName);

            var reportTitle = $"{ViewResources.BonusInfo}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}, {brandName}," +
                $" {string.Format(ViewResources.Viewin, metaModel.BaseCurrency.ToUpperInvariant())}, {metaModel.BonusCode}";

            // Action
            excel.Export(data, metaModel);

            // Assert
            _excelBuilder.Verify(ex => ex.SetRowValue(_sheet.Object, _templateSheet.Object,
                RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle), Times.Once);
        }

        [Fact]
        public void AssignHeader_WhenCall_AssignCorrectValueForHeaderRows()
        {
            // Arrange
            var excel = CreateExcel();
            var rows = CreateRows();
            var headerTextList = new[]
            {
                ViewResources.Username,
                ViewResources.BonusCode,
                ViewResources.ClaimDate,
                ViewResources.BonusAmountClaimed,
                ViewResources.TotalDeposit,
                ViewResources.TotalWithdrawal,
                ViewResources.Status,
                ViewResources.Completion + "%"
            };

            var data = new BonusInfoDetailDTO();
            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();

            // Action
            excel.Export(data, metaModel);
            var headerRow = rows[0];

            // Assert
            ExcelTestHelper.AssertSetTextRowHeader(headerRow.Object, headerTextList);
        }

        [Fact]
        public void AssignNoInfoRow_WhenCall_AssignCorrectValueForContent()
        {
            // Arrange
            var excel = CreateExcel();
            CreateRows();

            var data = new BonusInfoDetailDTO();
            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();

            // Action
            excel.Export(data, metaModel);

            // Assert
            _excelBuilder.Verify(ex => ex.SetRowValue(_sheet.Object, _templateSheet.Object,
                RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo), Times.Once);
        }

        [Fact]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var excel = CreateExcel();
            var rows = CreateRows();

            var data = _fixture.Build<BonusInfoDetailDTO>()
                .With(x => x.DataList, new List<BonusInfoDetailItemDTO> { _fixture.Create<BonusInfoDetailItemDTO>() })
                .With(x => x.TotalRows, 1)
                .Create();

            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();

            // Action
            excel.Export(data, metaModel);
            var bodyRow = rows[1];

            // Assert
            AssertRowData(bodyRow, data);
        }

        private IList<Mock<IRow>> CreateRows()
        {
            var rowList = CreateRowList();
            var headerRow = rowList[0];
            var bodyRow = rowList[1];

            _excelBuilder.Setup(ex => ex.CopyRow(_sheet.Object, _templateSheet.Object, RowHeaderTemplate, RowHeaderTemplate))
                .Returns(headerRow.Object);
            _excelBuilder.Setup(ex => ex.CopyRow(_sheet.Object, _templateSheet.Object, It.Is<int>(x => x == RowContentTemplate || x == RowNoInfoTemplate), RowContentTemplate))
                .Returns(bodyRow.Object);

            return rowList;
        }

        private IList<Mock<IRow>> CreateRowList()
        {
            var headerRow = ExcelTestHelper.MockGetRow(_sheet, ColumnTemplateCount, RowHeaderTemplate);
            var bodyRow = ExcelTestHelper.MockGetRow(_sheet, ColumnTemplateCount, RowContentTemplate);

            return new List<Mock<IRow>> { headerRow, bodyRow };
        }

        private BonusInfoDetailExcel CreateExcel()
        {
            _excelBuilder = new Mock<IExcelBuilder>();
            var stringFilePath = _fixture.Create<string>();
            _fileProvider = new Mock<IFileProvider>();
            _fileProvider.Setup(f => f.GetFileInfo(TemplatePath).PhysicalPath).Returns(stringFilePath);

            var workbook = new Mock<IWorkbook>();
            _excelBuilder.Setup(eb => eb.LoadTemplate(stringFilePath)).Returns(workbook.Object);

            _sheet = new Mock<ISheet>();
            _excelBuilder.Setup(eb => eb.CloneSheet(workbook.Object, null, SheetName)).Returns(_sheet.Object);

            _templateSheet = new Mock<ISheet>();
            workbook.Setup(w => w.GetSheet(It.Is<string>(x => x == TemplateSheetName))).Returns(_templateSheet.Object);

            return new BonusInfoDetailExcel(_excelBuilder.Object, _fileProvider.Object);
        }

        private void AssertRowData(Mock<IRow> row, BonusInfoDetailDTO data)
        {
            AssertCommonCellData(row, data.DataList.FirstOrDefault());
        }

        private void AssertCommonCellData(Mock<IRow> row, BonusInfoDetailItemDTO item)
        {
            Mock.Get(row.Object.Cells[ColumnUserName]).Verify(c => c.SetCellValue(item.UserName), Times.Once);
            Mock.Get(row.Object.Cells[ColumnBonusCode]).Verify(c => c.SetCellValue(item.BonusCode), Times.Once);
            Mock.Get(row.Object.Cells[ColumnClaimDate]).Verify(c => c.SetCellValue(item.ClaimedDate.ToString(DateCellFormat)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnBonusAmountClaimed]).Verify(c => c.SetCellValue(Convert.ToDouble(item.ClaimedAmount)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnTotalDeposit]).Verify(c => c.SetCellValue(Convert.ToDouble(item.TotalDeposit)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnTotalWithdrawal]).Verify(c => c.SetCellValue(Convert.ToDouble(item.TotalWithdrawal)), Times.Once);
            Mock.Get(row.Object.Cells[ColumnStatus]).Verify(c => c.SetCellValue(item.StatusText), Times.Once);
            Mock.Get(row.Object.Cells[ColumnCompletion]).Verify(c => c.SetCellValue(item.PercentagedCompletion), Times.Once);
        }
    }
}