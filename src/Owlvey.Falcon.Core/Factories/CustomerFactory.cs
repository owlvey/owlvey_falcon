using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity
    {
        public static class Factory {

            public static CustomerEntity Create(string name, string createdBy)
            {
                var entity = new CustomerEntity()
                {
                    Name = name,
                    CreatedBy = createdBy
                };

                entity.Create(createdBy, DateTime.UtcNow);

                return entity;
            }
        }
    }
}
