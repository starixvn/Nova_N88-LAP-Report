using Fanex.Athena.Report.AbnormalBet.Excels;
using Fanex.Athena.Report.AbnormalBet.Excels.Interfaces;
using Fanex.Athena.Report.AbnormalBet.Repositories;
using Fanex.Athena.Report.AbnormalBet.Repositories.Interfaces;
using Fanex.Athena.Report.AbnormalBet.Services;
using Fanex.Athena.Report.AbnormalBet.Services.Interfaces;
using Fanex.Core.Utils.Repository;
using Fanex.Core.Utils.Repository.Interfaces;
using Fanex.Core.Utils.Service;
using Fanex.Core.Utils.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Athena.Report.AbnormalBet
{
    public static class ReportAbnormalBetDIConfig
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
            services.AddTransient<IAbnormalBetRepository, AbnormalBetRepository>();
            services.AddTransient<IIpRepository, IpRepository>();

            return services;
        }

        public static IServiceCollection SetupServices(this IServiceCollection services)
        {
            services.AddTransient<IAbnormalBetService, AbnormalBetService>();
            services.AddTransient<ISubIssueService, SubIssueService>();
            services.AddTransient<IIpService, IpService>();

            return services;
        }

        public static IServiceCollection SetupExcels(this IServiceCollection services)
        {
            services.AddTransient<IAbnormalBetDetailExcel, AbnormalBetDetailExcel>();
            services.AddTransient<IAbnormalBetIpSummaryExcel, AbnormalBetIpSummaryExcel>();
            services.AddTransient<IAbnormalBetSummaryExcel, AbnormalBetSummaryExcel>();

            return services;
        }
    }
}