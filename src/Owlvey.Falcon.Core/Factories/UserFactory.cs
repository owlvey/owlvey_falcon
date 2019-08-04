using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class UserEntity
    {
        public static class Factory {
            public static UserEntity Create(string user, DateTime on, string email) {
                var result = new UserEntity
                {
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Email = email
                };
                return result;
            }
        }
        
    }
}
