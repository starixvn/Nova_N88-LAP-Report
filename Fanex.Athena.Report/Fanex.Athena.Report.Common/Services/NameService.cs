using System;
using System.Collections.Generic;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Model.DataDomain;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.Resources.Constants;

namespace Fanex.Athena.Report.Common.Services
{
    public class NameService : INameService
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        public NameService(ICacheRefAdapter cacheRefAdapter)
            => _cacheRefAdapter = cacheRefAdapter;

        public string GetBetTypeName(int betTypeId, string lang = null)
        {
            string suffix = GetBettypeNameSuffix(betTypeId);
            return _cacheRefAdapter.GetBettype(Convert.ToInt32(betTypeId)) + suffix;
        }

        public string GetSportName(int sportTypeId, string lang = null)
        {
            if (sportTypeId > 0)
            {
                return _cacheRefAdapter.GetSportName(sportTypeId, lang);
            }
            if (sportTypeId == HardCodeSportType.Parlay)
            {
                return ViewResources.Parlay;
            }
            string sportName = GetSabaSportSuffixName(sportTypeId, lang);
            var prefixText = ViewResources.Saba;
            if (String.IsNullOrWhiteSpace(sportName))
            {
                return sportTypeId.ToString();
            }
            return $"{prefixText}{sportName}";
        }

        private string GetSabaSportSuffixName(int sportTypeId, string lang)
        {
            return GetSportNameFromMapping(sportTypeId, lang) ?? string.Empty;
        }

        private string GetSportNameFromMapping(int sportTypeId, string lang)
        {
            var sportMappings = new Dictionary<int, Func<string>>
            {
                { HardCodeSportType.SabaBasketball, () => _cacheRefAdapter.GetSportName(SportType.BasketBall, lang) },
                { HardCodeSportType.SabaSoccer, () => _cacheRefAdapter.GetSportName(SportType.Soccer, lang) },
                { HardCodeSportType.SabaCricket, () => _cacheRefAdapter.GetSportName(SportType.Cricket, lang) },
                { HardCodeSportType.SabaSoccerPinGoal, () => ViewResources.SoccerPinGoal },
                { HardCodeSportType.SabaBasketballPinGoal, () => ViewResources.BasketballPinGoal },
                { HardCodeSportType.SabaESportPinGoal, () => ViewResources.ESportsPinGoal },
                { HardCodeSportType.SabaFutsal, () => _cacheRefAdapter.GetSportName(SportType.Futsal, lang) },
                { HardCodeSportType.SabaTennis, () => _cacheRefAdapter.GetSportName(SportType.Tennis, lang) },
                { HardCodeSportType.SabaIceHockey, () => _cacheRefAdapter.GetSportName(SportType.IceHockey, lang) },
                { HardCodeSportType.SabaKabaddi, () => _cacheRefAdapter.GetSportName(SportType.Kabaddi, lang) },
                { HardCodeSportType.SabaSoccerMarble, () => ViewResources.SoccerMarble }
            };

            return sportMappings.TryGetValue(sportTypeId, out var nameProvider) ? nameProvider() : null;
        }

        private string GetBettypeNameSuffix(int betType)
        {
            if (betType == BetType.NumberGameHappy5OddEven)
            {
                return " (" + _cacheRefAdapter.GetSportName(SportType.NumberGameHappy5) + ")";
            }

            if (betType >= BetType.VirtualBasketballOverUnder && betType <= BetType.VirtualBasketballFirstHalfHandicap)
            {
                return " (" + _cacheRefAdapter.GetSportName(SportType.VirtualBasketball) + ")";
            }

            if (betType == BetType.VirtualSportsTennisCorrectSoccer)
            {
                return $" ({_cacheRefAdapter.GetSportName(SportType.Tennis)})";
            }

            return string.Empty;
        }
    }
}