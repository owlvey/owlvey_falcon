using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class CustomerBaseRp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class CustomerGetRp : CustomerBaseRp {
        public decimal Availability { get; set; }
        public IEnumerable<ProductGetListRp> Products { get; set; } = new List<ProductGetListRp>();
    }

    public class CustomerGetListRp : CustomerBaseRp
    {
        public int ProductsCount { get; set; }
        public decimal Availability { get; set; }
    }

    public class CustomerPostRp {
        [Required]
        public string Name { get; set; }
        
    }

    public class CustomerPutRp
    {        
        public string Name { get; set; }

        public string Avatar { get; set; }
    }
}
