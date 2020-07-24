using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class PerformanceHighlightRepository : BaseRepository, IPerformanceHighlightRepository
    {
        public async Task<PerformanceHighlightDTO> GetData(PerformanceHighlightMetaModel metaModel)
        {
            using (var db = ObjectDbFactory.CreateInstance(SpConsts.PerformanceHighlight))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var data = GetDataItems<PerformanceHighlightDTO>(reader);
                    var result = data.FirstOrDefault();
                    return result;
                }
            }
        }
    }
}