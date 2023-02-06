using Fanex.Athena.Report.AbnormalBet.DTO;
using Fanex.Athena.Report.AbnormalBet.MetaModels;
using Fanex.Athena.Report.Common.Model.ViewModel;

namespace Fanex.Athena.Report.AbnormalBet.Excels.Interfaces
{
    public interface IAbnormalBetIpSummaryExcel
    {
        byte[] Export(string template, SortableResultModel<AbnormalBetIpSummary> data, AbnormalBetMetaModel metaModel);
    }
}