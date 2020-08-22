public class SecurityRiskEntity {
    public decimal LikeHood {get ; set ;}
    public decimal Impact {get; set;}

    public int SourceId { get; set; }

    public int ThreatId {get; set; }

    #region Likehood 
    
    #region threat agent factors
    public int AgentSkillLevel { get; set; }
    public int Motive { get; set; }
    public int Opportunity { get; set; }
    public int Size {get; set; }
    
    #endregion 

    #region vulnerability factors
    public int EasyDiscovery {get; set;}
    public int EasyExploit {get;set;}
    public int Awareness {get;set;}
    public int IntrusionDetection {get; set;}

    #endregion

    #endregion 
    
    #region Impact

    #region Technical Impact
    public int LossConfidentiality { get; set; }
    public int LossIntegrity {get; set;}
    public int LossAvailability {get;set;}
    public int LossAccountability {get;set;}
    #endregion 

    #region Business Impact

    public int FinancialDamage { get; set; }
    public int ReputationDamage { get; set; }
    public int NonCompliance { get; set; }
    public int PrivacyViolation { get; set; }
    
    #endregion
    #endregion 
    public decimal Risk { 
        get{
            return LikeHood * Impact;
        }
    }
}