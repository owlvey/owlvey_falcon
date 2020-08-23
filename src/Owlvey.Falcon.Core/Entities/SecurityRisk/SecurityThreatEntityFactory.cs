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
                CreatedBy = createdBy,
                CreatedOn = createdOn,
                ModifiedBy = createdBy,
                ModifiedOn = createdOn
            };
            return entity;
        }
    }
}
