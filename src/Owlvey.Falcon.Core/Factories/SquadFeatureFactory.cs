using System;
namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadFeatureEntity
    {
        public static class Factory {
            public static SquadFeatureEntity Create(SquadEntity squad, FeatureEntity feature, DateTime on , string createdBy) {
                var entity = new SquadFeatureEntity() {
                    Feature = feature,
                    Squad = squad,
                    CreatedBy = createdBy,
                    ModifiedBy = createdBy,
                    CreatedOn = on,
                    ModifiedOn = on,
                };
                entity.Validate();
                return entity;
            }
        }        
    }
}
