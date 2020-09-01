using Fanex.Athena.Report.Marketing.Excels;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NewlyRegisterUsersController : ControllerBase
    {
        private readonly INewlyRegisterUsersService _service;

        public NewlyRegisterUsersController(INewlyRegisterUsersService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<JsonResult> GetSummaryData([FromBody]NewlyRegisterUsersSummaryMetaModel metaModel)
        {
            var result = await _service.GetSummaryData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetDetailsData([FromBody]NewlyRegisterUsersDetailMetaModel metaModel)
        {
            var result = await _service.GetDetailsData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<FileResult> ExportData(
            [FromServices]INewlyRegisterUsersExcel excel,
            NewlyRegisterUsersExportMetaModel metaModel)
        {
            var (summaryData, detailData) = await _service.GetExportData(metaModel);
            var workBook = excel.Export(summaryData, detailData, metaModel);
            var stream = new MemoryStream();
            workBook.Write(stream);

            return new FileContentResult(stream.ToArray(), "application/octet-stream");
        }
    }
}