using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Owlvey.Falcon.Core;
using System.Linq;
using Owlvey.Falcon.Core.Entities;
using System.Text.Json.Serialization;

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
        public IEnumerable<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        public decimal Availability { get; set; }        
        public decimal Budget { get {
                return QualityUtils.MeasureBudget(Availability, SLO);
            } }
        public decimal FeatureSlo {
            get {
                if (this.Features.Count() == 0 ) {
                    return this.SLO;
                }
                return Math.Round((decimal)Math.Pow((double)this.SLO, 1 / (double)this.Features.Count()), 4);
            }            
        }
        public decimal BudgetMinutes { get; set; }
    }

    public class ServiceGetListRp : ServiceBaseRp
    {
        public int FeaturesCount { get; set; }
        public decimal Availability { get; set; }
        public string Deploy { get; set; }
        public string Risk { get; set; }
        public decimal FeatureSlo { get {
                if (this.FeaturesCount == 0) return this.SLO;
                return Math.Round((decimal)Math.Pow((double)this.SLO, 1 / (double)this.FeaturesCount), 4);                
            } }
        public decimal Budget
        {
            get
            {
                return QualityUtils.MeasureBudget(Availability, SLO);
            }
        }
        public decimal BudgetMinutes { get; set; }
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
