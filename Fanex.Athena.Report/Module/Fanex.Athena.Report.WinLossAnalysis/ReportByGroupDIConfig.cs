using Fanex.Athena.Report.WinLossAnalysis.Excels;
using Fanex.Athena.Report.WinLossAnalysis.Repositories;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.WinLossAnalysis
{
    public static class ReportByGroupDIConfig
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddTransient<IWinLossRepository, WinLossRepository>();
            services.AddTransient<IWinLossService, WinLossService>();
            services.AddTransient<IWinLossExcelCreator, WinLossExcelCreator>();
            services.AddTransient<IWinLossExcelCreator, WinLossExcelCreator>();
            SetupExcel(services);
        }

        private static void SetupExcel(IServiceCollection services)
        {
            services.AddTransient<WinLossExcel>();
            services.AddTransient<WinLossPlayerExcel>();
        }
    }
}