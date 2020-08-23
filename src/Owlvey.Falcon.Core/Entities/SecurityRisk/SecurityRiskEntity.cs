using System;

namespace Owlvey.Falcon.Core.Entities
{
    public partial class SecurityRiskEntity : BaseEntity
    {
        public int SourceId { get; set; }
        public SourceEntity Source { get; set; } 

        public int SecurityThreatId { get; set; }        
        public SecurityThreatEntity SecurityThreat { get; set; }

        #region Likehood 

        #region threat agent factors
        public int AgentSkillLevel { get; set; } = 1;
        public int Motive { get; set; } = 1;
        public int Opportunity { get; set; } = 1;
        public int Size { get; set; } = 1;

        #endregion

        #region vulnerability factors
        public int EasyDiscovery { get; set; } = 1;
        public int EasyExploit { get; set; } = 1;
        public int Awareness { get; set; } = 1;
        public int IntrusionDetection { get; set; } = 1;

        #endregion

        #endregion

        #region Impact

        #region Technical Impact
        public int LossConfidentiality { get; set; } = 1;
        public int LossIntegrity { get; set; } = 1;
        public int LossAvailability { get; set; } = 1;
        public int LossAccountability { get; set; } = 1;
        #endregion

        #region Business Impact

        public int FinancialDamage { get; set; } = 1;
        public int ReputationDamage { get; set; } = 1;
        public int NonCompliance { get; set; } = 1;
        public int PrivacyViolation { get; set; } = 1;

        #endregion
        #endregion

        public decimal LikeHood { get; set; } 
        public decimal Impact { get; set; } 

        public decimal Risk
        {
            get
            {
                return LikeHood * Impact;
            }
        }

        public void Update(DateTime on, string ModifiedBy, int AgentSkillLevel) {
            this.AgentSkillLevel = AgentSkillLevel;
            this.ModifiedOn = on;
            this.ModifiedBy = ModifiedBy;

        }
    }
}
