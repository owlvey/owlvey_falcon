using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity
    {
        public static class Factory {

            public static CustomerEntity Create(string name, DateTime on, string user)
            {
                var entity = new CustomerEntity()
                {
                    Name = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                };

                return entity;
            }
        }
    }
}
