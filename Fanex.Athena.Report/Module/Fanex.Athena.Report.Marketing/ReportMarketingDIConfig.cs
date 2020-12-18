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
            services
                .SetupRepos()
                .SetupServices()
                .SetupExcels();
        }

        public static IServiceCollection SetupRepos(this IServiceCollection services)
        {
            services.AddTransient<IPerformanceHighlightRepository, PerformanceHighlightRepository>();
            services.AddTransient<IUserActivitiesRepository, UserActivitiesRepository>();
            services.AddTransient<IRevenueRepository, RevenueRepository>();
            services.AddTransient<INewlyRegisterUsersRepository, NewlyRegisterUsersRepository>();
            services.AddTransient<IWithdrawalRepository, WithdrawalRepository>();
            services.AddTransient<IBonusClaimedRepository, BonusClaimedRepository>();
            services.AddTransient<IDepositAmountRepository, DepositAmountRepository>();

            return services;
        }

        public static IServiceCollection SetupServices(this IServiceCollection services)
        {
            services.AddTransient<IPerformanceHighlightService, PerformanceHighlightService>();
            services.AddTransient<IUserActivitiesService, UserActivitiesService>();
            services.AddTransient<IRevenueService, RevenueService>();
            services.AddTransient<INewlyRegisterUsersService, NewlyRegisterUsersService>();
            services.AddTransient<IWithdrawalService, WithdrawalService>();
            services.AddTransient<IBonusClaimedService, BonusClaimedService>();
            services.AddTransient<IDepositAmountService, DepositAmountService>();

            return services;
        }

        public static IServiceCollection SetupExcels(this IServiceCollection services)
        {
            services.AddTransient<INewlyRegisterUsersExcel, NewlyRegisterUsersExcel>();
            services.AddTransient<IPerformanceHighlightExcel, PerformanceHighlightExcel>();
            services.AddTransient<IWithdrawalExcel, WithdrawalExcel>();
            services.AddTransient<IBonusClaimedExcel, BonusClaimedExcel>();
            services.AddTransient<IDepositAmountExcel, DepositAmountExcel>();

            return services;
        }
    }
}