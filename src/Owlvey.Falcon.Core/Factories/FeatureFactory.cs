using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class FeatureEntity
    {
        public static class Factory {

            public static FeatureEntity Create(string name, string createdBy)
            {
                var entity = new FeatureEntity()
                {
                    Name = name,
                    CreatedBy = createdBy
                };

                entity.Create(createdBy, DateTime.UtcNow);

                return entity;
            }
        }
    }
}
