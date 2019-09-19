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
            public static IncidentEntity Create(string key, string title, DateTime on, string user, ProductEntity product)
            {
                var entity = new IncidentEntity()
                {  
                      CreatedBy = user,
                      CreatedOn  = on,
                      Key = key,                      
                      Title = title,
                      ModifiedBy = user,
                      ModifiedOn = on,
                      Tags = "",
                      Url = "https://landing.google.com/sre/books/",
                      TTD = 10,
                      TTE =  10,
                      TTF = 10,                      
                      End = on.AddMinutes(30),
                      Product = product,
                      Affected = 1
                };
                entity.Validate();
                return entity;
            }
        }
    }
}
