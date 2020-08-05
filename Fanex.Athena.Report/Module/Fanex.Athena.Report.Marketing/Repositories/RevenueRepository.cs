using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class RevenueRepository : BaseRepository, IRevenueRepository
    {
        public async Task<IEnumerable<AvgRevenueDTO>> GetAvgData(AvgRevenueMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.AvgRevenue))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var data = GetDataItems<AvgRevenueDTO>(reader);
                    return data;
                }
            }
        }
    }
}