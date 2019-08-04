using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity
    {
        public static class Factory{
            public static CustomerEntity Create(string user, DateTime on, string name) {
                var result = new CustomerEntity
                {
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Name = name
                };
                return result;
            }
        }
    }
}
