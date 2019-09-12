using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ProductBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ProductMigrationRp {        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }                
    }

    public class ProductGetRp : ProductBaseRp {
        public IEnumerable<ServiceGetListRp> Services { get; set; } = new List<ServiceGetListRp>();
    }

    public class ProductGetListRp : ProductBaseRp
    {
        public int ServicesCount { get; set; }

    }

    public class ProductPostRp {
        [Required]
        public string Name { get; set; }        
        [Required]
        public int CustomerId { get; set; }
    }

    public class ProductPutRp
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
