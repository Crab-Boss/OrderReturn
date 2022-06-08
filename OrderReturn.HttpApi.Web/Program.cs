using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace OrderReturn.HttpApi.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                                            .AddJsonFile("appsettings.Development.json")
#else
                                            .AddJsonFile("appsettings.json")
#endif
                                            .AddEnvironmentVariables()
                                            .Build();

            Log.Logger = new LoggerConfiguration()
#if DEBUG
                                   .MinimumLevel.Debug()
#else
                                   .MinimumLevel.Error()
#endif
                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                   .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                                   .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                                   .Enrich.FromLogContext()
                                   .WriteTo.Map(evt => evt.Level, (level, wt) => wt.File($"Logs\\" + level + $"-{DateTime.Now.ToString("yyyyMMdd")}.log"))
#if DEBUG
                                    .WriteTo.Console()
#else
                                    .WriteTo.Logger(l => l
                                              .Filter
                                              .ByIncludingOnly(f => f.Level== LogEventLevel.Error)
                                              .WriteTo.Console())
#endif
                                    .CreateLogger();
            try
            {
                Log.Information("Starting OrderReturn.HttpApi.Host...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "OrderReturn.HttpApi.Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseAutofac()
            .UseSerilog();
    }
}
