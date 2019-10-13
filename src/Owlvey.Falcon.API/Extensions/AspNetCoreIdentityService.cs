using Owlvey.Falcon.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Extensions
{
    public class AspNetCoreIdentityService : IUserIdentityGateway
    {
        public string GetIdentity()
        {
            return "userId";
        }
    }
}
