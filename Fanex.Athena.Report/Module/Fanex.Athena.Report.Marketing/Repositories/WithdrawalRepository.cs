using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class WithdrawalRepository : BaseRepository, IWithdrawalRepository
    {
        protected override string TotalRowsParamName => "NRows";

        public async Task<WithdrawalSummaryDTO> GetSummaryData(WithdrawalSummaryMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.WithdrawalSummary))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var data = GetDataItems<WithdrawalSummaryDTO>(reader);
                    return data.FirstOrDefault();
                }
            }
        }

        public async Task<(IEnumerable<WithdrawalDetailDTO> rows, int totalRows)> GetDetailsData(WithdrawalDetailMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.WithdrawalDetail))
            {
                IEnumerable<WithdrawalDetailDTO> rows = null;
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    rows = GetDataItems<WithdrawalDetailDTO>(reader);
                }

                var totalRows = GetTotalRows(db);
                return (rows, totalRows);
            }
        }
    }
}