using Fanex.Athena.Report.Core.Helpers;
using Fanex.Core.Utils;
using Fanex.NVC.Authentication;
using Fanex.NVC.Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Fanex.Athena.Report.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequestValidationAttribute : TypeFilterAttribute
    {
        public RequestValidationAttribute(bool validateCurrency = false, bool validateCurrencyList = false, bool validateBrand = false, bool validateBrandList = false)
            : base(typeof(RequestValidationImplAttribute))
        {
            Arguments = new[] { validateCurrency as object, validateCurrencyList as object, validateBrand as object, validateBrandList as object };
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequestValidationImplAttribute : ActionFilterAttribute
    {
        private const string ErrorMsg = "Invalid Params";

        private readonly ICurrencyValidation _currencyValidation;
        private readonly IBrandValidation _brandValidation;
        private readonly INVCAuthInfoService _authInfoService;

        private readonly bool _validateCurrency;
        private readonly bool _validateCurrencyList;
        private readonly bool _validateBrand;
        private readonly bool _validateBrandList;

        public RequestValidationImplAttribute(ICurrencyValidation currencyValidation, IBrandValidation brandValidation, INVCAuthInfoService authInfoService,
            bool validateCurrency,
            bool validateCurrencyList,
            bool validateBrand,
            bool validateBrandList
            )
        {
            _currencyValidation = currencyValidation;
            _brandValidation = brandValidation;
            _authInfoService = authInfoService;

            _validateCurrency = validateCurrency;
            _validateCurrencyList = validateCurrencyList;
            _validateBrand = validateBrand;
            _validateBrandList = validateBrandList;
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

        private bool IsValid(ActionExecutingContext filterContext)
        {
            var result = MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out IValidationMetaModel validationMetaModel) && validationMetaModel.IsValid;

            IBrandIdMetaModel brandIdMetaModel = null;
            IBrandIdListMetaModel brandIdListMetaModel = null;

            result &= !_validateBrand || (MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out brandIdMetaModel)
                && _brandValidation.IsValidBrandId(brandIdMetaModel, _authInfoService.BrandList));

            result &= !_validateBrandList || (MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out brandIdListMetaModel)
                && _brandValidation.IsValidBrandIdList(brandIdListMetaModel, _authInfoService.BrandList));

            result &= !_validateCurrency || (MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out ICurrencyIdMetaModel currencyIdMetaModel)
               && _currencyValidation.IsValidCurrency(currencyIdMetaModel.CurrencyId, AuthHelper.GetCurrencyList(_authInfoService.BrandList, brandIdMetaModel, brandIdListMetaModel)));

            result &= !_validateCurrencyList || (MetaModelHelper.IsMetaModel(filterContext.ActionArguments, out ICurrencyIdListMetaModel currencyIdListMetaModel)
                && _currencyValidation.IsValidCurrencyList(currencyIdListMetaModel.CurrencyIdList, AuthHelper.GetCurrencyList(_authInfoService.BrandList, brandIdMetaModel, brandIdListMetaModel)));

            return result;
        }
    }
}