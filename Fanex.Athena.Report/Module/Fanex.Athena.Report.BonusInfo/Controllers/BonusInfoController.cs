using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.Excels;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.BonusInfo.Services;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Core.Attributes;
using Fanex.NVC.Authentication.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.BonusInfo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [NVCAuthorize(PermissionKeys.ReportBonusInfo)]
    public class BonusInfoController : ControllerBase
    {
        private const int MaxRowExport = 50000;
        private const int OneDayInSecond = 24 * 60 * 60;

        private readonly IBonusInfoService _service;

        public BonusInfoController(IBonusInfoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ResponseCache(Duration = OneDayInSecond, Location = ResponseCacheLocation.Client)]
        public async Task<JsonResult> GetSetting()
        {
            var bonusTypes = await _service.GetBonusTypes();
            var result = new BonusInfoDropdownOptionsDTO(bonusTypes);
            return new JsonResult(result);
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true)]
        public async Task<JsonResult> GetSummaryData([FromBody]BonusInfoSummaryMetaModel metaModel)
        {
            var result = await _service.GetSummaryData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true)]
        public async Task<JsonResult> GetDetailsData([FromBody]BonusInfoDetailMetaModel metaModel)
        {
            var result = await _service.GetDetailsData(metaModel);
            return new JsonResult(result);
        }

        [HttpPost]
        [RequestValidation(validateBrand: true, validateCurrencyList: true)]
        public async Task<FileContentResult> ExportSummaryData(
            [FromServices]IBonusInfoSummaryExcel excel,
            BonusInfoSummaryMetaModel metaModel)
        {
            var summaryData = await _service.GetSummaryData(metaModel);
            var workBook = excel.Export(summaryData, metaModel);
            var stream = new MemoryStream();
            workBook.Write(stream);

            return new FileContentResult(stream.ToArray(), "application/octet-stream");
        }

        [HttpPost]
        [RequestValidation(validateBrand: true)]
        public async Task<FileContentResult> ExportDetailData(
        [FromServices]IBonusInfoDetailExcel excel,
        BonusInfoDetailMetaModel metaModel)
        {
            EnsureMetaModelExport(metaModel);

            var detailData = await _service.GetDetailsData(metaModel);
            var workBook = excel.Export(detailData, metaModel);
            var stream = new MemoryStream();
            workBook.Write(stream);

            return new FileContentResult(stream.ToArray(), "application/octet-stream");
        }

        private static void EnsureMetaModelExport(BonusInfoDetailMetaModel metaModel)
        {
            metaModel.PageIndex = 1;
            metaModel.PageSize = MaxRowExport;
        }
    }
}