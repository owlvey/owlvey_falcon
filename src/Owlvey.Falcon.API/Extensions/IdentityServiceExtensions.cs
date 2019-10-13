using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Owlvey.Falcon.Gateways;

namespace Owlvey.Falcon.API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddAspNetCoreIndentityService(this IServiceCollection services)
        {
            services.TryAddTransient<IUserIdentityGateway, AspNetCoreIdentityService>();

            return services;
        }
    }
}
