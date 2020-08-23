using Microsoft.AspNetCore.Http;
using Owlvey.Falcon.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Extensions
{
    public class AspNetCoreIdentityGateway : IUserIdentityGateway
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AspNetCoreIdentityGateway(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetIdentity()
        {
            return this._httpContextAccessor.HttpContext.User.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.InvariantCultureIgnoreCase) || c.Type.Equals("sub", StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public string GetName()
        {
            return this._httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "fullname").Value;
        }

        public string GetRole()
        {
            return this._httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "role").Value;
        }
        
        public bool IsAdmin()
        {
            return this._httpContextAccessor.HttpContext.User.IsInRole("admin");
        }

        public bool IsGuest()
        {
            return this._httpContextAccessor.HttpContext.User.IsInRole("guest");
        }
        
    }
}
