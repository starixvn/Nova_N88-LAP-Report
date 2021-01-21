using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Athena.Report.Resources;
using System.Collections.Generic;

namespace Fanex.Athena.Report.BonusInfo.Consts
{
    public static class BonusInfoDropdownOptions
    {
        public static readonly IEnumerable<MultipleSelectOptions> BonusTypeList = new List<MultipleSelectOptions> {
            new MultipleSelectOptions { Key = 1, Value = ViewResources.BonusTypeOptionWelcomeDeposit, Status = true },
            new MultipleSelectOptions { Key = 2, Value = ViewResources.BonusTypeOptionReDeposit, Status = true },
            new MultipleSelectOptions { Key = 3, Value = ViewResources.BonusTypeOptionFreeBet, Status = true },
            new MultipleSelectOptions { Key = 6, Value = ViewResources.BonusTypeOptionRebate, Status = true },
            new MultipleSelectOptions { Key = 7, Value = ViewResources.BonusTypeOptionCashback, Status = true }
        };

        public static readonly IEnumerable<MultipleSelectOptions> BonusStatusList = new List<MultipleSelectOptions> {
            new MultipleSelectOptions { Key = 1, Value = ViewResources.BonusStatusOptionActive, Status = true },
            new MultipleSelectOptions { Key = 2, Value = ViewResources.BonusStatusOptionInactive, Status = true }
        };

        public static readonly IEnumerable<MultipleSelectOptions> ClaimStatusList = new List<MultipleSelectOptions> {
            new MultipleSelectOptions { Key = 1, Value = ViewResources.ClaimStatusOptionRunning, Status = true },
            new MultipleSelectOptions { Key = 2, Value = ViewResources.ClaimStatusOptionCanceled, Status = true },
            new MultipleSelectOptions { Key = 4, Value = ViewResources.ClaimStatusOptionFulfill, Status = true },
            new MultipleSelectOptions { Key = 5, Value = ViewResources.ClaimStatusOptionOtherPlatform, Status = true }
        };
    }
}