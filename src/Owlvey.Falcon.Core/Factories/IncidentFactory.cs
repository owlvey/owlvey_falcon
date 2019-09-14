using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class IncidentEntity
    {
        public static class Factory
        {
            public static IncidentEntity Create(string title, DateTime on, string user, ProductEntity product)
            {
                var entity = new IncidentEntity()
                {  
                      CreatedBy = user,
                      CreatedOn  = on, 
                      Description = title,
                      Title = title,
                      ModifiedBy = user,
                      ModifiedOn = on,
                      Tags = "",
                      Url = "enter url",
                      MTTD = 10,
                      MTTE =  10,
                      MTTF = 10,
                      Product = product
                };
                entity.Validate();
                return entity;
            }
        }
    }
}
