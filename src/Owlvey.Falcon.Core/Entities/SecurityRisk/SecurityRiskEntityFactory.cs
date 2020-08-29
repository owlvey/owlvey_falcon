using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SecurityRiskEntity 
    {
        public static class Factory {

            public static SecurityRiskEntity Create(int sourceId, DateTime on, string ModifiedBy, string name,
                string description, string tags, string reference, string avatar,
                int agentSkillLevel, int motive, int opportunity, int size,
                int easyDiscovery, int easyExploit, int awareness, int intrusionDetection,
                int lossConfidentiality, int lossIntegrity, int lossAvailability, int lossAccountability,
                int financialDamage, int reputationDamage, int nonCompliance, int privacyViolation
                )
            {
                var entity = new SecurityRiskEntity
                {
                    SourceId = sourceId,                    
                    Name = name, 
                    Description = description, 
                    Tags = tags,
                    Avatar = avatar,
                    Reference = reference,
                    CreatedBy = ModifiedBy,
                    CreatedOn = on,
                    ModifiedBy = ModifiedBy,
                    ModifiedOn = on
                };
                entity.AgentSkillLevel = agentSkillLevel;
                entity.Motive = motive;
                entity.Opportunity = opportunity;
                entity.Size = size;
                entity.EasyDiscovery = easyDiscovery;
                entity.EasyExploit = easyExploit;
                entity.Awareness = awareness;
                entity.IntrusionDetection = intrusionDetection;
                entity.LossConfidentiality = lossConfidentiality;
                entity.LossIntegrity = lossIntegrity;
                entity.LossAvailability = lossAvailability;
                entity.LossAccountability = lossAccountability;

                entity.FinancialDamage = financialDamage;
                entity.ReputationDamage = reputationDamage;
                entity.NonCompliance = nonCompliance;
                entity.PrivacyViolation = privacyViolation;
                return entity;
            }

        }
    }
}
