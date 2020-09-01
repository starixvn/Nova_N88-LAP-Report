using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class NewlyRegisterUsersExcel : INewlyRegisterUsersExcel
    {
        private const string TemplatePath = "/ExcelTemplate/Marketing/NewlyRegisterUsers.xls";

        private readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        public NewlyRegisterUsersExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(NewlyRegisterUsersSummaryDTO summaryData, ResultModel<NewlyRegisterUsersDetailDTO> detailData, NewlyRegisterUsersExportMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);

            BuildSummarySheet(workBook, metaModel, summaryData);
            BuildDetailSheet(workBook, metaModel, detailData);

            return workBook;
        }
    }
}