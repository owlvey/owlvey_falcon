using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public abstract class SecurityRiskRp
    {
        
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string Tags { get; set; }

        #region Likehood 

        #region threat agent factors
        public decimal ThreatAgentFactor { get; set; }
        public int AgentSkillLevel { get; set; } = 0;
        public int Motive { get; set; } = 0;
        public int Opportunity { get; set; } = 0;
        public int Size { get; set; } = 0;

        #endregion

        #region vulnerability factors
        public decimal VulnerabilityFactor { get; set; }
        public int EasyDiscovery { get; set; } = 0;
        public int EasyExploit { get; set; } = 0;
        public int Awareness { get; set; } = 0;
        public int IntrusionDetection { get; set; } = 0;

        #endregion

        #endregion

        #region Impact

        #region Technical Impact
        public decimal TechnicalImpact { get; set; }
        public int LossConfidentiality { get; set; } = 0;
        public int LossIntegrity { get; set; } = 0;
        public int LossAvailability { get; set; } = 0;
        public int LossAccountability { get; set; } = 0;
        #endregion

        #region Business Impact
        public decimal BusinessImpact { get; set; }
        public int FinancialDamage { get; set; } = 0;
        public int ReputationDamage { get; set; } = 0;
        public int NonCompliance { get; set; } = 0;
        public int PrivacyViolation { get; set; } = 0;

        #endregion
        #endregion

        public decimal LikeHood { get; set; }
        public decimal Impact { get; set; }
        public decimal Risk { get; set; }
    }
    public class SecurityRiskGetRp: SecurityRiskRp
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public int SourceId { get; set; }
    }
    public class SecurityRiskPost { 
        [Required]
        public int SourceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Avatar { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Reference { get; set; }
        [Required]
        public string Tags { get; set; }

        #region Likehood 

        #region threat agent factors
        public decimal ThreatAgentFactor { get; set; }
        public int AgentSkillLevel { get; set; } = 0;
        public int Motive { get; set; } = 0;
        public int Opportunity { get; set; } = 0;
        public int Size { get; set; } = 0;

        #endregion

        #region vulnerability factors
        public decimal VulnerabilityFactor { get; set; }
        public int EasyDiscovery { get; set; } = 0;
        public int EasyExploit { get; set; } = 0;
        public int Awareness { get; set; } = 0;
        public int IntrusionDetection { get; set; } = 0;

        #endregion

        #endregion

        #region Impact

        #region Technical Impact
        public decimal TechnicalImpact { get; set; }
        public int LossConfidentiality { get; set; } = 0;
        public int LossIntegrity { get; set; } = 0;
        public int LossAvailability { get; set; } = 0;
        public int LossAccountability { get; set; } = 0;
        #endregion

        #region Business Impact
        public decimal BusinessImpact { get; set; }
        public int FinancialDamage { get; set; } = 0;
        public int ReputationDamage { get; set; } = 0;
        public int NonCompliance { get; set; } = 0;
        public int PrivacyViolation { get; set; } = 0;

        #endregion
        #endregion


    }
    public class SecurityRiskPut: SecurityRiskRp
    {   
        
    }
}
