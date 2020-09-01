using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using Owlvey.Falcon.Core.Aggregates;
using Owlvey.Falcon.Core.Values;

namespace Owlvey.Falcon.Core.Entities
{

    public class FeatureEntityCompare : IEqualityComparer<FeatureEntity>
    {
        public bool Equals(FeatureEntity x, FeatureEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(FeatureEntity obj)
        {
            return obj.Id.Value;
        }
    }


    public partial class FeatureEntity: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Avatar { get; set; }

        public int ProductId { get; set; }

        public int JourneyMapId { get; set; }

        public decimal AvailabilitySLO { get {
                decimal slo = 0;
                
                foreach (var item in this.JourneyMaps)
                {
                    if (item.Journey.AvailabilitySlo > slo) {
                        slo = item.Journey.AvailabilitySlo;
                    }
                }
                return slo; 
            } 
        }
        public decimal LatencySLO {
            get {
                decimal slo = decimal.MaxValue;
                foreach (var item in this.JourneyMaps)
                {
                    if (item.Journey.LatencySlo < slo)
                    {
                        slo = item.Journey.LatencySlo;
                    }
                }
                return slo;
            }
        }
        public decimal ExperienceSLO {
            get {
                decimal slo = 0;
                foreach (var item in this.JourneyMaps)
                {
                    if (item.Journey.ExperienceSlo > slo)
                    {
                        slo = item.Journey.ExperienceSlo;
                    }
                }
                return slo;
            }
        }
        public virtual ProductEntity Product { get; set; }

        public virtual ICollection<JourneyMapEntity> JourneyMaps { get; set; } = new List<JourneyMapEntity>();

        public virtual ICollection<IndicatorEntity> Indicators { get; set; } = new List<IndicatorEntity>();

        public virtual ICollection<SquadFeatureEntity> Squads { get; set; } = new List<SquadFeatureEntity>();

        public virtual ICollection<IncidentMapEntity> IncidentMap { get; set; } = new List<IncidentMapEntity>();

        public virtual void Update(DateTime on, string modifiedBy, string name, string avatar , string description)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? this.Name : name;
            this.Avatar = string.IsNullOrWhiteSpace(avatar) ? this.Avatar : avatar;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.Description : description;
            this.ModifiedOn = on;
            this.ModifiedBy = modifiedBy;            
        }


        public SLOValue SLO
        {
            get
            {
                var temp = this.JourneyMaps.Select(c => c.Journey).ToList();
                if (temp.Count() > 0)
                {
                    var ava = temp.Max(c => c.AvailabilitySlo);
                    var latency = temp.Min(c => c.LatencySlo);
                    var experience = temp.Max(c => c.AvailabilitySlo);
                    return new SLOValue(ava, latency, experience);
                }
                return null;
            }
        }

        public decimal SecurityRisk
        {
            get
            {
                var sources = this.Indicators.Select(c => c.Source).ToList();

                if (sources.Count > 0)
                    return sources.Max(c => c.SecurityRisk); 
                return 0;
            }
        }
        public string SecurityRiskLabel
        {
            get
            {
                return QualityUtils.SecurityRiskToLabel(this.SecurityRisk);
            }
        }

        public decimal ReliabilityRisk
        {
            get
            {
                var sources = this.Indicators.Select(c => c.Source).ToList();

                if (sources.Count > 0)
                    return sources.Max(c => c.ReliabilityRisk);
                return 0;                
            }
        }
        public string ReliabilityRiskLabel
        {
            get
            {
                if (this.SLO != null)
                {
                    var error = this.ReliabilityRisk;
                    return QualityUtils.ReliabilityRiskToLabel(this.SLO.Availability, error);
                }
                return "no slo";
            }
        }




    }
}
