namespace Fanex.Athena.Report.ByGroup.Excels
{
    public interface IWinLossExcelCreator
    {
        IWinLossExcel CreateWinLossExcel(byte group);
    }
}