using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SecurityRiskEntity 
    {
        public static class Factory {

            public static SecurityRiskEntity Create(
                int sourceId,
                int threatId,
                string CreatedBy,
                DateTime CreatedOn
                ) {
                var entity = new SecurityRiskEntity
                {
                    SourceId = sourceId,
                    SecurityThreatId = threatId,
                    CreatedBy = CreatedBy,
                    CreatedOn = CreatedOn,
                    ModifiedBy = CreatedBy,
                    ModifiedOn = CreatedOn
                };
                return entity;
            }

        }
    }
}
