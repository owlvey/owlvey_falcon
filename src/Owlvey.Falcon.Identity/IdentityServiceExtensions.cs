using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Owlvey.Falcon.Components.Gateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Identity
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddAspNetCoreIndentityService(this IServiceCollection services)
        {
            services.TryAddTransient<IUserIdentityService, AspNetCoreIdentityService>();

            return services;
        }
    }
}
