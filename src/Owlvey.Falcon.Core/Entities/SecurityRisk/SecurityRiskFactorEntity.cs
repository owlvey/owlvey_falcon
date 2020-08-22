namespace Owlvey.Falcon.Core.Entities
{
}
public class RiskWeightEntity {

    public int Id { get; set; }
    public int ProductId { get; set; }

    #region Likehood 
    
    #region threat agent factors
    public int AgentSkillLevel { get; set; } = 1;
    public int Motive { get; set; } = 1;
    public int Opportunity { get; set; } = 1;
    public int Size {get; set; } = 1;
    
    #endregion 

    #region vulnerability factors
    public int EasyDiscovery {get; set;} = 1;
    public int EasyExploit {get;set;} = 1;
    public int Awareness {get;set;} =1;
    public int IntrusionDetection {get; set;} =1;

    #endregion

    #endregion 
    
    #region Impact

    #region Technical Impact
    public int LossConfidentiality { get; set; } =1;
    public int LossIntegrity {get; set;}=1;
    public int LossAvailability {get;set;}=1;
    public int LossAccountability {get;set;}=1;
    #endregion 

    #region Business Impact

    public int FinancialDamage { get; set; } =1;
    public int ReputationDamage { get; set; } =1;
    public int NonCompliance { get; set; } =1;
    public int PrivacyViolation { get; set; } =1;
    
    #endregion
    #endregion    

}