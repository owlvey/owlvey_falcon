using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Owlvey.Falcon.Models
{
    public class SourceLiteRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; } = "";
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Percentile { get; set; }
        public DefinitionValue AvailabilityDefinition { get; set; }
        public DefinitionValue LatencyDefinition { get; set; }
        public DefinitionValue ExperienceDefinition { get; set; }

        public string ReliabilityRiskLabel { get; set; }
        public string SecurityRiskLabel { get; set; }
        public decimal SecurityRisk { get; set; }
        public decimal ReliabilityRisk { get; set; }
        public SLOValue SLO { get; set; }

    }
    public class SourceAnchorRp { 
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Target { get; set; }
    }
    public class SourceGetRp : SourceLiteRp
    {        
        public QualityMeasureValue Quality { get; set; }
        public DebtMeasureValue Debt { get; set; } = new DebtMeasureValue();
        public IEnumerable<FeatureGetListRp>  Features  { get; set; } = new List<FeatureGetListRp>();
        public Dictionary<string, decimal> Clues { get; set; } = new Dictionary<string, decimal>();
        public IEnumerable<DayMeasureValue> Daily { get; set; } = new List<DayMeasureValue>();
        public IEnumerable<JourneyGetListRp> Journeys { get; set; } = new List<JourneyGetListRp>();
    }

    public class SourcesGetRp
    {
        public List<SourceGetListRp> Items { get; set; } = new List<SourceGetListRp>();

        public decimal Availability { get; set; }

        public int AvailabilityInteractionsTotal { get; set; }
        public int AvailabilityInteractionsGood { get; set; }
        public decimal AvailabilityInteractions { get; set; }

        public decimal Latency { get; set; }

        public decimal Experience { get; set; }
        
    }

    public class SourceMigrateRp {
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; } = "";
        public string GoodDefinitionAvailability { get; set; }
        public string TotalDefinitionAvailability { get; set; }

        public string GoodDefinitionLatency { get; set; }
        public string TotalDefinitionLatency { get; set; }

        public string GoodDefinitionExperience { get; set; }
        public string TotalDefinitionExperience { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }        
    }

    public class SourceGetListRp : SourceLiteRp
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public QualityMeasureValue Measure { get; set; }        
        public double? Correlation { get; set; }
        public int References { get; set; }

        public DebtMeasureValue Debt { get; set; } = new DebtMeasureValue();

    }




    public class SourcePostRp
    {
        public int ProductId { get; set; }
        public string Name { get; set; }        
    }

    public class SourcePutRp
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string AvailabilityGoodDefinition { get; set; }
        public string AvailabilityTotalDefinition { get; set; }

        public DefinitionValue AvailabilityDefinition {
            get {
                return new DefinitionValue(this.AvailabilityGoodDefinition, this.AvailabilityTotalDefinition);
            }
        }

        public string LatencyGoodDefinition { get; set; }
        public string LatencyTotalDefinition { get; set; }
        public DefinitionValue LatencyDefinition
        {
            get
            {
                return new DefinitionValue(this.LatencyGoodDefinition, this.LatencyTotalDefinition);
            }
        }
        public string ExperienceGoodDefinition  { get; set; }
        public string ExperienceTotalDefinition { get; set; }
        public DefinitionValue ExperienceDefinition
        {
            get
            {
                return new DefinitionValue(this.ExperienceGoodDefinition, this.ExperienceTotalDefinition);
            }
        }
        [Required]
        public decimal Percentile { get; set; }
        public string Tags { get; set; }
    }
}
