using AutoFixture;
using Fanex.Athena.Report.Common.Excel;
using Microsoft.Extensions.FileProviders;
using Moq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.UnitTestUtils.Excels
{
    public class ExcelTest<T>
        where T : class
    {
        protected readonly T _target;
        protected readonly Mock<IExcelBuilder> _mockExcelBuilder;
        protected readonly Mock<IFileProvider> _mockFileProvider;
        protected readonly Mock<IWorkbook> _mockWorkbook;
        protected readonly Mock<IFileInfo> _mockFileInfo;
        protected readonly IFixture _fixture = new Fixture();

        protected ExcelTest()
        {
            _mockExcelBuilder = new Mock<IExcelBuilder>();
            _mockFileProvider = new Mock<IFileProvider>();
            _mockWorkbook = new Mock<IWorkbook>();
            _mockFileInfo = new Mock<IFileInfo>();
            _target = (T)Activator.CreateInstance(typeof(T), _mockExcelBuilder.Object, _mockFileProvider.Object);
        }

        protected void SetupTemplate()
        {
            _mockFileInfo.Setup(_ => _.PhysicalPath).Returns(_fixture.Create<string>());
            _mockFileProvider.Setup(_ => _.GetFileInfo(It.IsAny<string>())).Returns(_mockFileInfo.Object);
            _mockExcelBuilder.Setup(_ => _.LoadTemplate(It.IsAny<string>())).Returns(_mockWorkbook.Object);
        }

        protected void SetupRowsWithOrderCell(Mock<IRow> mockRow, int countCell)
        {
            mockRow.Setup(_ => _.Cells).Returns(MockListCell(countCell).ToList());
            _mockExcelBuilder.Setup(_ => _.CopyRow(It.IsAny<ISheet>(), It.IsAny<ISheet>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int[]>())).Returns(mockRow.Object);
        }

        protected static IEnumerable<ICell> MockListCell(int count)
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