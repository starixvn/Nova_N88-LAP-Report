using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _service;

        public RevenueController(IRevenueService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<JsonResult> GetAvgData([FromBody]AvgRevenueMetaModel metaModel)
        {
            var result = await _service.GetAvgData(metaModel);
            return new JsonResult(result);
        }
    }
}