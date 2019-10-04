using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ServiceEntity
    {
        public static class Factory {

            public static ServiceEntity Create(string name, DateTime on, string user, ProductEntity product)
            {
                var entity = new ServiceEntity()
                {
                    Name = name,                    
                    Slo = 0.99m,
                    Avatar = "https://cdn4.iconfinder.com/data/icons/pretty-office-part-7-reflection-style/256/Cup-gold.png",
                    CreatedBy = user,
                    ModifiedBy = user,
                    Owner = user,
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
