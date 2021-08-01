using Fanex.Athena.Report.Core.Attributes;
using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.NVC.Authentication.Extensions;
using Fanex.NVC.Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Controllers
{
    [ApiController]
    [Route("api/Marketing/[controller]/[action]")]
    [NVCAuthorize(NVCFunctionKeys.ReportMarketing)]
    public class BonusClaimedController : ControllerBase
    {
        private readonly IBonusClaimedService _service;

        public BonusClaimedController(IBonusClaimedService service)
        {
            _service = service;
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true, valiDataRangeLimitation: true)]
        public async Task<JsonResult> GetSummaryData([FromBody]BonusClaimedSummaryMetaModel metaModel)
        {
            var result = await _service.GetSummaryData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true, valiDataRangeLimitation: true)]
        public async Task<JsonResult> GetDetailsData([FromBody]BonusClaimedDetailMetaModel metaModel)
        {
            var result = await _service.GetDetailsData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true, valiDataRangeLimitation: true)]
        public async Task<FileResult> ExportData(
            [FromServices]IBonusClaimedExcel excel,
            BonusClaimedExportMetaModel metaModel)
        {
            var (summaryData, detailData) = await _service.GetExportData(metaModel);
            var workBook = excel.Export(summaryData, detailData.DataList, metaModel);
            var stream = new MemoryStream();
            workBook.Write(stream);

            return new FileContentResult(stream.ToArray(), "application/octet-stream");
        }
    }
}