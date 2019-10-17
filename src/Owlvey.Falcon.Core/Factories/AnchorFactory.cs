using System;
using Owlvey.Falcon.Core.Entities;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class AnchorEntity
    {
        public static class Factory {
            public static AnchorEntity Create(string name, DateTime on, string user, ProductEntity product)
            {
                var entity = new AnchorEntity()
                {
                    Name = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Target = new DateTime(on.Year, 1, 1).Date
                };
                entity.Product = product;                
                entity.Validate();
                return entity;
            }
        }
    }
}
