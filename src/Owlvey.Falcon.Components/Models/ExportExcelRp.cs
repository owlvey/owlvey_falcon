using Owlvey.Falcon.Core;
using Owlvey.Falcon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ExportExcelFeatureRp {
        public int FeatureId { get; set; }          
        public string Name { get; set; }
        public decimal Availability { get; set; }        

        public ExportExcelFeatureRp() { }

        public ExportExcelFeatureRp(FeatureEntity feature) {
            this.FeatureId = feature.Id.Value;            
            this.Name = feature.Name;            
            this.Availability = feature.Availability;            
        }
    }
    public class ExportExcelFeatureDetailRp{
        public int FeatureId { get; set; }
        public int SourceId { get; set; }
        public string Feature { get; set; }
        public decimal FeatureAvailability { get; set; }
        public string Source { get; set; }
        public decimal SourceAvailability { get; set; }

        public ExportExcelFeatureDetailRp() { }

        public ExportExcelFeatureDetailRp(IndicatorEntity indicator)
        {
            FeatureEntity feature = indicator.Feature;
            this.FeatureId = feature.Id.Value;
            this.SourceId = indicator.SourceId;
            this.Feature = feature.Name;
            this.Source = indicator.Source.Name;
            this.FeatureAvailability = feature.Availability;
            this.SourceAvailability = indicator.Source.Availability;
        }
    }
     
    public class ExportExcelSourceRp {
        public int SourceId { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }                         
        public decimal Availability { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
        public ExportExcelSourceRp() { }
        public ExportExcelSourceRp(SourceEntity source) {
            this.SourceId = source.Id.Value;
            this.Source = source.Name;
            this.Kind = source.Kind.ToString();
            this.Availability = source.Availability;            
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
        public decimal SLO { get; set; }
        public decimal Availability { get; set; }
        public decimal Budget { get                 
            {
                return AvailabilityUtils.MeasureBudget(Availability, SLO);                
            }
        }
        public string Action { get {
                return AvailabilityUtils.BudgetToAction(this.Budget);
            }
        }        

        public ExportExcelServiceRp() { }
        public ExportExcelServiceRp(ServiceEntity service) {
            this.ServiceId = service.Id.Value;
            this.Product = service.Product.Name;
            this.Group = service.Group;
            this.Name = service.Name;
            this.SLO = service.Slo;
            this.Availability = service.Availability;
            this.Description = service.Description;         
        }

    }

    public class ExportExcelServiceDetailRp {
        public int ServiceId { get; set; }
        public int FeatureId { get; set; }        
        public string Service { get; set; }
        public string Feature { get; set; }
        public decimal SLO { get; set; }
        public decimal ServiceAvailability { get; set; }
        public decimal ServiceBudget
        {
            get
            {
                return AvailabilityUtils.MeasureBudget(this.ServiceAvailability, this.SLO);
            }
        }
        public decimal FeatureSLO { get; set; }        
        public decimal FeatureAvailability { get; set; }
        public decimal FeatureBudget { get {
                return AvailabilityUtils.MeasureBudget(this.FeatureAvailability, this.FeatureSLO);
            } }

        public ExportExcelServiceDetailRp() { }

        public ExportExcelServiceDetailRp(ServiceMapEntity map) {
            ServiceEntity service = map.Service;
            FeatureEntity feature = map.Feature;
            this.ServiceId = service.Id.Value;
            this.FeatureId = feature.Id.Value;            
            this.Service = service.Name;
            this.Feature = feature.Name;
            this.SLO = service.Slo;
            this.FeatureSLO = service.FeatureSLO;
            this.ServiceAvailability = service.Availability;
            this.FeatureAvailability = feature.Availability;
        }
    }

}
