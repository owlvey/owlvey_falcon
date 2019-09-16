using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Owlvey.Falcon.Core;

namespace Owlvey.Falcon.Models
{
    public class ServiceBaseRp
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }        
        public decimal SLO { get; set; }
        public decimal Impact { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        
        public decimal MTTD { get; set; }        
        public decimal MTTE { get; set; }        
        public decimal MTTF { get; set; }
        public decimal MTTM { get; set; }

    }

    public class ServiceMigrateRp {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }        
        public decimal SLO { get; set; }
        public string ProductName { get; set; }        
    }

    public class ServiceGetRp : ServiceBaseRp {
        public IEnumerable<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
        public decimal Availability { get; set; }        
        public decimal Budget { get {
                return AvailabilityUtils.MeasureBudget(Availability, SLO);
            } }
        public decimal BudgetMinutes { get; set; }
    }

    public class ServiceGetListRp : ServiceBaseRp
    {
        public int FeaturesCount { get; set; }
        public decimal Availability { get; set; }
        public string Deploy { get; set; }
        public string Risk { get; set; }
        public decimal Budget
        {
            get
            {
                return AvailabilityUtils.MeasureBudget(Availability, SLO);
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
        [Required]
        public int ProductId { get; set; }        
        public decimal? Slo { get; set; }        
        public string Avatar { get; set; }
        public string Description { get; set; }        
    }
}
