using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity
    {
        public static class Factory {

            public static ServiceEntity Create(string name, float slo, DateTime on, string user, ProductEntity product)
            {

                var entity = new ServiceEntity()
                {
                    Name = name,                    
                    Slo = slo,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Product = product
                };

                entity.Validate();
                return entity;
            }
        }
    }
}
