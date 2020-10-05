using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class WithdrawalExcel : PerformanceHighlightDetailExcel<
        WithdrawalExportMetaModel,
        WithdrawalSummaryDTO,
        IEnumerable<WithdrawalDetailDTO>>, IWithdrawalExcel
    {
        public WithdrawalExcel(IExcelBuilder builder, IFileProvider fileProvider) : base(builder, fileProvider)
        {
        }

        protected override string TemplatePath => "/ExcelTemplate/Marketing/WithdrawalAmount.xls";

        protected override string BuildTitle(WithdrawalExportMetaModel metaModel)
        {
            var title = $"{ViewResources.WithdrawalAmount}, " +
                          $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                          $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";
            return title;
        }
    }
}