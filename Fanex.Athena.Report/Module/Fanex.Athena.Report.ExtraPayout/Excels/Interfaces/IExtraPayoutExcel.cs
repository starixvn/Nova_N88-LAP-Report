using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.MetaModels;

namespace Fanex.Athena.Report.ExtraPayout.Excels.Interfaces
{
    public interface IExtraPayoutExcel
    {
        byte[] Export(string template, SortableResultModel<ExtraPayoutDTO> data, ExtraPayoutMetaModel metaModel);
    }
}