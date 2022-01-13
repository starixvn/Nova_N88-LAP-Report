using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.MetaModels;

namespace Fanex.Athena.Report.ExtraPayout.Excels.Interfaces
{
    public interface IExtraPayoutDetailExcel
    {
        byte[] Export(string template, SortableResultModel<ExtraPayoutDetailDTO> data, ExtraPayoutDetailMetaModel metaModel);
    }
}