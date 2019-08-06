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
        
    }

    public class CustomerGetListRp : CustomerBaseRp
    {
        
    }

    public class CustomerPostRp {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Avatar { get; set; }
    }

    public class CustomerPutRp
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Avatar { get; set; }
    }
}
