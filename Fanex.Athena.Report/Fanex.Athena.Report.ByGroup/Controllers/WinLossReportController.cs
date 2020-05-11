using Fanex.Athena.Report.ByGroup.MetaModels;
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

        [HttpGet]
        public async Task<dynamic> GetData()
        {
            var metaModel = new WinLossMetaModel
            {
                LicenseeId = 402,
                FilterBy = 1,
                GroupBy = 1,
                FromDate = new System.DateTime(2019, 10, 1),
                ToDate = new System.DateTime(2020, 6, 1),
                ProductIds = "5",
                UserName = string.Empty,
                CurrencyIds = "2,3,51,52",
                DisplayCurrency = "EUR",
                IsIncludedTestData = true,
                IsNoDrawTicket = false,
                IsExportExcel = false,
                PageIndex = 1,
                PageSize = 10,
            };
            var x = await _winLossService.GetData(metaModel);
            return x;
        }

        [HttpPost]
        public async Task<dynamic> GetData(WinLossMetaModel metaModel)
        {
            metaModel = new WinLossMetaModel()
            {
                LicenseeId = 402,
                FilterBy = 1,
                GroupBy = 1,
                FromDate = new System.DateTime(2019, 10, 1),
                ToDate = new System.DateTime(2020, 6, 1),
                ProductIds = "5",
                UserName = string.Empty,
                CurrencyIds = "2,3,51,52",
                DisplayCurrency = "EUR",
                IsIncludedTestData = true,
                IsNoDrawTicket = false,
                IsExportExcel = false,
                PageIndex = 1,
                PageSize = 10,
            };
            var x = await _winLossService.GetData(metaModel);
            return x;
        }

        [HttpPost]
        public async Task<dynamic> ExportData()
        {
            return null;
        }
    }
}