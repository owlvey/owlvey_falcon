using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddAuthority(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment environtment)
        {   
            services.AddHttpContextAccessor();
            services.Configure<AuthorityOptions>(options => {
                options.Authority = configuration["Authentication:Authority"];
                options.ApiName = configuration["Authentication:ApiName"];
                options.NameClaimType = configuration["Authentication:NameClaimType"];
                options.RoleClaimType = configuration["Authentication:RoleClaimType"];
            });

            var sp = services.BuildServiceProvider();
            var authenticationOptions = sp.GetService<IOptions<AuthorityOptions>>();
            //
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = authenticationOptions.Value.Authority;
                options.Audience = authenticationOptions.Value.ApiName;
                options.TokenValidationParameters.ValidIssuers = new[] { authenticationOptions.Value.Authority };
                options.SecurityTokenValidators.Clear();
                options.RequireHttpsMetadata = false;
                options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler
                {
                    MapInboundClaims = false
                });
                options.TokenValidationParameters.NameClaimType = authenticationOptions.Value.NameClaimType;
                options.TokenValidationParameters.RoleClaimType = authenticationOptions.Value.RoleClaimType;
            }); /*
            .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authenticationOptions.Value.Authority;
                options.RequireHttpsMetadata = false;
                //options.MetadataAddress = authenticationOptions.Value.Authority;    
                
                options.ApiName = authenticationOptions.Value.ApiName;
                options.NameClaimType = authenticationOptions.Value.NameClaimType;
                options.RoleClaimType = authenticationOptions.Value.RoleClaimType;
            });*/

        }
    }

}
