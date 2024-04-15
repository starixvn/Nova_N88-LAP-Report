namespace Fanex.Athena.Report.Common
{
    using Fanex.Athena.Report.Common.Excel;
    using Fanex.Athena.Report.Common.Services;
    using Fanex.Athena.Report.Common.Services.Interfaces;
    using Fanex.NVC.Authentication;
    using Microsoft.Extensions.DependencyInjection;

    public static class SharedServiceDIConfig
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddTransient<IExcelBuilder, ExcelBuilder>();
            services.AddTransient<ICacheRefAdapter, CacheRefAdapter>();
            services.AddTransient<INVCPermissionService, NVCPermissionService>();
            services.AddTransient<IDataPermissionService, DataPermissionService>();
            services.AddSingleton<IBalanceUpService, BalanceUpService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ICurrencyService, CurrencyService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IDataMapper, DataMapper>();
            services.AddSingleton<ICalculationService, CalculationService>();
            services.AddSingleton<ICacheRefAdapter, CacheRefAdapter>();
            services.AddSingleton<ICompressService, CompressService>();
            services.AddSingleton<INameService, NameService>();
        }
    }
}