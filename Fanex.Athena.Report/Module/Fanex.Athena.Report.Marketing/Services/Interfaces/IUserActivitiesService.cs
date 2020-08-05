using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public interface IUserActivitiesService
    {
        Task<IEnumerable<UserActivitiesViewModel>> GetData(UserActivitiesMetaModel metaModel);
    }
}