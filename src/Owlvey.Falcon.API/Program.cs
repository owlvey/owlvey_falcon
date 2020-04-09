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
using Serilog.Enrichers;
using System.Threading;

namespace Owlvey.Falcon.API
{
    public class Program
    {
        static readonly LoggerProviderCollection Providers = new LoggerProviderCollection();
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)                    
                    .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {                
                Log.Information("Starting web host at " + DateTime.Now.ToLongTimeString() );                                    
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
