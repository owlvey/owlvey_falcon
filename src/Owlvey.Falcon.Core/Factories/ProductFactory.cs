using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ProductEntity
    {
        public static class Factory {

            public static ProductEntity Create(string name, string description, 
                DateTime on, string user)
            {
                var entity = new ProductEntity()
                {
                    Name = name,
                    Description = description,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                };
                entity.Validate();
                return entity;
            }
        }
    }
}
