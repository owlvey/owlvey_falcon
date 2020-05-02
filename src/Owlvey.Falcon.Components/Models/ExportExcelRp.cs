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
        public decimal Quality { get; set; }        

        public ExportExcelFeatureRp() { }

        public ExportExcelFeatureRp(FeatureEntity feature) {
            this.FeatureId = feature.Id.Value;            
            this.Name = feature.Name;            
            this.Quality = feature.MeasureQuality().Quality;            
        }
    }
    public class ExportExcelFeatureDetailRp{
        public int FeatureId { get; set; }
        public int SourceId { get; set; }
        public string Feature { get; set; }
        public decimal FeatureQuality { get; set; }
        public string Source { get; set; }
        public decimal SourceQuality { get; set; }

        public ExportExcelFeatureDetailRp() { }

        public ExportExcelFeatureDetailRp(IndicatorEntity indicator)
        {
            FeatureEntity feature = indicator.Feature;
            this.FeatureId = feature.Id.Value;
            this.SourceId = indicator.SourceId;
            this.Feature = feature.Name;
            this.Source = indicator.Source.Name;
            this.FeatureQuality = feature.MeasureQuality().Quality;
            this.SourceQuality = indicator.Source.MeasureProportion().Proportion;
        }
    }
     
    public class ExportExcelSourceRp {
        public int SourceId { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }                         
        public decimal Quality { get; set; }
        public string GoodDefinition { get; set; }
        public string TotalDefinition { get; set; }
        public ExportExcelSourceRp() { }
        public ExportExcelSourceRp(SourceEntity source) {
            this.SourceId = source.Id.Value;
            this.Source = source.Name;
            this.Kind = source.Kind.ToString();
            this.Quality = source.MeasureProportion().Proportion;            
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
        public decimal Quality { get; set; }
        public decimal Budget { get                 
            {
                return QualityUtils.MeasureBudget(Quality, SLO);                
            }
        }
        public string Action { get {
                return QualityUtils.BudgetToAction(this.Budget);
            }
        }        

        public ExportExcelServiceRp() { }
        public ExportExcelServiceRp(ServiceEntity service) {
            this.ServiceId = service.Id.Value;
            this.Product = service.Product.Name;
            this.Group = service.Group;
            this.Name = service.Name;
            this.SLO = service.Slo;
            this.Quality = service.MeasureQuality().Quality;
            this.Description = service.Description;         
        }

    }

    public class ExportExcelServiceDetailRp {
        public int ServiceId { get; set; }
        public int FeatureId { get; set; }        
        public string Service { get; set; }
        public string Feature { get; set; }
        public decimal SLO { get; set; }
        public decimal ServiceQuality { get; set; }
        public decimal ServiceBudget
        {
            get
            {
                return QualityUtils.MeasureBudget(this.ServiceQuality, this.SLO);
            }
        }
        public decimal FeatureSLO { get; set; }        
        public decimal FeatureQuality { get; set; }
        public decimal FeatureBudget { get {
                return QualityUtils.MeasureBudget(this.FeatureQuality, this.FeatureSLO);
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
            this.ServiceQuality = service.MeasureQuality().Quality;
            this.FeatureQuality= feature.MeasureQuality().Quality;
        }
    }

}
