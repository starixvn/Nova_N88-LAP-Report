namespace Fanex.Athena.Report.Common.Model.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ResultModel<T>
    {
        public virtual IEnumerable<T> DataList { get; set; }

        public T TotalData { get; set; }

        public int TotalRows { get; set; }

        public string AdditionalInfo { get; set; }

        public static ResultModel<T> Default => new ResultModel<T>
        {
            DataList = Enumerable.Empty<T>(),
            TotalData = Activator.CreateInstance<T>(),
        };
    }
}