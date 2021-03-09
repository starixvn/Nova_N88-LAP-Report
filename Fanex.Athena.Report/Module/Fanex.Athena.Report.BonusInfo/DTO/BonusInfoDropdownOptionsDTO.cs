using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.Common.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.Report.BonusInfo.DTO
{
    public class BonusInfoDropdownOptionsDTO
    {
        public IEnumerable<MultipleSelectOption> BonusTypeList { get; set; }

        public IEnumerable<MultipleSelectOption> BonusStatusList { get; set; }

        public IEnumerable<MultipleSelectOption> ClaimStatusList { get; set; }

        public BonusInfoDropdownOptionsDTO()
        {
            BonusTypeList = BonusInfoDropdownOptions.BonusTypeList;
            BonusStatusList = BonusInfoDropdownOptions.BonusStatusList;
            ClaimStatusList = BonusInfoDropdownOptions.ClaimStatusList;
        }
    }
}