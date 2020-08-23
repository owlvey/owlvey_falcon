using Owlvey.Falcon.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class JourneyMapEntity
    {
        public static class Factory {

            public static JourneyMapEntity Create(JourneyEntity journey, FeatureEntity feature, DateTime on, string createdBy) {

                var entity = new JourneyMapEntity()
                {
                    Journey = journey,
                    Feature = feature,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on
                };

                if (journey.ProductId != feature.ProductId) {
                    throw new InvalidStateException("journey and feature must be come from same product");
                }

                entity.Validate();

                return entity;                
            }

        }
    }
}
