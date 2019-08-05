using Owlvey.Falcon.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceMapEntity
    {
        public static class Factory {

            public static ServiceMapEntity Create(ServiceEntity service, FeatureEntity feature, DateTime on, string createdBy) {

                var entity = new ServiceMapEntity()
                {
                    Service = service,
                    Feature = feature,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on
                };

                if (service.Product.Id != feature.Product.Id) {
                    throw new InvalidStateException("service and feature must be come from same product");
                }

                entity.Validate();

                return entity;                
            }

        }
    }
}
