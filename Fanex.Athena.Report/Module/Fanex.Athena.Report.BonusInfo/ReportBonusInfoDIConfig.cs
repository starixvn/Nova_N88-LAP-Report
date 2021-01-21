using Fanex.Athena.Report.BonusInfo.Excels;
using Fanex.Athena.Report.BonusInfo.Repositories;
using Fanex.Athena.Report.BonusInfo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.BonusInfo
{
    public static class ReportBonusInfoDIConfig
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
            services.AddTransient<IBonusInfoRepository, BonusInfoRepository>();

            return services;
        }

        public static IServiceCollection SetupServices(this IServiceCollection services)
        {
            services.AddTransient<IBonusInfoService, BonusInfoService>();

            return services;
        }

        public static IServiceCollection SetupExcels(this IServiceCollection services)
        {
            services.AddTransient<IBonusInfoDetailExcel, BonusInfoDetailExcel>();
            services.AddTransient<IBonusInfoSummaryExcel, BonusInfoSummaryExcel>();

            return services;
        }
    }
}