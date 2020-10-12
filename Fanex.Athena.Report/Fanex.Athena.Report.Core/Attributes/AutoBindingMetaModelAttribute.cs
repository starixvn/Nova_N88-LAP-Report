using Fanex.NVC.Authentication;
using Fanex.NVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoBindingMetaModelAttribute : TypeFilterAttribute
    {
        public AutoBindingMetaModelAttribute(bool bindAgent = false)
            : base(typeof(AutoBindingMetaModelImplAttribute))
        {
            Arguments = new[] { bindAgent as object };
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoBindingMetaModelImplAttribute : ActionFilterAttribute
    {
        private readonly INVCAuthInfoService _authInfoService;
        private readonly INVCMetaModelBindingService _bindingService;

        private readonly bool _bindAgent;

        public AutoBindingMetaModelImplAttribute(INVCAuthInfoService authInfoService, INVCMetaModelBindingService bindingService,
            bool bindAgent
         )
        {
            _authInfoService = authInfoService;
            _bindingService = bindingService;

            _bindAgent = bindAgent;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_bindAgent)
            {
                _bindingService.BindAgentXml(context.ActionArguments, _authInfoService.CurrencyAgentList);
            }

            await next();
        }
    }
}