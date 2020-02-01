using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Owlvey.Falcon.Core.Entities;

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
        public List<ServiceGetListRp> Services { get; set; } = new List<ServiceGetListRp>();
    }

    public class FeatureQualityGetRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public decimal Quality { get; set; }

        public decimal Latency { get; set; }
        public decimal Availability { get; set; }
        public string MTTM { get; set; }
        public string MTTE { get; set; }
        public string MTTD { get; set; }
        public string MTTF { get; set; }
        public List<IndicatorAvailabilityGetListRp> Indicators { get; set; } = new List<IndicatorAvailabilityGetListRp>();
        public List<SquadGetListRp> Squads { get; set; } = new List<SquadGetListRp>();
        public List<IncidentGetListRp> Incidents { get; set; } = new List<IncidentGetListRp>();
        public List<ServiceGetListRp> Services { get; set; } = new List<ServiceGetListRp>();
    }

    public class FeatureGetListRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public string Product { get; set; }
        public int IndicatorsCount { get; set; }
        public int ServiceCount { get; set; }
        public decimal Availability { get; set; }
        public decimal Quality { get; set; }
        public decimal Latency { get; set; }
        public int MapId { get; set; }
        public string MTTM { get; set; }
        public string MTTE { get; set; }
        public string MTTD { get; set; }
        public string MTTF { get; set; }

        public override void Read(FeatureEntity Entity){
            base.Read(Entity); 

        }
    }

    public class FeatureAvailabilityGetListRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }        
        public string Product { get; set; }
        public int IndicatorsCount { get; set; }
        public int ServiceCount { get; set; }
        public decimal Availability { get; set; }
        public decimal Quality { get; set; }
        public decimal Latency { get; set; }
        public int Total { get; set; }
        public int Good { get; set; }
        public int Squads { get; set; }
    }

    public class SequenceFeatureGetListRp : FeatureGetListRp {
        public decimal FeatureSlo { get; set; }
        public int Sequence { get; set; }
        public int Total { get; set; }
        public decimal Budget
        {
            get
            {
                return this.Quality - this.FeatureSlo;
            }
        }
    }



    public class FeatureBySquadRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public string ServiceAvatar { get; set; }
        public string Product { get; set; }
        public int IndicatorsCount { get; set; }
        public decimal SLO { get; set; }
        public decimal Impact { get; set; }
        public decimal Availability { get; set; }
        public decimal Points { get; set; }
    }

    public class FeaturePostRp {
        [Required]
        public string Name { get; set; }        
        [Required]
        public int ProductId { get; set; }        
    }

    public class FeaturePutRp
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Avatar { get; set; }
        public decimal? MTTD { get; set; }
        public decimal? MTTR { get; set; }
    }
}
