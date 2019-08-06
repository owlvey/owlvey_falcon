using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class ServiceBaseRp
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class ServiceGetRp : ServiceBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ServiceGetListRp : ServiceBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class ServicePostRp {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public float SLO { get; set; }
    }

    public class ServicePutRp
    {
        public string Value { get; set; }
    }
}
