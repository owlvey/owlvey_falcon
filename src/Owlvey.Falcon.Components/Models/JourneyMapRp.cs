using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Models
{    
    public class JourneyMapBaseRp
    {        
        public int Id { get; set; }
    }

    public class JourneyMapMigrateRp {
        public string Product { get; set; }
        public string Journey { get; set; }
        public string Feature { get; set; }
    }
    public class JourneyMapGetRp : JourneyMapBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class JourneyMapGetListRp : JourneyMapBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class JourneyMapPostRp
    {
        [Required]
        public int? JourneyId { get; set; }
        [Required]
        public int? FeatureId { get; set; }
    }
}
