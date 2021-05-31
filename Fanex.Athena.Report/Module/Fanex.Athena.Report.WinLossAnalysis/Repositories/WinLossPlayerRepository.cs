using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Repositories
{
    public class WinLossPlayerRepository : BaseRepository, IWinLossPlayerRepository
    {
        public async Task<IEnumerable<WinLossPlayerDetailDTO>> GetPlayerDetail(WinLossPlayerDetailMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.WinLossPlayerDetail))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var dataItems = GetDataItems<WinLossPlayerDetailDTO>(reader);
                    return dataItems;
                }
            }
        }

        public async Task<(IEnumerable<WinLossPlayerBetListDTO>, WinLossPlayerBetListTotalDTO, int)> GetPlayerBetList(WinLossPlayerBetListMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.WinLossPlayerBetList))
            {
                var (dataItems, dataTotal) = await GetDataSet(metaModel, db);
                var totalRows = GetTotalRows(db);
                return (dataItems, dataTotal, totalRows);
            }
        }

        private async Task<(IEnumerable<WinLossPlayerBetListDTO>, WinLossPlayerBetListTotalDTO)> GetDataSet(WinLossPlayerBetListMetaModel metaModel, IObjectDb db)
        {
            using (var reader = await db.ExecuteReaderAsync(metaModel))
            {
                var dataItems = GetDataItems<WinLossPlayerBetListDTO>(reader);
                var dataTotal = GetDataTotal<WinLossPlayerBetListTotalDTO>(reader);
                return (dataItems, dataTotal);
            }
        }
    }
}