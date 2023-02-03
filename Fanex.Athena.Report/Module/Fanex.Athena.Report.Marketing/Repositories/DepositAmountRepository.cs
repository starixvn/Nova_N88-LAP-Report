using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class DepositAmountRepository : BaseRepository, IDepositAmountRepository
    {
        protected override string TotalRowsParamName => "NRows";

        public async Task<DepositAmountSummaryDTO> GetSummaryData(DepositAmountSummaryMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.DepositAmountSummary))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var totalData = GetDataItems<TotalDepositAmountSummaryDTO>(reader);
                    var signupData = GetDataItems<SignupUrlDepositAmountDTO>(reader, true);

                    return new DepositAmountSummaryDTO
                    {
                        TotalData = totalData,
                        SignupData = signupData,
                    };
                }
            }
        }

        public async Task<(IEnumerable<DepositAmountDetailDTO> rows, int totalRows)> GetDetailsData(DepositAmountDetailMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.DepositAmountDetail))
            {
                IEnumerable<DepositAmountDetailDTO> rows = null;
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    rows = GetDataItems<DepositAmountDetailDTO>(reader);
                }

                var totalRows = GetTotalRows(db);
                return (rows, totalRows);
            }
        }
    }
}