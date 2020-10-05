using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public interface IWithdrawalService
    {
        Task<WithdrawalSummaryDTO> GetSummaryData(WithdrawalSummaryMetaModel metaModel);

        Task<ResultModel<WithdrawalDetailDTO>> GetDetailsData(WithdrawalDetailMetaModel metaModel);

        Task<(WithdrawalSummaryDTO, ResultModel<WithdrawalDetailDTO>)> GetExportData(WithdrawalExportMetaModel metaModel);
    }
}