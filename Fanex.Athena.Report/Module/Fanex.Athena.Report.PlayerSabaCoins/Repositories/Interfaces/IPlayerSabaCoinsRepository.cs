using Fanex.Athena.Report.PlayerSabaCoins.DTO;
using Fanex.Athena.Report.PlayerSabaCoins.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.PlayerSabaCoins.Repositories.Interfaces
{
    public interface IPlayerSabaCoinsRepository
    {
        Task<IEnumerable<PlayerSabaCoinsDTO>> GetMainData(PlayerSabaCoinsMetaModel metaModel);
        Task<IEnumerable<PlayerSabaCoinsDTO>> GetExtraInfoData(PlayerSabaCoinsMetaModel metaModel);
    }
}