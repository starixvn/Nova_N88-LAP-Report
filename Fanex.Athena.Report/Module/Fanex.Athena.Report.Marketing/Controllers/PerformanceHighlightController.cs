using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PerformanceHighlightController : ControllerBase
    {
        private readonly IPerformanceHighlightService _service;

        public PerformanceHighlightController(IPerformanceHighlightService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<JsonResult> GetData([FromBody]PerformanceHighlightMetaModel metaModel)
        {
            var result = await _service.GetData(metaModel);
            return new JsonResult(result);
        }
    }
}