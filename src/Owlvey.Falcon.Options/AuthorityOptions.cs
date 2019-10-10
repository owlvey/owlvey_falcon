using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Options
{
    public class AuthorityOptions
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
    }
}
