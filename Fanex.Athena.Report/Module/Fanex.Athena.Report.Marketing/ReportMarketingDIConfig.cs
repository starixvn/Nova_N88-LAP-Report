using Fanex.Athena.Report.Marketing.Excels;
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
            services.AddTransient<IUserActivitiesRepository, UserActivitiesRepository>();
            services.AddTransient<IRevenueRepository, RevenueRepository>();
            services.AddTransient<INewlyRegisterUsersRepository, NewlyRegisterUsersRepository>();

            services.AddTransient<IPerformanceHighlightService, PerformanceHighlightService>();
            services.AddTransient<IUserActivitiesService, UserActivitiesService>();
            services.AddTransient<IRevenueService, RevenueService>();
            services.AddTransient<INewlyRegisterUsersService, NewlyRegisterUsersService>();

            services.AddTransient<INewlyRegisterUsersExcel, NewlyRegisterUsersExcel>();
        }
    }
}