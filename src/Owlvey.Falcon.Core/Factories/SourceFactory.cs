using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SourceEntity
    {
        public static class Factory {

            public static SourceEntity Create(ProductEntity product,  string name, DateTime on, string user)
            {
                string goodDefinition = "e.g. successful requests, as measured from the laod balancer metrics, Any HTTP status othen than 500-599 is considered successful.";
                string totalDefinition = "e.g. All requests measured from the load balancer.";
                var entity = new SourceEntity()
                {
                    Name = name,
                    GoodDefinition = goodDefinition,
                    TotalDefinition = totalDefinition,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Product = product
                };
                entity.Avatar = "https://d2.alternativeto.net/dist/icons/restpack-html-to-pdf-api_135030.png?width=128&height=128&mode=crop&upscale=false";
                entity.Validate();
                return entity;
            }
        }
    }
}
