using Fanex.Athena.Report.PlayerSabaCoins.DTO;
using Fanex.Athena.Report.PlayerSabaCoins.MetaModels;
using Fanex.Athena.Report.Common.Model.ResultModel;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.PlayerSabaCoins.Services.Interfaces
{
    public interface IPlayerSabaCoinsService
    {
        Task<SortableReportResultModel<PlayerSabaCoinsDTO>> GetDataAsync(PlayerSabaCoinsMetaModel metaModel);
    }
}