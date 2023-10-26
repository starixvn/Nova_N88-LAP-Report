using Fanex.Athena.Report.Core.Attributes;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.Helpers;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Fanex.NVC.Authentication.Extensions;
using Fanex.NVC.Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [NVCAuthorize(NVCFunctionKeys.ReportWinLoss)]
    public partial class WinLossReportController : ControllerBase
    {
        private const int MaxRowExport = 50000;
        private const int OneMonthInSecond = 30 * 24 * 60 * 60;

        [HttpGet]
        [ResponseCache(Duration = OneMonthInSecond, Location = ResponseCacheLocation.Client)]
        public JsonResult GetSetting()
        {
            return new JsonResult(new
            {
                Timezones = DataHelper.TimezoneOptions.Value
            });
        }

        [HttpPost]
        [RequestValidation(validateBrandList: true, validateCurrencyList: true, valiDataRangeLimitation: true)]
        public async Task<JsonResult> GetData([FromServices] IWinLossService service, WinLossMetaModel metaModel)
        {
            var data = await service.GetData(metaModel);
            return new JsonResult(data);
        }

        [HttpPost]
        [RequestValidation(validateBrandList: true, validateCurrencyList: true, valiDataRangeLimitation: true)]
        public async Task<FileResult> ExportData(
            [FromServices] IWinLossExcel excel,
            [FromServices] IWinLossService service,
            ExportWinLossMetaModel metaModel
            )
        {
            EnsureMetaModelExport(metaModel);

            var data = await service.GetData(metaModel);
            var stream = new MemoryStream();
            try
            {
                var workBook = excel.Export(data, metaModel);
                workBook.Write(stream);
                return new FileContentResult(stream.ToArray(), ExcelConsts.OctetStreamContentType);
            }
            finally
            {
                stream.Dispose();
                (data?.DataList as List<WinLossRecordViewModel>)?.Clear();
            }
        }

        [HttpPost]
        [RequestValidation(validateBrandList: true, validateCurrencyList: true, valiDataRangeLimitation: true)]
        public async Task<FileResult> ExportDetailData(
            [FromServices] IWinLossDetailExcel excel,
            [FromServices] IWinLossService service,
            WinLossDetailMetaModel metaModel
            )
        {
            var data = await service.GetDetailData(metaModel);
            var stream = new MemoryStream();
            try
            {
                var workBook = excel.Export(data, metaModel);
                workBook.Write(stream);
                return new FileContentResult(stream.ToArray(), ExcelConsts.OctetStreamContentType);
            }
            finally
            {
                stream.Dispose();
            }
        }

        private static void EnsureMetaModelExport(ExportWinLossMetaModel metaModel)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = MaxRowExport;
        }
    }
}