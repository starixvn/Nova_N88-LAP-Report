using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using System.Collections.Generic;

namespace Fanex.Athena.Report.BonusInfo.Consts
{
    public static class BonusInfoDropdownOptions
    {
        public static readonly IEnumerable<MultipleSelectOption> BonusTypeList = new List<MultipleSelectOption> {
            new MultipleSelectOption { Key = 1, Value = ViewResources.BonusTypeOptionWelcomeDeposit, Status = true },
            new MultipleSelectOption { Key = 2, Value = ViewResources.BonusTypeOptionReDeposit, Status = true },
            new MultipleSelectOption { Key = 3, Value = ViewResources.BonusTypeOptionFreeBet, Status = true },
            new MultipleSelectOption { Key = 6, Value = ViewResources.BonusTypeOptionRebate, Status = true },
            new MultipleSelectOption { Key = 7, Value = ViewResources.BonusTypeOptionCashback, Status = true }
        };

        public static readonly IEnumerable<MultipleSelectOption> BonusStatusList = new List<MultipleSelectOption> {
            new MultipleSelectOption { Key = 1, Value = ViewResources.BonusStatusOptionActive, Status = true },
            new MultipleSelectOption { Key = 2, Value = ViewResources.BonusStatusOptionInactive, Status = true }
        };

        public static readonly IEnumerable<MultipleSelectOption> ClaimStatusList = new List<MultipleSelectOption> {
            new MultipleSelectOption { Key = 1, Value = ViewResources.ClaimStatusOptionRunning, Status = true },
            new MultipleSelectOption { Key = 2, Value = ViewResources.ClaimStatusOptionCanceled, Status = true },
            new MultipleSelectOption { Key = 4, Value = ViewResources.ClaimStatusOptionFulfill, Status = true },
            new MultipleSelectOption { Key = 5, Value = ViewResources.ClaimStatusOptionOtherPlatform, Status = true }
        };
    }
}