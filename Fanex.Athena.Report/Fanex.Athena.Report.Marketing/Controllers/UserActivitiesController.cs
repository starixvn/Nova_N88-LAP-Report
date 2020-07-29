using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserActivitiesController
    {
        private readonly IUserActivitiesService _service;

        public UserActivitiesController(IUserActivitiesService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<JsonResult> GetData([FromBody]UserActivitiesMetaModel metaModel)
        {
            var result = await _service.GetData(metaModel);
            return new JsonResult(result);
        }
    }
}