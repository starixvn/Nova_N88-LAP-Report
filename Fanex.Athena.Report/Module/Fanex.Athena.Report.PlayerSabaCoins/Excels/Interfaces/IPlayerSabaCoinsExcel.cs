using Fanex.Athena.Report.Common.Model.ResultModel;
using Fanex.Athena.Report.PlayerSabaCoins.DTO;
using Fanex.Athena.Report.PlayerSabaCoins.MetaModels;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Report.PlayerSabaCoins.Excels.Interfaces
{
    public interface IPlayerSabaCoinsExcel
    {
        byte[] Export(string template, SortableReportResultModel<PlayerSabaCoinsDTO> data, PlayerSabaCoinsMetaModel metaModel);

        IWorkbook GetWorkbook(string template, SortableReportResultModel<PlayerSabaCoinsDTO> data, PlayerSabaCoinsMetaModel metaModel);
    }
}