using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Models
{
    public class FeatureBaseRp
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int Id { get; set; } 
        public int ProductId { get; set; }  

        public virtual void Read(FeatureEntity entity){
            this.Id = entity.Id.Value;
            this.Name = entity.Name;
            this.Avatar = entity.Avatar;
            this.Description = entity.Description; 
            this.ProductId = entity.ProductId;       
        }               
    }

    public class FeatureMigrateRp  {
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }                
        public string Avatar { get; set; }
    }

    public class FeatureLiteRp : FeatureBaseRp {
        
    }

    public class FeatureGetRp : FeatureBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
              
        public string MTTM { get; set; }
        public string MTTE { get; set; }
        public string MTTD { get; set; }
        public string MTTF { get; set; }
        public List<IndicatorGetListRp> Indicators { get; set; } = new List<IndicatorGetListRp>();
        public List<SquadGetListRp> Squads { get; set; } = new List<SquadGetListRp>();
        public List<IncidentGetListRp> Incidents { get; set; } = new List<IncidentGetListRp>();
        public List<JourneyGetListRp> Journeys { get; set; } = new List<JourneyGetListRp>();
    }

    public class FeatureQualityGetRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }                
        public decimal Latency { get; set; }
        public decimal Availability { get; set; }
        public decimal Experience { get; set; }
        public void LoadQuality(QualityMeasureValue quality)
        {
            this.Availability = quality.Availability;            
            this.Latency = quality.Latency;
            this.Experience = quality.Experience;
        }
        public DebtMeasureValue Debt { get; set; } = new DebtMeasureValue();
        
        public List<IndicatorDetailRp> Indicators { get; set; } = new List<IndicatorDetailRp>();
        public List<SquadGetListRp> Squads { get; set; } = new List<SquadGetListRp>();
        public List<IncidentGetListRp> Incidents { get; set; } = new List<IncidentGetListRp>();
        public List<JourneyGetListRp> Journeys { get; set; } = new List<JourneyGetListRp>();
    }

    public class FeatureGetListRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public string Product { get; set; }
        public int IndicatorsCount { get; set; }
        public int JourneyCount { get; set; }
        public decimal Availability { get; set; }        
        public decimal Latency { get; set; }

        public decimal Experience { get; set; }
        public int MapId { get; set; }
        
        public DebtMeasureValue Debt { get; set; }
        public override void Read(FeatureEntity Entity){
            base.Read(Entity); 

        }

        public void LoadMeasure(QualityMeasureValue quality) {
            this.Availability = quality.Availability;
            this.Experience = quality.Experience;
            this.Latency = quality.Latency;
        }
    }

    public class FeatureAvailabilityGetListRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public string Product { get; set; }
        public int IndicatorsCount { get; set; }
        public int JourneyCount { get; set; }
        public QualityMeasureValue Quality { get; set; }
        
        public int Squads { get; set; }

        public DebtMeasureValue Debt { get; set; } = new DebtMeasureValue();

    }

    public class SequenceFeatureGetListRp : FeatureGetListRp {        
        public int Sequence { get; set; }                
    }



    public class FeatureBySquadRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int JourneyId { get; set; }
        public string Journey { get; set; }
        public string JourneyAvatar { get; set; }
        public string Product { get; set; }

        public SLOValue SLO { get; set; }
        public DebtMeasureValue Debt { get; set; }
        
        public QualityMeasureValue Quality { get; set; }
        
    }

    public class FeaturePostRp {
        [Required]
        public string Name { get; set; }        
        [Required]
        public int ProductId { get; set; }        
    }

    public class FeaturePutRp
    {        
        public string Name { get; set; }        
        public string Description { get; set; }
        public string Avatar { get; set; }        
    }
}
