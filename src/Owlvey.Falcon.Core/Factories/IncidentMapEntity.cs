using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class IncidentMapEntity
    {
        public static class Factory
        {
            public static IncidentMapEntity Create(DateTime on, string user, 
                FeatureEntity feature, 
                IncidentEntity incident)
            {
                var entity = new IncidentMapEntity()
                {
                     CreatedBy = user,
                     CreatedOn = on,
                     ModifiedBy = user,
                     ModifiedOn = on,
                     Feature = feature,
                     Incident = incident,                     
                };
                entity.Validate();
                return entity;
            }
        }
    }
}
