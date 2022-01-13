using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Core.Attributes;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.Excels.Interfaces;
using Fanex.Athena.Report.ExtraPayout.MetaModels;
using Fanex.Athena.Report.ExtraPayout.Models;
using Fanex.Athena.Report.ExtraPayout.Services.Interfaces;
using Fanex.NVC.Authentication.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ExtraPayout.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [NVCAuthorize(PermissionKeys.ExtraPayout)]
    public class ExtraPayoutController : ControllerBase
    {
        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<ExtraPayoutResultModel> GetData(ExtraPayoutMetaModel metaModel, [FromServices] IExtraPayoutService service)
        {
            var result = await service.GetDataAsync(metaModel).ConfigureAwait(false);

            if (!result.DataList.Any())
            {
                return GetEmptyResult();
            }

            return new ExtraPayoutResultModel
            {
                Data = result.DataList.ToList(),
                TotalCurrency = result.TotalDataList.ToList(),
                TotalSummary = result.TotalData,
                TotalRows = result.TotalRows
            };
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<ExtraPayoutDetailResultModel> GetDetailData(ExtraPayoutDetailMetaModel metaModel, [FromServices] IExtraPayoutService service)
        {
            var result = await service.GetDetailDataAsync(metaModel).ConfigureAwait(false);

            if (!result.DataList.Any())
            {
                return new ExtraPayoutDetailResultModel
                {
                    Data = new List<ExtraPayoutDetailDTO>(),
                    TotalSummary = new ExtraPayoutDetailDTO()
                };
            }

            return new ExtraPayoutDetailResultModel
            {
                Data = result.DataList.ToList(),
                TotalSummary = result.TotalData,
                TotalRows = result.TotalRows
            };
        }

        private static dynamic GetEmptyResult()
        {
            return new ExtraPayoutResultModel
            {
                Data = new List<ExtraPayoutDTO>(),
                TotalCurrency = new List<ExtraPayoutDTO>(),
                TotalSummary = new ExtraPayoutDTO()
            };
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<FileContentResult> ExportExcel(
            [FromServices] IExtraPayoutService service,
            [FromServices] IFileProvider file,
            [FromServices] IExtraPayoutExcel excel,
            ExtraPayoutMetaModel metaModel)
        {
            var template = file.GetFileInfo("/ExcelTemplate/ExtraPayout/ExtraPayout.xls").PhysicalPath;

            var result = await service.GetDataAsync(metaModel).ConfigureAwait(false);

            var fileContent = excel.Export(template, result, metaModel);

            return new FileContentResult(fileContent, "application/octet-stream");
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<FileContentResult> ExportDetailExcel(
            [FromServices] IExtraPayoutService service,
            [FromServices] IFileProvider file,
            [FromServices] IExtraPayoutDetailExcel excel,
            ExtraPayoutDetailMetaModel metaModel)
        {
            var template = file.GetFileInfo("/ExcelTemplate/ExtraPayout/ExtraPayoutDetail.xls").PhysicalPath;

            var result = await service.GetDetailDataAsync(metaModel).ConfigureAwait(false);

            var fileContent = excel.Export(template, result, metaModel);

            return new FileContentResult(fileContent, "application/octet-stream");
        }
    }
}