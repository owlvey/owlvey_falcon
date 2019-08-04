using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class CustomerEntity
    {
<<<<<<< HEAD
        public static class Factory{
            public static CustomerEntity Create(string user, DateTime on, 
                string name,
                string avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png")
            {
                var result = new CustomerEntity
=======
        public static class Factory {

            public static CustomerEntity Create(string name, DateTime on, string user)
            {
                var entity = new CustomerEntity()
>>>>>>> 4c019572161af3f9d8d9106d964c228d54e41492
                {
                    Name = name,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
<<<<<<< HEAD
                    Name = name,
                    Avatar = avatar
                };
                result.Validate();
                return result;
=======
                };

                return entity;
>>>>>>> 4c019572161af3f9d8d9106d964c228d54e41492
            }
        }
    }
}
