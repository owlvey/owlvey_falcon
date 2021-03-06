﻿using System;
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
                    Description = name,
                    Avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png",
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    
                };

                var anchorSLI = AnchorEntity.Factory.Create("sample", on, user, entity);                
                entity.Anchors.Add(anchorSLI);                                
                entity.Validate();
                entity.Customer = customer;                
                customer.Products.Add(entity);


                return entity;
            }
        }
    }
}
