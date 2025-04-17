using Fanex.Athena.Report.Common.Extensions;

namespace Fanex.Athena.Report.Common.Model.ResultModel
{
    public class SortableReportResultModel<T> : ReportResultModel<T>
    {
        public new SortableList<T> DataList { get; set; }
    }
}