using Fanex.Athena.Report.Common.Helpers;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Core.Attributes;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Controllers
{
    public partial class WinLossReportController : ControllerBase
    {
        private const string ConcurrentExportBetListKey = "WinLossReport_ExportPlayerBetList";
        private const int ConcurrentExportBetListRequest = 2;

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
           [FromServices] IProductService productService,
           [FromServices] IDataMapper dataMapper,
           ExportWinLossPlayerBetListMetaModel metaModel
           )
        {
            var stream = new MemoryStream();
            try
            {
                await BindProductName(metaModel, productService, dataMapper);
                var workBook = await LockHelper.LockAsync(ConcurrentExportBetListKey, () => excel.Export(metaModel), ConcurrentExportBetListRequest);
                workBook.Write(stream);
                return new FileContentResult(stream.ToArray(), ExcelConsts.OctetStreamContentType);
            }
            finally
            {
                stream?.Dispose();
            }
        }

        private static async Task BindProductName(ExportWinLossPlayerBetListMetaModel metaModel, IProductService productService, IDataMapper dataMapper)
        {
            var products = await productService.GetDictionaryTpProductAsync();
            metaModel.ProductName = dataMapper.ToTpProductName(products, metaModel.ProductId);
        }
    }
}