using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Controllers
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
            [FromServices]IWinLossExcelCreator excelCreator,
            ExportWinLossMetaModel metaModel
            )
        {
            var data = await _winLossService.GetData(metaModel);
            var sortAbleList = data.DataList as ISortableList<WinLossRecordViewModel>;
            sortAbleList?.Sort(metaModel.SortBy, metaModel.SortType);

            var excel = excelCreator.CreateWinLossExcel(metaModel.GroupBy);
            var workBook = excel.Export(data, metaModel);
            var stream = new MemoryStream();
            workBook.Write(stream);

            return new FileContentResult(stream.ToArray(), ExcelConsts.OctetStreamContentType);
        }
    }
}