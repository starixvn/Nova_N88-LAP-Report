using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Model.MetaModels.Interfaces;
using Fanex.Athena.Report.Core.Helpers;
using Fanex.Core.Utils;
using Fanex.NVC.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Fanex.Athena.Report.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequestBonusInfoValidationAttribute : TypeFilterAttribute
    {
        public RequestBonusInfoValidationAttribute(
            bool valiBonusTypeList = false,
            bool valiBonusInfo = false)
            : base(typeof(RequestBonusInfoValidationImplAttribute))
        {
            Arguments = new object[] {
                valiBonusTypeList,
                valiBonusInfo
            };
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequestBonusInfoValidationImplAttribute : ActionFilterAttribute
    {
        private const string ErrorMsg = "Invalid Params";

        private readonly INVCAuthInfoService _authInfoService;

        private readonly bool _valiBonusTypeList;
        private readonly bool _valiBonusInfo;

#pragma warning disable S107 // Methods should not have too many parameters

        public RequestBonusInfoValidationImplAttribute(INVCAuthInfoService authInfoService, bool valiBonusTypeList, bool valiBonusInfo)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _authInfoService = authInfoService;

            _valiBonusTypeList = valiBonusTypeList;
            _valiBonusInfo = valiBonusInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsValid(context))
            {
                context.Result = new JsonResult(new { Msg = ErrorMsg, IsSuccess = false })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

#pragma warning disable S1541 // Methods and properties should not be too complex

        private bool IsValid(ActionExecutingContext filterContext)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            var result = MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out IValidationMetaModel validationMetaModel) && validationMetaModel.IsValid;

            result &= !_valiBonusTypeList || (MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out IBonusTypeMetaModel bonusType)
                && (_authInfoService.Level != (int)Constant.Level.User || BonusInfoAuthHelper.IsValidBonusType(bonusType, _authInfoService.FunctionList)));

            result &= !_valiBonusInfo || (MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out IBonusInfoMetaModel bonusInfo)
                && (_authInfoService.Level != (int)Constant.Level.User || BonusInfoAuthHelper.IsValidBonusInfo(bonusInfo, _authInfoService.FunctionList)));

            return result;
        }
    }
}