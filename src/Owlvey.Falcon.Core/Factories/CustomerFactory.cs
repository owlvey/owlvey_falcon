using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity
    {

        public static class Factory{
            public static CustomerEntity Create(string user, DateTime on, 
                string name,
                string avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png")
            {
                var result = new CustomerEntity
                {
                    Name = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,                    
                    Avatar = avatar
                };
                result.Validate();
                return result;
            }
        }
    }
}
