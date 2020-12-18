using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public interface IDepositAmountService
    {
        Task<DepositAmountSummaryDTO> GetSummaryData(DepositAmountSummaryMetaModel metaModel);

        Task<ResultModel<DepositAmountDetailDTO>> GetDetailsData(DepositAmountDetailMetaModel metaModel);

        Task<(DepositAmountSummaryDTO, ResultModel<DepositAmountDetailDTO>)> GetExportData(DepositAmountExportMetaModel metaModel);
    }
}