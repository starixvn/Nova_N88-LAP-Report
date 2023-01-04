using Fanex.Athena.Report.AbnormalBet.DTO;
using Fanex.Athena.Report.AbnormalBet.MetaModels;
using Fanex.Athena.Report.Common.Model.ViewModel;

namespace Fanex.Athena.Report.AbnormalBet.Excels.Interfaces
{
    public interface IAbnormalBetSummaryExcel
    {
        byte[] Export(string template, SortableResultModel<AbnormalBetSummary> data, AbnormalBetMetaModel metaModel);
    }
}