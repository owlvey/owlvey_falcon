using GST.Fake.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Owlvey.Falcon.Components;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Owlvey.Falcon.IntegrationTests.Setup
{
    public class Shell : IDisposable
    {
        private static Lazy<Shell> _Builder = new Lazy<Shell>(() =>
        {
            var shell = new Shell(new Container());
            shell.Init();
            return shell;
        });

        public readonly IContainer Container;
        private TestServer Server { get; set; }
        private Shell(IContainer container)
        {
            this.Container = container;
        }
        
        public static Shell Instance
        {
            get
            {
                return _Builder.Value;
            }

        }
        public static bool IsDevelopment()
        {
            var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT") ?? "Developmenta";
            return environment == "Development";
        }
        public static string APIHost()
        {
            var environment = Environment.GetEnvironmentVariable("OWLVEY_API_HOST") ?? "http://api.owlvey.com:48100";
            return environment;
        }
        public static string IdentityHost()
        {
            var environment = Environment.GetEnvironmentVariable("OWLVEY_IDENTITY_HOST") ?? "http://identity.owlvey.com:48100";
            return environment;
        }
        private void Init()
        {
            var services = new ServiceCollection();                                          
            if (IsDevelopment())
            {
                // add TestServer Client
                var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<TestStartup>();
                this.Server = new TestServer(builder);
                var client = this.Server.CreateClient();                
                services.AddSingleton<HttpClient>(client);                
            }
            else {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(APIHost())
                };
                services.AddSingleton<HttpClient>(client);
                
            }
            this.Container.Configure(config =>
            {
                config.Scan(sp =>
                {
                    sp.AssemblyContainingType(typeof(IBaseTest));
                    sp.WithDefaultConventions();
                });
                config.Populate(services);
            });
        }

        public void Dispose()
        {
            if (Server != null)
            {
                this.Server.Dispose();
            }
            if (this.Container != null)
            {
                this.Container.Dispose();
            }
        }
    }
}
