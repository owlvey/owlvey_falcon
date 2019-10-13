using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Extensions
{
    public class AuthorityOptions
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
    }
}
