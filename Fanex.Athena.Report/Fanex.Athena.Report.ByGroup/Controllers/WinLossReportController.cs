using Fanex.Athena.Report.ByGroup.Consts;
using Fanex.Athena.Report.ByGroup.Excels;
using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.Services;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO;
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
        public async Task<ResultModel<WinLossRecordViewModel>> GetData(WinLossMetaModel metaModel)
        {
            var data = await _winLossService.GetData(metaModel);
            return data;
        }

        [HttpPost]
        public async Task<FileResult> ExportData(
            [FromServices]IFileProvider fileProvider,
            [FromServices]IWinLossExcel excel,
            ExportWinLossMetaModel metaModel
            )
        {
            var data = await _winLossService.GetData(metaModel);
            var sortAbleList = data.DataList as ISortableList<WinLossRecordViewModel>;
            sortAbleList?.Sort(metaModel.SortBy, metaModel.SortType);

            var template = fileProvider.GetFileInfo(ExcelConsts.WinLossExcelTemplatePath).PhysicalPath;
            var workBook = excel.Export(template, data, metaModel);
            var stream = new MemoryStream();
            workBook.Write(stream);

            return new FileContentResult(stream.ToArray(), ExcelConsts.OctetStreamContentType);
        }
    }
}