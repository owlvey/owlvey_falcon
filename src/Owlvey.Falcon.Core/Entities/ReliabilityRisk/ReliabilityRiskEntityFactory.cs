using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class ReliabilityRiskEntity { 
        public void Update(
                DateTime ModifiedOn, 
                string ModifiedBy,
                string name, string avatar,
                string reference, string description, string tags,
                decimal ettd, decimal ette, decimal ettf,
                decimal userImpact,
                decimal ettfail)
        {            
            this.ModifiedBy = ModifiedBy;            
            this.ModifiedOn = ModifiedOn;
            this.Name = name;
            this.Avatar = avatar;
            this.Reference = reference;
            this.Description = description;
            this.Tags = tags;
            this.ETTD = ettd;
            this.ETTE = ette;
            this.ETTF = ettf;
            this.UserImpact = userImpact;
            this.ETTFail = ettfail;
        }
        public static class Factory
        {            
            public static ReliabilityRiskEntity Create(SourceEntity source, 
                DateTime on, string ModifiedBy, string name, string avatar, 
                string reference, string description, string tags,
                decimal ettd, decimal ette, decimal ettf, 
                decimal userImpact, 
                decimal ettfail)
            {
                var entity = new ReliabilityRiskEntity();
                entity.Source = source; 
                entity.CreatedBy = ModifiedBy;
                entity.CreatedOn = on;
                entity.Update(on, ModifiedBy, name, avatar, reference, description, tags, ettd, ette, ettf, userImpact, ettfail);                
                return entity;
            }
        }
    }
    
}
