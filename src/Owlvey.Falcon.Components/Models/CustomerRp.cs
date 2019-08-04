using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Models
{
    public class CustomerBaseRp
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CustomerGetRp : CustomerBaseRp {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class CustomerGetListRp : CustomerBaseRp
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class CustomerPostRp {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CustomerPutRp
    {
        public string Value { get; set; }
    }
}
