using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class ServiceBaseRp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ServiceGetRp : ServiceBaseRp {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ServiceGetListRp : ServiceBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ServicePostRp {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ServicePutRp
    {
        public string Value { get; set; }
    }
}
