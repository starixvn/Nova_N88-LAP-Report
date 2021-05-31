using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using NPOI.SS.UserModel;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Excels
{
    public interface IWinLossPlayerBetListExcel
    {
        Task<IWorkbook> Export(ExportWinLossPlayerBetListMetaModel metaModel);
    }
}