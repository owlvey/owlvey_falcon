using System;
using System.ComponentModel.DataAnnotations;

namespace Owlvey.Falcon.Models
{    
    public class ServiceMapBaseRp
    {        
        public int Id { get; set; }
    }

    public class ServiceMapGetRp : ServiceMapBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ServiceMapGetListRp : ServiceMapBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ServiceMapPostRp
    {
        [Required]
        public int? ServiceId { get; set; }
        [Required]
        public int? FeatureId { get; set; }
    }
}
