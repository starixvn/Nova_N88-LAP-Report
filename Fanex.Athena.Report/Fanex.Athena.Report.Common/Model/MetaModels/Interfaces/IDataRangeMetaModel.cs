using System;

namespace Fanex.Athena.Report.Common.Model.MetaModels.Interfaces
{
    public interface IDataRangeMetaModel
    {
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
    }
}