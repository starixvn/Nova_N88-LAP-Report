using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public interface INewlyRegisterUsersExcel
    {
        IWorkbook Export(NewlyRegisterUsersSummaryDTO summaryData, ResultModel<NewlyRegisterUsersDetailDTO> detailData, NewlyRegisterUsersExportMetaModel metaModel);
    }
}