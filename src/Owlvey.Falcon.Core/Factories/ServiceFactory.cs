using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity
    {
        public static class Factory {

            public static ServiceEntity Create(string name, string description, float slo, string createdBy)
            {
                var entity = new ServiceEntity()
                {
                    Name = name,
                    Description = description,
                    SLO = slo,
                    CreatedBy = createdBy
                };

                entity.Create(createdBy, DateTime.UtcNow);

                return entity;
            }
        }
    }
}
