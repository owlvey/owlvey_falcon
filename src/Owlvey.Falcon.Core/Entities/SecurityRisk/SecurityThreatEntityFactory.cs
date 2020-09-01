using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SecurityThreatEntity
    {
        public static SecurityThreatEntity Create(string name,
            string createdBy, DateTime createdOn)
        {
            var entity = new SecurityThreatEntity()
            {
                Name = name,
                Description = "Threat description",
                Avatar = "https://www.flaticon.com/premium-icon/icons/svg/379/379743.svg",
                Reference= "https://en.wikipedia.org/wiki/Threat_(computer)",
                Tags="#security,#threat",
                CreatedBy = createdBy,
                CreatedOn = createdOn,
                ModifiedBy = createdBy,
                ModifiedOn = createdOn
            };
            return entity;
        }
    }
}
