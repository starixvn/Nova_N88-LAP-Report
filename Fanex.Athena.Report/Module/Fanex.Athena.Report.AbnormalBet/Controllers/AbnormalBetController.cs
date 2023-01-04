using System.Threading.Tasks;
using Fanex.Athena.Report.AbnormalBet.Excels.Interfaces;
using Fanex.Athena.Report.AbnormalBet.MetaModels;
using Fanex.Athena.Report.AbnormalBet.Services.Interfaces;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Core.Attributes;
using Fanex.NVC.Authentication.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Fanex.Athena.Report.AbnormalBet.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [NVCAuthorize(PermissionKeys.AbnormalBet)]
    public class AbnormalBetController : ControllerBase
    {
        private readonly IAbnormalBetService _service;

        public AbnormalBetController(IAbnormalBetService service)
            => _service = service;

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<JsonResult> GetData([FromBody] AbnormalBetMetaModel metaModel)
        {
            var result = await _service.GetDataAsync(metaModel);

            return new JsonResult(new { data = result.DataList });
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<FileContentResult> ExportSummaryExcel(
            [FromServices] IFileProvider file,
            [FromServices] IAbnormalBetSummaryExcel excel,
            [FromBody] AbnormalBetMetaModel metaModel)
        {
            var template = file.GetFileInfo("/ExcelTemplate/AbnormalBet/AbnormalBetSummary.xls").PhysicalPath;

            var result = await _service.GetDataAsync(metaModel).ConfigureAwait(false);

            var fileContent = excel.Export(template, result, metaModel);

            return new FileContentResult(fileContent, "application/octet-stream");
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<FileContentResult> ExportIpSummaryExcel(
            [FromServices] IFileProvider file,
            [FromServices] IAbnormalBetIpSummaryExcel excel,
            [FromBody] AbnormalBetMetaModel metaModel)
        {
            var template = file.GetFileInfo("/ExcelTemplate/AbnormalBet/AbnormalBetIpSummary.xls").PhysicalPath;

            var result = await _service.GetDataAsync(metaModel).ConfigureAwait(false);

            var fileContent = excel.Export(template, result, metaModel);

            return new FileContentResult(fileContent, "application/octet-stream");
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<FileContentResult> ExportDetailExcel(
            [FromServices] IFileProvider file,
            [FromServices] IAbnormalBetDetailExcel excel,
            [FromBody] AbnormalBetMetaModel metaModel)
        {
            var template = file.GetFileInfo("/ExcelTemplate/AbnormalBet/AbnormalBetDetail.xls").PhysicalPath;

            var result = await _service.GetDataAsync(metaModel).ConfigureAwait(false);

            var fileContent = excel.Export(template, result, metaModel);

            return new FileContentResult(fileContent, "application/octet-stream");
        }
    }
}