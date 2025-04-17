using Fanex.Athena.Report.PlayerSabaCoins.Excels;
using Fanex.Athena.Report.PlayerSabaCoins.Excels.Interfaces;
using Fanex.Athena.Report.PlayerSabaCoins.Repositories;
using Fanex.Athena.Report.PlayerSabaCoins.Repositories.Interfaces;
using Fanex.Athena.Report.PlayerSabaCoins.Services;
using Fanex.Athena.Report.PlayerSabaCoins.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.PlayerSabaCoins

{
    public static class ReportPlayerSabaCoinsDIConfig
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
            services.AddTransient<IPlayerSabaCoinsRepository, PlayerSabaCoinsRepository>();

            return services;
        }

        public static IServiceCollection SetupServices(this IServiceCollection services)
        {
            services.AddTransient<IPlayerSabaCoinsService, PlayerSabaCoinsService>();

            return services;
        }

        public static IServiceCollection SetupExcels(this IServiceCollection services)
        {
            services.AddTransient<IPlayerSabaCoinsExcel, PlayerSabaCoinsExcel>();

            return services;
        }
    }
}