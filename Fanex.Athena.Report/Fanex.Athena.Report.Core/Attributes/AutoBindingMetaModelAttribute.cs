using Fanex.Core.Utils;
using Fanex.NVC.Authentication;
using Fanex.NVC.Authentication.Infrastructure;
using Fanex.NVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoBindingMetaModelAttribute : TypeFilterAttribute
    {
        public AutoBindingMetaModelAttribute(bool bindAgent = false)
            : base(typeof(AutoBindingMetaModelImplAttribute))
        {
            Arguments = new object[] { bindAgent };
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
                HandleBindAgent(context);
            }

            await next();
        }

        private void HandleBindAgent(ActionExecutingContext context)
        {
            IDictionary<int, int[]> agentList = null;
            if (MetaModelHelper.IsMetaModel(context.ActionArguments, out IBrandIdListMetaModel brandIdListMetaModel))
            {
                agentList = BrandHelper.GetAgentList(_authInfoService.BrandList, brandIdListMetaModel.BrandIds);
            }
            else if (MetaModelHelper.IsMetaModel(context.ActionArguments, out IBrandIdMetaModel brandIdMetaModel))
            {
                agentList = BrandHelper.GetAgentList(_authInfoService.BrandList, new[] { brandIdMetaModel.BrandId });
            }
            else
            {
                agentList = _authInfoService.CurrencyAgentList;
            }

            _bindingService.BindAgentXml(context.ActionArguments, agentList);
        }
    }
}