using System;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SecurityRiskEntity : BaseEntity
    {
        public int SourceId { get; set; }
        public SourceEntity Source { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string Tags { get; set; }

        #region Likehood 

        #region threat agent factors
        public int AgentSkillLevel { get; set; } = 0;
        public int Motive { get; set; } = 0;
        public int Opportunity { get; set; } = 0;
        public int Size { get; set; } = 0;

        #endregion

        #region vulnerability factors
        public int EasyDiscovery { get; set; } = 0;
        public int EasyExploit { get; set; } = 0;
        public int Awareness { get; set; } = 0;
        public int IntrusionDetection { get; set; } = 0;

        #endregion

        #endregion

        #region Impact

        #region Technical Impact
        public int LossConfidentiality { get; set; } = 0;
        public int LossIntegrity { get; set; } = 0;
        public int LossAvailability { get; set; } = 0;
        public int LossAccountability { get; set; } = 0;
        #endregion

        #region Business Impact

        public int FinancialDamage { get; set; } = 0;
        public int ReputationDamage { get; set; } = 0;
        public int NonCompliance { get; set; } = 0;
        public int PrivacyViolation { get; set; } = 0;

        #endregion
        #endregion


        public decimal ThreatAgentFactor
        {
            get
            {
                return (this.AgentSkillLevel + this.Motive + this.Opportunity + this.Size) / 4;
            }
        }
        public decimal VulnerabilityFactor
        {
            get
            {
                return (this.EasyDiscovery + this.EasyExploit + this.Awareness + this.IntrusionDetection) / 4;
            }
        }

        public decimal TechnicalImpact
        {
            get
            {
                return (LossConfidentiality + LossIntegrity + LossAvailability + LossAccountability) / 4;
            }
        }
        public decimal BusinessImpact
        {
            get
            {
                return (this.FinancialDamage + this.ReputationDamage + this.NonCompliance + this.PrivacyViolation) / 4;
            }
        }
        public decimal LikeHood
        {
            get
            {
                return (ThreatAgentFactor + VulnerabilityFactor) / 2;
            }
        }
        public decimal Impact
        {
            get
            {
                return (this.TechnicalImpact + this.BusinessImpact) / 2;
            }
        }

        public decimal Risk
        {
            get
            {
                return LikeHood * Impact;
            }
        }

        public void Update(DateTime on, string ModifiedBy, string name,
                string description, string tags, string reference,
                int? agentSkillLevel, int? motive, int? opportunity, int? size,
                int? easyDiscovery, int? easyExploit, int? awareness, int? intrusionDetection,
                int? lossConfidentiality, int? lossIntegrity, int? lossAvailability, int? lossAccountability,
                int? financialDamage, int? reputationDamage, int? nonCompliance, int? privacyViolation
            )
        {

            this.AgentSkillLevel = agentSkillLevel.HasValue ? agentSkillLevel.Value : this.AgentSkillLevel;
            this.Motive = motive.HasValue ? motive.Value : this.Motive;
            this.Opportunity = opportunity.HasValue ? opportunity.Value : this.Opportunity;
            this.Size = size.HasValue ? size.Value : this.Size;
            this.EasyDiscovery = easyDiscovery.HasValue ? easyDiscovery.Value : this.EasyDiscovery;
            this.EasyExploit = easyExploit.HasValue ? easyExploit.Value : this.EasyExploit;
            this.Awareness = awareness.HasValue ? awareness.Value : this.Awareness;
            this.IntrusionDetection = intrusionDetection.HasValue ? intrusionDetection.Value : this.IntrusionDetection;
            this.LossConfidentiality = lossConfidentiality.HasValue ? lossConfidentiality.Value : this.LossAccountability;
            this.LossIntegrity = lossIntegrity.HasValue ? lossIntegrity.Value : this.LossIntegrity;
            this.LossAvailability = lossAvailability.HasValue ? lossAvailability.Value : this.LossAvailability;
            this.LossAccountability = lossAccountability.HasValue ? lossAccountability.Value : this.LossAccountability;

            this.FinancialDamage = financialDamage.HasValue ? financialDamage.Value : this.FinancialDamage;
            this.ReputationDamage = reputationDamage.HasValue ? reputationDamage.Value : this.ReputationDamage;
            this.NonCompliance = nonCompliance.HasValue ? nonCompliance.Value : this.NonCompliance;
            this.PrivacyViolation = privacyViolation.HasValue ? privacyViolation.Value : this.PrivacyViolation;

            this.Name = string.IsNullOrWhiteSpace(name) ? this.Name : name;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.Tags = string.IsNullOrWhiteSpace(tags) ? this.Tags : tags;
            this.Reference = string.IsNullOrWhiteSpace(reference) ? this.Reference : reference;
            this.ModifiedBy = ModifiedBy;
            this.ModifiedOn = on;
        }
    }
}
