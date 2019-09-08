using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class SquadBaseRp
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string Avatar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadGetRp : SquadBaseRp {
        public IEnumerable<UserGetListRp> Members { get; set; } = new List<UserGetListRp>();
        public IEnumerable<FeatureGetListRp> Features { get; set; } = new List<FeatureGetListRp>();
    }

    public class SquadGetListRp : SquadBaseRp
    {
        
    }

    public class SquadPostRp {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CustomerId { get; set; }        
    }

    public class SquadPutRp
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
    }
}
