using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Model.MetaModels;
using Fanex.Athena.Report.Common.Model.MetaModels.Interfaces;
using Fanex.NVC.Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Xml;

namespace Fanex.Athena.Report.Core.Helpers
{
    public static class BonusInfoAuthHelper
    {
        public static bool IsValidBonusType(IBonusTypeMetaModel bonusType, IEnumerable<NVCFunctionAuthInfo> FunctionList)
        {
            var bonusTypeList = bonusType.BonusType.Split(',');

            return bonusTypeList.All(id => FunctionList.Any(f => f.FunctionKey.Equals(PermissionKeys.BonusTypePrefix + id)));
        }

        public static bool IsValidBonusInfo(IBonusInfoMetaModel bonusInfo, IEnumerable<NVCFunctionAuthInfo> FunctionList)
        {
            const string bonuTypeIdElement = "bonustypeid";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(bonusInfo.BonusInfo);

            XmlNodeList elemlist = xmlDoc.GetElementsByTagName("r");
            List<string> bonusTypeList = new List<string>();

            foreach (XmlNode a in elemlist)
            {
                if (a.Attributes?[bonuTypeIdElement] != null)
                {
                    bonusTypeList.Add(a.Attributes[bonuTypeIdElement].Value);
                }
            }

            bonusTypeList = bonusTypeList.Distinct().ToList();

            return bonusTypeList.All(id => FunctionList.Any(f => f.FunctionKey.Equals(PermissionKeys.BonusTypePrefix + id)));
        }
    }
}