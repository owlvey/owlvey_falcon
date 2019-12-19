using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class FeatureEntity
    {
        public static class Factory {

            public static FeatureEntity Create(string name, DateTime on, string user, 
                ProductEntity product)
            {
                var entity = new FeatureEntity()
                {
                    Name = name,
                    Description = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Product = product,                    
                };
                entity.Avatar = "https://cdn.iconscout.com/icon/premium/png-256-thumb/hexagonal-1-727847.png";
                entity.Validate();
                return entity;
            }
        }
    }
}
