using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class UserActivitiesRepository : BaseRepository, IUserActivitiesRepository
    {
        public async Task<IEnumerable<UserActivitiesDTO>> GetData(UserActivitiesMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.UserActivities))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var data = GetDataItems<UserActivitiesDTO>(reader);
                    return data;
                }
            }
        }
    }
}