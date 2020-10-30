using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class PerformanceHighlightRepository : BaseRepository, IPerformanceHighlightRepository
    {
        public async Task<PerformanceHighlightDTO> GetData(PerformanceHighlightMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.PerformanceHighlight))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var data = GetDataItems<PerformanceHighlightDTO>(reader);
                    var result = data.FirstOrDefault();
                    return result;
                }
            }
        }

        public async Task<PerformanceHighlightExportDTO> ExportData(PerformanceHighlightExportAllMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.PerformanceHighlightExport))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var commonData = GetDataItems<PerformanceHighlightCommonExportDTO>(reader);
                    var registrationDetailsData = GetDataItems<PerformanceHighlightRegistrationExportDTO>(reader, true);

                    return new PerformanceHighlightExportDTO
                    {
                        CommonData = commonData,
                        RegistrationData = registrationDetailsData
                    };
                }
            }
        }
    }
}