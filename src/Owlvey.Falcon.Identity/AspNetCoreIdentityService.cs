using Owlvey.Falcon.Gateways;
using System;
using System.Linq;

namespace Owlvey.Falcon.Identity
{
    public class AspNetCoreIdentityService : IUserIdentityGateway
    {
        public string GetIdentity()
        {
            return "userId";
        }
    }
}
