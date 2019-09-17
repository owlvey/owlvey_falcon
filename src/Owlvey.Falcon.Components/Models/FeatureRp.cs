using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class FeatureBaseRp
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }                
    }

    public class FeatureMigrateRp  {
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }                
        public string Avatar { get; set; }
    }

    public class FeatureLiteRp : FeatureBaseRp {
        
    }

    public class FeatureGetRp : FeatureBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ProductId { get; set; }
        public decimal Availability { get; set; }
        public int MTTM { get; set; }
        public int MTTE { get; set; }
        public int MTTD { get; set; }
        public int MTTF { get; set; }
        public IEnumerable<IndicatorGetListRp> Indicators { get; set; } = new List<IndicatorGetListRp>();
        public IEnumerable<SquadGetListRp> Squads { get; set; } = new List<SquadGetListRp>();

        public IEnumerable<IncidentGetListRp> Incidents { get; set; } = new List<IncidentGetListRp>();
    }

    public class FeatureGetListRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public int IndicatorsCount { get; set; }
        public int ServiceCount { get; set; }
        public decimal Availability { get; set; }

        public int MTTM { get; set; }
        public int MTTE { get; set; }
        public int MTTD { get; set; }
        public int MTTF { get; set; }
    }

    public class FeatureBySquadRp : FeatureBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ProductId { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public string ServiceAvatar { get; set; }
        public string Product { get; set; }
        public int IndicatorsCount { get; set; }
        public decimal SLO { get; set; }
        public decimal Impact { get; set; }
        public decimal Availability { get; set; }
        public decimal Points { get; set; }
    }

    public class FeaturePostRp {
        [Required]
        public string Name { get; set; }        
        [Required]
        public int ProductId { get; set; }        
    }

    public class FeaturePutRp
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Avatar { get; set; }
        public decimal? MTTD { get; set; }
        public decimal? MTTR { get; set; }
    }
}
