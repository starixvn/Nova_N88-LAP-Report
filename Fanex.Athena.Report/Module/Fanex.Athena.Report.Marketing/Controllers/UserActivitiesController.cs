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
    public class UserActivitiesController : ControllerBase
    {
        private readonly IUserActivitiesService _service;

        public UserActivitiesController(IUserActivitiesService service)
        {
            _service = service;
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true)]
        public async Task<JsonResult> GetData([FromBody]UserActivitiesMetaModel metaModel)
        {
            var result = await _service.GetData(metaModel);
            return new JsonResult(result);
        }
    }
}