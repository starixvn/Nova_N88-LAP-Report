using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using System.Threading.Tasks;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.Report.WinLossAnalysis.Services
{
    public interface IWinLossService
    {
        Task<ResultModel<WinLossRecordViewModel>> GetData(WinLossMetaModel metaModel);
        Task<IEnumerable<WinLossDataDetailItemDTO>> GetDetailData(WinLossDetailMetaModel metaModel);
    }
}