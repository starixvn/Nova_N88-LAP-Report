using Fanex.Athena.Report.ByGroup.Excels;
using Fanex.Athena.Report.ByGroup.Repositories;
using Fanex.Athena.Report.ByGroup.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.ByGroup
{
    public static class ReportByGroupDIConfig
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddTransient<IWinLossRepository, WinLossRepository>();
            services.AddTransient<IWinLossService, WinLossService>();
            services.AddTransient<IWinLossExcelCreator, WinLossExcelCreator>();
            services.AddTransient<IWinLossExcel, WinLossExcel>();
        }
    }
}