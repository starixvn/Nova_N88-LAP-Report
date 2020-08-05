using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public interface IRevenueRepository
    {
        Task<IEnumerable<AvgRevenueDTO>> GetAvgData(AvgRevenueMetaModel metaModel);
    }
}