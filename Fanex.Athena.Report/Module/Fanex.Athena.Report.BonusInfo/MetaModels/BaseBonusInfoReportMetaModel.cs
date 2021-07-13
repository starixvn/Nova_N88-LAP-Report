using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Core.Utils;
using Fanex.Data;
using Fanex.NVC.Authentication.Infrastructure;
using System;

namespace Fanex.Athena.Report.BonusInfo.MetaModels
{
    public class BaseBonusInfoReportMetaModel : ITimezoneMetaModel, IBrandIdMetaModel, IValidationMetaModel, ICurrencyIdListMetaModel
    {
        [SpParam(SqlParamName = "LicenseeId")]
        public int BrandId { get; set; }

        [SpParam(SqlParamName = "CurrencyIds")]
        public string CurrencyIdList { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Timezone { get; set; } = FilterConsts.DefaultTimezone;

        [SpParam(SqlParamName = "ClaimedStatusIds")]
        public string ClaimStatus { get; set; }

        [SpParam(SqlParamName = "DisplayCurrency")]
        public string BaseCurrency { get; set; }

        [SpParam(Ignored = true)]
        public virtual bool IsValid => BrandId > 0 && FromDate <= ToDate
            && !string.IsNullOrWhiteSpace(BaseCurrency);
    }
}