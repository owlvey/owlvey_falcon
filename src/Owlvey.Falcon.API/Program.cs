using System;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Compact;

namespace Owlvey.Falcon.API
{
    public class Program
    {
        static readonly LoggerProviderCollection Providers = new LoggerProviderCollection();
        public static int Main(string[] args)
        {
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new RenderedCompactJsonFormatter(), 
                         "./logs/log.log", shared: false,
                        fileSizeLimitBytes: 1_000_000,
                        rollOnFileSizeLimit: true,
                        rollingInterval: RollingInterval.Hour,
                        flushToDiskInterval: TimeSpan.FromSeconds(10))               
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var configuration = new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build();
                BuildWebHost(args, configuration).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }            
        }

        public static IWebHost BuildWebHost(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog(providers: Program.Providers)
            .UseConfiguration(configuration)
            .UseStartup<Startup>()            
            .Build();        
    }
}
