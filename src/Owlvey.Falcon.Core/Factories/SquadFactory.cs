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
                string avatar = "https://icons.iconarchive.com/icons/limav/game-of-thrones/256/Stark-icon.png";
                var entity = new SquadEntity()
                {
                    Name = name,                    
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Avatar = avatar,
                    
                };
                entity.Validate();
                entity.Customer = customer;
                customer.Squads.Add(entity);
                return entity;
            }            
        }
    }
}
