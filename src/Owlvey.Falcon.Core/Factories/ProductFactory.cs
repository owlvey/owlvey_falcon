using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ProductEntity
    {
        public static class Factory {

            public static ProductEntity Create(string name, string description, string createdBy)
            {
                var entity = new ProductEntity()
                {
                    Name = name,
                    Description = description,
                    CreatedBy = createdBy
                };

                entity.Create(createdBy, DateTime.UtcNow);

                return entity;
            }
        }
    }
}
