using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity
    {
        public static class Factory {

            public static ServiceEntity Create(string name, string description, float slo, DateTime on, string user)
            {
                var entity = new ServiceEntity()
                {
                    Name = name,
                    Description = description,
                    SLO = slo,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                };
                entity.Validate();
                return entity;
            }
        }
    }
}
