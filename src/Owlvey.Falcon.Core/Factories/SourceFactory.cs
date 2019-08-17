using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        public static class Factory {

            public static SourceEntity Create(ProductEntity product,  string name, DateTime on, string user)
            {
                string goodDefinition = "good definition of events";
                string totalDefinition = "total definition of events";
                var entity = new SourceEntity()
                {
                    Name = name,
                    GoodDefinition = goodDefinition,
                    TotalDefinition = totalDefinition,
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
