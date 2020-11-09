using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace Fanex.Athena.Report.Marketing.Excels
{
    public partial class BonusClaimedExcel : PerformanceHighlightDetailExcel<
        BonusClaimedExportMetaModel,
        BonusClaimedSummaryDTO,
        IEnumerable<BonusClaimedDetailDTO>>, IBonusClaimedExcel
    {
        public BonusClaimedExcel(IExcelBuilder builder, IFileProvider fileProvider) : base(builder, fileProvider)
        {
        }

        protected override string TemplatePath => "/ExcelTemplate/Marketing/BonusClaimed.xls";

        protected override string BuildTitle(BonusClaimedExportMetaModel metaModel)
        {
            var title = $"{ViewResources.BonusAmountClaimed}, " +
                          $"{metaModel.FromDate.ToString(DateTimeFormat.Date)} - {metaModel.ToDate.ToString(DateTimeFormat.Date)}, " +
                          $"{ViewResources.Viewin.Replace("{0}", metaModel.BaseCurrency.ToUpperInvariant())}";
            return title;
        }
    }
}