using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class NewlyRegisterUsersExcel : PerformanceHighlightDetailExcel<
        NewlyRegisterUsersExportMetaModel,
        NewlyRegisterUsersSummaryDTO,
        IEnumerable<NewlyRegisterUsersDetailDTO>>, INewlyRegisterUsersExcel
    {
        public NewlyRegisterUsersExcel(IExcelBuilder builder, IFileProvider fileProvider)
            : base(builder, fileProvider)
        {
        }

        protected override string TemplatePath => "/ExcelTemplate/Marketing/NewlyRegisterUsers.xls";

        protected override string BuildTitle(NewlyRegisterUsersExportMetaModel metaModel)
        {
            var title = $"{ViewResources.NewlyRegisteredUsers}, " +
              $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
              $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";
            return title;
        }
    }
}