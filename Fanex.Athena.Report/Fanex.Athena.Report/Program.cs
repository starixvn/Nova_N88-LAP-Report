namespace Fanex.Athena.Report
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console.Internal;
    using Sentry.Protocol;
    using System;

    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "N88 Report API";
            var host = CreateWebHostBuilder(args).Build();
            host.Run();

            throw new Exception("Application starting...");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) =>
                 {
                     var env = builderContext.HostingEnvironment;
                     config.SetBasePath(env.ContentRootPath);
                     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                     config.AddEnvironmentVariables();
                 })
             .UseStartup<Startup>()
             .UseSentry();
    }
}