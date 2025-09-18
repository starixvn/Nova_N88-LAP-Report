using Fanex.Athena.Report.Core.Attributes;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.NVC.Authentication.Extensions;
using Fanex.NVC.Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Controllers
{
    [ApiController]
    [Route("api/Marketing/[controller]/[action]")]
    [NVCAuthorize(NVCFunctionKeys.ReportMarketing)]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _service;

        public RevenueController(IRevenueService service)
        {
            _service = service;
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true, valiDataRangeLimitation: true)]
        public async Task<JsonResult> GetAvgData([FromBody] AvgRevenueMetaModel metaModel)
        {
            var result = await _service.GetAvgData(metaModel);
            return new JsonResult(result);
        }
    }
}