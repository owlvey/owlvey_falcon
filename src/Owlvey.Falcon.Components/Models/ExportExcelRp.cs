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
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
        public ExportExcelSourceRp() { }
        public ExportExcelSourceRp(SourceEntity source) {
            this.SourceId = source.Id.Value;
            this.Source = source.Name;
            var quality = source.Measure();
            this.Availability = quality.Availability;
            this.Experience = quality.Experience;
            this.Latency = quality.Latency;
            this.TotalDefinition = source.TotalDefinition;
            this.GoodDefinition = source.GoodDefinition;
            this.Description = source.Description;            
        }
    }

    public class ExportExcelServiceRp
    {
        public int ServiceId { get; set; }
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
            

        public ExportExcelServiceRp() { }
        public ExportExcelServiceRp(ServiceEntity service) {
            this.ServiceId = service.Id.Value;
            this.Product = service.Product.Name;
            this.Group = service.Group;
            this.Name = service.Name;
            this.AvailabilitySLO = service.AvailabilitySlo;
            this.ExperienceSLO = service.ExperienceSlo;
            this.LatencySLO = service.LatencySlo;
            var measure = service.Measure();            
            this.Availability = measure.Availability;
            this.Latency = measure.Latency;
            this.Experience = measure.Experience;
            this.Description = service.Description;                   
        }
    }

    public class ExportExcelServiceDetailRp {
        public int ServiceId { get; set; }
        public int FeatureId { get; set; }        
        public string Service { get; set; }
        public string Feature { get; set; }
        public decimal AvailabilitySLO { get; set; }
        public decimal LatencySLO { get; set; }
        public decimal ExperienceSLO { get; set; }
        public decimal ServiceQuality { get; set; }
        public decimal ServiceBudget
        {
            get
            {
                return QualityUtils.MeasureBudget(this.ServiceQuality, this.AvailabilitySLO);
            }
        }
        
        

        public ExportExcelServiceDetailRp() { }

        public ExportExcelServiceDetailRp(ServiceMapEntity map) {
            ServiceEntity service = map.Service;
            FeatureEntity feature = map.Feature;
            this.ServiceId = service.Id.Value;
            this.FeatureId = feature.Id.Value;            
            this.Service = service.Name;
            this.Feature = feature.Name;
            this.AvailabilitySLO = service.AvailabilitySlo;            
            this.ServiceQuality = service.Measure().Availability;            
        }
    }

}
