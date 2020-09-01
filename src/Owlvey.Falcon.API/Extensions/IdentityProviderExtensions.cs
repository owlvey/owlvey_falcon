using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Owlvey.Falcon.Gateways;

namespace Owlvey.Falcon.API.Extensions
{
    public static class IdentityProviderExtensions
    {
        public static IServiceCollection AddAspNetCoreIndentityProvider(this IServiceCollection providers)
        {
            providers.TryAddTransient<IUserIdentityGateway, AspNetCoreIdentityGateway>();

            return providers;
        }
    }
}
