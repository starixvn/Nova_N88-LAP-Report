using Fanex.Athena.Report.ByGroup.Enums;
using Fanex.Athena.Report.Common.Excel;
using Microsoft.Extensions.FileProviders;

namespace Fanex.Athena.Report.ByGroup.Excels
{
    public class WinLossExcelCreator : IWinLossExcelCreator
    {
        protected readonly IExcelBuilder _excelBuilder;
        private readonly IFileProvider _fileProvider;

        public WinLossExcelCreator(IExcelBuilder excelBuilder, IFileProvider fileProvider)
        {
            _excelBuilder = excelBuilder;
            _fileProvider = fileProvider;
        }

        public IWinLossExcel CreateWinLossExcel(byte groupBy)
        {
            if (groupBy == (byte)GroupBy.Player)
            {
                return new WinLossByPlayerExcel(_excelBuilder, _fileProvider);
            }

            return new WinLossExcel(_excelBuilder, _fileProvider);
        }
    }
}