using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SquadEntity
    {
        public static class Factory {

            public static SquadEntity Create(string name, DateTime on, 
                string user, CustomerEntity customer)
            {
                string avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png";
                var entity = new SquadEntity()
                {
                    Name = name,                    
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Avatar = avatar,
                    Customer = customer
                };
                entity.Validate();
                return entity;
            }            
        }
    }
}
