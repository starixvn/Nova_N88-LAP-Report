using Fanex.Athena.Report.BonusInfo.Consts;
using Fanex.Athena.Report.Common.Model.MetaModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.Report.BonusInfo.DTO
{
    public class BonusInfoDropdownOptionsDTO
    {
        public IEnumerable<MultipleSelectOptions> BonusTypeList { get; set; }

        public IEnumerable<MultipleSelectOptions> BonusStatusList { get; set; }

        public IEnumerable<MultipleSelectOptions> ClaimStatusList { get; set; }

        public BonusInfoDropdownOptionsDTO()
        {
            BonusTypeList = BonusInfoDropdownOptions.BonusTypeList;
            BonusStatusList = BonusInfoDropdownOptions.BonusStatusList;
            ClaimStatusList = BonusInfoDropdownOptions.ClaimStatusList;
        }
    }
}