using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.Marketing
{
    public static class ReportMarketingDIConfig
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddTransient<IPerformanceHighlightRepository, PerformanceHighlightRepository>();
            services.AddTransient<IPerformanceHighlightService, PerformanceHighlightService>();
        }
    }
}