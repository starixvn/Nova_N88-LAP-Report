using Fanex.Athena.Report.Common.Repositories;
using Fanex.Athena.Report.Marketing.Consts;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public class NewlyRegisterUsersRepository : BaseRepository, INewlyRegisterUsersRepository
    {
        protected override string TotalRowsParamName => "NRows";

        public async Task<NewlyRegisterUsersSummaryDTO> GetSummaryData(NewlyRegisterUsersSummaryMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.NewlyRegisteredUsersSummary))
            {
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    var totalData = GetDataItems<TotalNewlyRegisterUsersDTO>(reader);
                    var campaignData = GetDataItems<CampaignNewlyRegisterUsersDTO>(reader, true);
                    var signupData = GetDataItems<SignupUrlNewlyRegisterUsersDTO>(reader, true);

                    return new NewlyRegisterUsersSummaryDTO
                    {
                        TotalData = totalData,
                        CampaignData = campaignData,
                        SignupData = signupData
                    };
                }
            }
        }

        public async Task<(IEnumerable<NewlyRegisterUsersDetailDTO> rows, int totalRows)> GetDetailsData(NewlyRegisterUsersDetailMetaModel metaModel)
        {
            using (var db = GetObjectDb(SpConsts.NewlyRegisteredUsersDetail))
            {
                IEnumerable<NewlyRegisterUsersDetailDTO> rows = null;
                using (var reader = await db.ExecuteReaderAsync(metaModel))
                {
                    rows = GetDataItems<NewlyRegisterUsersDetailDTO>(reader);
                }

                var totalRows = GetTotalRows(db);
                return (rows, totalRows);
            }
        }
    }
}