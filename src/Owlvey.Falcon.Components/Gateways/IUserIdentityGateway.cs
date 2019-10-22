using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Gateways
{
    public interface IUserIdentityGateway
    {
        string GetIdentity();
        string GetName();
        string GetRole();
        bool IsAdmin();
        bool IsGuest();
    }
}
