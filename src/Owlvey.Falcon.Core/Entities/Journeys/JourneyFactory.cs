using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class JourneyEntity
    {
        public static class Factory {

            public static JourneyEntity Create(string name, DateTime on, string user, ProductEntity product)
            {
                var entity = new JourneyEntity()
                {
                    Name = name,                    
                    AvailabilitySlo = 0.99m,
                    AvailabilitySla = 0.99m,                    
                    ExperienceSlo = 0.99m,
                    LatencySla = 1000m,
                    LatencySlo = 1000m,
                    Avatar = "https://cdn4.iconfinder.com/data/icons/pretty-office-part-7-reflection-style/256/Cup-gold.png",
                    CreatedBy = user,
                    ModifiedBy = user,                    
                    CreatedOn = on,
                    ModifiedOn = on,                    
                    Product = product,
                     
                    Group = "Default"
                };                
                entity.Validate();
                return entity;
            }
        }
    }
}
