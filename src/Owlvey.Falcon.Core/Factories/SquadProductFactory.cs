using System;
namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadProductEntity
    {
        public static class Factory {
            public static SquadProductEntity Create(SquadEntity squad, ProductEntity product, DateTime on , string createdBy) {
                var entity = new SquadProductEntity() {
                    Product = product,
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
