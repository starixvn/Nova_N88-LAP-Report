using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public interface INewlyRegisterUsersService
    {
        Task<NewlyRegisterUsersSummaryDTO> GetSummaryData(NewlyRegisterUsersSummaryMetaModel metaModel);

        Task<ResultModel<NewlyRegisterUsersDetailDTO>> GetDetailsData(NewlyRegisterUsersDetailMetaModel metaModel);

        Task<(NewlyRegisterUsersSummaryDTO, ResultModel<NewlyRegisterUsersDetailDTO>)> GetExportData(NewlyRegisterUsersExportMetaModel metaModel);
    }
}