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
