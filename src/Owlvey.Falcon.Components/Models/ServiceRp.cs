using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ServiceBaseRp
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }        
        public float SLO { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ServiceGetRp : ServiceBaseRp {
    }

    public class ServiceGetListRp : ServiceBaseRp
    {
    }

    public class ServicePostRp {
        [Required]
        public string Name { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public float SLO { get; set; }
        public string Description { get; set; }
    }

    public class ServicePutRp
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public float SLO { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}
