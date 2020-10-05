using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class BonusClaimedRepository : BaseRepository, IBonusClaimedRepository
    {
        protected override string TotalRowsParamName => "NRows";

        public async Task<BonusClaimedSummaryDTO> GetSummaryData(BonusClaimedSummaryMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.BonusClaimedSummary))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var totalData = GetDataItems<TotalBonusClaimedDTO>(reader);
                    var detailData = GetDataItems<DetailBonusClaimedDTO>(reader, true);

                    return new BonusClaimedSummaryDTO
                    {
                        TotalData = totalData,
                        DetailData = detailData,
                    };
                }
            }
        }

        public async Task<(IEnumerable<BonusClaimedDetailDTO> rows, int totalRows)> GetDetailsData(BonusClaimedDetailMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.BonusClaimedDetail))
            {
                var rows = Enumerable.Empty<BonusClaimedDetailDTO>();
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    rows = GetDataItems<BonusClaimedDetailDTO>(reader);
                }

                var totalRows = GetTotalRows(db);
                return (rows, totalRows);
            }
        }
    }
}