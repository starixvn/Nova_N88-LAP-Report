using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Repositories
{
    public interface IDepositAmountRepository
    {
        Task<DepositAmountSummaryDTO> GetSummaryData(DepositAmountSummaryMetaModel metaModel);

        Task<(IEnumerable<DepositAmountDetailDTO> rows, int totalRows)> GetDetailsData(DepositAmountDetailMetaModel metaModel);
    }
}