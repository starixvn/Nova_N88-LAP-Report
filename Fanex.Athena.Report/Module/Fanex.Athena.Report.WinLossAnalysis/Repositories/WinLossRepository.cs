using Fanex.Athena.Report.WinLossAnalysis.Consts;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.Common.Repositories;
using Fanex.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.WinLossAnalysis.Repositories
{
    public class WinLossRepository : BaseRepository, IWinLossRepository
    {
        public async Task<(IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO, int)> GetData(WinLossMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.WinLossReport))
            {
                var (dataItems, dataTotal) = await GetDataSet(metaModel, db);
                var totalRows = GetTotalRows(db);
                return (dataItems, dataTotal, totalRows);
            }
        }

        public async Task<IEnumerable<WinLossDataDetailItemDTO>> GetDetailData(WinLossDetailMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.WinLossReportDetail))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var dataItems = GetDataItems<WinLossDataDetailItemDTO>(reader);
                    return dataItems;
                }
            }
        }

        private async Task<(IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO)> GetDataSet(WinLossMetaModel metaModel, IObjectDb db)
        {
            using (var reader = await db.ExecuteReaderAsync(metaModel))
            {
                var dataItems = GetDataItems<WinLossDataItemDTO>(reader);
                var dataTotal = GetDataTotal<WinLossDataTotalDTO>(reader);
                return (dataItems, dataTotal);
            }
        }
    }
}