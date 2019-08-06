using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class SquadBaseRp
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class SquadGetRp : SquadBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadGetListRp : SquadBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class SquadPostRp {
        public string Name { get; set; }
        public int CustomerId { get; set; }
    }

    public class SquadPutRp
    {
        public string Value { get; set; }
    }
}
