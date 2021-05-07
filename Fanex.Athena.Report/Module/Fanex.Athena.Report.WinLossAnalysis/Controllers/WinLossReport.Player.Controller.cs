using Fanex.Athena.Report.Core.Attributes;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Controllers
{
    public partial class WinLossReportController : ControllerBase
    {
        [HttpPost]
        [RequestValidation]
        public async Task<JsonResult> GetPlayerDetail([FromServices] IWinLossPlayerService service, WinLossPlayerDetailMetaModel metaModel)
        {
            var data = await service.GetPlayerDetail(metaModel);
            return new JsonResult(data);
        }

        [HttpPost]
        [RequestValidation]
        public async Task<JsonResult> GetPlayerBetList([FromServices] IWinLossPlayerService service, WinLossPlayerBetListMetaModel metaModel)
        {
            var data = await service.GetPlayerBetList(metaModel);
            return new JsonResult(data);
        }

        [HttpPost]
        [RequestValidation]
        public async Task<FileResult> ExportPlayerBetList(
           [FromServices] IWinLossPlayerBetListExcel excel,
           [FromServices] IWinLossPlayerService service,
           ExportWinLossPlayerBetListMetaModel metaModel
           )
        {
            var data = await service.GetPlayerBetList(metaModel);
            var stream = new MemoryStream();
            try
            {
                var workBook = excel.Export(data, metaModel);
                workBook.Write(stream);
                return new FileContentResult(stream.ToArray(), ExcelConsts.OctetStreamContentType);
            }
            finally
            {
                stream?.Dispose();
                (data?.DataList as List<WinLossPlayerBetListViewModel>)?.Clear();
            }
        }
    }
}