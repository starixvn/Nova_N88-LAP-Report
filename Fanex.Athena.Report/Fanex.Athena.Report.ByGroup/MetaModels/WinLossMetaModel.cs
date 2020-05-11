using System;

namespace Fanex.Athena.Report.ByGroup.MetaModels
{
    public class WinLossMetaModel
    {
        public int LicenseeId { get; set; }
        public string CurrencyIds { get; set; }
        public byte FilterBy { get; set; }
        public byte GroupBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductIds { get; set; }
        public string UserName { get; set; }
        public string DisplayCurrency { get; set; }
        public bool IsIncludedTestData { get; set; }
        public bool IsNoDrawTicket { get; set; }
        public bool IsExportExcel { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}