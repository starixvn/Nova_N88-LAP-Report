using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Core.Attributes;
using Fanex.Athena.Report.PlayerSabaCoins.Excels.Interfaces;
using Fanex.Athena.Report.PlayerSabaCoins.MetaModels;
using Fanex.Athena.Report.PlayerSabaCoins.Models;
using Fanex.Athena.Report.PlayerSabaCoins.Services.Interfaces;
using Fanex.NVC.Authentication.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.PlayerSabaCoins.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [NVCAuthorize(PermissionKeys.PlayerSabaCoins)]
    [RequestValidation(validateCurrencyList: true, validateBrand: true)]
    public class PlayerSabaCoinsController : ControllerBase
    {
        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<IActionResult> GetData([FromBody] PlayerSabaCoinsMetaModel metaModel, [FromServices] IPlayerSabaCoinsService service)
        {
            var result = await service.GetDataAsync(metaModel).ConfigureAwait(false);

            return Ok(new PlayerSabaCoinsResultModel
            {
                Data = result.DataList.ToList(),
                TotalSummary = result.TotalData,
                TotalRows = result.TotalRows
            });
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<FileContentResult> ExportExcel(
            [FromServices] IPlayerSabaCoinsService service,
            [FromServices] IFileProvider file,
            [FromServices] IPlayerSabaCoinsExcel excel,
            [FromBody] PlayerSabaCoinsMetaModel metaModel)
        {
            var template = file.GetFileInfo("/ExcelTemplate/PlayerSabaCoins/PlayerSabaCoins.xls").PhysicalPath;
            var result = await service.GetDataAsync(metaModel).ConfigureAwait(false);
            var fileContent = excel.Export(template, result, metaModel);
            return new FileContentResult(fileContent, "application/octet-stream");
        }
    }
}