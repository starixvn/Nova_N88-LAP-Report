using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Fanex.Athena.Report.ExtraPayout.MetaModels
{
    public class ExtraPayoutMetaModel : BaseWinLossMetaModel
    {
        private string _username;

        public string PromotionTypeIdList { get; set; }

        public string ProductCodeList { get; set; }

        public int DrillDownLevel { get; set; }

        [DefaultValue("")]
        public string CurrencyName { get; set; }

        [DefaultValue("")]
        public string BaseCurrency { get; set; }

        public IDictionary<int, string> UserCurrencyList { get; set; }

        public string DrillDownCurrencyName { get; set; }

        public string Username
        {
            get
            {
                return string.IsNullOrEmpty(_username) ? string.Empty : _username?.Trim();
            }

            set
            {
                _username = value;
            }
        }

        public bool IsConvertAllToBaseCurrency { get; set; }
    }
}