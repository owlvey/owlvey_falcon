using Owlvey.Falcon.Components.Gateways;
using System;
using System.Linq;

namespace Owlvey.Falcon.Identity
{
    public class AspNetCoreIdentityService : IUserIdentityService
    {
        public string GetIdentity()
        {
            return "userId";
        }
    }
}
