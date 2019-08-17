using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class FeatureEntity
    {
        public static class Factory {

            public static FeatureEntity Create(string name, string description, DateTime on, string user, 
                ProductEntity product)
            {
                var entity = new FeatureEntity()
                {
                    Name = name,
                    Description = description,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Product = product
                };
                entity.Avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png";
                entity.Validate();
                return entity;
            }
        }
    }
}
