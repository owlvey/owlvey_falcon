using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Entities;
using Owlvey.Falcon.Core.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ExportExcelFeatureRp {
        public int FeatureId { get; set; }          
        public string Name { get; set; }
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }
        public decimal Experience{ get; set; }

        public ExportExcelFeatureRp() { }

        public ExportExcelFeatureRp(FeatureEntity feature) {
            this.FeatureId = feature.Id.Value;            
            this.Name = feature.Name;
            var measure = feature.Measure();
            this.Availability= measure.Availability;
            this.Latency = measure.Latency;
            this.Experience= measure.Experience;
        }
    }
    public class ExportExcelFeatureDetailRp{
        public int FeatureId { get; set; }
        public int SourceId { get; set; }
        public string Feature { get; set; }        
        public string Source { get; set; }
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }

        public decimal Experience { get; set; }

        public ExportExcelFeatureDetailRp() { }

        public ExportExcelFeatureDetailRp(IndicatorEntity indicator)
        {
            FeatureEntity feature = indicator.Feature;
            this.FeatureId = feature.Id.Value;
            this.SourceId = indicator.SourceId;
            this.Feature = feature.Name;
            this.Source = indicator.Source.Name;
            var measure = indicator.Source.Measure();
            this.Availability = measure.Availability;
            this.Experience = measure.Experience;
            this.Latency = measure.Latency;
        }
    }
     
    public class ExportExcelSourceRp {
        public int SourceId { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }                         
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }
        public decimal Experience { get; set; }
        public string GoodDefinitionAvailability { get; set; }
        public string TotalDefinitionAvailability { get; set; }

        public string GoodDefinitionLatency { get; set; }
        public string TotalDefinitionLatency { get; set; }

        public string GoodDefinitionExperience { get; set; }
        public string TotalDefinitionExperience { get; set; }
        public ExportExcelSourceRp() { }
        public ExportExcelSourceRp(SourceEntity source) {
            this.SourceId = source.Id.Value;
            this.Source = source.Name;
            var quality = source.Measure();
            this.Availability = quality.Availability;
            this.Experience = quality.Experience;
            this.Latency = quality.Latency;
            this.TotalDefinitionAvailability = source.TotalDefinitionAvailability;
            this.GoodDefinitionAvailability = source.GoodDefinitionAvailability;

            this.TotalDefinitionLatency = source.TotalDefinitionLatency;
            this.GoodDefinitionLatency = source.GoodDefinitionLatency;

            this.TotalDefinitionExperience = source.TotalDefinitionExperience;
            this.GoodDefinitionExperience = source.GoodDefinitionExperience;
            this.Description = source.Description;            
        }
    }

    public class ExportExcelJourneyRp
    {
        public int JourneyId { get; set; }
        public string Product { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }                
        public decimal AvailabilitySLO { get; set; }
        public decimal LatencySLO { get; set; }
        public decimal ExperienceSLO { get; set; }
        public decimal Experience { get; set; }
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }
        
        public decimal AvailabilityErrorBudget { get                 
            {
                return QualityUtils.MeasureBudget(this.Availability, this.AvailabilitySLO);                
            }
        }
            

        public ExportExcelJourneyRp() { }
        public ExportExcelJourneyRp(JourneyEntity journey) {
            this.JourneyId = journey.Id.Value;
            this.Product = journey.Product.Name;
            this.Group = journey.Group;
            this.Name = journey.Name;
            this.AvailabilitySLO = journey.AvailabilitySlo;
            this.ExperienceSLO = journey.ExperienceSlo;
            this.LatencySLO = journey.LatencySlo;
            var measure = journey.Measure();            
            this.Availability = measure.Availability;
            this.Latency = measure.Latency;
            this.Experience = measure.Experience;
            this.Description = journey.Description;                   
        }
    }

    public class ExportExcelJourneyDetailRp {
        public int JourneyId { get; set; }
        public int FeatureId { get; set; }        
        public string Journey { get; set; }
        public string Feature { get; set; }
        public decimal AvailabilitySLO { get; set; }
        public decimal LatencySLO { get; set; }
        public decimal ExperienceSLO { get; set; }
        public decimal JourneyQuality { get; set; }
        public decimal JourneyBudget
        {
            get
            {
                return QualityUtils.MeasureBudget(this.JourneyQuality, this.AvailabilitySLO);
            }
        }
        
        

        public ExportExcelJourneyDetailRp() { }

        public ExportExcelJourneyDetailRp(JourneyMapEntity map) {
            JourneyEntity journey = map.Journey;
            FeatureEntity feature = map.Feature;
            this.JourneyId = journey.Id.Value;
            this.FeatureId = feature.Id.Value;            
            this.Journey = journey.Name;
            this.Feature = feature.Name;
            this.AvailabilitySLO = journey.AvailabilitySlo;            
            this.JourneyQuality = journey.Measure().Availability;            
        }
    }

}
