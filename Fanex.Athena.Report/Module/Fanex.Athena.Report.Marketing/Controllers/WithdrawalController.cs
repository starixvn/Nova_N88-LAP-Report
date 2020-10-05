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
    public class WithdrawalController : ControllerBase
    {
        private readonly IWithdrawalService _service;

        public WithdrawalController(IWithdrawalService service)
        {
            _service = service;
        }

        [HttpPost]
        [RequestValidation(validateBrand: true)]
        public async Task<JsonResult> GetSummaryData([FromBody]WithdrawalSummaryMetaModel metaModel)
        {
            var result = await _service.GetSummaryData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        [RequestValidation(validateBrand: true)]
        public async Task<JsonResult> GetDetailsData([FromBody]WithdrawalDetailMetaModel metaModel)
        {
            var result = await _service.GetDetailsData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        [RequestValidation(validateBrand: true)]
        public async Task<FileResult> ExportData([FromServices]IWithdrawalExcel excel, WithdrawalExportMetaModel metaModel)
        {
            var (summaryData, detailData) = await _service.GetExportData(metaModel);
            var workBook = excel.Export(summaryData, detailData.DataList, metaModel);
            var stream = new MemoryStream();
            workBook.Write(stream);

            return new FileContentResult(stream.ToArray(), "application/octet-stream");
        }
    }
}