using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class IndicatorEntity
    {
        public static class Factory {
            public static IndicatorEntity Create(FeatureEntity feature, SourceEntity source, DateTime on,
                string user, string avatar = "https://cdn.iconscout.com/icon/free/png-256/avatar-375-456327.png")
            {
                var entity = new IndicatorEntity()
                {   
                    Source = source,
                    Feature = feature,
                    CreatedBy = user,
                    ModifiedBy = user,
                    CreatedOn = on,
                    ModifiedOn = on,
                    Avatar = avatar
                };
                entity.Validate();
                return entity;
            }
        }
    }
}
