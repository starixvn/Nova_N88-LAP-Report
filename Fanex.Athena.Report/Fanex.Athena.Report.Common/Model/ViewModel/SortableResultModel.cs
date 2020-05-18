namespace Fanex.Athena.Report.Common.Model.ViewModel
{
    using Fanex.Athena.Report.Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SortableResultModel<T> : ResultModel<T>
    {
        private ISortableList<T> _sortableDataList;

        public override IEnumerable<T> DataList
        {
            get => this._sortableDataList;
            set => _sortableDataList = new SortableList<T>(value);
        }

        public static SortableResultModel<T> Default => new SortableResultModel<T>
        {
            DataList = Enumerable.Empty<T>(),
            TotalData = Activator.CreateInstance<T>(),
        };
    }
}