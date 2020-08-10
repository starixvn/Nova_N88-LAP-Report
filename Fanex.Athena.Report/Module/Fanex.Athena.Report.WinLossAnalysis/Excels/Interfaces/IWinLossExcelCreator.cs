namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public interface IWinLossExcelCreator
    {
        IWinLossExcel CreateWinLossExcel(byte group);
    }
}