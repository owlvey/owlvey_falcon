namespace Owlvey.Falcon.Core.Entities
{
    public partial  class ReliabilityRiskEntity : BaseEntity
    {        
        public int SourceId { get; set; }
        public SourceEntity Source { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public decimal ETTD { get; set; } // minutes
        public decimal ETTE { get; set; } // minutes
        public decimal ETTF { get; set; } // minutes
        public decimal UserImpact { get; set; } // percentage of users affected 
        public decimal ETTFail { get; set; }  // Estimate time to fail
        public decimal ETTR
        {
            get
            {
                return this.ETTD + this.ETTE + this.ETTF;
            }
        }
        public decimal IncidentsPerYear
        {
            get
            {
                if (this.ETTFail > 0)
                {
                    return (365.25m / this.ETTFail);
                }
                return 0;
            }
        }
        public decimal BadMinutesPerYear
        {
            get
            {
                return this.ETTR * this.UserImpact * this.IncidentsPerYear;
            }
        }
    }
}