using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ProductEntity
    {
        public static class Factory {

            public static ProductEntity Create(string name, DateTime on, string user, CustomerEntity customer)
            {
                var entity = new ProductEntity()
                {
                    Name = name,                    
                    Avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png",
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                };
                entity.Customer = customer;
                entity.Validate();
                return entity;
            }
        }
    }
}
