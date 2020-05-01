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
        public decimal SLO { get; set; }
        public decimal Impact { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ServiceAggregationEnum Aggregation { get; set; }

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
        public decimal SLO { get; set; }
        public string Avatar { get; set; }
        public string Leaders { get; set; }
        public string Aggregation { get; set; }
        public string Group { get; set; }
    }

    public class ServiceGetRp : ServiceBaseRp {
        public List<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        public decimal Quality { get; set; }
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }

        public decimal PreviousQuality { get; set; }

        public decimal PreviousQualityII { get; set; }
        public decimal Budget { get {
                return QualityUtils.MeasureBudget(this.Quality, SLO);
            } }
        public decimal FeatureSlo {
            get {
                if (this.Features.Count() == 0) {
                    return this.SLO;
                }
                return Math.Round((decimal)Math.Pow((double)this.SLO, 1 / (double)this.Features.Count()), 4);
            }
        }
        public decimal BudgetMinutes { get; set; }

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
        public List<MonthRp> Quality { get; set; } = new List<MonthRp>();
        public MultiSeriesGetRp Weekly { get; set; } = new MultiSeriesGetRp();


    }
    public class ServiceGroupListRp { 
        public string Name { get; set; }
        public decimal Status { get; set; } = 1;

        public decimal Previous { get; set; } = 1;
        public int Count { get; set; } = 1;
        public decimal SloAvg { get; set; } = 1;
        public decimal SloMin { get; set; } = 1;

        public decimal QualityAvg { get; set; } = 1;
        public decimal QualityMin { get; set; } = 1;

        public decimal AvailabilityAvg { get; set; } = 1;
        public decimal AvailabilityMin { get; set; } = 1;

        public decimal LatencyAvg { get; set; } = 1;
        public decimal LatencyMin { get; set; } = 1;

    }

    public class ServiceGetListRp : ServiceBaseRp
    {
        public int FeaturesCount { get; set; }
        public decimal Quality { get; set; }
        public decimal Availability { get; set; }
        public decimal Latency { get; set; }
        public decimal Previous { get; set; }

        public void LoadMeasure(QualityMeasureValue measure) {
            this.Quality = measure.Quality;
            this.Availability = measure.Availability;
            this.Latency = measure.Latency;
        }

        public string Deploy { get; set; }        
        public decimal FeatureSlo { get {
                if (this.FeaturesCount == 0) return this.SLO;
                return Math.Round((decimal)Math.Pow((double)this.SLO, 1 / (double)this.FeaturesCount), 4);                
            } }
        public decimal Budget
        {
            get
            {
                return QualityUtils.MeasureBudget(Quality, SLO);
            }
        }        
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
        public decimal? Slo { get; set; }        
        public string Avatar { get; set; }
        public string Description { get; set; }        
        public string Group { get; set; }
        public string Leaders { get; set; }
        public string Aggregation { get; set; }
    }
}
