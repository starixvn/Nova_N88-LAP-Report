using Fanex.Athena.Report.ByGroup.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ByGroup.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WinLossReportController : ControllerBase
    {
        private readonly IWinLossService _winLossService;

        public WinLossReportController(IWinLossService winLossService)
        {
            _winLossService = winLossService;
        }

        [HttpPost]
        public async Task<dynamic> GetData()
        {
            return null;
        }

        [HttpPost]
        public async Task<dynamic> ExportData()
        {
            return null;
        }
    }
}