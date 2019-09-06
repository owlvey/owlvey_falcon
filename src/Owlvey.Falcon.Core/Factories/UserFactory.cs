using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class UserEntity
    {
        public static class Factory {
            public static UserEntity Create(string user, DateTime on, string email) {
                string avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png";
                var result = new UserEntity
                {
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Email = email,
                    Avatar = avatar,
                    Name = email
                };
                result.Validate();
                return result;
            }
        }
        
    }
}
