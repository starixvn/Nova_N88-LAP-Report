using Fanex.Athena.Report.SabaWinLossAnalysis.Excels;
using Fanex.Athena.Report.SabaWinLossAnalysis.Excels.Interfaces;
using Fanex.Athena.Report.SabaWinLossAnalysis.Repositories;
using Fanex.Athena.Report.SabaWinLossAnalysis.Repositories.Interfaces;
using Fanex.Athena.Report.SabaWinLossAnalysis.Services;
using Fanex.Athena.Report.SabaWinLossAnalysis.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.SabaWinLossAnalysis
{
    public static class ReportSabaWinLossAnalysisDIConfig
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
            services.AddTransient<IWinlossAnalysisRepository, WinlossAnalysisRepository>();

            return services;
        }

        public static IServiceCollection SetupServices(this IServiceCollection services)
        {
            services.AddTransient<IWinlossAnalysisService, WinlossAnalysisService>();

            return services;
        }

        public static IServiceCollection SetupExcels(this IServiceCollection services)
        {
            services.AddTransient<IWinlossAnalysisExcel, WinlossAnalysisExcel>();

            return services;
        }
    }
}