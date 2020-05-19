using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Owlvey.Falcon.Core;
using System.Linq;
using Owlvey.Falcon.Core.Entities;
using System.Text.Json.Serialization;
using Owlvey.Falcon.Core.Values;
using System.Linq.Expressions;
using Owlvey.Falcon.Components;

namespace Owlvey.Falcon.Models
{
    public class ServiceBaseRp
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }

        public string Leaders { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal AvailabilitySLO { get; set; }
        public decimal LatencySLO { get; set; }
        public decimal ExperienceSLO { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int MTTD { get; set; }
        public int MTTE { get; set; }
        public int MTTF { get; set; }
        public int MTTM { get {
                return this.MTTD + this.MTTE + this.MTTF;
            }
        }
        public string MTTMS {
            get {
                return DateTimeUtils.FormatTimeToInMinutes(this.MTTM);
            }
        }

        public string Group { get; set; }
    }

    public class ServiceMigrateRp {
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal AvailabilitySLO { get; set; }
        public decimal LatencySLO { get; set; }
        public decimal ExperienceSLO { get; set; }
        public string Avatar { get; set; }
        public string Leaders { get; set; }
        public string Aggregation { get; set; }
        public string Group { get; set; }
    }

    public class ServiceGetRp : ServiceBaseRp {
        public List<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }
        public decimal Experience { get; set; }

        public decimal PreviousAvailability { get; set; }
        public decimal PreviousLatency { get; set; }
        public decimal PreviousExperience { get; set; }

        public decimal BeforeAvailability { get; set; }
        public decimal BeforeLatency { get; set; }
        public decimal BeforeExperience { get; set; }
                
        public decimal AvailabilityErrorBudget { get; set; }
        public decimal LatencyErrorBudget { get; set; }
        public decimal ExperienceErrorBudget { get; set; }

        internal void LoadMeasure(ServiceQualityMeasureValue measure) {
            this.Availability = measure.Availability;
            this.Experience = measure.Experience;
            this.Latency = measure.Latency;
            this.AvailabilityErrorBudget = measure.AvailabilityErrorBudget;
            this.ExperienceErrorBudget = measure.ExperienceErrorBudget;
            this.LatencyErrorBudget = measure.LatencyErrorBudget;
        }
        internal void LoadPrevious(ServiceQualityMeasureValue measure)
        {
            this.PreviousAvailability= measure.Availability;
            this.PreviousExperience = measure.Experience;
            this.PreviousLatency = measure.Latency;            
        }
        internal void LoadBefore(ServiceQualityMeasureValue measure)
        {
            this.BeforeAvailability = measure.Availability;
            this.BeforeExperience = measure.Experience;
            this.BeforeLatency = measure.Latency;
        }
        internal void MergeFeaturesFrom(IEnumerable<FeatureEntity> features) {
            var result = new List<FeatureGetListRp>(this.Features);
            foreach (var item in features)
            {
                var temporal = new FeatureGetListRp();
                temporal.Read(item);
                result.Add(temporal);
            }
            this.Features = result;
        }

    }

    public class MonthRp
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Jan { get; set; } = 1;
        public decimal Feb { get; set; } = 1;
        public decimal Mar { get; set; } = 1;
        public decimal Apr { get; set; } = 1;
        public decimal May { get; set; } = 1;
        public decimal Jun { get; set; } = 1;
        public decimal Jul { get; set; } = 1;
        public decimal Aug { get; set; } = 1;
        public decimal Sep { get; set; } = 1;
        public decimal Oct { get; set; } = 1;
        public decimal Nov { get; set; } = 1;
        public decimal Dec { get; set; } = 1;

        public void SetMonthValue(int month, decimal value) {
            switch (month)
            {
                case 0: this.Jan = value; break;
                case 1: this.Feb = value; break;
                case 2: this.Mar = value; break;
                case 3: this.Apr = value; break;
                case 4: this.May = value; break;
                case 5: this.Jun = value; break;                
                case 6: this.Jul = value; break;
                case 7: this.Aug = value; break;
                case 8: this.Sep = value; break;
                case 9: this.Oct = value; break;
                case 10: this.Nov = value; break;
                case 11: this.Dec = value; break;
                default: break;
            }
        }
    }
    public class AnnualServiceGroupListRp 
    {
        
        public List<MonthRp> Availability { get; set; } = new List<MonthRp>();
        public List<MonthRp> Latency { get; set; } = new List<MonthRp>();
        public List<MonthRp> Experience { get; set; } = new List<MonthRp>();        
        public IList<MultiSerieItemGetRp> Weekly { get; set; } = new List<MultiSerieItemGetRp>();
        

    }
    public class ServiceGroupListRp {

        public class ServiceGrouptem {
            public string Name { get; set; }
            
            public int Count { get; set; } = 1;

            public decimal AvailabilitySloAvg { get; set; } = 1;
            public decimal AvailabilitySloMin { get; set; } = 1;
            public decimal AvailabilityAvg { get; set; } = 1;
            public decimal AvailabilityMin { get; set; } = 1;
            public decimal AvailabilityDebt { get; set; } = 0;

            public decimal LatencySloAvg { get; set; } = 1;
            public decimal LatencySloMin { get; set; } = 1;
            public decimal LatencyAvg { get; set; } = 1;
            public decimal LatencyMin { get; set; } = 1;
            public decimal LatencyDebt { get; set; } = 0;

            public decimal ExperienceSloAvg { get; set; } = 1;
            public decimal ExperienceSloMin { get; set; } = 1;
            public decimal ExperienceAvg { get; set; } = 1;
            public decimal ExperienceMin { get; set; } = 1;
            public decimal ExperienceDebt { get; set; } = 0;

        }
        public IList<ServiceGrouptem> Items { get; set; } = new List<ServiceGrouptem>();
        public IList<MultiSerieItemGetRp> Series { get; set; } = new List<MultiSerieItemGetRp>();

    }

    public class ServiceGetListRp : ServiceBaseRp
    {
        public int FeaturesCount { get; set; }        
        public decimal Availability { get; set; }
        public decimal AvailabilityErrorBudget { get; set; }
        public decimal Latency { get; set; }

        public decimal LatencyErrorBudget { get; set; }

        public decimal Experience { get; set; }
        public decimal ExperienceErrorBudget { get; set; }
        

        public void LoadMeasure(ServiceQualityMeasureValue measure) {            
            this.Availability = measure.Availability;
            this.Latency = measure.Latency;
            this.Experience = measure.Experience;
            this.Deploy = QualityUtils.BudgetToAction(measure);
            this.AvailabilityErrorBudget = measure.AvailabilityErrorBudget;
            this.LatencyErrorBudget = measure.LatencyErrorBudget;
            this.ExperienceErrorBudget = measure.ExperienceErrorBudget;
        }        

        public string Deploy { get; set; }        
        
           
    }

    public class ServicePostRp {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ProductId { get; set; }        
    }

    public class ServicePutRp
    {
        [Required]
        public string Name { get; set; }        
        public decimal? AvailabilitySlo { get; set; }
        public decimal? LatencySlo { get; set; }
        public decimal? ExperienceSlo { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }        
        public string Group { get; set; }
        public string Leaders { get; set; }
        
    }
}
