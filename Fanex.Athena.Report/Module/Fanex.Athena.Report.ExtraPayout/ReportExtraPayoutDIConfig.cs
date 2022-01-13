using Fanex.Athena.Report.ExtraPayout.Excels;
using Fanex.Athena.Report.ExtraPayout.Excels.Interfaces;
using Fanex.Athena.Report.ExtraPayout.Repositories;
using Fanex.Athena.Report.ExtraPayout.Repositories.Interfaces;
using Fanex.Athena.Report.ExtraPayout.Services;
using Fanex.Athena.Report.ExtraPayout.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.ExtraPayout
{
    public static class ReportExtraPayoutDIConfig
    {
        public static void Setup(IServiceCollection services)
        {
            services
                .SetupRepos()
                .SetupServices()
                .SetupExcels();
        }

        public static IServiceCollection SetupRepos(this IServiceCollection services)
        {
            services.AddTransient<IExtraPayoutRepository, ExtraPayoutRepository>();

            return services;
        }

        public static IServiceCollection SetupServices(this IServiceCollection services)
        {
            services.AddTransient<IExtraPayoutService, ExtraPayoutService>();

            return services;
        }

        public static IServiceCollection SetupExcels(this IServiceCollection services)
        {
            services.AddTransient<IExtraPayoutDetailExcel, ExtraPayoutDetailExcel>();
            services.AddTransient<IExtraPayoutExcel, ExtraPayoutExcel>();

            return services;
        }
    }
}