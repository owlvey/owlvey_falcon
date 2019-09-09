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
                    MTTD = 10,
                    MTTR = 10,
                    MTTF = 43200
                };
                entity.Avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png";
                entity.Validate();
                return entity;
            }
        }
    }
}
